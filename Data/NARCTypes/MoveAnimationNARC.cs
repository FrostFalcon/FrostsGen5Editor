using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;
using static System.Net.WebRequestMethods;

namespace NewEditor.Data.NARCTypes
{
    public class MoveAnimationNARC : NARC
    {
        public List<MoveAnimationEntry> animations;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            animations = new List<MoveAnimationEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                MoveAnimationEntry m = new MoveAnimationEntry(bytes) { nameID = i };
                animations.Add(m);

                pos += 8;
            }
        }

        public override void WriteData()
        {
            List<byte> newByteData = new List<byte>();
            List<byte> oldByteData = new List<byte>(byteData);

            newByteData.AddRange(oldByteData.GetRange(0, pointerStartAddress));
            newByteData.AddRange(oldByteData.GetRange(BTNFPosition, FileEntryStart - BTNFPosition));

            //Write Files
            int totalSize = 0;
            int pPos = pointerStartAddress;
            foreach (MoveAnimationEntry m in animations)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += m.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (MoveAnimationEntry m in animations)
            {
                newByteData.AddRange(m.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(animations.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            MoveAnimationNARC other = narc as MoveAnimationNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                if (i > other.animations.Count || !animations[i].bytes.SequenceEqual(other.animations[i].bytes))
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(animations[i].bytes.Length));
                    bytes.AddRange(animations[i].bytes);
                }
            }

            return bytes.ToArray();
        }

        public override void ReadPatchBytes(byte[] bytes)
        {
            int pos = 0;
            while (pos < bytes.Length)
            {
                int id = HelperFunctions.ReadInt(bytes, pos);
                int size = HelperFunctions.ReadInt(bytes, pos + 4);
                pos += 8;

                if (id > animations.Count)
                {
                    //Don't accept extra files here
                }
                else animations[id].bytes = new List<byte>(bytes).GetRange(pos, size).ToArray();
                pos += size;
            }
        }
    }

    public class MoveAnimationEntry
    {
        public byte[] bytes;
        public int nameID;

        public List<List<MoveAnimationCommand>> sequences;

        public MoveAnimationEntry(byte[] bytes)
        {
            this.bytes = bytes;
            sequences = new List<List<MoveAnimationCommand>>();

            int numSeq = HelperFunctions.ReadInt(bytes, 0);
            for (int i = 0; i < numSeq; i++)
            {
                sequences.Add(new List<MoveAnimationCommand>());
                int pos = HelperFunctions.ReadInt(bytes, 4 + 56 * i);
                while (pos < bytes.Length)
                {
                    MoveAnimationCommand cmd = new MoveAnimationCommand(bytes, ref pos);
                    sequences[i].Add(cmd);
                    if (cmd.commandID == 0x4D) break;
                }
            }
        }

        public void ApplyData()
        {
            List<byte> newBytes = new List<byte>();
            newBytes.AddRange(BitConverter.GetBytes(sequences.Count));
            for (int i = 0; i < sequences.Count * 56; i++) newBytes.Add(0);

            for (int i = 0; i < sequences.Count; i++)
            {
                for (int j = 0; j < 14; j++) HelperFunctions.WriteInt(newBytes, 4 + 56 * i + 4 * j, newBytes.Count);
                foreach (MoveAnimationCommand cmd in sequences[i])
                    newBytes.AddRange(cmd.GetBytes());
            }
            bytes = newBytes.ToArray();
        }

        public override string ToString()
        {
            return nameID < MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.MoveNameTextFileID].text[nameID] + " - " + nameID : "Name not found";
        }
    }

    public struct MoveAnimationCommand
    {
        public static List<List<string>> commandDefinitions = new List<List<string>>()
        {
            new List<string>() { "MoveCamera", "p1", "p2: 9 = focus user, 11 = focus target", "p3: how long it takes to reach the target", "p4", "p5" },
            new List<string>() { "AdjustCamera", "p1", "p2: moves around in a circle", "p3: like p2 but moves vertically", "p4: zooms in or out", "p5", "p6: moves vertically", "p7: moves horizontally", "p8: how long the adjustment takes", "p9", "p10" },
            new List<string>() { "Cmd2", "p1", "p2", "p3", "p4", "p5", "p6" },
            new List<string>() { "ShakeScreen", "p1: 0 = shake horizontally, 1 = shake vertically", "p2: intensity", "p3", "p4", "p5: duration", "p6: number of times to shake" },
            new List<string>() { "Cmd4", "p1", "p2" },
            new List<string>() { "Cmd5" },
            new List<string>() { "LoadSPA", "p1: special effect ID" },
            new List<string>() { "PlaySPAAnimation", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3: position of the particle, 9 = user, 11 = target", "p4", "p5: vertical offset", "p6", "p7", "p8: particle width", "p9", "p10: particle height", "p11" },
            new List<string>() { "PlaySPAScreenAnimation", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3: X position", "p4: Y position", "p5: Z position", "p6", "p7", "p8", "p9", "p10", "p11", "p12", "p13", "p14", "p15" },
            new List<string>() { "PlaySPAAnimation2", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3: position of the particle, 9 = user, 11 = target", "p4", "p5: horizontal offset", "p6: vertical offset", "p7", "p8: particle width", "p9", "p10: particle height", "p11" },
            new List<string>() { "CmdA", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10" },
            new List<string>() { "CmdB", "p1" },
            new List<string>() { "PlaySPAProjectile", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3: movement pattern", "p4: projectile source, 9 = user, 11 = target", "p5: projectile destination, 9 = user, 11 = target", "p6", "p7: slows down the projectile", "p8: arc height", "p9", "p10", "p11" },
            new List<string>() { "PlaySPAProjectile2", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3", "p4", "p5: vertical position", "p6", "p7: projectile destination, 9 = user, 11 = target", "p8", "p9", "p10", "p11", "p12", "p13" },
            new List<string>() { "PlaySPAProjectile3", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3", "p4: projectile source, 9 = user, 11 = target", "p5: projectile destination, 9 = user, 11 = target", "p6", "p7: slows down the projectile", "p8: arc height", "p9", "p10", "p11" },
            new List<string>() { "CmdF", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10", "p11", "p12", "p13" },
            new List<string>() { "PlaySPACircle", "p1: special effect ID from LoadSPA", "p2: particle ID", "p3: 3 = clockwise, 2 = counter-clockwise", "p4", "p5", "p6", "p7", "p8", "p9", "p10" },
            new List<string>() { "Cmd11", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8", "p9", "p10" },
            new List<string>() { "ShakeSprite", "p1: 14 = shake user, 16 = shake target, 18 = shake both", "p2", "p3: horizontal intensity", "p4: vertical intensity", "p5: speed", "p6: duration", "p7, number of times to shake" },
            new List<string>() { "MoveSprite", "p1: 14 = move user, 16 = move target, 18 = move both", "p2", "p3", "p4", "p5", "p6: speed", "p7: duration", "p8: number of times", "p9" },
            new List<string>() { "Cmd14", "p1", "p2", "p3", "p4", "p5", "p6" },
            new List<string>() { "DistortSprite", "p1: 14 = distort user, 16 = distort target, 18 = distort both", "p2", "p3: horizontal squash and stretch", "p4: vertical squash and stretch", "p5: speed", "p6: duration", "p7: number of times" },
            new List<string>() { "TiltSprite", "p1: 14 = tilt user, 16 = tilt target, 18 = tilt both", "p2: mode", "p3: tilt direction", "p4: speed", "p5: duration", "p6: number of times" },
            new List<string>() { "SpriteOpacity", "p1: 14 = affect user, 16 = affect target, 18 = affect both", "p2: transition speed", "p3: opacity, ranges from 0 (transparent) to 31 (opaque)", "p4: speed", "p5: duration", "p6" },
            new List<string>() { "Cmd18", "p1", "p2", "p3", "p4", "p5", "p6" },
            new List<string>() { "Cmd19", "p1", "p2", "p3", "p4" },
            new List<string>() { "FreezeSprite", "p1: 14 = affect user, 16 = affect target, 18 = affect both", "p2: 1 = freeze sprite, 0 = unfreeze sprite" },
            new List<string>() { "ChangeColor", "p1: 14 = affect user, 16 = affect target, 18 = affect both", "p2: starting saturation, ranges from 0 to 16", "p3: ending saturation, ranges from 0 to 16", "p4: speed", "p5: 15 bit color" },
            new List<string>() { "ChangeVisibility", "p1: 14 = affect user, 16 = affect target, 18 = affect both", "p2: 3 = invisible, 4 = visible" },
            new List<string>() { "Cmd1D", "p1: 14 = affect user, 16 = affect target, 18 = affect both", "p2" },
            new List<string>() { "Cmd1E", "p1", "p2", "p3", "p4", "p5", "p6", "p7" },
            new List<string>() { "Cmd1F", "p1" },
            new List<string>() { "Cmd20", "p1", "p2", "p3", "p4", "p5" },
            new List<string>() { "Cmd21", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8" },
            new List<string>() { "Cmd22", "p1", "p2" },
            new List<string>() { "Cmd23", "p1" },
            new List<string>() { "LoadBackground", "p1: background ID" },
            new List<string>() { "MoveBackground", "p1", "p2", "p3", "p4: duration", "p5", "p6" },
            new List<string>() { "DistortBackground", "p1: 0 = horizontal, 1 = vertical", "p2", "p3", "p4: duration", "p5", "p6" },
            new List<string>() { "Cmd27", "p1", "p2" },
            new List<string>() { "Cmd28", "p1" },
            new List<string>() { "Cmd29", "p1", "p2", "p3", "p4", "p5", "p6" },
            new List<string>() { "ChangeBackgroundColor", "p1", "p2: starting saturation", "p3: ending saturation", "p4: speed", "p5: 15 bit color" },
            new List<string>() { "ApplyBackground", "p1", "p2" },
            new List<string>() { "Cmd2C", "p1", "p2", "p3", "p4", "p5", "p6", "p7" },
            new List<string>() { "Cmd2D", "p1", "p2", "p3", "p4", "p5", "p6", "p7" },
            new List<string>() { "Cmd2E", "p1", "p2", "p3", "p4", "p5", "p6", "p7" },
            new List<string>() { "Cmd2F", "p1", "p2", "p3", "p4", "p5", "p6", "p7" },
            new List<string>() { "Cmd30", "p1", "p2" },
            new List<string>() { "Cmd31", "p1", "p2", "p3", "p4", "p5" },
            new List<string>() { "Cmd32", "p1" },
            new List<string>() { "Cmd33", "p1", "p2" },
            new List<string>() { "PlaySound", "p1: sound ID", "p2", "p3: audio position, 14 = user, 16 = target", "p4: delay before playing", "p5: pitch", "p6: volume, ranges from 0 to 127", "p7", "p8", "p9" },
            new List<string>() { "Cmd35", "p1" },
            new List<string>() { "SwitchAudioSide", "p1", "p2", "p3", "p4", "p5", "p6", "p7", "p8" },
            new List<string>() { "AdjustSound", "p1", "p2", "p3: mode, 0 = pitch, 1 = volume, 2 = pan", "p4: start value", "p5: end value", "p6: transition duration", "p7", "p8", "p9" },
            new List<string>() { "WaitForCommands", "p1" },
            new List<string>() { "Wait", "p1: number of frames to wait" },
            new List<string>() { "AudioContainer", "p1: 0 if line below is an audio command, otherwise 1" },
            new List<string>() { "CheckMoveUser", "p1", "p2", "p3", "p4" },
            new List<string>() { "Cmd3C", "p1", "p2", "p3", "p4" },
            new List<string>() { "Cmd3D", "p1", "p2", "p3" },
            new List<string>() { "Cmd3E", "p1" },
            new List<string>() { "Cmd3F", "p1" },
            new List<string>() { "Cmd40", "p1", "p2" },
            new List<string>() { "Cmd41", "p1" },
            new List<string>() { "Cmd42", "p1", "p2", "p3", "p4", "p5", "p6", "p7" },
            new List<string>() { "PlayPokemonCry", "p1: 14 = user, 16 = target, 18 = both", "p2: pitch and speed", "p3", "p4", "p5", "p6: reverse", "p7: delay before playing" },
            new List<string>() { "Cmd44", "p1" },
            new List<string>() { "Cmd45", "p1", "p2", "p3", "p4", "p5", "p6" },
            new List<string>() { "Cmd46", "p1", "p2", "p3" },
            new List<string>() { "Cmd47" },
            new List<string>() { "CheckMoveUserElse", "p1" },
            new List<string>() { "Cmd49" },
            new List<string>() { "CallMoveAnimation", "p1: move ID" },
            new List<string>() { "Cmd4B", "p1" },
            new List<string>() { "Cmd4C", "p1" },
            new List<string>() { "End" },
        };

        public int commandID;
        public int[] parameters;

        public MoveAnimationCommand(int commandID, int[] parameters)
        {
            this.commandID = commandID;
            this.parameters = parameters;
        }

        public MoveAnimationCommand(byte[] byteData, ref int position)
        {
            commandID = HelperFunctions.ReadShort(byteData, position);
            List<string> type = commandDefinitions[commandID];
            parameters = new int[type.Count - 1];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = HelperFunctions.ReadInt(byteData, position + 2 + 4 * i);
            }
            position += 2 + 4 * parameters.Length;
        }

        public List<byte> GetBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((short)commandID));
            foreach (var p in parameters)
                bytes.AddRange(BitConverter.GetBytes(p));
            return bytes;
        }
    }
}