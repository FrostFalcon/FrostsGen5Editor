using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class ScriptNARC : NARC
    {
        public List<ScriptFile> scriptFiles;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            scriptFiles = new List<ScriptFile>();

            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                ScriptFile file = new ScriptFile(bytes);
                scriptFiles.Add(file);

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
            foreach (ScriptFile s in scriptFiles)
            {
                //s.ApplyData();
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += s.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (ScriptFile s in scriptFiles)
            {
                for (int i = 0; i < s.bytes.Length; i++) newByteData.Add((byte)s.bytes[i]);
            }

            byteData = newByteData.ToArray();

            FixHeaders(scriptFiles.Count);

            base.WriteData();
        }

        public override byte[] GetPatchBytes(NARC narc)
        {
            ScriptNARC other = narc as ScriptNARC;
            List<byte> bytes = new List<byte>();

            for (int i = 0; i < numFileEntries; i++)
            {
                bool byteChanges = false;
                if (scriptFiles[i].bytes.Length != other.scriptFiles[i].bytes.Length) byteChanges = true;
                else
                {
                    for (int j = 0; j < scriptFiles[i].bytes.Length; j++)
                        if ((byte)scriptFiles[i].bytes[j] != (byte)other.scriptFiles[i].bytes[j])
                        {
                            byteChanges = true;
                            break;
                        }
                }
                if (i > other.scriptFiles.Count || byteChanges)
                {
                    bytes.AddRange(BitConverter.GetBytes(i));
                    bytes.AddRange(BitConverter.GetBytes(scriptFiles[i].bytes.Length));
                    byte[] file = new byte[scriptFiles[i].bytes.Length];
                    for (int j = 0; j < file.Length; j++) file[j] = scriptFiles[i].bytes[j];
                    bytes.AddRange(file);
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

                if (id > scriptFiles.Count)
                {
                    //Don't accept extra files here
                }
                else scriptFiles[id] = new ScriptFile(new List<byte>(bytes).GetRange(pos, size).ToArray());
                pos += size;
            }
        }
    }

    public class ScriptFile
    {
        public RefByte[] bytes;

        public bool valid = false;

        public List<ScriptSequence> sequences;

        public ScriptFile(byte[] bytes)
        {
            this.bytes = new RefByte[bytes.Length];
            for (int i = 0; i < bytes.Length; i++) this.bytes[i] = bytes[i];

            ReadData();
        }

        private ScriptFile()
        {
            this.bytes = new RefByte[0];
            sequences = new List<ScriptSequence>();
            valid = true;
        }

        public static ScriptFile FromFile(StreamReader stream)
        {
            ScriptFile sf = new ScriptFile();
            var comDict = CommandReference.CommandsByName();
            Dictionary<string, int> conditionals = new Dictionary<string, int>()
            {
                { "==", 1 },
                { "!=", 5 },
                { ">", 2 },
                { ">=", 4 },
                { "<=", 3 },
                { "<", 0 },
            };

            List<ScriptSequence> stack = new List<ScriptSequence>();
            int lineNumber = 0;

            string currentRoutineName = "";
            Dictionary<string, int> routineLocations = new Dictionary<string, int>();
            List<string> routinePointers = new List<string>();

            List<List<RefByte>> movements = new List<List<RefByte>>();
            List<int[]> movementPointers = new List<int[]>();
            bool inMovement = false;

            int lastIfJump = 0;

            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine();
                while (line.StartsWith("\t") || line.StartsWith(" ")) line = line.Substring(1);
                lineNumber++;

                if (line.Length == 0 || line.StartsWith("#") || line.StartsWith("//") || line.StartsWith("{")) continue;

                if (!inMovement)
                {
                    //New Sequence
                    if (line.StartsWith("void"))
                    {
                        stack.Add(new ScriptSequence());
                        string name = line.Substring(line.IndexOf(' ') + 1, line.IndexOf('(') - line.IndexOf(' ') - 1);
                        currentRoutineName = name;
                        continue;
                    }

                    if (stack.Count != 0)
                    {
                        if (line.StartsWith("Movement"))
                        {
                            int npcID = 0;
                            try
                            {
                                string npc = line.Substring(line.LastIndexOf('/') + 1).Trim();
                                npcID = int.Parse(npc);
                            }
                            catch
                            {
                                throw new Exception("Failed to parse movement definition at line " + lineNumber);
                            }
                            ScriptCommand sc = new ScriptCommand(0x64, new int[] { npcID, 0 });
                            movementPointers.Add(sc.parameters);
                            stack[stack.Count - 1].commands.Add(sc);

                            inMovement = true;
                            movements.Add(new List<RefByte>());
                            continue;
                        }

                        if (line.StartsWith("}"))
                        {
                            //End Sequence
                            if (stack.Count == 1)
                            {
                                if (stack[0].commands.Count == 0) stack[0].commands.Add(new ScriptCommand(2, new int[0]));

                                int seqLen = 0;
                                foreach (ScriptCommand c in stack[0].commands) seqLen += c.ByteLength;
                                if (seqLen % 2 == 1)
                                {
                                    seqLen++;
                                    stack[0].miscBytes.Add(0);
                                }

                                int seqPos = 0;
                                int movementID = 0;
                                int movementPos = seqLen;
                                foreach (ScriptCommand c in stack[0].commands)
                                {
                                    if (movementID >= movements.Count) break;
                                    seqPos += c.ByteLength;
                                    if (c.commandID == 0x64)
                                    {
                                        c.parameters[1] = movementPos - seqPos;
                                        movementPos += movements[movementID].Count;
                                        movementID++;
                                    }
                                }

                                while (movements.Count > 0)
                                {
                                    stack[0].miscBytes.AddRange(movements[0]);
                                    movements.RemoveAt(0);
                                }

                                sf.ApplyData(false);
                                int pos = sf.bytes.Length - 4 * sf.sequences.Count - 2;
                                sf.sequences.Add(stack[0]);
                                sf.ApplyData(false);
                                routineLocations.Add(currentRoutineName, pos);
                                stack.RemoveAt(0);
                            }
                            else
                            {
                                int jumpAmount = 0;
                                foreach (ScriptCommand c in stack[stack.Count - 1].commands) jumpAmount += c.ByteLength;
                                stack[stack.Count - 2].commands.Add(new ScriptCommand(0x1E, new int[] { jumpAmount }));
                                lastIfJump = stack[stack.Count - 2].commands.Count - 1;
                                stack[stack.Count - 2].commands.AddRange(stack[stack.Count - 1].commands);
                                stack.RemoveAt(stack.Count - 1);
                            }
                            continue;
                        }

                        if (line.StartsWith("else"))
                        {
                            if (lastIfJump != 0) stack[stack.Count - 1].commands[lastIfJump].parameters[0] += 6;
                            stack.Add(new ScriptSequence());
                            continue;
                        }

                        string com = "";
                        List<int> pars = new List<int>();
                        try
                        {
                            com = line.Substring(0, line.IndexOf('('));
                            string[] ps = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1).Split(',');
                            if (ps.Length == 1 && ps[0] == "") ps = new string[0];

                            if (line.StartsWith("if"))
                            {
                                ps = ps[0].Split(' ');
                                if (ps.Length == 1)
                                {
                                    pars.Add(int.Parse(ps[0]));
                                }
                                else
                                {
                                    for (int i = 0; i < ps.Length; i++)
                                    {
                                        ps[i] = ps[i].Trim();
                                        if (ps[i].StartsWith("0x")) pars.Add(int.Parse(ps[i].Substring(2), System.Globalization.NumberStyles.HexNumber));
                                        else if (conditionals.ContainsKey(ps[i])) pars.Add(conditionals[ps[i]]);
                                        else pars.Add(int.Parse(ps[i]));
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < ps.Length; i++)
                                {
                                    ps[i] = ps[i].Trim();
                                    if (ps[i].StartsWith("0x")) pars.Add(int.Parse(ps[i].Substring(2), System.Globalization.NumberStyles.HexNumber));
                                    else pars.Add(int.Parse(ps[i]));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to parse command at line " + lineNumber);
                        }

                        if (com.StartsWith("if"))
                        {
                            if (pars.Count == 1)
                            {
                                ScriptCommand sc = new ScriptCommand(0x1F, new int[] { pars[0], 6 });
                                stack[stack.Count - 1].commands.Add(sc);
                            }
                            else if (pars.Count == 3)
                            {
                                ScriptCommand sc = new ScriptCommand(0x19, new int[] { pars[0], pars[2] });
                                stack[stack.Count - 1].commands.Add(sc);
                                sc = new ScriptCommand(0x1F, new int[] { pars[1], 6 });
                                stack[stack.Count - 1].commands.Add(sc);
                            }
                            else
                            {
                                throw new Exception("Invalid conditional at line " + lineNumber);
                            }
                            stack.Add(new ScriptSequence());
                            continue;
                        }

                        if (comDict.ContainsKey(com))
                        {
                            ScriptCommand sc = new ScriptCommand((short)comDict[com], pars.ToArray());
                            stack[stack.Count - 1].commands.Add(sc);
                            if (sc.commandID == 0x4) routinePointers.Add("");
                            continue;
                        }
                        else
                        {
                            ScriptCommand sc = new ScriptCommand(0x4, new int[] { 0 });
                            routinePointers.Add(com);
                            stack[stack.Count - 1].commands.Add(sc);
                            continue;
                        }
                    }
                }
                else
                {
                    if (line.StartsWith("}"))
                    {
                        List<RefByte> m = movements[movements.Count - 1];
                        if (m[m.Count - 4] != 0xFE) m.AddRange(new RefByte[] { 0xFE, 0, 0, 0 });
                        inMovement = false;
                        continue;
                    }

                    try
                    {
                        List<string> ps = new List<string>(line.Split(','));
                        while (ps[ps.Count - 1].Length == 0) ps.RemoveAt(ps.Count - 1);
                        if (ps.Count == 1 && ps[0] == "") continue;
                        byte[] bytes = new byte[ps.Count];
                        for (int i = 0; i < ps.Count; i++)
                        {
                            if (ps[i].Trim().StartsWith("0x")) bytes[i] = byte.Parse(ps[i].Trim().Substring(2), System.Globalization.NumberStyles.HexNumber);
                            else bytes[i] = byte.Parse(ps[i].Trim());
                        }

                        if (ps.Count == 1) movements[movements.Count - 1].AddRange(new RefByte[] { bytes[0], 0, 0, 0 });
                        else
                        {
                            movements[movements.Count - 1].AddRange(new RefByte[] { bytes[0], 0, bytes[1], 0 });
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to parse movement command at line " + lineNumber);
                    }
                }
            }

            //Assign routines
            sf.ApplyData();
            for (int j = 0; j < sf.sequences.Count; j++)
            {
                int jumpTo = 0;
                int n = HelperFunctions.ReadInt(sf.bytes, j * 4);
                n = j * 4 + n + 4;
                int start = n;
                for (int i = start; i < sf.bytes.Length && routinePointers.Count != 0; i++)
                {
                    ScriptCommand sc = new ScriptCommand(sf.bytes, i);
                    if (sc.commandID == 0x4)
                    {
                        int jump = -i - 6;
                        if (routineLocations.ContainsKey(routinePointers[0])) jump += routineLocations[routinePointers[0]] + sf.sequences.Count * 4 + 2;
                        else jump = 0;
                        if (jump != 0) HelperFunctions.WriteInt(sf.bytes, i + 2, jump);
                        routinePointers.RemoveAt(0);
                    }
                    if (sc.commandID == 0x1E) jumpTo = i + 6 + sc.parameters[0];
                    if (sc.commandID == 0x1F) jumpTo = i + 7 + sc.parameters[1];
                    i += sc.ByteLength - 1;
                    if (sc.commandID == 2 && i >= jumpTo) break;
                }
            }
            sf.ReadData();

            return sf;
        }

        public void Export(FileStream file)
        {
            file.WriteLine("#include \"ScriptCommands.h\"");
            file.WriteLine("#include \"MovementCommands.h\"");

            int seqID = 0;

            List<int> routines = new List<int>();

            foreach (ScriptSequence seq in sequences)
            {
                file.WriteLine("\nvoid Sequence" + seqID + "()\n{");

                foreach (ScriptCommand com in seq.commands)
                {
                    int n = HelperFunctions.ReadInt(bytes, seqID * 4);
                    for (int i = 0; i <= seq.commands.IndexOf(com); i++) n += seq.commands[i].ByteLength;
                    n = (int)seqID * 4 + n + 4;
                    WriteCommandToFile(file, n, com, routines);
                }

                file.WriteLine("}");
                seqID++;
            }

            int routineID = 0;

            while (routineID < routines.Count)
            {
                file.WriteLine("\nvoid Routine" + routineID + "()\n{");
                int pos = routines[routineID];
                int jumpMax = 0;
                while (pos < bytes.Length)
                {
                    ScriptCommand com = new ScriptCommand(bytes, pos);
                    if (com.commandID == 0x1F) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[1]);
                    if (com.commandID == 0x1E) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[0]);
                    WriteCommandToFile(file, pos + com.ByteLength, com, routines);
                    if ((com.commandID == 0x2 || com.commandID == 0x5) && pos >= jumpMax) break;
                    pos += com.ByteLength;
                }
                file.WriteLine("}");
                routineID++;
            }
        }

        void WriteCommandToFile(FileStream file, int pos, ScriptCommand com, List<int> routines)
        {
            //Handle movements
            if (com.commandID == 0x64)
            {
                file.WriteLine("\n\tMovement m[] = { //" + com.parameters[0]);
                int n = pos;
                n += com.parameters[1];

                for (int i = n; bytes[i] != 0xFE && i < bytes.Length - 4; i += 4)
                {
                    file.WriteLine("\t\t0x" + ((byte)bytes[i]).ToString("X2") + ", " + ((byte)bytes[i + 2]) + ",");
                }
                file.WriteLine("\t};\n");
            }

            //Handle Routines
            else if (com.commandID == 0x4)
            {
                int n = pos;
                n += com.parameters[0];
                if (routines.Contains(n)) file.WriteLine("\tRoutine" + routines.IndexOf(n) + "();");
                else
                {
                    file.WriteLine("\tRoutine" + routines.Count + "();");
                    routines.Add(n);
                }
            }

            else file.WriteLine("\t" + com.CString());
        }

        public void ReadData()
        {
            int pointerPos = 0;

            while (pointerPos < bytes.Length - 1)
            {
                if (HelperFunctions.ReadShort(bytes, pointerPos) == 0xFD13)
                {
                    valid = true;
                    break;
                }
                pointerPos++;
            }

            pointerPos = 0;

            if (valid)
            {
                sequences = new List<ScriptSequence>();

                List<int> pointers = new List<int>();
                while (pointerPos < bytes.Length - 1 && HelperFunctions.ReadShort(bytes, pointerPos) != 0xFD13)
                {
                    pointers.Add(pointerPos + HelperFunctions.ReadInt(bytes, pointerPos) + 4);
                    pointerPos += 4;
                }
                List<int> sortedPointers = new List<int>(pointers);
                sortedPointers.Sort((n1, n2) => n1 - n2);


                foreach (int p in pointers)
                {
                    int pos = p;
                    int end = pos;
                    if (sortedPointers.IndexOf(p) < sortedPointers.Count - 1) end = sortedPointers[sortedPointers.IndexOf(p) + 1];
                    else end = bytes.Length;

                    int jumpPos = 0;
                    bool routineOverride = false;

                    ScriptSequence sequence = new ScriptSequence();
                    while ((pos < bytes.Length - 1 && HelperFunctions.ReadShort(bytes, pos) != 0x0002) || pos < jumpPos)
                    {
                        ScriptCommand s = new ScriptCommand(bytes, pos);
                        sequence.commands.Add(s);
                        if (s.ByteLength == -1) break;
                        if (s.ByteLength == -1) break;
                        pos += s.ByteLength;

                        if (s.commandID == 0x4)
                        {
                            //jumpPos = pos + 6 + s.parameters[0];
                            //routineOverride = true;
                        }
                        if (s.commandID == 0x12 && !routineOverride) jumpPos = pos + 8;
                        if (s.commandID == 0x13 && !routineOverride) jumpPos = pos + 8;
                        if (s.commandID == 0x1E && !routineOverride) jumpPos = pos + s.parameters[0];
                        if (s.commandID == 0x1F && !routineOverride) jumpPos = pos + s.parameters[1];
                    }
                    sequence.commands.Add(new ScriptCommand(new RefByte[] { 02, 00 }, 0));

                    for (int i = pos + 2; i < end; i++) sequence.miscBytes.Add(bytes[i]);

                    sequences.Add(sequence);

                    pointerPos += 4;
                }
            }
        }

        public void ApplyData(bool padEnd = true)
        {
            int pointerPos = 0;

            if (valid)
            {
                List<RefByte> newBytes = new List<RefByte>();

                List<RefByte> movementPointers = new List<RefByte>();
                List<RefByte> movementPositions = new List<RefByte>();

                for (int i = 0; i < sequences.Count; i++) newBytes.AddRange(new RefByte[] { 0, 0, 0, 0 });
                newBytes.Add(0x13);
                newBytes.Add(0xFD);

                foreach (ScriptSequence seq in sequences)
                {
                    newBytes.RemoveRange(pointerPos, 4);
                    byte[] point = BitConverter.GetBytes(newBytes.Count - pointerPos);
                    for (int i = 0; i < 4; i++) newBytes.Insert(pointerPos + i, point[i]);

                    foreach (ScriptCommand c in seq.commands)
                    {
                        byte[] num = BitConverter.GetBytes(c.commandID);
                        for (int i = 0; i < 2; i++) newBytes.Add(num[i]);

                        for (int p = 0; p < c.parameters.Length; p++)
                        {
                            if (CommandReference.commandList.ContainsKey(c.commandID))
                            {
                                if (CommandReference.commandList[c.commandID].parameterBytes[p] == 1) newBytes.Add((byte)c.parameters[p]);
                                if (CommandReference.commandList[c.commandID].parameterBytes[p] == 2)
                                {
                                    num = BitConverter.GetBytes(c.parameters[p]);
                                    for (int i = 0; i < 2; i++) newBytes.Add(num[i]);
                                }
                                if (CommandReference.commandList[c.commandID].parameterBytes[p] == 4)
                                {
                                    num = BitConverter.GetBytes(c.parameters[p]);
                                    for (int i = 0; i < 4; i++) newBytes.Add(num[i]);
                                }
                            }
                            else
                            {
                                num = BitConverter.GetBytes(c.parameters[p]);
                                for (int i = 0; i < 2; i++) newBytes.Add(num[i]);
                            }
                        }

                        if (c.commandID == 0x64)
                        {
                            //movementPointers.Add(newBytes[newBytes.Count - 4]);
                            //movementPositions.Add(c.movementPosition);
                        }
                    }

                    foreach (RefByte b in seq.miscBytes) newBytes.Add(b);

                    pointerPos += 4;
                }

                for (int i = 0; i < movementPointers.Count; i++)
                {
                    //int pos1 = newBytes.IndexOf(movementPointers[i]);
                    //int pos2 = newBytes.IndexOf(movementPositions[i]);
                    //
                    //int val = pos2 - (pos1 + 4);
                    //
                    //byte[] num = BitConverter.GetBytes(val);
                    //for (int j = 0; j < 4; j++) newBytes[pos1 + j] = num[j];

                    //Debug.WriteLine(pos1 + " | " + pos2);
                }

                //Debug.WriteLine(movements.Count);

                while (padEnd && newBytes.Count % 4 != 0) newBytes.Add(0);

                //if (!bytes.SequenceEqual(newBytes.ToArray())) System.Diagnostics.Debug.WriteLine(bytes.Length + " | " + newBytes.Count);

                bytes = new RefByte[newBytes.Count];
                for (int i = 0; i < newBytes.Count; i++) bytes[i] = newBytes[i];

                ReadData();
            }
        }
    }

    public class ScriptSequence
    {
        public List<ScriptCommand> commands;
        public List<RefByte> miscBytes;

        public ScriptSequence()
        {
            commands = new List<ScriptCommand>();
            miscBytes = new List<RefByte>();
        }
    }

    public struct ScriptCommand
    {
        public short commandID;
        public int[] parameters;
        public RefByte movementPosition;

        public int ByteLength
        {
            get
            {
                if (!CommandReference.commandList.ContainsKey(commandID))
                {
                    if (commandID >= 0x3E8 && commandID <= 0x3FF) return 2 + parameters.Length * 2;
                    return 2;
                }
                int n = 2;
                for (int i = 0; i < CommandReference.commandList[commandID].parameterBytes.Count; i++) n += CommandReference.commandList[commandID].parameterBytes[i];
                return n;
            }
        }

        public ScriptCommand(RefByte[] bytes, int readOffset)
        {
            movementPosition = 0;
            commandID = (short)HelperFunctions.ReadShort(bytes, readOffset);

            if (CommandReference.commandList.ContainsKey(commandID))
            {
                int pos = readOffset + 2;
                parameters = new int[CommandReference.commandList[commandID].numParameters];
                for (int i = 0; i < CommandReference.commandList[commandID].parameterBytes.Count; i++)
                {
                    if (CommandReference.commandList[commandID].parameterBytes[i] == 1) parameters[i] = (byte)bytes[pos];
                    if (CommandReference.commandList[commandID].parameterBytes[i] == 2) parameters[i] = HelperFunctions.ReadShort(bytes, pos);
                    if (CommandReference.commandList[commandID].parameterBytes[i] == 4) parameters[i] = HelperFunctions.ReadInt(bytes, pos);
                    pos += CommandReference.commandList[commandID].parameterBytes[i];
                }

                if (commandID == 0x64)
                {
                    //if (parameters[1] + pos < bytes.Length && parameters[1] + pos >= 0) movementPosition = bytes[pos + parameters[1]];
                }
            }
            else if (commandID >= 0x3E8 && commandID <= 0x3FF)
            {
                int pos = readOffset + 2;
                int next = HelperFunctions.ReadShort(bytes, pos);
                pos += 2;
                List<int> p = new List<int>();

                if (commandID <= 0x3EC || commandID == 0x3F5)
                {
                    while (next < 2) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                    while (next <= 0xA && next > 1) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                    while (next >= 0x4000) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                }
                else if (commandID <= 0x3F4)
                {
                    while (next <= 40) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                    while (next >= 0x4000) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                }
                else if (commandID <= 0x3FB)
                {
                    while (next <= 40) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                    while (next <= 0xA && next > 1) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                    while (next >= 0x4000) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                }
                else if (commandID == 0x3FF)
                {
                    p.Add(next);
                    next = HelperFunctions.ReadShort(bytes, pos);
                    pos += 2;
                    while (next >= 0x4000) { p.Add(next); next = HelperFunctions.ReadShort(bytes, pos); pos += 2; }
                }

                parameters = p.ToArray();
            }
            else parameters = new int[0];
        }

        public ScriptCommand(short commandID, int[] parameters)
        {
            movementPosition = 0;
            this.commandID = commandID;
            this.parameters = parameters;
        }

        public override string ToString()
        {
            if (!CommandReference.commandList.ContainsKey(commandID)) return "Error";
            CommandType cmd = CommandReference.commandList[commandID];
            StringBuilder str = new StringBuilder(cmd.name);
            str.Append("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0) str.Append(", ");
                str.Append(parameters[i] >= 0x4000 ? "0x" + parameters[i].ToString("X") : parameters[i].ToString());
            }
            str.Append(") - " + ByteLength + " bytes");
            return str.ToString();
        }

        public string CString()
        {
            if (!CommandReference.commandList.ContainsKey(commandID)) return "Error";
            CommandType cmd = CommandReference.commandList[commandID];
            StringBuilder str = new StringBuilder(cmd.name);
            str.Append("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0) str.Append(", ");
                str.Append(parameters[i] >= 0x4000 ? "0x" + parameters[i].ToString("X") : parameters[i].ToString());
            }
            str.Append(");");
            return str.ToString();
        }
    }

    internal static class CommandReference
    {
        internal static Dictionary<int, CommandType> commandList = new Dictionary<int, CommandType>()
        {
            {0x0, new CommandType("c0x0", 0)},
            {0x1, new CommandType("c0x1", 0)},
            {0x2, new CommandType("End", 0)},
            {0x3, new CommandType("ReturnAfterDelay", 1, 2)},
            {0x4, new CommandType("CallRoutine", 1, 4)},
            {0x5, new CommandType("EndFunction", 0)},
            {0x6, new CommandType("Logic06", 1, 2)},
            {0x7, new CommandType("Logic07", 1, 2)},
            {0x8, new CommandType("CompareTo", 1, 2)},
            {0x9, new CommandType("StoreVar", 1, 2)},
            {0xA, new CommandType("ClearVar", 1, 2)},
            {0xB, new CommandType("c0x0B", 1, 2)},
            {0xC, new CommandType("c0x0C", 1, 2)},
            {0xD, new CommandType("c0x0D", 1, 2)},
            {0xE, new CommandType("c0x0E", 1, 2)},
            {0xF, new CommandType("c0x0F", 1, 2)},
            {0x10, new CommandType("StoreFlag", 1, 2)},
            {0x11, new CommandType("Condition", 1, 2)},
            {0x12, new CommandType("c0x12", 2, 2, 2)},
            {0x13, new CommandType("c0x13", 2, 2, 2)},
            {0x14, new CommandType("c0x14", 1, 2)},
            {0x15, new CommandType("c0x15", 0)},
            {0x16, new CommandType("c0x16", 1, 2)},
            {0x17, new CommandType("c0x17", 1, 2)},
            {0x18, new CommandType("c0x18", 0)},
            {0x19, new CommandType("Compare", 2, 2, 2)},
            {0x1A, new CommandType("c0x1A", 0)},
            {0x1B, new CommandType("c0x1B", 0)},
            {0x1C, new CommandType("CallStd", 1, 2)},
            {0x1D, new CommandType("ReturnStd", 0)},
            {0x1E, new CommandType("Jump", 1, 4)},
            {0x1F, new CommandType("If", 2, 1, 4)},
            {0x20, new CommandType("c0x20", 0)},
            {0x21, new CommandType("c0x21", 1, 2)},
            {0x22, new CommandType("c0x22", 1, 2)},
            {0x23, new CommandType("SetFlag", 1, 2)},
            {0x24, new CommandType("ClearFlag", 1, 2)},
            {0x25, new CommandType("SetVarFlagStatus", 2, 2, 2)},
            {0x26, new CommandType("AddToVar", 2, 2, 2)},
            {0x27, new CommandType("SubtractVar", 2, 2, 2)},
            {0x28, new CommandType("SetVarEqVal", 2, 2, 2)},
            {0x29, new CommandType("SetVar29", 2, 2, 2)},
            {0x2A, new CommandType("SetVar2A", 2, 2, 2)},
            {0x2B, new CommandType("SetVar2B", 1, 2)},
            {0x2C, new CommandType("c0x2C", 0)},
            {0x2D, new CommandType("c0x2D", 1, 2)},
            {0x2E, new CommandType("LockAll", 0)},
            {0x2F, new CommandType("UnlockAll", 0)},
            {0x30, new CommandType("WaitMoment", 0)},
            {0x31, new CommandType("c0x31", 0)},
            {0x32, new CommandType("WaitButton", 0)},
            {0x33, new CommandType("MusicalMessage", 1, 2)},
            {0x34, new CommandType("EventGreyMessage", 2, 2, 2)},
            {0x35, new CommandType("CloseMusicalMessage", 0)},
            {0x36, new CommandType("CloseEventGreyMessage", 0)},
            {0x37, new CommandType("c0x37", 0)},
            {0x38, new CommandType("BubbleMessage", 2, 2, 1)},
            {0x39, new CommandType("CloseBubbleMessage", 0)},
            {0x3A, new CommandType("ShowMessageAt", 4, 2, 2, 2, 2)},
            {0x3B, new CommandType("CloseShowMessageAt", 1, 2)},
            {0x3C, new CommandType("Message", 6, 1, 1, 2, 2, 2, 2)},
            {0x3D, new CommandType("Message2", 5, 1, 1, 2, 2, 2)},
            {0x3E, new CommandType("CloseMessageKeyPress", 0)},
            {0x3F, new CommandType("CloseMessageKeyPress2", 0)},
            {0x40, new CommandType("MoneyBox", 2, 2, 2)},
            {0x41, new CommandType("CloseMoneyBox", 0)},
            {0x42, new CommandType("UpdateMoneyBox", 0)},
            {0x43, new CommandType("BorderedMessage", 2, 2, 2)},
            {0x44, new CommandType("CloseBorderedMessage", 0)},
            {0x45, new CommandType("PaperMessage", 2, 2, 2)},
            {0x46, new CommandType("ClosePaperMessage", 0)},
            {0x47, new CommandType("YesNoBox", 1, 2)},
            {0x48, new CommandType("Message3", 7, 1, 1, 2, 2, 2, 2, 2)},
            {0x49, new CommandType("DoubleMessage", 7, 1, 1, 2, 2, 2, 2, 2)},
            {0x4A, new CommandType("AngryMessage", 3, 2, 1, 2)},
            {0x4B, new CommandType("CloseAngryMessage", 0)},
            {0x4C, new CommandType("SetVarHero", 1, 1)},
            {0x4D, new CommandType("SetVarItem", 2, 1, 2)},
            {0x4E, new CommandType("c0x4E", 4, 1, 2, 2, 1)},
            {0x4F, new CommandType("SetVarItem2", 2, 1, 2)},
            {0x50, new CommandType("SetVarItem3", 2, 1, 2)},
            {0x51, new CommandType("SetVarMove", 2, 1, 2)},
            {0x52, new CommandType("SetVarBag", 2, 1, 2)},
            {0x53, new CommandType("SetVarPartyPokemon", 2, 1, 2)},
            {0x54, new CommandType("SetVarPartyPokemon2", 2, 1, 2)},
            {0x55, new CommandType("SetVar55", 2, 1, 2)},
            {0x56, new CommandType("SetVarType", 2, 1, 2)},
            {0x57, new CommandType("SetVarPokemon", 2, 1, 2)},
            {0x58, new CommandType("SetVarPokemon2", 2, 1, 2)},
            {0x59, new CommandType("SetVarLocation", 2, 1, 2)},
            {0x5A, new CommandType("SetVarPokemonNick", 2, 1, 2)},
            {0x5B, new CommandType("SetVar5B", 2, 1, 2)},
            {0x5C, new CommandType("SetVarStoreVal5C", 3, 1, 2, 2)},
            {0x5D, new CommandType("SetVarMusicalInfo", 2, 2, 2)},
            {0x5E, new CommandType("SetVarNations", 2, 1, 2)},
            {0x5F, new CommandType("SetVarActivities", 2, 1, 2)},
            {0x60, new CommandType("SetVarPower", 2, 1, 2)},
            {0x61, new CommandType("SetVarTrainerType", 2, 1, 2)},
            {0x62, new CommandType("SetVarTrainerType2", 2, 1, 2)},
            {0x63, new CommandType("SetVarGeneralWord", 2, 1, 2)},
            {0x64, new CommandType("ApplyMovement", 2, 2, 4)},
            {0x65, new CommandType("WaitMovement", 0)},
            {0x66, new CommandType("StoreHeroPosition_c0x66", 2, 2, 2)},
            {0x67, new CommandType("c0x67", 1, 2)},
            {0x68, new CommandType("StoreHeroPosition", 2, 2, 2)},
            {0x69, new CommandType("CreateNPC", 6, 2, 2, 2, 2, 2, 2)},
            {0x6A, new CommandType("c0x6A", 2, 2, 2)},
            {0x6B, new CommandType("AddNPC", 1, 2)},
            {0x6C, new CommandType("RemoveNPC", 1, 2)},
            {0x6D, new CommandType("SetOWPosition", 5, 2, 2, 2, 2, 2)},
            {0x6E, new CommandType("c0x6E", 1, 2)},
            {0x6F, new CommandType("c0x6F", 1, 2)},
            {0x70, new CommandType("c0x70", 5, 2, 2, 2, 2, 2)},
            {0x71, new CommandType("c0x71", 3, 2, 2, 2)},
            {0x72, new CommandType("c0x72", 4, 2, 2, 2, 2)},
            {0x73, new CommandType("c0x73", 2, 2, 2)},
            {0x74, new CommandType("FacePlayer", 0)},
            {0x75, new CommandType("Release", 1, 2)},
            {0x76, new CommandType("ReleaseAll", 0)},
            {0x77, new CommandType("Lock", 1, 2)},
            {0x78, new CommandType("c0x78", 1, 2)},
            {0x79, new CommandType("c0x79", 3, 2, 2, 2)},
            {0x7A, new CommandType("c0x7A", 0)},
            {0x7B, new CommandType("MoveNpctoCoordinates", 4, 2, 2, 2, 2)},
            {0x7C, new CommandType("c0x7C", 4, 2, 2, 2, 2)},
            {0x7D, new CommandType("c0x7D", 4, 2, 2, 2, 2)},
            {0x7E, new CommandType("TeleportUpNPc", 1, 2)},
            {0x7F, new CommandType("c0x7F", 2, 2, 2)},
            {0x80, new CommandType("c0x80", 1, 2)},
            {0x81, new CommandType("c0x81", 0)},
            {0x82, new CommandType("c0x82", 2, 2, 2)},
            {0x83, new CommandType("SetVarc0x83", 1, 2)},
            {0x84, new CommandType("SetVarc0x84", 1, 2)},
            {0x85, new CommandType("SingleTrainerBattle", 3, 2, 2, 2)},
            {0x86, new CommandType("DoubleTrainerBattle", 4, 2, 2, 2, 2)},
            {0x87, new CommandType("c0x87", 3, 2, 2, 2)},
            {0x88, new CommandType("c0x88", 3, 2, 2, 2)},
            {0x89, new CommandType("c0x89", 0)},
            {0x8A, new CommandType("c0x8A", 2, 2, 2)},
            {0x8B, new CommandType("PlayTrainerMusic", 1, 2)},
            {0x8C, new CommandType("EndBattle", 0)},
            {0x8D, new CommandType("StoreBattleResult", 1, 2)},
            {0x8E, new CommandType("DisableTrainer", 0)},
            {0x8F, new CommandType("c0x8F", 0)},
            {0x90, new CommandType("dvar90", 2, 2, 2)},
            {0x91, new CommandType("c0x91", 0)},
            {0x92, new CommandType("dvar92", 2, 2, 2)},
            {0x93, new CommandType("dvar93", 2, 2, 2)},
            {0x94, new CommandType("TrainerBattle", 4, 2, 2, 2, 2)},
            {0x95, new CommandType("DeactiveTrainerId", 1, 2)},
            {0x96, new CommandType("c0x96", 1, 2)},
            {0x97, new CommandType("StoreActiveTrainerId", 2, 2, 2)},
            {0x98, new CommandType("ChangeMusic", 1, 2)},
            {0x99, new CommandType("c0x99", 0)},
            {0x9A, new CommandType("c0x9A", 0)},
            {0x9B, new CommandType("c0x9B", 0)},
            {0x9C, new CommandType("c0x9C", 0)},
            {0x9D, new CommandType("c0x9D", 0)},
            {0x9E, new CommandType("FadeToDefaultMusic", 0)},
            {0x9F, new CommandType("c0x9F", 1, 2)},
            {0xA0, new CommandType("c0xA0", 0)},
            {0xA1, new CommandType("c0xA1", 0)},
            {0xA2, new CommandType("c0xA2", 2, 2, 2)},
            {0xA3, new CommandType("c0xA3", 1, 2)},
            {0xA4, new CommandType("c0xA4", 1, 2)},
            {0xA5, new CommandType("c0xA5", 2, 2, 2)},
            {0xA6, new CommandType("PlaySound", 1, 2)},
            {0xA7, new CommandType("WaitSoundA7", 0)},
            {0xA8, new CommandType("WaitSound", 0)},
            {0xA9, new CommandType("PlayFanfare", 1, 2)},
            {0xAA, new CommandType("WaitFanfare", 0)},
            {0xAB, new CommandType("Cry", 2, 2, 2)},
            {0xAC, new CommandType("WaitCry", 0)},
            {0xAD, new CommandType("c0xAD", 0)},
            {0xAE, new CommandType("c0xAE", 0)},
            {0xAF, new CommandType("SetTextScriptMessage", 3, 2, 2, 2)},
            {0xB0, new CommandType("CloseMulti", 0)},
            {0xB1, new CommandType("c0xB1", 0)},
            {0xB2, new CommandType("Multi2", 6, 1, 1, 1, 1, 1, 2)},
            {0xB3, new CommandType("FadeScreen", 4, 2, 2, 2, 2)},
            {0xB4, new CommandType("ResetScreen", 3, 2, 2, 2)},
            {0xB5, new CommandType("Screenc0xB5", 3, 2, 2, 2)},
            {0xB6, new CommandType("TakeItem", 3, 2, 2, 2)},
            {0xB7, new CommandType("CheckItemBagSpace", 3, 2, 2, 2)},
            {0xB8, new CommandType("CheckItemBagNumber", 3, 2, 2, 2)},
            {0xB9, new CommandType("StoreItemCount", 2, 2, 2)},
            {0xBA, new CommandType("c0xBA", 4, 2, 2, 2, 2)},
            {0xBB, new CommandType("c0xBB", 2, 2, 2)},
            {0xBC, new CommandType("c0xBC", 1, 2)},
            {0xBD, new CommandType("c0xBD", 0)},
            {0xBE, new CommandType("Warp", 3, 2, 2, 2)},
            {0xBF, new CommandType("TeleportWarp", 4, 2, 2, 2, 2)},
            {0xC0, new CommandType("c0xC0", 0)},
            {0xC1, new CommandType("FallWarp", 3, 2, 2, 2)},
            {0xC2, new CommandType("FastWarp", 4, 2, 2, 2, 2)},
            {0xC3, new CommandType("UnionWarp", 0)},
            {0xC4, new CommandType("TeleportWarp2", 5, 2, 2, 2, 2, 2)},
            {0xC5, new CommandType("SurfAnimation", 0)},
            {0xC6, new CommandType("SpecialAnimation", 1, 2)},
            {0xC7, new CommandType("SpecialAnimation2", 2, 2, 2)},
            {0xC8, new CommandType("CallAnimation", 2, 2, 2)},
            {0xC9, new CommandType("c0xC9", 0)},
            {0xCA, new CommandType("c0xCA", 0)},
            {0xCB, new CommandType("StoreRandomNumber", 2, 2, 2)},
            {0xCC, new CommandType("StoreVarItem", 1, 2)},
            {0xCD, new CommandType("StoreVarc0xCD", 1, 2)},
            {0xCE, new CommandType("StoreVarc0xCE", 1, 2)},
            {0xCF, new CommandType("StoreVarc0xCF", 1, 2)},
            {0xD0, new CommandType("StoreDate", 2, 2, 2)},
            {0xD1, new CommandType("Storec0xD1", 2, 2, 2)},
            {0xD2, new CommandType("Storec0xD2", 1, 2)},
            {0xD3, new CommandType("Storec0xD3", 1, 2)},
            {0xD4, new CommandType("StoreBirthDay", 2, 2, 2)},
            {0xD5, new CommandType("StoreBadge", 2, 2, 2)},
            {0xD6, new CommandType("SetBadge", 1, 2)},
            {0xD7, new CommandType("StoreBadgeNumber", 1, 2)},
            {0xD8, new CommandType("c0xD8", 0)},
            {0xD9, new CommandType("SetRespawnZone", 1, 2)},
            {0xDA, new CommandType("c0xDA", 3, 2, 2, 2)},
            {0xDB, new CommandType("c0xDB", 0)},
            {0xDC, new CommandType("c0xDC", 0)},
            {0xDD, new CommandType("c0xDD", 2, 2, 2)},
            {0xDE, new CommandType("SpeciesDisplayDE", 2, 2, 2)},
            {0xDF, new CommandType("c0xDF", 0)},
            {0xE0, new CommandType("StoreVersion", 1, 2)},
            {0xE1, new CommandType("StoreHeroGender", 1, 2)},
            {0xE2, new CommandType("c0xE2", 0)},
            {0xE3, new CommandType("c0xE3", 0)},
            {0xE4, new CommandType("c0xE4", 1, 2)},
            {0xE5, new CommandType("StoreE5", 1, 2)},
            {0xE6, new CommandType("c0xE6", 0)},
            {0xE7, new CommandType("ActivateRelocator", 1, 2)},
            {0xE8, new CommandType("c0xE8", 0)},
            {0xE9, new CommandType("c0xE9", 0)},
            {0xEA, new CommandType("StoreEA", 1, 2)},
            {0xEB, new CommandType("StoreEB", 1, 2)},
            {0xEC, new CommandType("StoreEC", 0)},
            {0xED, new CommandType("StoreED", 0)},
            {0xEE, new CommandType("StoreEE", 1, 2)},
            {0xEF, new CommandType("StoreEF", 1, 2)},
            {0xF0, new CommandType("c0xF0", 3, 2, 2, 2)},
            {0xF1, new CommandType("StoreF1", 1, 2)},
            {0xF2, new CommandType("c0xF2", 2, 2, 2)},
            {0xF3, new CommandType("c0xF3", 2, 2, 2)},
            {0xF4, new CommandType("c0xF4", 2, 2, 2)},
            {0xF5, new CommandType("c0xF5", 2, 2, 2)},
            {0xF6, new CommandType("c0xF6", 2, 2, 2)},
            {0xF7, new CommandType("RequestPokemonForDayCare", 1, 2)},
            {0xF8, new CommandType("c0xF8", 2, 2, 2)},
            {0xF9, new CommandType("c0xF9", 1, 2)},
            {0xFA, new CommandType("TakeMoney", 1, 2)},
            {0xFB, new CommandType("CheckMoney", 2, 2, 2)},
            {0xFC, new CommandType("StorePartyHappiness", 2, 2, 2)},
            {0xFD, new CommandType("c0xFD", 3, 2, 2, 2)},
            {0xFE, new CommandType("StorePartySpecies", 2, 2, 2)},
            {0xFF, new CommandType("c0xFF", 2, 2, 2)},
            {0x100, new CommandType("c0x100", 0)},
            {0x101, new CommandType("c0x101", 2, 2, 2)},
            {0x102, new CommandType("StorePartyNotEgg", 2, 2, 2)},
            {0x103, new CommandType("StorePartyCountMore", 2, 2, 2)},
            {0x104, new CommandType("HealPokemon", 0)},
            {0x105, new CommandType("c0x105", 3, 2, 2, 2)},
            {0x106, new CommandType("c0x106", 1, 2)},
            {0x107, new CommandType("OpenChoosePokemonMenu", 4, 2, 2, 2, 2)},
            {0x108, new CommandType("c0x108", 2, 2, 2)},
            {0x109, new CommandType("c0x109", 4, 2, 2, 2, 2)},
            {0x10A, new CommandType("c0x10A", 3, 2, 2, 2)},
            {0x10B, new CommandType("c0x10B", 3, 2, 2, 2)},
            {0x10C, new CommandType("GivePokemon", 4, 2, 2, 2, 2)},
            {0x10D, new CommandType("StorePokemonPartyAt", 2, 2, 2)},
            {0x10E, new CommandType("GivePokemon2", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x10F, new CommandType("GiveEgg", 3, 2, 2, 2)},
            {0x110, new CommandType("StorePokemonSex", 3, 2, 2, 2)},
            {0x111, new CommandType("SetPokemonIV", 3, 2, 2, 2)},
            {0x112, new CommandType("c0x112", 0)},
            {0x113, new CommandType("c0x113", 2, 2, 2)},
            {0x114, new CommandType("c0x114", 2, 2, 2)},
            {0x115, new CommandType("StorePartyCanLearnMove", 3, 2, 2, 2)},
            {0x116, new CommandType("SetVarPartyHasMove", 2, 2, 2)},
            {0x117, new CommandType("VarValDualCompare117", 4, 2, 2, 2, 2)},
            {0x118, new CommandType("c0x118", 3, 2, 2, 2)},
            {0x119, new CommandType("c0x119", 0)},
            {0x11A, new CommandType("c0x11A", 4, 2, 2, 2, 2)},
            {0x11B, new CommandType("StorePartyType", 3, 2, 2, 2)},
            {0x11C, new CommandType("c0x11C", 3, 2, 2, 2)},
            {0x11D, new CommandType("SetFavorite", 1, 2)},
            {0x11E, new CommandType("c0x11E", 1, 2)},
            {0x11F, new CommandType("c0x11F", 3, 2, 1, 2)},
            {0x120, new CommandType("c0x120", 2, 2, 2)},
            {0x121, new CommandType("c0x121", 1, 2)},
            {0x122, new CommandType("AddBoxPokemon", 4, 2, 2, 2, 2)},
            {0x123, new CommandType("AddBoxPokemon2", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x124, new CommandType("c0x124", 0)},
            {0x125, new CommandType("c0x125", 0)},
            {0x126, new CommandType("c0x126", 4, 2, 2, 2, 2)},
            {0x127, new CommandType("c0x127", 4, 2, 2, 2, 2)},
            {0x128, new CommandType("c0x128", 1, 2)},
            {0x129, new CommandType("c0x129", 2, 2, 2)},
            {0x12A, new CommandType("c0x12A", 1, 2)},
            {0x12B, new CommandType("c0x12B", 3, 2, 2, 2)},
            {0x12C, new CommandType("c0x12C", 1, 2)},
            {0x12D, new CommandType("c0x12D", 4, 2, 2, 2, 2)},
            {0x12E, new CommandType("c0x12E", 3, 2, 2, 2)},
            {0x12F, new CommandType("c0x12F", 0)},
            {0x130, new CommandType("BootPCSound", 0)},
            {0x131, new CommandType("PC_131", 0)},
            {0x132, new CommandType("c0x132", 1, 2)},
            {0x133, new CommandType("c0x133", 0)},
            {0x134, new CommandType("c0x134", 2, 2, 2)},
            {0x135, new CommandType("c0x135", 0)},
            {0x136, new CommandType("c0x136", 1, 1)},
            {0x137, new CommandType("c0x137", 1, 2)},
            {0x138, new CommandType("c0x138", 1, 1)},
            {0x139, new CommandType("c0x139", 1, 1)},
            {0x13A, new CommandType("c0x13A", 1, 1)},
            {0x13B, new CommandType("c0x13B", 1, 2)},
            {0x13C, new CommandType("c0x13C", 0)},
            {0x13D, new CommandType("c0x13D", 0)},
            {0x13E, new CommandType("c0x13E", 0)},
            {0x13F, new CommandType("StartCameraEvent", 0)},
            {0x140, new CommandType("StopCameraEvent", 0)},
            {0x141, new CommandType("LockCamera", 0)},
            {0x142, new CommandType("ReleaseCamera", 0)},
            {0x143, new CommandType("MoveCamera", 11, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x144, new CommandType("c0x144", 1, 2)},
            {0x145, new CommandType("EndCameraEvent", 0)},
            {0x146, new CommandType("c0x146", 0)},
            {0x147, new CommandType("ResetCamera", 1, 2)},
            {0x148, new CommandType("c0x148", 8, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x149, new CommandType("c0x149", 2, 2, 2)},
            {0x14A, new CommandType("c0x14A", 0)},
            {0x14B, new CommandType("c0x14B", 0)},
            {0x14C, new CommandType("c0x14C", 0)},
            {0x14D, new CommandType("c0x14D", 2, 2, 2)},
            {0x14E, new CommandType("c0x14E", 3, 2, 2, 2)},
            {0x14F, new CommandType("c0x14F", 2, 2, 2)},
            {0x150, new CommandType("c0x150", 1, 2)},
            {0x151, new CommandType("c0x151", 2, 2, 2)},
            {0x152, new CommandType("c0x152", 0)},
            {0x153, new CommandType("c0x153", 1, 2)},
            {0x154, new CommandType("PlayCutscene", 2, 2, 2)},
            {0x155, new CommandType("c0x155", 2, 2, 2)},
            {0x156, new CommandType("c0x156", 1, 2)},
            {0x157, new CommandType("c0x157", 0)},
            {0x158, new CommandType("c0x158", 0)},
            {0x159, new CommandType("c0x159", 1, 2)},
            {0x15A, new CommandType("c0x15A", 4, 2, 2, 2, 2)},
            {0x15B, new CommandType("c0x15B", 1, 1)},
            {0x15C, new CommandType("c0x15C", 1, 1)},
            {0x15D, new CommandType("c0x15D", 0)},
            {0x15E, new CommandType("c0x15E", 0)},
            {0x15F, new CommandType("c0x15F", 0)},
            {0x160, new CommandType("c0x160", 0)},
            {0x161, new CommandType("c0x161", 0)},
            {0x162, new CommandType("c0x162", 0)},
            {0x163, new CommandType("c0x163", 2, 2, 1)},
            {0x164, new CommandType("c0x164", 1, 1)},
            {0x165, new CommandType("c0x165", 3, 1, 2, 2)},
            {0x166, new CommandType("c0x166", 3, 1, 2, 2)},
            {0x167, new CommandType("c0x167", 4, 2, 2, 2, 2)},
            {0x168, new CommandType("c0x168", 1, 2)},
            {0x169, new CommandType("c0x169", 1, 2)},
            {0x16A, new CommandType("c0x16A", 2, 2, 2)},
            {0x16B, new CommandType("PokemonMenuMusicalFunctions", 4, 2, 2, 2, 2)},
            {0x16C, new CommandType("c0x16C", 1, 2)},
            {0x16D, new CommandType("c0x16D", 0)},
            {0x16E, new CommandType("c0x16E", 0)},
            {0x16F, new CommandType("c0x16F", 0)},
            {0x170, new CommandType("c0x170", 0)},
            {0x171, new CommandType("c0x171", 0)},
            {0x172, new CommandType("SetVar172", 1, 2)},
            {0x173, new CommandType("c0x173", 0)},
            {0x174, new CommandType("StartWildBattle", 3, 2, 2, 2)},
            {0x175, new CommandType("EndWildBattle", 0)},
            {0x176, new CommandType("WildBattle1", 0)},
            {0x177, new CommandType("SetVarBattle177", 1, 2)},
            {0x178, new CommandType("BattleStoreResult", 1, 2)},
            {0x179, new CommandType("c0x179", 0)},
            {0x17A, new CommandType("c0x17A", 1, 2)},
            {0x17B, new CommandType("c0x17B", 1, 2)},
            {0x17C, new CommandType("c0x17C", 1, 2)},
            {0x17D, new CommandType("c0x17D", 1, 2)},
            {0x17E, new CommandType("c0x17E", 1, 2)},
            {0x17F, new CommandType("c0x17F", 1, 2)},
            {0x180, new CommandType("c0x180", 1, 2)},
            {0x181, new CommandType("c0x181", 1, 2)},
            {0x182, new CommandType("c0x182", 1, 2)},
            {0x183, new CommandType("c0x183", 1, 2)},
            {0x184, new CommandType("c0x184", 1, 2)},
            {0x185, new CommandType("c0x185", 1, 2)},
            {0x186, new CommandType("c0x186", 1, 2)},
            {0x187, new CommandType("c0x187", 1, 2)},
            {0x188, new CommandType("c0x188", 1, 2)},
            {0x189, new CommandType("c0x189", 1, 2)},
            {0x18A, new CommandType("c0x18A", 1, 2)},
            {0x18B, new CommandType("c0x18B", 1, 2)},
            {0x18C, new CommandType("c0x18C", 4, 2, 2, 2, 2)},
            {0x18D, new CommandType("c0x18D", 1, 2)},
            {0x18E, new CommandType("c0x18E", 1, 2)},
            {0x18F, new CommandType("c0x18F", 1, 2)},
            {0x190, new CommandType("c0x190", 1, 2)},
            {0x191, new CommandType("c0x191", 1, 2)},
            {0x192, new CommandType("c0x192", 1, 2)},
            {0x193, new CommandType("c0x193", 0)},
            {0x194, new CommandType("c0x194", 0)},
            {0x195, new CommandType("c0x195", 0)},
            {0x196, new CommandType("c0x196", 0)},
            {0x197, new CommandType("c0x197", 0)},
            {0x198, new CommandType("c0x198", 0)},
            {0x199, new CommandType("c0x199", 1, 2)},
            {0x19A, new CommandType("c0x19A", 0)},
            {0x19B, new CommandType("Animate19B", 1, 2)},
            {0x19C, new CommandType("c0x19C", 1, 2)},
            {0x19D, new CommandType("c0x19D", 0)},
            {0x19E, new CommandType("c0x19E", 1, 2)},
            {0x19F, new CommandType("CallScreenAnimation", 1, 2)},
            {0x1A0, new CommandType("c0x1A0", 0)},
            {0x1A1, new CommandType("Xtransciever1", 4, 2, 2, 2, 2)},
            {0x1A2, new CommandType("c0x1A2", 0)},
            {0x1A3, new CommandType("FlashBlackInstant", 0)},
            {0x1A4, new CommandType("Xtransciever4", 0)},
            {0x1A5, new CommandType("Xtransciever5", 0)},
            {0x1A6, new CommandType("Xtransciever6", 3, 2, 2, 2)},
            {0x1A7, new CommandType("Xtransciever7", 0)},
            {0x1A8, new CommandType("c0x1A8", 3, 2, 2, 2)},
            {0x1A9, new CommandType("c0x1A9", 4, 2, 2, 2, 2)},
            {0x1AA, new CommandType("c0x1AA", 4, 2, 2, 2, 2)},
            {0x1AB, new CommandType("FadeFromBlack", 0)},
            {0x1AC, new CommandType("FadeIntoBlack", 0)},
            {0x1AD, new CommandType("FadeFromWhite", 0)},
            {0x1AE, new CommandType("FadeIntoWhite", 0)},
            {0x1AF, new CommandType("c0x1AF", 2, 2, 2)},
            {0x1B0, new CommandType("c0x1B0", 0)},
            {0x1B1, new CommandType("E4StatueGoDown", 0)},
            {0x1B2, new CommandType("c0x1B2", 0)},
            {0x1B3, new CommandType("c0x1B3", 0)},
            {0x1B4, new CommandType("TradeNPCStart", 2, 2, 2)},
            {0x1B5, new CommandType("TradeNPCQualify", 3, 2, 2, 2)},
            {0x1B6, new CommandType("c0x1B6", 0)},
            {0x1B7, new CommandType("c0x1B7", 0)},
            {0x1B8, new CommandType("c0x1B8", 0)},
            {0x1B9, new CommandType("c0x1B9", 0)},
            {0x1BA, new CommandType("c0x1BA", 2, 2, 2)},
            {0x1BB, new CommandType("c0x1BB", 0)},
            {0x1BC, new CommandType("c0x1BC", 0)},
            {0x1BD, new CommandType("c0x1BD", 2, 2, 2)},
            {0x1BE, new CommandType("c0x1BE", 2, 2, 2)},
            {0x1BF, new CommandType("CompareChosenPokemon", 3, 2, 2, 2)},
            {0x1C0, new CommandType("c0x1C0", 0)},
            {0x1C1, new CommandType("c0x1C1", 5, 2, 2, 2, 2, 2)},
            {0x1C2, new CommandType("StartEventBC", 0)},
            {0x1C3, new CommandType("EndEventBC", 0)},
            {0x1C4, new CommandType("StoreTrainerID", 2, 2, 2)},
            {0x1C5, new CommandType("c0x1C5", 1, 2)},
            {0x1C6, new CommandType("EnableNationalDex", 0)},
            {0x1C7, new CommandType("c0x1C7", 1, 2)},
            {0x1C8, new CommandType("c0x1C8", 0)},
            {0x1C9, new CommandType("StoreVarMessage", 2, 2, 2)},
            {0x1CA, new CommandType("c0x1CA", 0)},
            {0x1CB, new CommandType("c0x1CB", 5, 2, 2, 2, 2, 2)},
            {0x1CC, new CommandType("c0x1CC", 1, 2)},
            {0x1CD, new CommandType("c0x1CD", 1, 2)},
            {0x1CE, new CommandType("c0x1CE", 2, 2, 2)},
            {0x1CF, new CommandType("c0x1CF", 1, 2)},
            {0x1D0, new CommandType("c0x1D0", 4, 2, 2, 2, 2)},
            {0x1D1, new CommandType("c0x1D1", 2, 2, 2)},
            {0x1D2, new CommandType("c0x1D2", 3, 2, 2, 2)},
            {0x1D3, new CommandType("c0x1D3", 3, 2, 2, 2)},
            {0x1D4, new CommandType("c0x1D4", 3, 2, 2, 2)},
            {0x1D5, new CommandType("c0x1D5", 2, 2, 2)},
            {0x1D6, new CommandType("c0x1D6", 2, 2, 2)},
            {0x1D7, new CommandType("c0x1D7", 4, 2, 2, 2, 2)},
            {0x1D8, new CommandType("c0x1D8", 4, 2, 2, 2, 2)},
            {0x1D9, new CommandType("c0x1D9", 4, 2, 2, 2, 2)},
            {0x1DA, new CommandType("c0x1DA", 1, 2)},
            {0x1DB, new CommandType("c0x1DB", 1, 2)},
            {0x1DC, new CommandType("c0x1DC", 1, 2)},
            {0x1DD, new CommandType("c0x1DD", 3, 2, 2, 2)},
            {0x1DE, new CommandType("c0x1DE", 2, 2, 2)},
            {0x1DF, new CommandType("c0x1DF", 0)},
            {0x1E0, new CommandType("c0x1E0", 4, 2, 2, 2, 2)},
            {0x1E1, new CommandType("c0x1E1", 0)},
            {0x1E2, new CommandType("c0x1E2", 0)},
            {0x1E3, new CommandType("c0x1E3", 3, 2, 2, 2)},
            {0x1E4, new CommandType("c0x1E4", 4, 2, 2, 2, 2)},
            {0x1E5, new CommandType("c0x1E5", 0)},
            {0x1E6, new CommandType("c0x1E6", 0)},
            {0x1E7, new CommandType("c0x1E7", 0)},
            {0x1E8, new CommandType("c0x1E8", 0)},
            {0x1E9, new CommandType("c0x1E9", 0)},
            {0x1EA, new CommandType("c0x1EA", 4, 2, 2, 2, 2)},
            {0x1EB, new CommandType("c0x1EB", 0)},
            {0x1EC, new CommandType("SwitchOwPosition", 5, 2, 2, 2, 2, 2)},
            {0x1ED, new CommandType("c0x1ED", 3, 2, 2, 2)},
            {0x1EE, new CommandType("c0x1EE", 2, 2, 2)},
            {0x1EF, new CommandType("c0x1EF", 2, 2, 2)},
            {0x1F0, new CommandType("c0x1F0", 2, 2, 2)},
            {0x1F1, new CommandType("c0x1F1", 0)},
            {0x1F2, new CommandType("c0x1F2", 1, 2)},
            {0x1F3, new CommandType("c0x1F3", 4, 2, 2, 2, 2)},
            {0x1F4, new CommandType("c0x1F4", 2, 2, 2)},
            {0x1F5, new CommandType("c0x1F5", 0)},
            {0x1F6, new CommandType("c0x1F6", 4, 2, 2, 2, 2)},
            {0x1F7, new CommandType("c0x1F7", 6, 2, 2, 2, 2, 2, 2)},
            {0x1F8, new CommandType("c0x1F8", 2, 2, 2)},
            {0x1F9, new CommandType("c0x1F9", 0)},
            {0x1FA, new CommandType("c0x1FA", 0)},
            {0x1FB, new CommandType("c0x1FB", 2, 2, 2)},
            {0x1FC, new CommandType("c0x1FC", 2, 2, 2)},
            {0x1FD, new CommandType("c0x1FD", 0)},
            {0x1FE, new CommandType("c0x1FE", 0)},
            {0x1FF, new CommandType("c0x1FF", 0)},
            {0x200, new CommandType("c0x200", 1, 2)},
            {0x201, new CommandType("c0x201", 0)},
            {0x202, new CommandType("c0x202", 1, 2)},
            {0x203, new CommandType("c0x203", 0)},
            {0x204, new CommandType("c0x204", 0)},
            {0x205, new CommandType("c0x205", 1, 2)},
            {0x206, new CommandType("c0x206", 0)},
            {0x207, new CommandType("c0x207", 2, 2, 2)},
            {0x208, new CommandType("c0x208", 1, 2)},
            {0x209, new CommandType("c0x209", 2, 2, 2)},
            {0x20A, new CommandType("c0x20A", 4, 2, 2, 2, 2)},
            {0x20B, new CommandType("c0x20B", 2, 2, 2)},
            {0x20C, new CommandType("StorePasswordClown", 4, 2, 2, 2, 2)},
            {0x20D, new CommandType("c0x20D", 0)},
            {0x20E, new CommandType("c0x20E", 2, 2, 2)},
            {0x20F, new CommandType("c0x20F", 3, 2, 2, 2)},
            {0x210, new CommandType("c0x210", 0)},
            {0x211, new CommandType("c0x211", 0)},
            {0x212, new CommandType("c0x212", 0)},
            {0x213, new CommandType("c0x213", 0)},
            {0x214, new CommandType("c0x214", 4, 2, 2, 2, 2)},
            {0x215, new CommandType("c0x215", 2, 2, 2)},
            {0x216, new CommandType("c0x216", 0)},
            {0x217, new CommandType("c0x217", 1, 2)},
            {0x218, new CommandType("c0x218", 2, 2, 2)},
            {0x219, new CommandType("c0x219", 2, 2, 2)},
            {0x21A, new CommandType("c0x21A", 2, 2, 2)},
            {0x21B, new CommandType("c0x21B", 0)},
            {0x21C, new CommandType("c0x21C", 2, 2, 2)},
            {0x21D, new CommandType("c0x21D", 1, 2)},
            {0x21E, new CommandType("HipWaderPKMGet", 1, 2)},
            {0x21F, new CommandType("c0x21F", 2, 2, 2)},
            {0x220, new CommandType("c0x220", 1, 2)},
            {0x221, new CommandType("c0x221", 2, 2, 2)},
            {0x222, new CommandType("c0x222", 0)},
            {0x223, new CommandType("StoreHiddenPowerType", 2, 2, 2)},
            {0x224, new CommandType("c0x224", 3, 2, 2, 2)},
            {0x225, new CommandType("c0x225", 1, 2)},
            {0x226, new CommandType("c0x226", 1, 2)},
            {0x227, new CommandType("c0x227", 2, 2, 2)},
            {0x228, new CommandType("c0x228", 0)},
            {0x229, new CommandType("c0x229", 2, 2, 2)},
            {0x22A, new CommandType("c0x22A", 1, 2)},
            {0x22B, new CommandType("c0x22B", 2, 2, 2)},
            {0x22C, new CommandType("c0x22C", 2, 2, 2)},
            {0x22D, new CommandType("c0x22D", 1, 2)},
            {0x22E, new CommandType("c0x22E", 0)},
            {0x22F, new CommandType("c0x22F", 2, 2, 2)},
            {0x230, new CommandType("c0x230", 2, 2, 2)},
            {0x231, new CommandType("c0x231", 1, 2)},
            {0x232, new CommandType("c0x232", 0)},
            {0x233, new CommandType("c0x233", 1, 2)},
            {0x234, new CommandType("c0x234", 2, 2, 2)},
            {0x235, new CommandType("c0x235", 0)},
            {0x236, new CommandType("c0x236", 4, 2, 2, 2, 2)},
            {0x237, new CommandType("c0x237", 2, 2, 2)},
            {0x238, new CommandType("c0x238", 0)},
            {0x239, new CommandType("c0x239", 1, 2)},
            {0x23A, new CommandType("c0x23A", 2, 2, 2)},
            {0x23B, new CommandType("c0x23B", 0)},
            {0x23C, new CommandType("c0x23C", 0)},
            {0x23D, new CommandType("c0x23D", 2, 2, 2)},
            {0x23E, new CommandType("c0x23E", 3, 2, 2, 2)},
            {0x23F, new CommandType("Close23F", 0)},
            {0x240, new CommandType("c0x240", 0)},
            {0x241, new CommandType("c0x241", 0)},
            {0x242, new CommandType("c0x242", 2, 2, 2)},
            {0x243, new CommandType("c0x243", 0)},
            {0x244, new CommandType("c0x244", 0)},
            {0x245, new CommandType("c0x245", 1, 2)},
            {0x246, new CommandType("c0x246", 1, 2)},
            {0x247, new CommandType("c0x247", 5, 2, 2, 2, 2, 2)},
            {0x248, new CommandType("c0x248", 2, 2, 2)},
            {0x249, new CommandType("c0x249", 4, 2, 2, 2, 2)},
            {0x24A, new CommandType("c0x24A", 2, 2, 2)},
            {0x24B, new CommandType("c0x24B", 0)},
            {0x24C, new CommandType("c0x24C", 1, 2)},
            {0x24D, new CommandType("c0x24D", 0)},
            {0x24E, new CommandType("c0x24E", 2, 2, 2)},
            {0x24F, new CommandType("c0x24F", 6, 2, 2, 2, 2, 2, 2)},
            {0x250, new CommandType("c0x250", 7, 2, 2, 2, 2, 2, 2, 2)},
            {0x251, new CommandType("c0x251", 1, 2)},
            {0x252, new CommandType("c0x252", 1, 2)},
            {0x253, new CommandType("c0x253", 1, 1)},
            {0x254, new CommandType("c0x254", 1, 2)},
            {0x255, new CommandType("c0x255", 0)},
            {0x256, new CommandType("c0x256", 0)},
            {0x257, new CommandType("c0x257", 0)},
            {0x258, new CommandType("c0x258", 0)},
            {0x259, new CommandType("c0x259", 0)},
            {0x25A, new CommandType("c0x25A", 1, 2)},
            {0x25B, new CommandType("c0x25B", 0)},
            {0x25C, new CommandType("c0x25C", 6, 2, 2, 2, 2, 2, 2)},
            {0x25D, new CommandType("c0x25D", 0)},
            {0x25E, new CommandType("c0x25E", 0)},
            {0x25F, new CommandType("c0x25F", 1, 2)},
            {0x260, new CommandType("c0x260", 0)},
            {0x261, new CommandType("c0x261", 0)},
            {0x262, new CommandType("c0x262", 2, 2, 2)},
            {0x263, new CommandType("c0x263", 1, 2)},
            {0x264, new CommandType("c0x264", 0)},
            {0x265, new CommandType("c0x265", 0)},
            {0x266, new CommandType("c0x266", 1, 2)},
            {0x267, new CommandType("c0x267", 0)},
            {0x268, new CommandType("c0x268", 0)},
            {0x269, new CommandType("c0x269", 0)},
            {0x26A, new CommandType("c0x26A", 0)},
            {0x26B, new CommandType("c0x26B", 0)},
            {0x26C, new CommandType("StoreMedals26C", 2, 1, 2)},
            {0x26D, new CommandType("StoreMedals26D", 2, 1, 2)},
            {0x26E, new CommandType("CountMedals26E", 2, 1, 2)},
            {0x26F, new CommandType("c0x26F", 0)},
            {0x270, new CommandType("c0x270", 0)},
            {0x271, new CommandType("c0x271", 2, 2, 2)},
            {0x272, new CommandType("c0x272", 2, 2, 2)},
            {0x273, new CommandType("c0x273", 1, 2)},
            {0x274, new CommandType("c0x274", 0)},
            {0x275, new CommandType("c0x275", 3, 1, 2, 2)},
            {0x276, new CommandType("c0x276", 2, 2, 2)},
            {0x277, new CommandType("c0x277", 0)},
            {0x278, new CommandType("c0x278", 0)},
            {0x279, new CommandType("c0x279", 0)},
            {0x27A, new CommandType("c0x27A", 0)},
            {0x27B, new CommandType("c0x27B", 0)},
            {0x27C, new CommandType("c0x27C", 0)},
            {0x27D, new CommandType("c0x27D", 0)},
            {0x27E, new CommandType("c0x27E", 0)},
            {0x27F, new CommandType("c0x27F", 0)},
            {0x280, new CommandType("c0x280", 0)},
            {0x281, new CommandType("c0x281", 0)},
            {0x282, new CommandType("c0x282", 0)},
            {0x283, new CommandType("c0x283", 2, 1, 1)},
            {0x284, new CommandType("c0x284", 2, 1, 1)},
            {0x285, new CommandType("c0x285", 3, 2, 2, 2)},
            {0x286, new CommandType("c0x286", 0)},
            {0x287, new CommandType("c0x287", 3, 2, 2, 2)},
            {0x288, new CommandType("c0x288", 3, 2, 2, 2)},
            {0x289, new CommandType("c0x289", 1, 2)},
            {0x28A, new CommandType("c0x28A", 0)},
            {0x28B, new CommandType("c0x28B", 0)},
            {0x28C, new CommandType("c0x28C", 0)},
            {0x28D, new CommandType("c0x28D", 0)},
            {0x28E, new CommandType("c0x28E", 0)},
            {0x28F, new CommandType("c0x28F", 0)},
            {0x290, new CommandType("c0x290", 1, 1)},
            {0x291, new CommandType("c0x291", 0)},
            {0x292, new CommandType("c0x292", 1, 1)},
            {0x293, new CommandType("c0x293", 1, 1)},
            {0x294, new CommandType("c0x294", 2, 1, 1)},
            {0x295, new CommandType("c0x295", 0)},
            {0x296, new CommandType("c0x296", 0)},
            {0x297, new CommandType("c0x297", 1, 2)},
            {0x298, new CommandType("c0x298", 0)},
            {0x299, new CommandType("c0x299", 0)},
            {0x29A, new CommandType("c0x29A", 2, 1, 2)},
            {0x29B, new CommandType("c0x29B", 1, 1)},
            {0x29C, new CommandType("c0x29C", 0)},
            {0x29D, new CommandType("c0x29D", 0)},
            {0x29E, new CommandType("c0x29E", 2, 2, 2)},
            {0x29F, new CommandType("c0x29F", 1, 2)},
            {0x2A0, new CommandType("StoreHasMedal", 2, 2, 2)},
            {0x2A1, new CommandType("c0x2A1", 1, 2)},
            {0x2A2, new CommandType("c0x2A2", 0)},
            {0x2A3, new CommandType("c0x2A3", 0)},
            {0x2A4, new CommandType("c0x2A4", 0)},
            {0x2A5, new CommandType("c0x2A5", 1, 2)},
            {0x2A6, new CommandType("c0x2A6", 0)},
            {0x2A7, new CommandType("c0x2A7", 1, 2)},
            {0x2A8, new CommandType("c0x2A8", 0)},
            {0x2A9, new CommandType("c0x2A9", 0)},
            {0x2AA, new CommandType("c0x2AA", 0)},
            {0x2AB, new CommandType("c0x2AB", 0)},
            {0x2AC, new CommandType("c0x2AC", 0)},
            {0x2AD, new CommandType("c0x2AD", 0)},
            {0x2AE, new CommandType("c0x2AE", 0)},
            {0x2AF, new CommandType("StoreDifficulty", 1, 2)},
            {0x2B0, new CommandType("UnlockKey", 1, 2)},
            {0x2B1, new CommandType("c0x2B1", 1, 2)},
            {0x2B2, new CommandType("c0x2B2", 2, 2, 2)},
            {0x2B3, new CommandType("c0x2B3", 2, 2, 2)},
            {0x2B4, new CommandType("c0x2B4", 2, 2, 2)},
            {0x2B5, new CommandType("c0x2B5", 2, 2, 2)},
            {0x2B6, new CommandType("c0x2B6", 2, 2, 2)},
            {0x2B7, new CommandType("c0x2B7", 1, 2)},
            {0x2B8, new CommandType("FollowMeStart", 0)},
            {0x2B9, new CommandType("FollowMeEnd", 0)},
            {0x2BA, new CommandType("FollowMeCopyStepsTo", 1, 2)},
            {0x2BB, new CommandType("c0x2BB", 0)},
            {0x2BC, new CommandType("c0x2BC", 2, 2, 2)},
            {0x2BD, new CommandType("c0x2BD", 1, 2)},
            {0x2BE, new CommandType("c0x2BE", 5, 2, 2, 2, 2, 2)},
            {0x2BF, new CommandType("c0x2BF", 0)},
            {0x2C0, new CommandType("c0x2C0", 2, 2, 2)},
            {0x2C1, new CommandType("c0x2C1", 0)},
            {0x2C2, new CommandType("c0x2C2", 0)},
            {0x2C3, new CommandType("c0x2C3", 2, 2, 2)},
            {0x2C4, new CommandType("c0x2C4", 0)},
            {0x2C5, new CommandType("c0x2C5", 1, 2)},
            {0x2C6, new CommandType("c0x2C6", 0)},
            {0x2C7, new CommandType("c0x2C7", 0)},
            {0x2C8, new CommandType("c0x2C8", 0)},
            {0x2C9, new CommandType("c0x2C9", 0)},
            {0x2CA, new CommandType("c0x2CA", 0)},
            {0x2CB, new CommandType("c0x2CB", 1, 2)},
            {0x2CC, new CommandType("c0x2CC", 0)},
            {0x2CD, new CommandType("c0x2CD", 0)},
            {0x2CE, new CommandType("c0x2CE", 0)},
            {0x2CF, new CommandType("c0x2CF", 4, 2, 2, 2, 2)},
            {0x2D0, new CommandType("EnableHabitatList", 0)},
            {0x2D1, new CommandType("c0x2D1", 1, 2)},
            {0x2D2, new CommandType("c0x2D2", 0)},
            {0x2D3, new CommandType("c0x2D3", 0)},
            {0x2D4, new CommandType("c0x2D4", 1, 2)},
            {0x2D5, new CommandType("c0x2D5", 2, 2, 2)},
            {0x2D6, new CommandType("c0x2D6", 0)},
            {0x2D7, new CommandType("c0x2D7", 2, 2, 2)},
            {0x2D8, new CommandType("c0x2D8", 0)},
            {0x2D9, new CommandType("c0x2D9", 2, 2, 2)},
            {0x2DA, new CommandType("c0x2DA", 1, 2)},
            {0x2DB, new CommandType("c0x2DB", 2, 2, 2)},
            {0x2DC, new CommandType("c0x2DC", 3, 2, 2, 2)},
            {0x2DD, new CommandType("StoreUnityVisitors", 1, 2)},
            {0x2DE, new CommandType("c0x2DE", 0)},
            {0x2DF, new CommandType("StoreMyActivities", 1, 2)},
            {0x2E0, new CommandType("c0x2E0", 0)},
            {0x2E1, new CommandType("c0x2E1", 0)},
            {0x2E2, new CommandType("c0x2E2", 0)},
            {0x2E3, new CommandType("c0x2E3", 0)},
            {0x2E4, new CommandType("c0x2E4", 0)},
            {0x2E5, new CommandType("c0x2E5", 0)},
            {0x2E6, new CommandType("c0x2E6", 0)},
            {0x2E7, new CommandType("c0x2E7", 0)},
            {0x2E8, new CommandType("c0x2E8", 2, 2, 2)},
            {0x2E9, new CommandType("c0x2E9", 0)},
            {0x2EA, new CommandType("GivePokemon_N", 6, 2, 2, 2, 2, 2, 2)},
            {0x2EB, new CommandType("c0x2EB", 0)},
            {0x2EC, new CommandType("c0x2EC", 0)},
            {0x2ED, new CommandType("c0x2ED", 2, 2, 2)},
            {0x2EE, new CommandType("Prop2EE", 2, 2, 2)},
            {0x2EF, new CommandType("c0x2EF", 1, 2)},
            {0x2F0, new CommandType("c0x2F0", 0)},
            {0x2F1, new CommandType("c0x2F1", 1, 2)},
            {0x2F2, new CommandType("c0x2F2", 0)},
            {0x2F3, new CommandType("c0x2F3", 0)},
            {0x2F4, new CommandType("c0x2F4", 0)},
            {0x2F5, new CommandType("c0x2F5", 0)},
            {0x2F6, new CommandType("c0x2F6", 0)},
            {0x2F7, new CommandType("c0x2F7", 0)},
            {0x2F8, new CommandType("c0x2F8", 0)},
            {0x2F9, new CommandType("c0x2F9", 0)},
            {0x2FA, new CommandType("c0x2FA", 0)},
            {0x2FB, new CommandType("c0x2FB", 0)},
            {0x2FC, new CommandType("c0x2FC", 0)},
            {0x2FD, new CommandType("c0x2FD", 0)},
            {0x2FE, new CommandType("c0x2FE", 0)},
            {0x2FF, new CommandType("c0x2FF", 0)},
            {0x300, new CommandType("c0x300", 0)},
            {0x301, new CommandType("c0x301", 0)},
            {0x302, new CommandType("c0x302", 0)},
            {0x303, new CommandType("c0x303", 0)},
            {0x304, new CommandType("c0x304", 0)},
            {0x305, new CommandType("c0x305", 0)},
            {0x306, new CommandType("c0x306", 0)},
            {0x307, new CommandType("c0x307", 0)},
            {0x308, new CommandType("c0x308", 0)},
            {0x309, new CommandType("c0x309", 0)},
            {0x30A, new CommandType("c0x30A", 0)},
            {0x30B, new CommandType("c0x30B", 0)},
            {0x30C, new CommandType("c0x30C", 0)},
            {0x30D, new CommandType("c0x30D", 0)},
            {0x30E, new CommandType("c0x30E", 0)},
            {0x30F, new CommandType("c0x30F", 0)},
            {0x310, new CommandType("c0x310", 0)},
            {0x311, new CommandType("c0x311", 0)},
            {0x312, new CommandType("c0x312", 0)},
            {0x313, new CommandType("c0x313", 0)},
            {0x314, new CommandType("c0x314", 0)},
            {0x315, new CommandType("c0x315", 0)},
            {0x316, new CommandType("c0x316", 0)},
            {0x317, new CommandType("c0x317", 0)},
            {0x318, new CommandType("c0x318", 0)},
            {0x319, new CommandType("c0x319", 0)},
            {0x31A, new CommandType("c0x31A", 0)},
            {0x31B, new CommandType("c0x31B", 0)},
            {0x31C, new CommandType("c0x31C", 0)},
            {0x31D, new CommandType("c0x31D", 0)},
            {0x31E, new CommandType("c0x31E", 0)},
            {0x31F, new CommandType("c0x31F", 0)},
            {0x320, new CommandType("c0x320", 0)},
            {0x321, new CommandType("c0x321", 0)},
            {0x322, new CommandType("c0x322", 0)},
            {0x323, new CommandType("c0x323", 0)},
            {0x324, new CommandType("c0x324", 0)},
            {0x325, new CommandType("c0x325", 0)},
            {0x326, new CommandType("c0x326", 0)},
            {0x327, new CommandType("c0x327", 0)},
            {0x328, new CommandType("c0x328", 0)},
            {0x329, new CommandType("c0x329", 0)},
            {0x32A, new CommandType("c0x32A", 0)},
            {0x32B, new CommandType("c0x32B", 0)},
            {0x32C, new CommandType("c0x32C", 0)},
            {0x32D, new CommandType("c0x32D", 0)},
            {0x32E, new CommandType("c0x32E", 0)},
            {0x32F, new CommandType("c0x32F", 0)},
            {0x330, new CommandType("c0x330", 0)},
            {0x331, new CommandType("c0x331", 0)},
            {0x332, new CommandType("c0x332", 0)},
            {0x333, new CommandType("c0x333", 0)},
            {0x334, new CommandType("c0x334", 0)},
            {0x335, new CommandType("c0x335", 0)},
            {0x336, new CommandType("c0x336", 0)},
            {0x337, new CommandType("c0x337", 0)},
            {0x338, new CommandType("c0x338", 0)},
            {0x339, new CommandType("c0x339", 0)},
            {0x33A, new CommandType("c0x33A", 0)},
            {0x33B, new CommandType("c0x33B", 0)},
            {0x33C, new CommandType("c0x33C", 0)},
            {0x33D, new CommandType("c0x33D", 0)},
            {0x33E, new CommandType("c0x33E", 0)},
            {0x33F, new CommandType("c0x33F", 0)},
            {0x340, new CommandType("c0x340", 0)},
            {0x341, new CommandType("c0x341", 0)},
            {0x342, new CommandType("c0x342", 0)},
            {0x343, new CommandType("c0x343", 0)},
            {0x344, new CommandType("c0x344", 0)},
            {0x345, new CommandType("c0x345", 0)},
            {0x346, new CommandType("c0x346", 0)},
            {0x347, new CommandType("c0x347", 0)},
            {0x348, new CommandType("c0x348", 0)},
            {0x349, new CommandType("c0x349", 0)},
            {0x34A, new CommandType("c0x34A", 0)},
            {0x34B, new CommandType("c0x34B", 0)},
            {0x34C, new CommandType("c0x34C", 0)},
            {0x34D, new CommandType("c0x34D", 0)},
            {0x34E, new CommandType("c0x34E", 0)},
            {0x34F, new CommandType("c0x34F", 0)},

            {0x3E8, new CommandType("c0x3E8", 0)},
            {0x3F3, new CommandType("c0x3F3", 0)},
            {0x3F4, new CommandType("c0x3F4", 1, 2)},
            {0x3F6, new CommandType("c0x3F6", 1, 2)},
            {0x3F9, new CommandType("c0x3F9", 0)},
            {0x3FA, new CommandType("c0x3FA", 1, 2)},
            {0x3FC, new CommandType("c0x3FC", 1, 2)},
            {0x3FD, new CommandType("c0x3FD", 0)},
            {0x3FE, new CommandType("c0x3FE", 2, 2, 2)},

            {0x401, new CommandType("c0x401", 1, 2)},
            {0x402, new CommandType("c0x402", 1, 2)},
            {0x403, new CommandType("c0x403", 2, 2, 2)},
            {0x404, new CommandType("c0x404", 1, 2)},
            {0x406, new CommandType("c0x406", 2, 2, 2)},
            {0x407, new CommandType("c0x407", 2, 2, 2)},
            {0x40D, new CommandType("c0x40D", 1, 2)},
            {0x40E, new CommandType("c0x40E", 1, 2)},
            {0x410, new CommandType("c0x410", 1, 2)},
            {0x411, new CommandType("c0x411", 1, 2)},
            {0x412, new CommandType("c0x412", 1, 2)},
            {0x414, new CommandType("c0x414", 2, 1, 2)},
            {0x415, new CommandType("c0x415", 1, 1)},
            {0x416, new CommandType("c0x416", 1, 2)},
            {0x417, new CommandType("c0x417", 1, 2)},
            {0x418, new CommandType("c0x418", 1, 2)},
            {0x419, new CommandType("c0x419", 2, 2, 2)},
            {0x41A, new CommandType("c0x41A", 1, 2)},
            {0x41B, new CommandType("c0x41B", 2, 2, 2)},
            {0x41C, new CommandType("c0x41C", 1, 2)},
            {0x41F, new CommandType("c0x41F", 2, 2, 2)},
            {0x420, new CommandType("c0x420", 1, 2)},
            {0x421, new CommandType("c0x421", 2, 2, 2)},
            {0x422, new CommandType("c0x422", 1, 2)}
        };

        internal static Dictionary<int, List<string>> parameters = new Dictionary<int, List<string>>()
        {
            { 0x3C, new List<string>() { "", "4", "Line", "NPC ID", "Top/Bot", "Box Type" } },
            { 0x3D, new List<string>() { "", "4", "Line", "Top/Bot", "Box Type" } },
            { 0x10C, new List<string>() { "Return var", "Pokemon", "Item", "Level" } },
            { 0x10E, new List<string>() { "Return var", "Pokemon", "Form", "Level", "Ability", "?", "?", "Shiny", "Held Item", "?" } },
        };

        internal static Dictionary<string, int> CommandsByName()
        {
            Dictionary<string, int> commands = new Dictionary<string, int>();
            foreach (var v in commandList)
            {
                commands.Add(v.Value.name, v.Key);
            }
            return commands;
        }
    }

    internal struct CommandType
    {
        public string name;
        public int numParameters;
        public List<int> parameterBytes;

        public CommandType(string name, int numParameters, params int[] parameterBytes)
        {
            this.name = name;
            this.numParameters = numParameters;
            this.parameterBytes = new List<int>(parameterBytes);
        }
    }
}
