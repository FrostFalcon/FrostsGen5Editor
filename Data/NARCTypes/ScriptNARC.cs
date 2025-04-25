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

            CommandReference.commandList = MainEditor.RomType == RomType.BW1 ? CommandReference.bw1CommandList : CommandReference.bw2CommandList;

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
            Dictionary<ScriptCommand, string> routines = new Dictionary<ScriptCommand, string>();
            Dictionary<ScriptCommand, string> routinePointers = new Dictionary<ScriptCommand, string>();

            List<List<RefByte>> movements = new List<List<RefByte>>();
            List<int[]> movementPointers = new List<int[]>();
            bool inMovement = false;

            Dictionary<ScriptCommand, string> labels = new Dictionary<ScriptCommand, string>();
            Dictionary<ScriptCommand, string> jumpPointers = new Dictionary<ScriptCommand, string>();
            Dictionary<ScriptCommand, string> ifPointers = new Dictionary<ScriptCommand, string>();
            List<string> labelQueue = new List<string>();

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
                        if (line.Split(' ')[0].EndsWith(":") || line.Split(' ')[0].EndsWith(":;"))
                        {
                            labelQueue.Add(line.Split(':')[0]);
                            continue;
                        }

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
                            while (labelQueue.Count > 0)
                            {
                                if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                labelQueue.RemoveAt(0);
                            }

                            inMovement = true;
                            movements.Add(new List<RefByte>());
                            continue;
                        }

                        if (line.StartsWith("}"))
                        {
                            //End Sequence
                            if (stack.Count == 1)
                            {
                                if (stack[0].commands.Count == 0)
                                {
                                    stack[0].commands.Add(new ScriptCommand(2, new int[0]));
                                    while (labelQueue.Count > 0)
                                    {
                                        if (!labels.ContainsKey(stack[0].commands[stack[0].commands.Count - 1])) labels.Add(stack[0].commands[stack[0].commands.Count - 1], labelQueue[0]);
                                        labelQueue.RemoveAt(0);
                                    }
                                }

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

                                //sf.ApplyData(true);
                                //int pos = sf.bytes.Length - 4 * sf.sequences.Count - 2;
                                //sf.ApplyData(false);
                                sf.sequences.Add(stack[0]);
                                routines.Add(stack[0].commands[0], currentRoutineName);
                                stack.RemoveAt(0);
                            }
                            else
                            {
                                int jumpAmount = 0;
                                foreach (ScriptCommand c in stack[stack.Count - 1].commands) jumpAmount += c.ByteLength;
                                stack[stack.Count - 2].commands.Add(new ScriptCommand(0x1E, new int[] { jumpAmount }));
                                while (labelQueue.Count > 0)
                                {
                                    labels.Add(stack[stack.Count - 2].commands[stack[stack.Count - 2].commands.Count - 1], labelQueue[0]);
                                    labelQueue.RemoveAt(0);
                                }
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

                        if (line.StartsWith("goto"))
                        {
                            ScriptCommand sc = new ScriptCommand(0x1E, new int[] { 0 });
                            stack[stack.Count - 1].commands.Add(sc);
                            while (labelQueue.Count > 0)
                            {
                                if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                labelQueue.RemoveAt(0);
                            }
                            string name = line.Split(' ')[1].Replace(";", "");
                            jumpPointers.Add(sc, name);
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
                            if (line.Contains("goto"))
                            {
                                if (pars.Count == 1)
                                {
                                    ScriptCommand sc = new ScriptCommand(0x1F, new int[] { pars[0], 6 });
                                    stack[stack.Count - 1].commands.Add(sc);
                                    while (labelQueue.Count > 0)
                                    {
                                        if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                        labelQueue.RemoveAt(0);
                                    }
                                    string name = line.Substring(line.IndexOf("goto") + 4).Replace(" ", "").Replace(";", "");
                                    ifPointers.Add(sc, name);
                                }
                                else if (pars.Count == 3)
                                {
                                    ScriptCommand sc = new ScriptCommand(0x19, new int[] { pars[0], pars[2] });
                                    stack[stack.Count - 1].commands.Add(sc);
                                    while (labelQueue.Count > 0)
                                    {
                                        if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                        labelQueue.RemoveAt(0);
                                    }
                                    sc = new ScriptCommand(0x1F, new int[] { pars[1], 6 });
                                    stack[stack.Count - 1].commands.Add(sc);
                                    string name = line.Substring(line.IndexOf("goto") + 4).Replace(" ", "").Replace(";", "");
                                    ifPointers.Add(sc, name);
                                }
                                else
                                {
                                    throw new Exception("Invalid conditional at line " + lineNumber);
                                }
                            }
                            else
                            {
                                if (pars.Count == 1)
                                {
                                    ScriptCommand sc = new ScriptCommand(0x1F, new int[] { pars[0], 6 });
                                    stack[stack.Count - 1].commands.Add(sc);
                                    while (labelQueue.Count > 0)
                                    {
                                        if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                        labelQueue.RemoveAt(0);
                                    }
                                }
                                else if (pars.Count == 3)
                                {
                                    ScriptCommand sc = new ScriptCommand(0x19, new int[] { pars[0], pars[2] });
                                    stack[stack.Count - 1].commands.Add(sc);
                                    while (labelQueue.Count > 0)
                                    {
                                        if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                        labelQueue.RemoveAt(0);
                                    }
                                    sc = new ScriptCommand(0x1F, new int[] { pars[1], 6 });
                                    stack[stack.Count - 1].commands.Add(sc);
                                }
                                else
                                {
                                    throw new Exception("Invalid conditional at line " + lineNumber);
                                }
                                stack.Add(new ScriptSequence());
                            }
                            continue;
                        }

                        if (comDict.ContainsKey(com))
                        {
                            ScriptCommand sc = new ScriptCommand((short)comDict[com], pars.ToArray());
                            
                            if (CommandReference.commandList[sc.commandID].numParameters != sc.parameters.Length)
                                throw new Exception("Incorrect number of parameters at line " + lineNumber + "\nExpected: " + CommandReference.commandList[sc.commandID].numParameters + ", Found: " + sc.parameters.Length);

                            stack[stack.Count - 1].commands.Add(sc);
                            while (labelQueue.Count > 0)
                            {
                                if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                labelQueue.RemoveAt(0);
                            }
                            if (sc.commandID == 0x4) routinePointers.Add(sc, "");
                            continue;
                        }
                        else
                        {
                            ScriptCommand sc = new ScriptCommand(0x4, new int[] { 0 });
                            routinePointers.Add(sc, com);
                            stack[stack.Count - 1].commands.Add(sc);
                            while (labelQueue.Count > 0)
                            {
                                if (!labels.ContainsKey(sc)) labels.Add(sc, labelQueue[0]);
                                labelQueue.RemoveAt(0);
                            }
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

                            if (CommandReference.reverseMovements.ContainsKey(ps[i].Trim())) bytes[i] = (byte)CommandReference.reverseMovements[ps[0].Trim()];
                            else if (ps[i].Trim().StartsWith("0x")) bytes[i] = byte.Parse(ps[i].Trim().Substring(2), System.Globalization.NumberStyles.HexNumber);
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

            //Calculate Labels
            sf.ApplyData();
            Dictionary<string, int> labelLocations = new Dictionary<string, int>();
            for (int j = 0; j < sf.sequences.Count; j++)
            {
                int pos = HelperFunctions.ReadInt(sf.bytes, j * 4) + 4 + j * 4;
                foreach (ScriptCommand com in sf.sequences[j].commands)
                {
                    if (labels.ContainsKey(com))
                    {
                        if (labelLocations.ContainsKey(labels[com]))
                            throw new Exception("Duplicate label found: " + labels[com]);
                        labelLocations.Add(labels[com], pos);
                    }
                    pos += com.ByteLength;
                }
            }
            for (int j = 0; j < sf.sequences.Count; j++)
            {
                int pos = HelperFunctions.ReadInt(sf.bytes, j * 4) + 4 + j * 4;
                foreach (ScriptCommand com in sf.sequences[j].commands)
                {
                    pos += com.ByteLength;
                    if (jumpPointers.ContainsKey(com) && labelLocations.ContainsKey(jumpPointers[com]))
                    {
                        com.parameters[0] = labelLocations[jumpPointers[com]] - pos;
                    }
                    if (ifPointers.ContainsKey(com) && labelLocations.ContainsKey(ifPointers[com]))
                    {
                        com.parameters[1] = labelLocations[ifPointers[com]] - pos;
                    }
                }
            }
            //Calculate Routines
            Dictionary<string, int> routineLocations = new Dictionary<string, int>();
            for (int j = 0; j < sf.sequences.Count; j++)
            {
                int pos = HelperFunctions.ReadInt(sf.bytes, j * 4) + 4 + j * 4;
                if (routines.ContainsKey(sf.sequences[j].commands[0])) routineLocations.Add(routines[sf.sequences[j].commands[0]], pos);
            }
            for (int j = 0; j < sf.sequences.Count; j++)
            {
                int pos = HelperFunctions.ReadInt(sf.bytes, j * 4) + 4 + j * 4;
                foreach (ScriptCommand com in sf.sequences[j].commands)
                {
                    pos += com.ByteLength;
                    if (routinePointers.ContainsKey(com) && routineLocations.ContainsKey(routinePointers[com]))
                    {
                        com.parameters[0] = routineLocations[routinePointers[com]] - pos;
                    }
                }
            }
            sf.ApplyData();
            //Assign routines
            //Dictionary<string, int> subRoutineLocations = new Dictionary<string, int>();
            //for (int j = 0; j < sf.sequences.Count; j++)
            //{
            //    int jumpTo = 0;
            //    int n = HelperFunctions.ReadInt(sf.bytes, j * 4);
            //    n = j * 4 + n + 4;
            //    int start = n;
            //    for (int i = start; i < sf.bytes.Length && routinePointers.Count != 0; i++)
            //    {
            //        ScriptCommand sc = new ScriptCommand(sf.bytes, i);
            //        if (sc.commandID == 0x4)
            //        {
            //            int jump = -i - 6;
            //            if (routineLocations.ContainsKey(routinePointers[0])) jump += routineLocations[routinePointers[0]] + sf.sequences.Count * 4 + 2;
            //            else jump = 0;
            //            if (jump != 0) HelperFunctions.WriteInt(sf.bytes, i + 2, jump);
            //            routinePointers.RemoveAt(0);
            //        }
            //        if (sc.commandID == 0x1E) jumpTo = i + 6 + sc.parameters[0];
            //        if (sc.commandID == 0x1F) jumpTo = i + 7 + sc.parameters[1];
            //        i += sc.ByteLength - 1;
            //        if (sc.commandID == 2 && i >= jumpTo) break;
            //    }
            //}
            //sf.ReadData();

            return sf;
        }

        public void Export(FileStream file, params string[] headers)
        {
            foreach (string str in headers)
            {
                file.WriteLine("#include \"" + str + "\"");
            }

            int seqID = 0;

            List<int> sequenceRoutines = new List<int>();
            List<int> routines = new List<int>();
            List<int> jumpLocations = new List<int>();

            //Find jump locations
            foreach (ScriptSequence seq in sequences)
            {
                int n = HelperFunctions.ReadInt(bytes, seqID * 4) + seqID * 4 + 4;
                sequenceRoutines.Add(n);
                foreach (ScriptCommand com in seq.commands)
                {
                    n += com.ByteLength;
                    if (com.commandID == 0x1E && !jumpLocations.Contains(n + com.parameters[0])) jumpLocations.Add(n + com.parameters[0]);
                    if (com.commandID == 0x1F && !jumpLocations.Contains(n + com.parameters[1])) jumpLocations.Add(n + com.parameters[1]);
                }
                seqID++;
            }

            seqID = 0;
            foreach (ScriptSequence seq in sequences)
            {
                file.WriteLine("\nvoid Sequence" + seqID + "()\n{");

                int pos = HelperFunctions.ReadInt(bytes, seqID * 4) + seqID * 4 + 4;
                int jumpMax = 0;
                while (pos < bytes.Length)
                {
                    ScriptCommand com = new ScriptCommand(bytes, pos);
                    if (jumpLocations.Contains(pos)) file.WriteLine("\nlabel" + jumpLocations.IndexOf(pos) + ": ;");
                    if (com.commandID == 0x1E) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[0]);
                    if (com.commandID == 0x1F) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[1]);
                    WriteCommandToFile(file, pos + com.ByteLength, com, routines, sequenceRoutines, jumpLocations);
                    if ((com.commandID == 0x2 || com.commandID == 0x5) && pos >= jumpMax) break;
                    pos += com.ByteLength;
                }

                file.WriteLine("}");
                seqID++;
            }

            int routineID = 0;

            while (routineID < routines.Count)
            {
                int pos = routines[routineID];
                int jumpMax = 0;
                while (pos < bytes.Length)
                {
                    ScriptCommand com = new ScriptCommand(bytes, pos);
                    if (com.commandID == 0x1E) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[0]);
                    if (com.commandID == 0x1F) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[1]);
                    if (com.commandID == 0x1E && !jumpLocations.Contains(pos + com.ByteLength + com.parameters[0])) jumpLocations.Add(pos + com.ByteLength + com.parameters[0]);
                    if (com.commandID == 0x1F && !jumpLocations.Contains(pos + com.ByteLength + com.parameters[1])) jumpLocations.Add(pos + com.ByteLength + com.parameters[1]);
                    if ((com.commandID == 0x2 || com.commandID == 0x5) && pos >= jumpMax) break;
                    pos += com.ByteLength;
                }
                routineID++;
            }

            routineID = 0;

            while (routineID < routines.Count)
            {
                file.WriteLine("\nvoid Routine" + routineID + "()\n{");
                int pos = routines[routineID];
                int jumpMax = 0;
                while (pos < bytes.Length)
                {
                    ScriptCommand com = new ScriptCommand(bytes, pos);
                    if (jumpLocations.Contains(pos)) file.WriteLine("\nlabel" + jumpLocations.IndexOf(pos) + ": ;");
                    if (com.commandID == 0x1E) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[0]);
                    if (com.commandID == 0x1F) jumpMax = Math.Max(jumpMax, pos + com.ByteLength + com.parameters[1]);
                    WriteCommandToFile(file, pos + com.ByteLength, com, routines, sequenceRoutines, jumpLocations);
                    if ((com.commandID == 0x2 || com.commandID == 0x5) && pos >= jumpMax) break;
                    pos += com.ByteLength;
                }
                file.WriteLine("}");
                routineID++;
            }
        }

        public void OldExport(FileStream file)
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
                    //WriteCommandToFile(file, n, com, routines);
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
                    //WriteCommandToFile(file, pos + com.ByteLength, com, routines);
                    if ((com.commandID == 0x2 || com.commandID == 0x5) && pos >= jumpMax) break;
                    pos += com.ByteLength;
                }
                file.WriteLine("}");
                routineID++;
            }
        }

        void WriteCommandToFile(FileStream file, int pos, ScriptCommand com, List<int> routines, List<int> sequenceRoutines, List<int> jumpLocations)
        {
            //Handle movements
            if (com.commandID == 0x64)
            {
                file.WriteLine("\n\tMovement m[] = { //" + com.parameters[0]);
                int n = pos;
                n += com.parameters[1];

                for (int i = n; bytes[i] != 0xFE && i < bytes.Length - 4; i += 4)
                {
                    string move = "0x" + ((byte)bytes[i]).ToString("X2");
                    if (CommandReference.movements.ContainsKey(bytes[i])) move = CommandReference.movements[bytes[i]];
                    file.WriteLine("\t\t" + move + ", " + ((byte)bytes[i + 2]) + ",");
                }
                file.WriteLine("\t};\n");
            }

            //Handle Routines
            else if (com.commandID == 0x4)
            {
                int n = pos;
                n += com.parameters[0];
                if (sequenceRoutines.Contains(n)) file.WriteLine("\tSequence" + sequenceRoutines.IndexOf(n) + "();");
                else if (routines.Contains(n)) file.WriteLine("\tRoutine" + routines.IndexOf(n) + "();");
                else
                {
                    file.WriteLine("\tRoutine" + routines.Count + "();");
                    routines.Add(n);
                }
            }

            //Handle jumps
            else if (com.commandID == 0x1E && jumpLocations.Contains(pos + com.parameters[0]))
            {
                file.WriteLine("\tgoto label" + jumpLocations.IndexOf(pos + com.parameters[0]) + ";");
            }
            else if (com.commandID == 0x1F && jumpLocations.Contains(pos + com.parameters[1]))
            {
                file.WriteLine("\tif (" + com.parameters[0] + ") goto label" + jumpLocations.IndexOf(pos + com.parameters[1]) + ";");
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

                //ReadData();
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

    public class ScriptCommand
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

                //if (commandID == 0x143 && i < 6)
                //{
                //    int before = parameters[i];
                //    int whole = parameters[i] >> 12;
                //    float dec = ((parameters[i] << 20) >> 20) / 4096f;
                //    int after = (int)Math.Ceiling(dec * 4096) + whole << 12;
                //    Debug.Write((whole + dec).ToString() + ", ");
                //    whole = after >> 12;
                //    dec = ((after << 20) >> 20) / 4096f;
                //    Debug.WriteLine((whole + dec).ToString());
                //
                //    str.Append((whole + dec).ToString());
                //}
                str.Append(parameters[i] >= 0x4000 ? "0x" + parameters[i].ToString("X") : parameters[i].ToString());
            }
            str.Append(");");
            return str.ToString();
        }
    }

    internal static class CommandReference
    {
        internal static Dictionary<int, string> movements = new Dictionary<int, string>()
        {
            { 0x0, "LookUp" },
            { 0x1, "LookDown" },
            { 0x2, "LookLeft" },
            { 0x3, "LookRight" },
            { 0x4, "SlowestWalkUp" },
            { 0x5, "SlowestWalkDown" },
            { 0x6, "SlowestWalkLeft" },
            { 0x7, "SlowestWalkRight" },
            { 0x8, "SlowWalkUp" },
            { 0x9, "SlowWalkDown" },
            { 0xa, "SlowWalkLeft" },
            { 0xb, "SlowWalkRight" },
            { 0xc, "WalkUp" },
            { 0xd, "WalkDown" },
            { 0xe, "WalkLeft" },
            { 0xf, "WalkRight" },
            { 0x10, "FastWalkUp" },
            { 0x11, "FastWalkDown" },
            { 0x12, "FastWalkLeft" },
            { 0x13, "FastWalkRight" },
            { 0x14, "FastestWalkUp" },
            { 0x15, "FastestWalkDown" },
            { 0x16, "FastestWalkLeft" },
            { 0x17, "FastestWalkRight" },
            { 0x18, "SlowestTurnUp" },
            { 0x19, "SlowestTurnDown" },
            { 0x1a, "SlowestTurnLeft" },
            { 0x1b, "SlowestTurnRight" },
            { 0x1c, "SlowTurnUp" },
            { 0x1d, "SlowTurnDown" },
            { 0x1e, "SlowTurnLeft" },
            { 0x1f, "SlowTurnRight" },
            { 0x20, "TurnUp" },
            { 0x21, "TurnDown" },
            { 0x22, "TurnLeft" },
            { 0x23, "TurnRight" },
            { 0x24, "FastTurnUp" },
            { 0x25, "FastTurnDown" },
            { 0x26, "FastTurnLeft" },
            { 0x27, "FastTurnRight" },
            { 0x28, "FastestTurnUp" },
            { 0x29, "FastestTurnDown" },
            { 0x2a, "FastestTurnLeft" },
            { 0x2b, "FastestTurnRight" },
            { 0x2c, "SlowHopUp" },
            { 0x2d, "SlowHopDown" },
            { 0x2e, "SlowHopLeft" },
            { 0x2f, "SlowHopRight" },
            { 0x30, "HopUp" },
            { 0x31, "HopDown" },
            { 0x32, "HopLeft" },
            { 0x33, "HopRight" },
            { 0x34, "JumpUp1" },
            { 0x35, "JumpDown1" },
            { 0x36, "JumpLeft1" },
            { 0x37, "JumpRight1" },
            { 0x38, "JumpUp2" },
            { 0x39, "JumpDown2" },
            { 0x3a, "JumpLeft2" },
            { 0x3b, "JumpRight2" },
            { 0x3c, "Wait1" },
            { 0x3d, "Wait2" },
            { 0x3e, "Wait4" },
            { 0x3f, "Wait8" },
            { 0x40, "Wait15" },
            { 0x41, "Wait16" },
            { 0x42, "Wait32" },
            { 0x43, "WarpPadUp" },
            { 0x44, "WarpPadDown" },
            { 0x45, "Vanish" },
            { 0x46, "Reappear" },
            { 0x47, "LockDirection" },
            { 0x48, "UnlockDirection" },
            { 0x49, "PauseAnim" },
            { 0x4a, "UnpauseAnim" },
            { 0x4b, "Exclaimation" },
            { 0x9f, "QuestionMark" },
            { 0xa0, "MusicNote" },
            { 0xa1, "Ellipses" },
            { 0x4c, "MediumFastWalkUp" },
            { 0x4d, "MediumFastWalkDown" },
            { 0x4e, "MediumFastWalkLeft" },
            { 0x4f, "MediumFastWalkRight" },
            { 0x50, "FasterWalkUp" },
            { 0x51, "FasterWalkDown" },
            { 0x52, "FasterWalkLeft" },
            { 0x53, "FasterWalkRight" },
            { 0x54, "InstantWalkUp" },
            { 0x55, "InstantWalkDown" },
            { 0x56, "InstantWalkLeft" },
            { 0x57, "InstantWalkRight" },
            { 0x58, "RunUp" },
            { 0x59, "RunDown" },
            { 0x5a, "RunLeft" },
            { 0x5b, "RunRight" }
        };

        internal static Dictionary<string, int> reverseMovements = new Dictionary<string, int>()
        {
            { "LookUp", 0x0 },
            { "LookDown", 0x1 },
            { "LookLeft", 0x2 },
            { "LookRight", 0x3 },
            { "SlowestWalkUp", 0x4 },
            { "SlowestWalkDown", 0x5 },
            { "SlowestWalkLeft", 0x6 },
            { "SlowestWalkRight", 0x7 },
            { "SlowWalkUp", 0x8 },
            { "SlowWalkDown", 0x9 },
            { "SlowWalkLeft", 0xa },
            { "SlowWalkRight", 0xb },
            { "WalkUp", 0xc },
            { "WalkDown", 0xd },
            { "WalkLeft", 0xe },
            { "WalkRight", 0xf },
            { "FastWalkUp", 0x10 },
            { "FastWalkDown", 0x11 },
            { "FastWalkLeft", 0x12 },
            { "FastWalkRight", 0x13 },
            { "FastestWalkUp", 0x14 },
            { "FastestWalkDown", 0x15 },
            { "FastestWalkLeft", 0x16 },
            { "FastestWalkRight", 0x17 },
            { "SlowestTurnUp", 0x18 },
            { "SlowestTurnDown", 0x19 },
            { "SlowestTurnLeft", 0x1a },
            { "SlowestTurnRight", 0x1b },
            { "SlowTurnUp", 0x1c },
            { "SlowTurnDown", 0x1d },
            { "SlowTurnLeft", 0x1e },
            { "SlowTurnRight", 0x1f },
            { "TurnUp", 0x20 },
            { "TurnDown", 0x21 },
            { "TurnLeft", 0x22 },
            { "TurnRight", 0x23 },
            { "FastTurnUp", 0x24 },
            { "FastTurnDown", 0x25 },
            { "FastTurnLeft", 0x26 },
            { "FastTurnRight", 0x27 },
            { "FastestTurnUp", 0x28 },
            { "FastestTurnDown", 0x29 },
            { "FastestTurnLeft", 0x2a },
            { "FastestTurnRight", 0x2b },
            { "SlowHopUp", 0x2c },
            { "SlowHopDown", 0x2d },
            { "SlowHopLeft", 0x2e },
            { "SlowHopRight", 0x2f },
            { "HopUp", 0x30 },
            { "HopDown", 0x31 },
            { "HopLeft", 0x32 },
            { "HopRight", 0x33 },
            { "JumpUp1", 0x34 },
            { "JumpDown1", 0x35 },
            { "JumpLeft1", 0x36 },
            { "JumpRight1", 0x37 },
            { "JumpUp2", 0x38 },
            { "JumpDown2", 0x39 },
            { "JumpLeft2", 0x3a },
            { "JumpRight2", 0x3b },
            { "Wait1", 0x3c },
            { "Wait2", 0x3d },
            { "Wait4", 0x3e },
            { "Wait8", 0x3f },
            { "Wait15", 0x40 },
            { "Wait16", 0x41 },
            { "Wait32", 0x42 },
            { "WarpPadUp", 0x43 },
            { "WarpPadDown", 0x44 },
            { "Vanish", 0x45 },
            { "Reappear", 0x46 },
            { "LockDirection", 0x47 },
            { "UnlockDirection", 0x48 },
            { "PauseAnim", 0x49 },
            { "UnpauseAnim", 0x4a },
            { "Exclaimation", 0x4b },
            { "QuestionMark", 0x9f },
            { "MusicNote", 0xa0 },
            { "Ellipses", 0xa1 },
            { "MediumFastWalkUp", 0x4c },
            { "MediumFastWalkDown", 0x4d },
            { "MediumFastWalkLeft", 0x4e },
            { "MediumFastWalkRight", 0x4f },
            { "FasterWalkUp", 0x50 },
            { "FasterWalkDown", 0x51 },
            { "FasterWalkLeft", 0x52 },
            { "FasterWalkRight", 0x53 },
            { "InstantWalkUp", 0x54 },
            { "InstantWalkDown", 0x55 },
            { "InstantWalkLeft", 0x56 },
            { "InstantWalkRight", 0x57 },
            { "RunUp", 0x58 },
            { "RunDown", 0x59 },
            { "RunLeft", 0x5a },
            { "RunRight", 0x5b }
        };

        internal static Dictionary<int, CommandType> commandList = new Dictionary<int, CommandType>();

        internal static Dictionary<int, CommandType> bw2CommandList = new Dictionary<int, CommandType>()
        {
            {0x0, new CommandType("c0x0", 0)},
            {0x1, new CommandType("c0x1", 0)},
            {0x2, new CommandType("End", 0)},
            {0x3, new CommandType("ReturnAfterDelay", 1, 2)},
            {0x4, new CommandType("CallRoutine", 1, 4)},
            {0x5, new CommandType("Return", 0)},
            {0x6, new CommandType("Logic06", 1, 2)},
            {0x7, new CommandType("Logic07", 1, 2)},
            {0x8, new CommandType("StackPushConst", 1, 2)},
            {0x9, new CommandType("StackPushVar", 1, 2)},
            {0xA, new CommandType("StackPop", 1, 2)},
            {0xB, new CommandType("StackDiscard", 0)},
            {0xC, new CommandType("StackAdd", 0)},
            {0xD, new CommandType("StackSub", 0)},
            {0xE, new CommandType("StackMult", 0)},
            {0xF, new CommandType("StackDiv", 0)},
            {0x10, new CommandType("StoreFlag", 1, 2)},
            {0x11, new CommandType("StackCompare", 1, 2)},
            {0x12, new CommandType("BitwiseAndVar", 2, 2, 2)},
            {0x13, new CommandType("BitwiseOrVar", 2, 2, 2)},
            {0x14, new CommandType("c0x14", 2, 1, 1)},
            {0x15, new CommandType("c0x15", 2, 1, 4)},
            {0x16, new CommandType("c0x16", 2, 1, 1)},
            {0x17, new CommandType("c0x17", 2, 1, 1)},
            {0x18, new CommandType("c0x18", 2, 1, 1)},
            {0x19, new CommandType("Compare", 2, 2, 2)},
            {0x1A, new CommandType("CompareVars", 2, 2, 2)},
            {0x1B, new CommandType("CallGlobalScriptAsync", 1, 2)},
            {0x1C, new CommandType("CallGlobalScript", 1, 2)},
            {0x1D, new CommandType("ReturnGlobalScript", 0)},
            {0x1E, new CommandType("Jump", 1, 4)},
            {0x1F, new CommandType("If", 2, 1, 4)},
            {0x20, new CommandType("c0x20", 2, 1, 4)},
            {0x21, new CommandType("c0x21", 1, 2)},
            {0x22, new CommandType("c0x22", 1, 2)},
            {0x23, new CommandType("SetFlag", 1, 2)},
            {0x24, new CommandType("ClearFlag", 1, 2)},
            {0x25, new CommandType("SetVarFlagStatus", 2, 2, 2)},
            {0x26, new CommandType("AddToVar", 2, 2, 2)},
            {0x27, new CommandType("SubtractVar", 2, 2, 2)},
            {0x28, new CommandType("SetVarEqVal", 2, 2, 2)},
            {0x29, new CommandType("SetVarEqVar", 2, 2, 2)},
            {0x2A, new CommandType("SetVarEqVar2", 2, 2, 2)},
            {0x2B, new CommandType("MultiplyVar", 2, 2, 2)},
            {0x2C, new CommandType("DivideVar", 2, 2, 2)},
            {0x2D, new CommandType("ModuloVar", 2, 2, 2)},
            {0x2E, new CommandType("LockAll", 0)},
            {0x2F, new CommandType("UnlockAll", 0)},
            {0x30, new CommandType("WaitMoment", 0)},
            {0x31, new CommandType("WaitForABInput", 0)},
            {0x32, new CommandType("WaitForButton", 0)},
            {0x33, new CommandType("SetMessageAutoscroll", 1, 2)},
            {0x34, new CommandType("EventGreyMessage", 2, 2, 2)},
            {0x35, new CommandType("EventMessageAsync", 2, 2, 2)},
            {0x36, new CommandType("CloseEventGreyMessage", 0)},
            {0x37, new CommandType("c0x37", 1, 2)},
            {0x38, new CommandType("BubbleMessage", 2, 2, 1)},
            {0x39, new CommandType("CloseBubbleMessage", 0)},
            {0x3A, new CommandType("ShowMessageAt", 4, 2, 2, 2, 2)},
            {0x3B, new CommandType("CloseShowMessageAt", 1, 2)},
            {0x3C, new CommandType("Message", 5, 2, 2, 2, 2, 2)},
            {0x3D, new CommandType("Message2", 4, 2, 2, 2, 2)},
            {0x3E, new CommandType("CloseMessageBox", 0)},
            {0x3F, new CommandType("CloseAllMessageBoxes", 0)},
            {0x40, new CommandType("MoneyBox", 2, 2, 2)},
            {0x41, new CommandType("CloseMoneyBox", 0)},
            {0x42, new CommandType("UpdateMoneyBox", 0)},
            {0x43, new CommandType("BorderedMessage", 2, 2, 2)},
            {0x44, new CommandType("CloseBorderedMessage", 0)},
            {0x45, new CommandType("CheckerMessage", 2, 2, 1, 1, 2)},
            {0x46, new CommandType("CloseCheckerMessage", 0)},
            {0x47, new CommandType("YesNoBox", 1, 2)},
            {0x48, new CommandType("GenderedMessage", 6, 2, 2, 2, 2, 2, 2)},
            {0x49, new CommandType("VersionMessage", 6, 2, 2, 2, 2, 2, 2)},
            {0x4A, new CommandType("AngryMessage", 2, 2, 1)},
            {0x4B, new CommandType("WaitMessage", 0)},
            {0x4C, new CommandType("SetWordPlayerName", 1, 1)},
            {0x4D, new CommandType("SetWordItem", 2, 1, 2)},
            {0x4E, new CommandType("SetWordItem2", 4, 1, 2, 2, 1)},
            {0x4F, new CommandType("SetWordItem3", 2, 1, 2)},
            {0x50, new CommandType("SetWordTM", 2, 1, 2)},
            {0x51, new CommandType("SetWordMove", 2, 1, 2)},
            {0x52, new CommandType("SetWordItemPocket", 2, 1, 2)},
            {0x53, new CommandType("SetWordPartyPokemon", 2, 1, 2)},
            {0x54, new CommandType("SetWordPartyNickname", 2, 1, 2)},
            {0x55, new CommandType("SetWordDaycarePokemon", 2, 1, 2)},
            {0x56, new CommandType("SetWordType", 2, 1, 2)},
            {0x57, new CommandType("SetWordPokemon", 2, 1, 2)},
            {0x58, new CommandType("SetWordPokemon2", 2, 1, 2)},
            {0x59, new CommandType("SetWordLocation", 2, 1, 2)},
            {0x5A, new CommandType("SetWordPokemonNick", 2, 1, 2)},
            {0x5B, new CommandType("SetWordDaycareNickname", 2, 1, 2)},
            {0x5C, new CommandType("SetWordNumber", 3, 1, 2, 2)},
            {0x5D, new CommandType("SetWordMusicalInfo", 3, 1, 1, 2)},
            {0x5E, new CommandType("SetWordCountry", 2, 1, 2)},
            {0x5F, new CommandType("SetWordActivities", 2, 1, 2)},
            {0x60, new CommandType("SetWordPower", 2, 1, 2)},
            {0x61, new CommandType("SetWordTrainerType", 2, 1, 2)},
            {0x62, new CommandType("SetWordTrainerType2", 2, 1, 2)},
            {0x63, new CommandType("SetWordGeneralWord", 2, 1, 2)},
            {0x64, new CommandType("ApplyMovement", 2, 2, 4)},
            {0x65, new CommandType("WaitMovement", 0)},
            {0x66, new CommandType("GetNPCMovementCode", 2, 2, 2)},
            {0x67, new CommandType("GetNPCPosition", 3, 2, 2, 2)},
            {0x68, new CommandType("GetHeroPosition", 2, 2, 2)},
            {0x69, new CommandType("CreateNPC", 6, 2, 2, 2, 2, 2, 2)},
            {0x6A, new CommandType("GetNPCSpawnFlag", 2, 2, 2)},
            {0x6B, new CommandType("AddNPC", 1, 2)},
            {0x6C, new CommandType("RemoveNPC", 1, 2)},
            {0x6D, new CommandType("SetNPCPosition", 5, 2, 2, 2, 2, 2)},
            {0x6E, new CommandType("GetPlayerDirection", 1, 2)},
            {0x6F, new CommandType("GetNPCInFrontOfPlayer", 2, 2, 2)},
            {0x70, new CommandType("c0x70", 5, 2, 2, 2, 2, 2)},
            {0x71, new CommandType("c0x71", 3, 2, 2, 2)},
            {0x72, new CommandType("c0x72", 4, 2, 2, 2, 2)},
            {0x73, new CommandType("c0x73", 2, 2, 2)},
            {0x74, new CommandType("FacePlayer", 0)},
            {0x75, new CommandType("PlayerPlaySequence", 1, 2)},
            {0x76, new CommandType("c0x76", 4, 2, 2, 2, 2)},
            {0x77, new CommandType("c0x77", 0)},
            {0x78, new CommandType("c0x78", 1, 2)},
            {0x79, new CommandType("c0x79", 3, 2, 2, 2)},
            {0x7A, new CommandType("c0x7A", 1, 2)},
            {0x7B, new CommandType("MoveNpctoCoordinates", 4, 2, 2, 2, 2)},
            {0x7C, new CommandType("c0x7C", 3, 2, 2, 2)},
            {0x7D, new CommandType("c0x7D", 4, 2, 2, 2, 2)},
            {0x7E, new CommandType("TeleportUpNPc", 1, 2)},
            {0x7F, new CommandType("c0x7F", 2, 2, 2)},
            {0x80, new CommandType("c0x80", 1, 2)},
            {0x81, new CommandType("c0x81", 0)},
            {0x82, new CommandType("c0x82", 2, 2, 2)},
            {0x83, new CommandType("SetVarc0x83", 1, 2)},
            {0x84, new CommandType("SetVarc0x84", 1, 2)},
            {0x85, new CommandType("StartTrainerBattle", 3, 2, 2, 2)},
            {0x86, new CommandType("StartMultiTrainerBattle", 4, 2, 2, 2, 2)},
            {0x87, new CommandType("c0x87", 3, 2, 2, 2)},
            {0x88, new CommandType("c0x88", 3, 2, 2, 2)},
            {0x8A, new CommandType("c0x8A", 2, 2, 2)},
            {0x8B, new CommandType("PlayTrainerMusic", 1, 2)},
            {0x8C, new CommandType("TrainerBattleLose", 0)},
            {0x8D, new CommandType("GetTrainerBattleResult", 1, 2)},
            {0x8E, new CommandType("TrainerBattleEnd", 0)},
            {0x8F, new CommandType("c0x8F", 1, 2)},
            {0x90, new CommandType("dvar90", 1, 2)},
            {0x92, new CommandType("dvar92", 2, 2, 2)},
            {0x93, new CommandType("dvar93", 2, 2, 2)},
            {0x94, new CommandType("TrainerBattle", 4, 2, 2, 2, 2)},
            {0x95, new CommandType("DeactiveTrainerId", 1, 2)},
            {0x96, new CommandType("ReactiveTrainerId", 1, 2)},
            {0x97, new CommandType("GetTrainerIDActive", 2, 2, 2)},
            {0x98, new CommandType("ChangeMusic", 1, 2)},
            {0x9B, new CommandType("c0x9B", 2, 2, 2)},
            {0x9E, new CommandType("FadeToDefaultMusic", 0)},
            {0x9F, new CommandType("c0x9F", 1, 2)},
            {0xA0, new CommandType("c0xA0", 0)},
            {0xA1, new CommandType("c0xA1", 1, 2)},
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
            {0xAD, new CommandType("SetupDialogueSelection2", 5, 1, 1, 2, 1, 2)},
            {0xAE, new CommandType("SetupDialogueSelection3", 5, 1, 1, 2, 1, 2)},
            {0xAF, new CommandType("AddDialogueOption", 3, 2, 2, 2)},
            {0xB0, new CommandType("ShowDialogueSelection", 0)},
            {0xB1, new CommandType("ShowDialogueSelection2", 0)},
            {0xB2, new CommandType("SetupDialogueSelection", 5, 1, 1, 2, 1, 2)},
            {0xB3, new CommandType("FadeScreen", 4, 2, 2, 2, 2)},
            {0xB4, new CommandType("ResetScreen", 0)},
            {0xB5, new CommandType("Screenc0xB5", 3, 2, 2, 2)},
            {0xB6, new CommandType("TakeItem", 3, 2, 2, 2)},
            {0xB7, new CommandType("CheckItemBagSpace", 3, 2, 2, 2)},
            {0xB8, new CommandType("CheckItemBagNumber", 3, 2, 2, 2)},
            {0xB9, new CommandType("StoreItemCount", 2, 2, 2)},
            {0xBA, new CommandType("c0xBA", 2, 2, 2)},
            {0xBB, new CommandType("c0xBB", 2, 2, 2)},
            {0xBC, new CommandType("c0xBC", 1, 2)},
            {0xBD, new CommandType("c0xBD", 2, 2, 2)},
            {0xBE, new CommandType("Warp", 4, 2, 2, 2, 2)},
            {0xBF, new CommandType("TeleportWarp", 4, 2, 2, 2, 2)},
            {0xC0, new CommandType("RailWarp", 5, 2, 2, 2, 2, 2)},
            {0xC1, new CommandType("QuicksandWarp", 3, 2, 2, 2)},
            {0xC2, new CommandType("MapChangeWarp", 4, 2, 2, 2, 2)},
            {0xC3, new CommandType("UnionWarp", 0)},
            {0xC4, new CommandType("TeleportWarp2", 5, 2, 2, 2, 2, 2)},
            {0xC5, new CommandType("UseSurf", 0)},
            {0xC6, new CommandType("UseWaterfall", 1, 2)},
            {0xC7, new CommandType("UseCut", 0)},
            {0xC8, new CommandType("UseDive", 1, 2)},
            {0xC9, new CommandType("c0xC9", 0)},
            {0xCA, new CommandType("c0xCA", 1, 2)},
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
            {0xD6, new CommandType("GiveBadge", 1, 2)},
            {0xD7, new CommandType("GetBadgeCount", 1, 2)},
            {0xD8, new CommandType("c0xD8", 2, 2, 2)},
            {0xD9, new CommandType("SetRespawnZone", 1, 2)},
            {0xDA, new CommandType("c0xDA", 3, 2, 2, 2)},
            {0xDB, new CommandType("c0xDB", 0)},
            {0xDC, new CommandType("c0xDC", 5, 2, 2, 2, 2, 2)},
            {0xDD, new CommandType("c0xDD", 2, 2, 2)},
            {0xDE, new CommandType("SpeciesDisplayDE", 2, 2, 2)},
            {0xDF, new CommandType("c0xDF", 3, 2, 2, 2)},
            {0xE0, new CommandType("GetVersion", 1, 2)},
            {0xE1, new CommandType("GetHeroGender", 1, 2)},
            {0xE2, new CommandType("c0xE2", 1, 2)},
            {0xE3, new CommandType("EnableRunningShoes", 0)},
            {0xE4, new CommandType("c0xE4", 1, 2)},
            {0xE5, new CommandType("c0xE5", 2, 2, 2)},
            {0xE6, new CommandType("c0xE6", 2, 2, 2)},
            {0xE7, new CommandType("c0xE7", 1, 2)},
            {0xE8, new CommandType("c0xE8", 2, 2, 2)},
            {0xEA, new CommandType("StoreEA", 1, 2)},
            {0xEB, new CommandType("StoreEB", 1, 2)},
            {0xEC, new CommandType("StoreEC", 0)},
            {0xED, new CommandType("StoreED", 0)},
            {0xEE, new CommandType("StoreEE", 1, 2)},
            {0xEF, new CommandType("StoreEF", 1, 2)},
            {0xF0, new CommandType("DaycareDeposit", 1, 2)},
            {0xF1, new CommandType("DaycareWithdraw", 1, 2)},
            {0xF2, new CommandType("DaycareGetSpecies", 2, 2, 2)},
            {0xF3, new CommandType("DaycareGetForm", 2, 2, 2)},
            {0xF4, new CommandType("DaycareGetNewLevel", 2, 2, 2)},
            {0xF5, new CommandType("DaycareGetLevelGain", 2, 2, 2)},
            {0xF6, new CommandType("DaycareGetWithdrawCost", 2, 2, 2)},
            {0xF7, new CommandType("RequestPokemonForDayCare", 1, 2)},
            {0xF8, new CommandType("DaycareGetGender", 2, 2, 2)},
            {0xF9, new CommandType("AddMoney", 1, 2)},
            {0xFA, new CommandType("TakeMoney", 1, 2)},
            {0xFB, new CommandType("CheckMoney", 2, 2, 2)},
            {0xFC, new CommandType("GetPartyFriendship", 2, 2, 2)},
            {0xFD, new CommandType("ChangePartyFriendship", 3, 2, 2, 2)},
            {0xFE, new CommandType("GetPartySpecies", 2, 2, 2)},
            {0xFF, new CommandType("GetPartyForm", 2, 2, 2)},
            {0x100, new CommandType("CheckPokerusInParty", 1, 2)},
            {0x101, new CommandType("GetPartyIsFullHp", 2, 2, 2)},
            {0x102, new CommandType("GetPartyIsEgg", 2, 2, 2)},
            {0x103, new CommandType("GetPartyCount", 2, 2, 2)},
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
            {0x110, new CommandType("GetPokemonParam", 3, 2, 2, 2)},
            {0x111, new CommandType("SetPokemonIV", 3, 2, 2, 2)},
            {0x112, new CommandType("GetPokemonEVTotal", 2, 2, 2)},
            {0x113, new CommandType("c0x113", 2, 2, 2)},
            {0x114, new CommandType("c0x114", 2, 2, 2)},
            {0x115, new CommandType("StorePartyCanLearnMove", 3, 2, 2, 2)},
            {0x116, new CommandType("SetVarPartyHasMove", 2, 2, 2)},
            {0x117, new CommandType("SetPokemonForm", 2, 2, 2)},
            {0x118, new CommandType("c0x118", 3, 2, 2, 2)},
            {0x119, new CommandType("c0x119", 3, 2, 2, 2)},
            {0x11A, new CommandType("c0x11A", 4, 2, 2, 2, 2)},
            {0x11B, new CommandType("StorePartyType", 3, 2, 2, 2)},
            {0x11C, new CommandType("c0x11C", 3, 2, 2, 2)},
            {0x11D, new CommandType("SetFavorite", 1, 2)},
            {0x11E, new CommandType("c0x11E", 1, 2)},
            {0x11F, new CommandType("c0x11F", 2, 2, 2)},
            {0x120, new CommandType("c0x120", 2, 2, 2)},
            {0x121, new CommandType("c0x121", 2, 2, 2)},
            {0x122, new CommandType("AddBoxPokemon", 4, 2, 2, 2, 2)},
            {0x123, new CommandType("AddBoxPokemon2", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x124, new CommandType("c0x124", 2, 2, 2)},
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
            {0x12F, new CommandType("c0x12F", 1, 2)},
            {0x130, new CommandType("BootPCSound", 0)},
            {0x131, new CommandType("PC_131", 0)},
            {0x132, new CommandType("c0x132", 1, 2)},
            {0x134, new CommandType("c0x134", 0)},
            {0x136, new CommandType("SetWeather", 2, 2, 2)},
            {0x137, new CommandType("c0x137", 1, 2)},
            {0x138, new CommandType("c0x138", 3, 2, 2, 2)},
            {0x139, new CommandType("c0x139", 1, 2)},
            {0x13A, new CommandType("c0x13A", 1, 2)},
            {0x13B, new CommandType("c0x13B", 1, 2)},
            {0x13C, new CommandType("c0x13C", 0)},
            {0x13D, new CommandType("c0x13D", 0)},
            {0x13E, new CommandType("c0x13E", 0)},
            {0x13F, new CommandType("StartCameraEvent", 0)},
            {0x140, new CommandType("StopCameraEvent", 0)},
            {0x141, new CommandType("LockCamera", 0)},
            {0x142, new CommandType("ReleaseCamera", 0)},
            {0x143, new CommandType("MoveCamera", 7, 2, 2, 4, 4, 4, 4, 2)},
            {0x144, new CommandType("c0x144", 1, 2)},
            {0x145, new CommandType("EndCameraEvent", 0)},
            {0x146, new CommandType("c0x146", 2, 2, 2)},
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
            {0x157, new CommandType("c0x157", 1, 2)},
            {0x158, new CommandType("c0x158", 0)},
            {0x159, new CommandType("c0x159", 1, 2)},
            {0x15A, new CommandType("c0x15A", 0)},
            {0x15B, new CommandType("c0x15B", 1, 2)},
            {0x15C, new CommandType("c0x15C", 1, 2)},
            {0x15D, new CommandType("c0x15D", 0)},
            {0x15E, new CommandType("c0x15E", 0)},
            {0x15F, new CommandType("c0x15F", 0)},
            {0x160, new CommandType("c0x160", 2, 2, 2)},
            {0x161, new CommandType("c0x161", 1, 2)},
            {0x162, new CommandType("c0x162", 0)},
            {0x163, new CommandType("c0x163", 2, 2, 1)},
            {0x164, new CommandType("c0x164", 1, 2)},
            {0x165, new CommandType("c0x165", 3, 1, 2, 2)},
            {0x166, new CommandType("c0x166", 3, 2, 1, 2)},
            {0x167, new CommandType("c0x167", 4, 2, 2, 2, 2)},
            {0x168, new CommandType("c0x168", 1, 2)},
            {0x169, new CommandType("c0x169", 1, 2)},
            {0x16A, new CommandType("c0x16A", 2, 2, 2)},
            {0x16E, new CommandType("c0x16E", 0)},
            {0x170, new CommandType("c0x170", 1, 2)},
            {0x172, new CommandType("SetVar172", 1, 2)},
            {0x174, new CommandType("StartWildBattle", 3, 2, 2, 2)},
            {0x175, new CommandType("WildBattleEnd", 0)},
            {0x176, new CommandType("WildBattleLose", 0)},
            {0x177, new CommandType("SetVarBattle177", 1, 2)},
            {0x178, new CommandType("BattleStoreResult", 1, 2)},
            {0x179, new CommandType("c0x179", 0)},
            {0x17A, new CommandType("c0x17A", 1, 2)},
            {0x17B, new CommandType("c0x17B", 2, 2, 2)},
            {0x17C, new CommandType("c0x17C", 1, 2)},
            {0x17D, new CommandType("c0x17D", 1, 2)},
            {0x186, new CommandType("c0x186", 0)},
            {0x187, new CommandType("c0x187", 1, 2)},
            {0x18C, new CommandType("c0x18C", 2, 2, 2)},
            {0x18D, new CommandType("c0x18D", 1, 2)},
            {0x18E, new CommandType("c0x18E", 1, 2)},
            {0x18F, new CommandType("c0x18F", 1, 2)},
            {0x190, new CommandType("c0x190", 1, 2)},
            {0x191, new CommandType("c0x191", 1, 2)},
            {0x197, new CommandType("c0x197", 1, 2)},
            {0x198, new CommandType("c0x198", 1, 2)},
            {0x199, new CommandType("c0x199", 0)},
            {0x19A, new CommandType("c0x19A", 1, 2)},
            {0x19B, new CommandType("c0x19B", 1, 2)},
            {0x19C, new CommandType("c0x19C", 1, 2)},
            {0x19D, new CommandType("c0x19D", 1, 2)},
            {0x1A1, new CommandType("c0x1A1", 4, 2, 2, 2, 2)},
            {0x1A2, new CommandType("c0x1A2", 1, 2)},
            {0x1A3, new CommandType("FadeFromBlack2", 0)},
            {0x1A4, new CommandType("FadeIntoBlack2", 0)},
            {0x1A5, new CommandType("FadeFromWhite2", 0)},
            {0x1A6, new CommandType("FadeIntoWhite2", 0)},
            {0x1A7, new CommandType("WaitFade", 0)},
            {0x1A8, new CommandType("c0x1A8", 0)},
            {0x1AA, new CommandType("DisplaySeasonBanner", 0)},
            {0x1AB, new CommandType("FadeFromBlack", 0)},
            {0x1AC, new CommandType("FadeIntoBlack", 0)},
            {0x1AD, new CommandType("FadeFromWhite", 0)},
            {0x1AE, new CommandType("FadeIntoWhite", 0)},
            {0x1AF, new CommandType("c0x1AF", 2, 2, 2)},
            {0x1B0, new CommandType("c0x1B0", 2, 2, 2)},
            {0x1B1, new CommandType("EliteFourLiftWarp", 0)},
            {0x1B2, new CommandType("c0x1B2", 1, 2)},
            {0x1B3, new CommandType("c0x1B3", 2, 2, 2)},
            {0x1B4, new CommandType("TradeNPCStart", 2, 2, 2)},
            {0x1B5, new CommandType("TradeNPCQualify", 3, 2, 2, 2)},
            {0x1C1, new CommandType("c0x1C1", 1, 4)},
            {0x1C2, new CommandType("c0x1C2", 0)},
            {0x1C3, new CommandType("c0x1C3", 1, 2)},
            {0x1C4, new CommandType("StoreTrainerID", 2, 2, 2)},
            {0x1C5, new CommandType("c0x1C5", 4, 2, 2, 2, 2)},
            {0x1C6, new CommandType("EnableNationalDex", 0)},
            {0x1C7, new CommandType("GetNationalDexEnabled", 1, 2)},
            {0x1C8, new CommandType("c0x1C8", 0)},
            {0x1C9, new CommandType("c0x1C9", 3, 2, 2, 2)},
            {0x1CA, new CommandType("c0x1CA", 0)},
            {0x1CB, new CommandType("c0x1CB", 0)},
            {0x1CC, new CommandType("c0x1CC", 1, 2)},
            {0x1CD, new CommandType("c0x1CD", 1, 2)},
            {0x1CE, new CommandType("c0x1CE", 2, 2, 2)},
            {0x1CF, new CommandType("c0x1CF", 2, 2, 2)},
            {0x1D0, new CommandType("c0x1D0", 4, 2, 2, 2, 2)},
            {0x1D1, new CommandType("c0x1D1", 2, 2, 2)},
            {0x1D2, new CommandType("c0x1D2", 3, 2, 2, 2)},
            {0x1D3, new CommandType("c0x1D3", 3, 2, 2, 2)},
            {0x1D4, new CommandType("c0x1D4", 3, 2, 2, 2)},
            {0x1D5, new CommandType("c0x1D5", 2, 2, 2)},
            {0x1D6, new CommandType("c0x1D6", 2, 2, 2)},
            {0x1D7, new CommandType("c0x1D7", 4, 2, 2, 2, 2)},
            {0x1D8, new CommandType("c0x1D8", 4, 2, 2, 2, 2)},
            {0x1D9, new CommandType("c0x1D9", 5, 2, 2, 2, 2, 2)},
            {0x1DA, new CommandType("c0x1DA", 4, 2, 2, 2, 2)},
            {0x1DB, new CommandType("c0x1DB", 2, 2, 2)},
            {0x1DC, new CommandType("c0x1DC", 2, 2, 2)},
            {0x1DD, new CommandType("c0x1DD", 3, 2, 2, 2)},
            {0x1DE, new CommandType("c0x1DE", 2, 2, 2)},
            {0x1DF, new CommandType("c0x1DF", 0)},
            {0x1E0, new CommandType("c0x1E0", 4, 2, 2, 2, 2)},
            {0x1E1, new CommandType("c0x1E1", 0)},
            {0x1E2, new CommandType("c0x1E2", 0)},
            {0x1E3, new CommandType("c0x1E3", 3, 2, 2, 2)},
            {0x1E4, new CommandType("c0x1E4", 6, 2, 2, 2, 2, 2, 2)},
            {0x1E5, new CommandType("c0x1E5", 0)},
            {0x1E6, new CommandType("c0x1E6", 0)},
            {0x1E7, new CommandType("c0x1E7", 0)},
            {0x1E8, new CommandType("c0x1E8", 1, 2)},
            {0x1E9, new CommandType("c0x1E9", 3, 2, 2, 2)},
            {0x1EA, new CommandType("c0x1EA", 2, 2, 2)},
            {0x1EB, new CommandType("c0x1EB", 2, 2, 2)},
            {0x1EC, new CommandType("c0x1EC", 0)},
            {0x1ED, new CommandType("c0x1ED", 1, 2)},
            {0x1EE, new CommandType("c0x1EE", 1, 2)},
            {0x1EF, new CommandType("c0x1EF", 1, 2)},
            {0x1F0, new CommandType("c0x1F0", 1, 2)},
            {0x1F1, new CommandType("c0x1F1", 2, 2, 2)},
            {0x1F2, new CommandType("c0x1F2", 1, 2)},
            {0x1F3, new CommandType("c0x1F3", 1, 2)},
            {0x1F4, new CommandType("c0x1F4", 2, 2, 2)},
            {0x1F5, new CommandType("c0x1F5", 1, 2)},
            {0x1F6, new CommandType("c0x1F6", 4, 2, 2, 2, 2)},
            {0x1F7, new CommandType("c0x1F7", 4, 2, 2, 2, 2)},
            {0x1F8, new CommandType("c0x1F8", 2, 2, 2)},
            {0x1F9, new CommandType("c0x1F9", 4, 2, 2, 2, 2)},
            {0x1FA, new CommandType("c0x1FA", 4, 2, 2, 2, 2)},
            {0x1FB, new CommandType("c0x1FB", 2, 2, 2)},
            {0x1FC, new CommandType("c0x1FC", 3, 2, 2, 2)},
            {0x1FD, new CommandType("c0x1FD", 4, 2, 2, 2, 2)},
            {0x1FE, new CommandType("c0x1FE", 0)},
            {0x1FF, new CommandType("c0x1FF", 1, 2)},
            {0x200, new CommandType("c0x200", 3, 2, 2, 2)},
            {0x201, new CommandType("c0x201", 2, 2, 2)},
            {0x202, new CommandType("c0x202", 2, 2, 2)},
            {0x203, new CommandType("c0x203", 1, 2)},
            {0x204, new CommandType("c0x204", 1, 2)},
            {0x205, new CommandType("c0x205", 1, 2)},
            {0x206, new CommandType("c0x206", 1, 2)},
            {0x207, new CommandType("c0x207", 1, 1)},
            {0x208, new CommandType("c0x208", 1, 1)},
            {0x209, new CommandType("c0x209", 2, 2, 2)},
            {0x20A, new CommandType("c0x20A", 0)},
            {0x20B, new CommandType("c0x20B", 1, 2)},
            {0x20C, new CommandType("c0x20C", 0)},
            {0x20D, new CommandType("c0x20D", 4, 2, 2, 2, 2)},
            {0x20E, new CommandType("c0x20E", 6, 2, 2, 2, 2, 2, 2)},
            {0x20F, new CommandType("c0x20F", 4, 2, 2, 2, 2)},
            {0x210, new CommandType("c0x210", 1, 2)},
            {0x211, new CommandType("c0x211", 1, 2)},
            {0x212, new CommandType("c0x212", 3, 2, 2, 2)},
            {0x213, new CommandType("c0x213", 1, 2)},
            {0x214, new CommandType("c0x214", 4, 2, 2, 2, 2)},
            {0x215, new CommandType("c0x215", 2, 2, 2)},
            {0x216, new CommandType("c0x216", 2, 2, 2)},
            {0x217, new CommandType("c0x217", 1, 2)},
            {0x218, new CommandType("c0x218", 2, 2, 2)},
            {0x219, new CommandType("c0x219", 2, 2, 2)},
            {0x21A, new CommandType("c0x21A", 2, 2, 2)},
            {0x21B, new CommandType("c0x21B", 0)},
            {0x21C, new CommandType("c0x21C", 2, 2, 2)},
            {0x21D, new CommandType("c0x21D", 1, 2)},
            {0x21E, new CommandType("c0x21E", 1, 2)},
            {0x21F, new CommandType("c0x21F", 2, 2, 2)},
            {0x220, new CommandType("c0x220", 2, 2, 2)},
            {0x221, new CommandType("c0x221", 2, 2, 2)},
            {0x222, new CommandType("c0x222", 1, 2)},
            {0x223, new CommandType("GetHiddenPowerType", 2, 2, 2)},
            {0x224, new CommandType("GetPokemonIV", 3, 2, 2, 2)},
            {0x225, new CommandType("c0x225", 1, 2)},
            {0x226, new CommandType("c0x226", 1, 2)},
            {0x227, new CommandType("c0x227", 2, 2, 2)},
            {0x228, new CommandType("c0x228", 2, 2, 2)},
            {0x22A, new CommandType("c0x22A", 2, 2, 2)},
            {0x22B, new CommandType("c0x22B", 2, 2, 2)},
            {0x22D, new CommandType("c0x22D", 1, 2)},
            {0x22E, new CommandType("c0x22E", 1, 2)},
            {0x22F, new CommandType("c0x22F", 2, 2, 2)},
            {0x230, new CommandType("c0x230", 2, 2, 2)},
            {0x231, new CommandType("c0x231", 1, 2)},
            {0x232, new CommandType("c0x232", 2, 2, 2)},
            {0x233, new CommandType("c0x233", 1, 2)},
            {0x234, new CommandType("c0x234", 4, 2, 2, 2, 2)},
            {0x235, new CommandType("c0x235", 2, 2, 2)},
            {0x236, new CommandType("c0x236", 4, 2, 2, 2, 2)},
            {0x237, new CommandType("c0x237", 2, 2, 2)},
            {0x238, new CommandType("c0x238", 1, 2)},
            {0x239, new CommandType("c0x239", 1, 2)},
            {0x23A, new CommandType("c0x23A", 2, 2, 2)},
            {0x23B, new CommandType("c0x23B", 0)},
            {0x23C, new CommandType("c0x23C", 0)},
            {0x23D, new CommandType("c0x23D", 0)},
            {0x23E, new CommandType("c0x23E", 3, 2, 2, 2)},
            {0x23F, new CommandType("Close23F", 0)},
            {0x240, new CommandType("c0x240", 2, 2, 2)},
            {0x241, new CommandType("c0x241", 1, 2)},
            {0x245, new CommandType("c0x245", 1, 2)},
            {0x246, new CommandType("c0x246", 1, 2)},
            {0x247, new CommandType("c0x247", 5, 2, 2, 2, 2, 2)},
            {0x248, new CommandType("c0x248", 4, 2, 2, 2, 2)},
            {0x249, new CommandType("c0x249", 6, 2, 2, 2, 2, 2, 2)},
            {0x24A, new CommandType("c0x24A", 1, 2)},
            {0x24B, new CommandType("c0x24B", 0)},
            {0x24C, new CommandType("c0x24C", 0)},
            {0x24D, new CommandType("c0x24D", 0)},
            {0x24E, new CommandType("c0x24E", 2, 2, 2)},
            {0x24F, new CommandType("NPCPathFind", 6, 2, 2, 2, 2, 2, 2)},
            {0x250, new CommandType("c0x250", 5, 2, 2, 2, 2, 2)},
            {0x251, new CommandType("c0x251", 2, 2, 2)},
            {0x252, new CommandType("c0x252", 1, 2)},
            {0x253, new CommandType("c0x253", 1, 1)},
            {0x25A, new CommandType("c0x25A", 1, 2)},
            {0x25B, new CommandType("c0x25B", 0)},
            {0x25C, new CommandType("c0x25C", 3, 2, 2, 2)},
            {0x25D, new CommandType("c0x25D", 1, 2)},
            {0x25F, new CommandType("c0x25F", 1, 2)},
            {0x262, new CommandType("c0x262", 2, 2, 2)},
            {0x263, new CommandType("c0x263", 1, 2)},
            {0x264, new CommandType("c0x264", 2, 2, 2)},
            {0x265, new CommandType("c0x265", 1, 2)},
            {0x266, new CommandType("c0x266", 1, 2)},
            {0x267, new CommandType("c0x267", 0)},
            {0x268, new CommandType("c0x268", 1, 2)},
            {0x269, new CommandType("c0x269", 0)},
            {0x26C, new CommandType("StoreMedals26C", 2, 1, 2)},
            {0x26D, new CommandType("StoreMedals26D", 2, 1, 2)},
            {0x26E, new CommandType("CountMedals26E", 2, 1, 2)},
            {0x271, new CommandType("c0x271", 2, 2, 2)},
            {0x272, new CommandType("c0x272", 2, 2, 2)},
            {0x273, new CommandType("c0x273", 1, 2)},
            {0x274, new CommandType("c0x274", 0)},
            {0x275, new CommandType("c0x275", 3, 1, 2, 2)},
            {0x276, new CommandType("c0x276", 2, 2, 2)},
            {0x277, new CommandType("c0x277", 0)},
            {0x278, new CommandType("c0x278", 3, 2, 2, 2)},
            {0x279, new CommandType("c0x279", 0)},
            {0x27A, new CommandType("c0x27A", 2, 2, 2)},
            {0x27B, new CommandType("c0x27B", 4, 2, 2, 2, 2)},
            {0x27C, new CommandType("c0x27C", 2, 2, 2)},
            {0x27D, new CommandType("c0x27D", 3, 2, 2, 2)},
            {0x27E, new CommandType("c0x27E", 2, 2, 2)},
            {0x27F, new CommandType("c0x27F", 2, 2, 2)},
            {0x284, new CommandType("c0x284", 2, 2, 2)},
            {0x285, new CommandType("c0x285", 3, 2, 2, 2)},
            {0x28A, new CommandType("c0x28A", 2, 2, 2)},
            {0x28B, new CommandType("c0x28B", 0)},
            {0x28E, new CommandType("c0x28E", 1, 2)},
            {0x28F, new CommandType("c0x28F", 1, 2)},
            {0x290, new CommandType("c0x290", 1, 1)},
            {0x291, new CommandType("c0x291", 1, 2)},
            {0x292, new CommandType("c0x292", 1, 2)},
            {0x293, new CommandType("GrottoWarpIn", 1, 2)},
            {0x294, new CommandType("GrottoGetParam", 2, 2, 2)},
            {0x295, new CommandType("GrottoReset", 0)},
            {0x296, new CommandType("GrottoWarpOut", 0)},
            {0x297, new CommandType("StartWildBattle2", 4, 2, 2, 2, 2)},
            {0x298, new CommandType("c0x298", 2, 1, 2)},
            {0x299, new CommandType("SetWordAbility", 2, 1, 2)},
            {0x29A, new CommandType("SetWordNature", 2, 1, 2)},
            {0x29B, new CommandType("c0x29B", 1, 1)},
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
            {0x2AC, new CommandType("c0x2AC", 0)},
            {0x2AE, new CommandType("c0x2AE", 1, 2)},
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
            {0x2C0, new CommandType("c0x2C0", 2, 2, 2)},
            {0x2C2, new CommandType("c0x2C2", 1, 2)},
            {0x2C3, new CommandType("c0x2C3", 1, 2)},
            {0x2C4, new CommandType("c0x2C4", 0)},
            {0x2C5, new CommandType("c0x2C5", 1, 2)},
            {0x2C6, new CommandType("c0x2C6", 0)},
            {0x2C7, new CommandType("c0x2C7", 0)},
            {0x2CA, new CommandType("c0x2CA", 1, 2)},
            {0x2CB, new CommandType("c0x2CB", 1, 2)},
            {0x2CF, new CommandType("c0x2CF", 4, 2, 2, 2, 2)},
            {0x2D0, new CommandType("EnableHabitatList", 0)},
            {0x2D1, new CommandType("c0x2D1", 1, 2)},
            {0x2D2, new CommandType("c0x2D2", 0)},
            {0x2D3, new CommandType("c0x2D3", 2, 2, 2)},
            {0x2D4, new CommandType("c0x2D4", 1, 2)},
            {0x2D5, new CommandType("c0x2D5", 2, 2, 2)},
            {0x2D7, new CommandType("c0x2D7", 2, 2, 2)},
            {0x2D8, new CommandType("c0x2D8", 1, 2)},
            {0x2D9, new CommandType("c0x2D9", 2, 2, 2)},
            {0x2DA, new CommandType("c0x2DA", 1, 2)},
            {0x2DB, new CommandType("c0x2DB", 2, 2, 2)},
            {0x2DC, new CommandType("c0x2DC", 3, 2, 2, 2)},
            {0x2DD, new CommandType("StoreUnityVisitors", 1, 2)},
            {0x2DE, new CommandType("c0x2DE", 1, 2)},
            {0x2DF, new CommandType("StoreMyActivities", 1, 2)},
            {0x2E1, new CommandType("c0x2E1", 0)},
            {0x2E3, new CommandType("c0x2E3", 0)},
            {0x2E5, new CommandType("GrottoSetEncounter", 4, 2, 2, 2, 2)},
            {0x2E6, new CommandType("GrottoCreateEvents", 0)},
            {0x2E8, new CommandType("c0x2E8", 2, 2, 2)},
            {0x2E9, new CommandType("c0x2E9", 2, 2, 2)},
            {0x2EA, new CommandType("GivePokemon_N", 6, 2, 2, 2, 2, 2, 2)},
            {0x2ED, new CommandType("c0x2ED", 2, 2, 2)},
            {0x2EE, new CommandType("Prop2EE", 2, 2, 2)},
            {0x2EF, new CommandType("c0x2EF", 1, 2)},
            {0x2F1, new CommandType("c0x2F1", 1, 2)},
            {0x2F2, new CommandType("c0x2F2", 2, 2, 2)},
        };

        internal static Dictionary<int, CommandType> bw2PokeScriptCommandList = new Dictionary<int, CommandType>()
        {
            {0x0, new CommandType("VMNop", 0)},
            {0x1, new CommandType("VMNop2", 0)},
            {0x2, new CommandType("VMHalt", 0)},
            {0x3, new CommandType("VMSleep", 1, 2)},
            {0x4, new CommandType("VMCall", 1, 4)},
            {0x5, new CommandType("VMReturn", 0)},
            {0x6, new CommandType("DebugPrint", 1, 2)},
            {0x7, new CommandType("DebugStack", 1, 2)},
            {0x8, new CommandType("StackPushConst", 1, 2)},
            {0x9, new CommandType("StackPush", 1, 2)},
            {0xA, new CommandType("StackPop", 1, 2)},
            {0xB, new CommandType("StackDiscard", 0)},
            {0xC, new CommandType("StackAdd", 0)},
            {0xD, new CommandType("StackSub", 0)},
            {0xE, new CommandType("StackMul", 0)},
            {0xF, new CommandType("StackDiv", 0)},
            {0x10, new CommandType("StackPushFlag", 1, 2)},
            {0x11, new CommandType("StackCmp", 1, 2)},
            {0x12, new CommandType("WorkAnd", 2, 2, 2)},
            {0x13, new CommandType("WorkOr", 2, 2, 2)},
            {0x14, new CommandType("VMRegSet8", 2, 1, 1)},
            {0x15, new CommandType("VMRegSet32", 2, 1, 4)},
            {0x16, new CommandType("VMRegMov", 2, 1, 1)},
            {0x17, new CommandType("VMRegCmp8", 2, 1, 1)},
            {0x18, new CommandType("VMRegCmpConst8", 2, 1, 1)},
            {0x19, new CommandType("WorkCmpConst", 2, 2, 2)},
            {0x1A, new CommandType("WorkCmpWork", 2, 2, 2)},
            {0x1B, new CommandType("RTCallGlobalAsync", 1, 2)},
            {0x1C, new CommandType("RTCallGlobal", 1, 2)},
            {0x1D, new CommandType("RTEndGlobal", 0)},
            {0x1E, new CommandType("VMJump", 1, 4)},
            {0x1F, new CommandType("VMJumpIf", 2, 1, 4)},
            {0x20, new CommandType("VMCallIf", 2, 1, 4)},
            {0x21, new CommandType("RTReserveScript", 1, 2)},
            {0x22, new CommandType("FieldGetContinueFlag", 1, 2)},
            {0x23, new CommandType("FlagSet", 1, 2)},
            {0x24, new CommandType("FlagReset", 1, 2)},
            {0x25, new CommandType("FlagGet", 2, 2, 2)},
            {0x26, new CommandType("WorkAdd", 2, 2, 2)},
            {0x27, new CommandType("WorkSub", 2, 2, 2)},
            {0x28, new CommandType("WorkSetConst", 2, 2, 2)},
            {0x29, new CommandType("WorkGet", 2, 2, 2)},
            {0x2A, new CommandType("WorkSet", 2, 2, 2)},
            {0x2B, new CommandType("WorkMul", 2, 2, 2)},
            {0x2C, new CommandType("WorkDiv", 2, 2, 2)},
            {0x2D, new CommandType("WorkMod", 2, 2, 2)},
            {0x2E, new CommandType("ActorPauseAll", 0)},
            {0x2F, new CommandType("ActorUnlockAll", 0)},
            {0x30, new CommandType("RTFinishSubEvents", 0)},
            {0x31, new CommandType("InputWaitAB", 0)},
            {0x32, new CommandType("InputWaitLast", 0)},
            {0x33, new CommandType("MsgSetAutoscrolls", 1, 2)},
            {0x34, new CommandType("MsgSystem", 2, 2, 2)},
            {0x35, new CommandType("MsgSystemAsync", 2, 2, 2)},
            {0x36, new CommandType("MsgSystemClose", 0)},
            {0x37, new CommandType("MsgSetLoadingSpinner", 1, 2)},
            {0x38, new CommandType("MsgInfo", 2, 2, 1)},
            {0x39, new CommandType("MsgInfoClose", 0)},
            {0x3A, new CommandType("MsgMulti", 4, 2, 2, 2, 2)},
            {0x3B, new CommandType("MsgWinCloseNo", 1, 2)},
            {0x3C, new CommandType("MsgActorEx", 5, 2, 2, 2, 2, 2)},
            {0x3D, new CommandType("MsgActor", 4, 2, 2, 2, 2)},
            {0x3E, new CommandType("MsgActorClose", 0)},
            {0x3F, new CommandType("MsgWinCloseAll", 0)},
            {0x40, new CommandType("MoneyWinDisp", 2, 2, 2)},
            {0x41, new CommandType("MoneyWinClose", 0)},
            {0x42, new CommandType("MoneyWinUpdate", 0)},
            {0x43, new CommandType("MsgPlaceSign", 2, 2, 2)},
            {0x44, new CommandType("MsgPlaceSignClose", 0)},
            {0x45, new CommandType("MsgCheckerBG", 4, 2, 1, 1, 2)},
            {0x46, new CommandType("MsgCheckerBGClose", 0)},
            {0x47, new CommandType("YesNoWin", 1, 2)},
            {0x48, new CommandType("MsgActorGendered", 6, 2, 2, 2, 2, 2, 2)},
            {0x49, new CommandType("MsgActorVersioned", 6, 2, 2, 2, 2, 2, 2)},
            {0x4A, new CommandType("MsgScream", 2, 2, 1)},
            {0x4B, new CommandType("MsgWaitAdvance", 0)},
            {0x4C, new CommandType("WordSetPlayerName", 1, 1)},
            {0x4D, new CommandType("WordSetItemName", 2, 1, 2)},
            {0x4E, new CommandType("WordSetItemNameEx", 4, 1, 2, 2, 1)},
            {0x4F, new CommandType("WordSetItemNameWithArticle", 2, 1, 2)},
            {0x50, new CommandType("WordSetTMMoveName", 2, 1, 2)},
            {0x51, new CommandType("WordSetMoveName", 2, 1, 2)},
            {0x52, new CommandType("WordSetItemPocketName", 2, 1, 2)},
            {0x53, new CommandType("WordSetPartyPokeSpecies", 2, 1, 2)},
            {0x54, new CommandType("WordSetPartyPokeName", 2, 1, 2)},
            {0x55, new CommandType("WordSetDaycarePokeSpecies", 2, 1, 2)},
            {0x56, new CommandType("WordSetPokeTypeName", 2, 1, 2)},
            {0x57, new CommandType("WordSetPokeSpecies", 2, 1, 2)},
            {0x58, new CommandType("WordSetPokeSpeciesWithArticle", 2, 1, 2)},
            {0x59, new CommandType("WordSetPlaceName", 2, 1, 2)},
            {0x5A, new CommandType("WordSetTrendName", 2, 1, 2)},
            {0x5B, new CommandType("WordSetDaycarePokeName", 2, 1, 2)},
            {0x5C, new CommandType("WordSetNumber", 3, 1, 2, 2)},
            {0x5D, new CommandType("WordSetMusicalInfo", 3, 1, 1, 2)},
            {0x5E, new CommandType("WordSetCountry", 2, 1, 2)},
            {0x5F, new CommandType("WordSetHobbyName", 2, 1, 2)},
            {0x60, new CommandType("WordSetPassPowerName", 2, 1, 2)},
            {0x61, new CommandType("WordSetTrainerClassName", 2, 1, 2)},
            {0x62, new CommandType("WordSetTrainerClassNameWithArticle", 2, 1, 2)},
            {0x63, new CommandType("WordSetSurveyAnswer", 2, 1, 2)},
            {0x64, new CommandType("ActorCmdExec", 2, 2, 4)},
            {0x65, new CommandType("ActorCmdWait", 0)},
            {0x66, new CommandType("ActorGetMoveCode", 2, 2, 2)},
            {0x67, new CommandType("ActorGetGPos", 3, 2, 2, 2)},
            {0x68, new CommandType("PlayerGetGPos", 2, 2, 2)},
            {0x69, new CommandType("ActorNew", 6, 2, 2, 2, 2, 2, 2)},
            {0x6A, new CommandType("ActorGetSpawnFlag", 2, 2, 2)},
            {0x6B, new CommandType("ActorAdd", 1, 2)},
            {0x6C, new CommandType("ActorDelete", 1, 2)},
            {0x6D, new CommandType("ActorSetGPos", 5, 2, 2, 2, 2, 2)},
            {0x6E, new CommandType("PlayerGetDir", 1, 2)},
            {0x6F, new CommandType("PlayerGetActorInFront", 2, 2, 2)},
            {0x70, new CommandType("ActorFindByGPos", 5, 2, 2, 2, 2, 2)},
            {0x71, new CommandType("PlayerGetRailPos", 3, 2, 2, 2)},
            {0x72, new CommandType("ActorGetRailPos", 4, 2, 2, 2, 2)},
            {0x73, new CommandType("ActorSetMoveCode", 2, 2, 2)},
            {0x74, new CommandType("ActorSetEyeToEye", 0)},
            {0x75, new CommandType("PlayerSetSpecialSequence", 1, 2)},
            {0x76, new CommandType("PlayerMoveToYAsync", 4, 2, 2, 2, 2)},
            {0x77, new CommandType("PlayerTurnByTrigger", 0)},
            {0x78, new CommandType("PlayerGetExState", 1, 2)},
            {0x79, new CommandType("ActorGetUserParam", 3, 2, 2, 2)},
            {0x7A, new CommandType("ActorPlayRailSlipdown", 1, 2)},
            {0x7B, new CommandType("ActorJumpToGPos", 4, 2, 2, 2, 2)},
            {0x7C, new CommandType("PlayerSetRailPos", 3, 2, 2, 2)},
            {0x7D, new CommandType("ActorSetRailPos", 4, 2, 2, 2, 2)},
            {0x7E, new CommandType("ActorPlayTeleportSeq", 1, 2)},
            {0x7F, new CommandType("TrainerEyeGetTrainerID", 2, 2, 2)},
            {0x80, new CommandType("TrainerEyeEventInit", 1, 2)},
            {0x81, new CommandType("TrainerEyeEventStart", 0)},
            {0x82, new CommandType("TrainerEyeGetActorID", 2, 2, 2)},
            {0x83, new CommandType("TrainerEyeGetBattleType", 1, 2)},
            {0x84, new CommandType("ActorGetTrainerID", 1, 2)},
            {0x85, new CommandType("CallTrainerBattle", 3, 2, 2, 2)},
            {0x86, new CommandType("CallTrainerMultiBattle", 4, 2, 2, 2, 2)},
            {0x87, new CommandType("TrainerEyeSayMessage", 3, 2, 2, 2)},
            {0x88, new CommandType("TrainerEyeGetMessageTypes", 3, 2, 2, 2)},
            {0x8A, new CommandType("TrainerGetBattleType", 2, 2, 2)},
            {0x8B, new CommandType("TrainerBGMPlayPush", 1, 2)},
            {0x8C, new CommandType("CallTrainerLose", 0)},
            {0x8D, new CommandType("TrainerBattleIsVictory", 1, 2)},
            {0x8E, new CommandType("CallTrainerBattleEnd", 0)},
            {0x8F, new CommandType("TrainerClassBGMPlayPush", 1, 2)},
            {0x90, new CommandType("TrainerBGMPlay", 1, 2)},
            {0x92, new CommandType("TrainerGetFieldAction", 2, 2, 2)},
            {0x93, new CommandType("TrainerGetPrizeItem", 2, 2, 2)},
            {0x94, new CommandType("CallTradedPokemonBattle", 4, 2, 2, 2, 2)},
            {0x95, new CommandType("TrainerFlagSet", 1, 2)},
            {0x96, new CommandType("TrainerFlagReset", 1, 2)},
            {0x97, new CommandType("TrainerFlagGet", 2, 2, 2)},
            {0x98, new CommandType("BGMPlay", 1, 2)},
            {0x9B, new CommandType("BGMIsPlaying", 2, 2, 2)},
            {0x9E, new CommandType("BGMChangeMap", 0)},
            {0x9F, new CommandType("BGMPlayPush", 1, 2)},
            {0xA0, new CommandType("BGMWait", 0)},
            {0xA1, new CommandType("BGMPush", 1, 2)},
            {0xA2, new CommandType("BGMPop", 2, 2, 2)},
            {0xA3, new CommandType("ISSSwitchEnable", 1, 2)},
            {0xA4, new CommandType("ISSSwitchDisable", 1, 2)},
            {0xA5, new CommandType("ISSSwitchQuery", 2, 2, 2)},
            {0xA6, new CommandType("SEPlay", 1, 2)},
            {0xA7, new CommandType("SEStop", 0)},
            {0xA8, new CommandType("SEWait", 0)},
            {0xA9, new CommandType("MEPlay", 1, 2)},
            {0xAA, new CommandType("MEWait", 0)},
            {0xAB, new CommandType("PVPlay", 2, 2, 2)},
            {0xAC, new CommandType("PVWait", 0)},
            {0xAD, new CommandType("ListMenuInitCommon", 5, 1, 1, 2, 1, 2)},
            {0xAE, new CommandType("ListMenuInitTL", 5, 1, 1, 2, 1, 2)},
            {0xAF, new CommandType("ListMenuAdd", 3, 2, 2, 2)},
            {0xB0, new CommandType("ListMenuShow", 0)},
            {0xB1, new CommandType("ListMenuShow2", 0)},
            {0xB2, new CommandType("ListMenuInitTR", 5, 1, 1, 2, 1, 2)},
            {0xB3, new CommandType("FadeEx", 4, 2, 2, 2, 2)},
            {0xB4, new CommandType("FadeExWait", 0)},
            {0xB5, new CommandType("ItemAdd", 3, 2, 2, 2)},
            {0xB6, new CommandType("ItemSub", 3, 2, 2, 2)},
            {0xB7, new CommandType("ItemCheckSpace", 3, 2, 2, 2)},
            {0xB8, new CommandType("ItemCheckCount", 3, 2, 2, 2)},
            {0xB9, new CommandType("ItemGetCount", 2, 2, 2)},
            {0xBA, new CommandType("ItemIsTMHM", 2, 2, 2)},
            {0xBB, new CommandType("ItemGetPocket", 2, 2, 2)},
            {0xBC, new CommandType("PhenomenonGetItemID", 1, 2)},
            {0xBD, new CommandType("ItemGetClass", 2, 2, 2)},
            {0xBE, new CommandType("MapChangeFake", 4, 2, 2, 2, 2)},
            {0xBF, new CommandType("MapChangeWarpPad", 4, 2, 2, 2, 2)},
            {0xC0, new CommandType("MapChangeWarpRail", 5, 2, 2, 2, 2, 2)},
            {0xC1, new CommandType("MapChangeQuicksand", 3, 2, 2, 2)},
            {0xC2, new CommandType("MapChangeWarp", 4, 2, 2, 2, 2)},
            {0xC3, new CommandType("MapChangeUnionRoom", 0)},
            {0xC4, new CommandType("MapChangeCore", 5, 2, 2, 2, 2, 2)},
            {0xC5, new CommandType("HMCallSurf", 0)},
            {0xC6, new CommandType("HMCallWaterfall", 1, 2)},
            {0xC7, new CommandType("HMCallCut", 0)},
            {0xC8, new CommandType("HMCallDiving", 1, 2)},
            {0xC9, new CommandType("CMD_C9", 0)},
            {0xCA, new CommandType("CMD_CA", 1, 2)},
            {0xCB, new CommandType("Random", 2, 2, 2)},
            {0xCC, new CommandType("RTGetTextFile", 1, 2)},
            {0xCD, new CommandType("RTCGetDayPart", 1, 2)},
            {0xCE, new CommandType("CMD_CE", 1, 2)},
            {0xCF, new CommandType("RTCGetWeekDay", 1, 2)},
            {0xD0, new CommandType("RTCGetDate", 2, 2, 2)},
            {0xD1, new CommandType("RTCGetTime", 2, 2, 2)},
            {0xD2, new CommandType("RTCGetSeason", 1, 2)},
            {0xD3, new CommandType("FieldGetZoneID", 1, 2)},
            {0xD4, new CommandType("TrainerCardGetBirthDate", 2, 2, 2)},
            {0xD5, new CommandType("TrainerCardHasBadge", 2, 2, 2)},
            {0xD6, new CommandType("TrainerCardAddBadge", 1, 2)},
            {0xD7, new CommandType("TrainerCardGetBadgeCount", 1, 2)},
            {0xD8, new CommandType("MapReplaceIsEventSet", 2, 2, 2)},
            {0xD9, new CommandType("FieldSetTeleportZone", 1, 2)},
            {0xDA, new CommandType("MapReplaceSetEvent", 3, 2, 2, 2)},
            {0xDB, new CommandType("FieldSetNextZoneHere", 0)},
            {0xDC, new CommandType("FieldSetNextZone", 5, 2, 2, 2, 2, 2)},
            {0xDD, new CommandType("PokeDexGetCount", 2, 2, 2)},
            {0xDE, new CommandType("PokeDexRegist", 2, 2, 2)},
            {0xDF, new CommandType("PokeDexIsRegist", 3, 2, 2, 2)},
            {0xE0, new CommandType("GameGetVersion", 1, 2)},
            {0xE1, new CommandType("TrainerCardGetSex", 1, 2)},
            {0xE2, new CommandType("SaveDataCheckRequired", 1, 2)},
            {0xE3, new CommandType("PlayerEnableRunningShoes", 0)},
            {0xE4, new CommandType("CMD_E4", 1, 2)},
            {0xE5, new CommandType("CMD_E5", 2, 2, 2)},
            {0xE6, new CommandType("CMD_E6", 2, 2, 2)},
            {0xE7, new CommandType("CMD_E7", 1, 2)},
            {0xE8, new CommandType("CMD_E8", 2, 2, 2)},
            {0xEA, new CommandType("HOFCheckIntegrity", 1, 2)},
            {0xEB, new CommandType("DayCareCheckSpawnFlag", 1, 2)},
            {0xEC, new CommandType("DayCareBreed", 0)},
            {0xED, new CommandType("DayCareResetSeed", 0)},
            {0xEE, new CommandType("DayCareGetPkmCount", 1, 2)},
            {0xEF, new CommandType("DayCareCalcEggSpawnChance", 1, 2)},
            {0xF0, new CommandType("DayCareDeposit", 1, 2)},
            {0xF1, new CommandType("DayCareWithdraw", 1, 2)},
            {0xF2, new CommandType("DayCareGetSpecies", 2, 2, 2)},
            {0xF3, new CommandType("DayCareGetForme", 2, 2, 2)},
            {0xF4, new CommandType("DayCareCalcNewLevel", 2, 2, 2)},
            {0xF5, new CommandType("DayCareCalcLevelGain", 2, 2, 2)},
            {0xF6, new CommandType("DayCareCalcWithdrawCost", 2, 2, 2)},
            {0xF7, new CommandType("DayCareCallPokeSelect", 1, 2)},
            {0xF8, new CommandType("DayCareGetSex", 2, 2, 2)},
            {0xF9, new CommandType("MoneyAdd", 1, 2)},
            {0xFA, new CommandType("MoneySub", 1, 2)},
            {0xFB, new CommandType("MoneyCheck", 2, 2, 2)},
            {0xFC, new CommandType("PokePartySetHappiness", 2, 2, 2)},
            {0xFD, new CommandType("PokePartyAdjustHappiness", 3, 2, 2, 2)},
            {0xFE, new CommandType("PokePartyGetSpecies", 2, 2, 2)},
            {0xFF, new CommandType("PokePartyGetForme", 2, 2, 2)},
            {0x100, new CommandType("PokePartyCheckPokerus", 1, 2)},
            {0x101, new CommandType("PokePartyIsFullHP", 2, 2, 2)},
            {0x102, new CommandType("PokePartyIsEgg", 2, 2, 2)},
            {0x103, new CommandType("PokePartyGetCount", 2, 2, 2)},
            {0x104, new CommandType("PokePartyRecoverAll", 0)},
            {0x105, new CommandType("CallPokeNameInput", 3, 2, 2, 2)},
            {0x106, new CommandType("CallEggHatch", 1, 2)},
            {0x107, new CommandType("CallPokeSelect", 4, 2, 2, 2, 2)},
            {0x108, new CommandType("PokePartyGetMoveCount", 2, 2, 2)},
            {0x109, new CommandType("CallPokeMoveReplace", 4, 2, 2, 2, 2)},
            {0x10A, new CommandType("PokePartyGetMove", 3, 2, 2, 2)},
            {0x10B, new CommandType("PokePartyLearnMove", 3, 2, 2, 2)},
            {0x10C, new CommandType("PokePartyAdd", 4, 2, 2, 2, 2)},
            {0x10D, new CommandType("PokePartyGetMemberByType", 2, 2, 2)},
            {0x10E, new CommandType("PokePartyAddEx", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x10F, new CommandType("PokePartyAddEgg", 3, 2, 2, 2)},
            {0x110, new CommandType("PokePartyGetParam", 3, 2, 2, 2)},
            {0x111, new CommandType("PokePartySetIV", 3, 2, 2, 2)},
            {0x112, new CommandType("PokePartyGetEVTotal", 2, 2, 2)},
            {0x113, new CommandType("PokePartyIsOriginGame", 2, 2, 2)},
            {0x114, new CommandType("PokePartyGetCountBySpecies", 2, 2, 2)},
            {0x115, new CommandType("PokePartyHasMove", 3, 2, 2, 2)},
            {0x116, new CommandType("PokePartyHasMoveAny", 2, 2, 2)},
            {0x117, new CommandType("PokePartySetForme", 2, 2, 2)},
            {0x118, new CommandType("PokePartyFind", 3, 2, 2, 2)},
            {0x119, new CommandType("PokePartyIsFromWhiteForest", 3, 2, 2, 2)},
            {0x11A, new CommandType("PokePartyGetMetDate", 4, 2, 2, 2, 2)},
            {0x11B, new CommandType("PokePartyGetTypes", 3, 2, 2, 2)},
            {0x11C, new CommandType("PokePartyChangeRotomForme", 3, 2, 2, 2)},
            {0x11D, new CommandType("TrainerCardSetFavePokemon", 1, 2)},
            {0x11E, new CommandType("TrainerCardSaveGymVictoryParty", 1, 2)},
            {0x11F, new CommandType("WordSetGymVictoryParty", 2, 2, 2)},
            {0x120, new CommandType("FieldTradeSavePokemon", 2, 2, 2)},
            {0x121, new CommandType("BoxGetCount", 2, 2, 2)},
            {0x122, new CommandType("BoxAdd", 4, 2, 2, 2, 2)},
            {0x123, new CommandType("BoxAddEx", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x124, new CommandType("BMHndAnmPlay", 2, 2, 2)},
            {0x125, new CommandType("BMPlayHOFMachineSeq", 0)},
            {0x126, new CommandType("BMChangeMdlID", 4, 2, 2, 2, 2)},
            {0x127, new CommandType("BMCreateHandleByGPos", 4, 2, 2, 2, 2)},
            {0x128, new CommandType("BMReleaseHandle", 1, 2)},
            {0x129, new CommandType("BMHndAudioVisualAnmPlay", 2, 2, 2)},
            {0x12A, new CommandType("BMHndAnmWait", 1, 2)},
            {0x12B, new CommandType("BMAnmPlayInv", 3, 2, 2, 2)},
            {0x12C, new CommandType("BMHndAnmPause", 1, 2)},
            {0x12D, new CommandType("BMSetVisible", 4, 2, 2, 2, 2)},
            {0x12E, new CommandType("BMAnmPlay", 3, 2, 2, 2)},
            {0x12F, new CommandType("PokecenPlayHealingSequence", 1, 2)},
            {0x130, new CommandType("PokecenPCOpen", 0)},
            {0x131, new CommandType("PokecenPCIdle", 0)},
            {0x132, new CommandType("PokecenPCClose", 1, 2)},
            {0x134, new CommandType("CasteliaRushInit", 0)},
            {0x136, new CommandType("FieldSetWeather", 2, 2, 2)},
            {0x137, new CommandType("SaveDataWrite", 1, 2)},
            {0x138, new CommandType("SaveDataGetStatus", 3, 2, 2, 2)},
            {0x139, new CommandType("GameCommDisconnect", 1, 2)},
            {0x13A, new CommandType("GameCommGetStatus", 1, 2)},
            {0x13B, new CommandType("GameCommCheckDSiWiFi", 1, 2)},
            {0x13C, new CommandType("CMD_13C", 0)},
            {0x13D, new CommandType("CMD_13D", 0)},
            {0x13E, new CommandType("CMD_13E", 0)},
            {0x13F, new CommandType("EVCameraInit", 0)},
            {0x140, new CommandType("EVCameraEnd", 0)},
            {0x141, new CommandType("EVCameraUnbind", 0)},
            {0x142, new CommandType("EVCameraRebind", 0)},
            {0x143, new CommandType("EVCameraMoveTo", 7, 2, 2, 4, 4, 4, 4, 2)},
            {0x144, new CommandType("EVCameraReturn", 1, 2)},
            {0x145, new CommandType("EVCameraWait", 0)},
            {0x146, new CommandType("EVCameraMoveToCommon", 2, 2, 2)},
            {0x147, new CommandType("EVCameraMoveToDefault", 1, 2)},
            {0x148, new CommandType("EVCameraShake", 8, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x149, new CommandType("CallFriendlyShopBuy", 2, 2, 2)},
            {0x14A, new CommandType("FieldOpen", 0)},
            {0x14B, new CommandType("FieldClose", 0)},
            {0x14C, new CommandType("RTFreeUserHeap", 0)},
            {0x14D, new CommandType("CallRecordSystem", 2, 2, 2)},
            {0x14E, new CommandType("CallBag", 3, 2, 2, 2)},
            {0x14F, new CommandType("CallPC", 2, 2, 2)},
            {0x150, new CommandType("CallMailbox", 1, 2)},
            {0x151, new CommandType("CallPokedexDiploma", 2, 2, 2)},
            {0x152, new CommandType("CallGeonet", 0)},
            {0x153, new CommandType("CallPoke3Select", 1, 2)},
            {0x154, new CommandType("Call3DDemo", 2, 2, 2)},
            {0x155, new CommandType("CallXTransceiver", 2, 2, 2)},
            {0x156, new CommandType("CallGameClear", 1, 2)},
            {0x157, new CommandType("ActorAnimationInit", 1, 2)},
            {0x158, new CommandType("ActorAnimationFree", 0)},
            {0x159, new CommandType("ActorAnimationPlay", 1, 2)},
            {0x15A, new CommandType("ActorAnimationWait", 0)},
            {0x15B, new CommandType("CMD_15B", 1, 2)},
            {0x15C, new CommandType("CMD_15C", 1, 2)},
            {0x15D, new CommandType("NetConnectWiFiClub", 0)},
            {0x15E, new CommandType("NetConnectGTS", 0)},
            {0x15F, new CommandType("CMD_15F", 0)},
            {0x160, new CommandType("NetConnectWiFiBattle", 2, 2, 2)},
            {0x161, new CommandType("NetConnectBattleVideo", 1, 2)},
            {0x162, new CommandType("NetConnectGTSNegotiation", 0)},
            {0x163, new CommandType("CMD_163", 2, 2, 1)},
            {0x164, new CommandType("CMD_164", 1, 2)},
            {0x165, new CommandType("CMD_165", 3, 1, 2, 2)},
            {0x166, new CommandType("CMD_166", 3, 2, 1, 2)},
            {0x167, new CommandType("CMD_167", 4, 2, 2, 2, 2)},
            {0x168, new CommandType("CMD_168", 1, 2)},
            {0x169, new CommandType("CMD_169", 1, 2)},
            {0x16A, new CommandType("CMD_16A", 2, 2, 2)},
            {0x16E, new CommandType("CMD_16E", 0)},
            {0x170, new CommandType("CMD_170", 1, 2)},
            {0x172, new CommandType("CMD_172", 1, 2)},
            {0x174, new CommandType("CallWildBattle", 3, 2, 2, 2)},
            {0x175, new CommandType("CallWildBattleEnd", 0)},
            {0x176, new CommandType("CallWildLose", 0)},
            {0x177, new CommandType("WildBattleIsVictory", 1, 2)},
            {0x178, new CommandType("WildBattleGetResult", 1, 2)},
            {0x179, new CommandType("CallCaptureDemo", 0)},
            {0x17A, new CommandType("CMD_17A", 1, 2)},
            {0x17B, new CommandType("CMD_17B", 2, 2, 2)},
            {0x17C, new CommandType("CMD_17C", 1, 2)},
            {0x17D, new CommandType("CMD_17D", 1, 2)},
            {0x186, new CommandType("CMD_186", 0)},
            {0x187, new CommandType("CMD_187", 1, 2)},
            {0x18C, new CommandType("CMD_18C", 2, 2, 2)},
            {0x18D, new CommandType("CMD_18D", 1, 2)},
            {0x18E, new CommandType("CMD_18E", 1, 2)},
            {0x18F, new CommandType("CMD_18F", 1, 2)},
            {0x190, new CommandType("CMD_190", 1, 2)},
            {0x191, new CommandType("CMD_191", 1, 2)},
            {0x197, new CommandType("CGearPowerOn", 1, 2)},
            {0x198, new CommandType("FieldSubscreenChange", 1, 2)},
            {0x199, new CommandType("FieldSubscreenReturn", 0)},
            {0x19A, new CommandType("CGearControlWarning", 1, 2)},
            {0x19B, new CommandType("PlayFieldEffect", 1, 2)},
            {0x19C, new CommandType("PlayHMCutInEffect", 1, 2)},
            {0x19D, new CommandType("PlayAlderFlyEffect", 1, 2)},
            {0x1A1, new CommandType("CMD_1A1", 4, 2, 2, 2, 2)},
            {0x1A2, new CommandType("CallRoyalUnovaView", 1, 2)},
            {0x1A3, new CommandType("FadeInBlackQ", 0)},
            {0x1A4, new CommandType("FadeOutBlackQ", 0)},
            {0x1A5, new CommandType("FadeInWhiteQ", 0)},
            {0x1A6, new CommandType("FadeOutWhiteQ", 0)},
            {0x1A7, new CommandType("FadeWait", 0)},
            {0x1A8, new CommandType("FadeInBlackQ_", 0)},
            {0x1AA, new CommandType("CallSeasonBanner", 0)},
            {0x1AB, new CommandType("FadeInBlack", 0)},
            {0x1AC, new CommandType("FadeOutBlack", 0)},
            {0x1AD, new CommandType("FadeInWhite", 0)},
            {0x1AE, new CommandType("FadeOutWhite", 0)},
            {0x1AF, new CommandType("CMD_1AF", 2, 2, 2)},
            {0x1B0, new CommandType("CMD_1B0", 2, 2, 2)},
            {0x1B1, new CommandType("CallLeagueLiftWarp", 0)},
            {0x1B2, new CommandType("CMD_1B2", 1, 2)},
            {0x1B3, new CommandType("CMD_1B3", 2, 2, 2)},
            {0x1B4, new CommandType("FieldTradeStart", 2, 2, 2)},
            {0x1B5, new CommandType("FieldTradeCheck", 3, 2, 2, 2)},
            {0x1C1, new CommandType("ElevatorSetTablePtr", 1, 4)},
            {0x1C2, new CommandType("ElevatorBuildListMenu", 0)},
            {0x1C3, new CommandType("ElevatorChangeMap", 1, 2)},
            {0x1C4, new CommandType("PokeDexIsComplete", 2, 2, 2)},
            {0x1C5, new CommandType("PokeDexGetEvaluationParams", 4, 2, 2, 2, 2)},
            {0x1C6, new CommandType("PokeDexGiveNational", 0)},
            {0x1C7, new CommandType("PokeDexHaveNational", 1, 2)},
            {0x1C8, new CommandType("PokeDexEnable", 0)},
            {0x1C9, new CommandType("NDemoStart", 3, 2, 2, 2)},
            {0x1CA, new CommandType("NDemoEnd", 0)},
            {0x1CB, new CommandType("NDemoReadyTalkMotion", 0)},
            {0x1CC, new CommandType("CMD_1CC", 1, 2)},
            {0x1CD, new CommandType("CMD_1CD", 1, 2)},
            {0x1CE, new CommandType("CMD_1CE", 2, 2, 2)},
            {0x1CF, new CommandType("UnityTowerGetStateParam", 2, 2, 2)},
            {0x1D0, new CommandType("UnityTowerCallFloorSelect", 4, 2, 2, 2, 2)},
            {0x1D1, new CommandType("MoveTutorCheckParty", 2, 2, 2)},
            {0x1D2, new CommandType("MoveTutorCheckPkm", 3, 2, 2, 2)},
            {0x1D3, new CommandType("MoveTutorGetMoveID", 3, 2, 2, 2)},
            {0x1D4, new CommandType("MoveTutorCallPokeSelect", 3, 2, 2, 2)},
            {0x1D5, new CommandType("MoveReminderCheckPkm", 2, 2, 2)},
            {0x1D6, new CommandType("MoveReminderCallMoveSelect", 2, 2, 2)},
            {0x1D7, new CommandType("ObjInitProxyGPos", 4, 2, 2, 2, 2)},
            {0x1D8, new CommandType("ObjInitWarpGPos", 4, 2, 2, 2, 2)},
            {0x1D9, new CommandType("ObjInitNPCGPos", 5, 2, 2, 2, 2, 2)},
            {0x1DA, new CommandType("CallPhraseSelect", 4, 2, 2, 2, 2)},
            {0x1DB, new CommandType("CMD_1DB", 2, 2, 2)},
            {0x1DC, new CommandType("CMD_1DC", 2, 2, 2)},
            {0x1DD, new CommandType("CMD_1DD", 3, 2, 2, 2)},
            {0x1DE, new CommandType("CMD_1DE", 2, 2, 2)},
            {0x1DF, new CommandType("CMD_1DF", 0)},
            {0x1E0, new CommandType("StadiumSetupActorsDouble", 4, 2, 2, 2, 2)},
            {0x1E1, new CommandType("StadiumLoadTrainerTable", 0)},
            {0x1E2, new CommandType("StadiumFreeTrainerTable", 0)},
            {0x1E3, new CommandType("StadiumSetupActorSingle", 3, 2, 2, 2)},
            {0x1E4, new CommandType("StadiumSetupActorsTriple", 6, 2, 2, 2, 2, 2, 2)},
            {0x1E5, new CommandType("StadiumResetTrainerFlags", 0)},
            {0x1E6, new CommandType("TrialHouseWorkInit", 0)},
            {0x1E7, new CommandType("TrialHouseWorkDelete", 0)},
            {0x1E8, new CommandType("TrialHousePrepareParty", 1, 2)},
            {0x1E9, new CommandType("TrialHouseCallTeamSelect", 3, 2, 2, 2)},
            {0x1EA, new CommandType("CMD_1EA", 2, 2, 2)},
            {0x1EB, new CommandType("TrialHouseMsgDisp", 2, 2, 2)},
            {0x1EC, new CommandType("TrialHouseStartBattle", 0)},
            {0x1ED, new CommandType("CMD_1ED", 1, 2)},
            {0x1EE, new CommandType("CMD_1EE", 1, 2)},
            {0x1EF, new CommandType("TrialHouseGetBattleTestRank", 1, 2)},
            {0x1F0, new CommandType("CMD_1F0", 1, 2)},
            {0x1F1, new CommandType("TrialHouseCalcPointsStars", 2, 2, 2)},
            {0x1F2, new CommandType("CMD_1F2", 1, 2)},
            {0x1F3, new CommandType("TrialHouseSaveData", 1, 2)},
            {0x1F4, new CommandType("CMD_1F4", 2, 2, 2)},
            {0x1F5, new CommandType("CMD_1F5", 1, 2)},
            {0x1F6, new CommandType("CMD_1F6", 4, 2, 2, 2, 2)},
            {0x1F7, new CommandType("CMD_1F7", 4, 2, 2, 2, 2)},
            {0x1F8, new CommandType("CMD_1F8", 2, 2, 2)},
            {0x1F9, new CommandType("CMD_1F9", 4, 2, 2, 2, 2)},
            {0x1FA, new CommandType("PepQuizGenerate", 4, 2, 2, 2, 2)},
            {0x1FB, new CommandType("ItemCollectorCheckGroup", 2, 2, 2)},
            {0x1FC, new CommandType("ItemCollectorGetPrice", 3, 2, 2, 2)},
            {0x1FD, new CommandType("CMD_1FD", 4, 2, 2, 2, 2)},
            {0x1FE, new CommandType("CMD_1FE", 0)},
            {0x1FF, new CommandType("SurveyGetCurrentQuestionID", 1, 2)},
            {0x200, new CommandType("SurveyGetCurrentAnswerIDs", 3, 2, 2, 2)},
            {0x201, new CommandType("SurveyGetPopularOptionMsgID", 2, 2, 2)},
            {0x202, new CommandType("CMD_202", 2, 2, 2)},
            {0x203, new CommandType("CMD_203", 1, 2)},
            {0x204, new CommandType("SurveyGetTime", 1, 2)},
            {0x205, new CommandType("CallGreetingPhraseInput", 1, 2)},
            {0x206, new CommandType("CallThanksPhraseInput", 1, 2)},
            {0x207, new CommandType("CMD_207", 1, 1)},
            {0x208, new CommandType("CMD_208", 1, 1)},
            {0x209, new CommandType("CMD_209", 2, 2, 2)},
            {0x20A, new CommandType("CMD_20A", 0)},
            {0x20B, new CommandType("CMD_20B", 1, 2)},
            {0x20C, new CommandType("CMD_20C", 0)},
            {0x20D, new CommandType("PokePartyFindEx", 4, 2, 2, 2, 2)},
            {0x20E, new CommandType("CMD_20E", 6, 2, 2, 2, 2, 2, 2)},
            {0x20F, new CommandType("CMD_20F", 4, 2, 2, 2, 2)},
            {0x210, new CommandType("CMD_210", 1, 2)},
            {0x211, new CommandType("CMD_211", 1, 2)},
            {0x212, new CommandType("CMD_212", 3, 2, 2, 2)},
            {0x213, new CommandType("CMD_213", 1, 2)},
            {0x214, new CommandType("CallPokemonPreview", 4, 2, 2, 2, 2)},
            {0x215, new CommandType("CMD_215", 2, 2, 2)},
            {0x216, new CommandType("CMD_216", 2, 2, 2)},
            {0x217, new CommandType("MapChangeEntreeForest", 1, 2)},
            {0x218, new CommandType("CMD_218", 2, 2, 2)},
            {0x219, new CommandType("CMD_219", 2, 2, 2)},
            {0x21A, new CommandType("WordSetEntreeForestPkmName", 2, 2, 2)},
            {0x21B, new CommandType("EntreeForestSpawnAllPkm", 0)},
            {0x21C, new CommandType("EntreeForestStartBattle", 2, 2, 2)},
            {0x21D, new CommandType("CMD_21D", 1, 2)},
            {0x21E, new CommandType("FishingChallengeGetRandomPkm", 1, 2)},
            {0x21F, new CommandType("CMD_21F", 2, 2, 2)},
            {0x220, new CommandType("CMD_220", 2, 2, 2)},
            {0x221, new CommandType("CMD_221", 2, 2, 2)},
            {0x222, new CommandType("CMD_222", 1, 2)},
            {0x223, new CommandType("PokePartyGetHiddenPowerType", 2, 2, 2)},
            {0x224, new CommandType("PokePartyGetIV", 3, 2, 2, 2)},
            {0x225, new CommandType("ItemGetRandomOwnedTMMove", 1, 2)},
            {0x226, new CommandType("CMD_226", 1, 2)},
            {0x227, new CommandType("RecordAdd", 2, 2, 2)},
            {0x228, new CommandType("RecordGet", 2, 2, 2)},
            {0x22A, new CommandType("CMD_22A", 2, 2, 2)},
            {0x22B, new CommandType("CMD_22B", 2, 2, 2)},
            {0x22D, new CommandType("CMD_22D", 1, 2)},
            {0x22E, new CommandType("CMD_22E", 1, 2)},
            {0x22F, new CommandType("WordSetPastTradePkmName", 2, 2, 2)},
            {0x230, new CommandType("CMD_230", 2, 2, 2)},
            {0x231, new CommandType("CMD_231", 1, 2)},
            {0x232, new CommandType("CMD_232", 2, 2, 2)},
            {0x233, new CommandType("CMD_233", 1, 2)},
            {0x234, new CommandType("ActorFallDownToXZ", 4, 2, 2, 2, 2)},
            {0x235, new CommandType("BGMPlayEx", 2, 2, 2)},
            {0x236, new CommandType("WordSetLoadItemCollectorPrice", 4, 2, 2, 2, 2)},
            {0x237, new CommandType("ItemCollectorSell", 2, 2, 2)},
            {0x238, new CommandType("BGMChangeMapEx", 1, 2)},
            {0x239, new CommandType("CMD_239", 1, 2)},
            {0x23A, new CommandType("CMD_23A", 2, 2, 2)},
            {0x23B, new CommandType("CMD_23B", 0)},
            {0x23C, new CommandType("CMD_23C", 0)},
            {0x23D, new CommandType("CMD_23D", 0)},
            {0x23E, new CommandType("DayCareGetSexForNamePrint", 3, 2, 2, 2)},
            {0x23F, new CommandType("CallPlaceNameDisp", 0)},
            {0x240, new CommandType("CMD_240", 2, 2, 2)},
            {0x241, new CommandType("CMD_241", 1, 2)},
            {0x245, new CommandType("BGMFadeOut", 1, 2)},
            {0x246, new CommandType("BGMFadeOutAll", 1, 2)},
            {0x247, new CommandType("MapChangeRail", 5, 2, 2, 2, 2, 2)},
            {0x248, new CommandType("PlayerMoveToYAsync_", 4, 2, 2, 2, 2)},
            {0x249, new CommandType("CMD_249", 6, 2, 2, 2, 2, 2, 2)},
            {0x24A, new CommandType("CMD_24A", 1, 2)},
            {0x24B, new CommandType("FieldSubscreenDisable", 0)},
            {0x24C, new CommandType("BGMAmbienceResume", 0)},
            {0x24D, new CommandType("CMD_24D", 0)},
            {0x24E, new CommandType("PokePartyIsFullPP", 2, 2, 2)},
            {0x24F, new CommandType("ActorWalkRoute", 6, 2, 2, 2, 2, 2, 2)},
            {0x250, new CommandType("ActorPairSet", 5, 2, 2, 2, 2, 2)},
            {0x251, new CommandType("ActorPairEnd", 2, 2, 2)},
            {0x252, new CommandType("ActorPairGetTrID", 1, 2)},
            {0x253, new CommandType("ActorPairSetMoveEnable", 1, 1)},
            {0x25A, new CommandType("CMD_25A", 1, 2)},
            {0x25B, new CommandType("CMD_25B", 0)},
            {0x25C, new CommandType("CMD_25C", 3, 2, 2, 2)},
            {0x25D, new CommandType("CMD_25D", 1, 2)},
            {0x25F, new CommandType("CMD_25F", 1, 2)},
            {0x262, new CommandType("CMD_262", 2, 2, 2)},
            {0x263, new CommandType("CMD_263", 1, 2)},
            {0x264, new CommandType("CMD_264", 2, 2, 2)},
            {0x265, new CommandType("CMD_265", 1, 2)},
            {0x266, new CommandType("CMD_266", 1, 2)},
            {0x267, new CommandType("CMD_267", 0)},
            {0x268, new CommandType("CMD_268", 1, 2)},
            {0x269, new CommandType("CMD_269", 0)},
            {0x26C, new CommandType("WordSetMedalName", 2, 1, 2)},
            {0x26D, new CommandType("WordSetMedalRank", 2, 1, 2)},
            {0x26E, new CommandType("MedalGetCount", 2, 1, 2)},
            {0x271, new CommandType("MedalAcknowledge", 2, 2, 2)},
            {0x272, new CommandType("MedalGetGuruActor", 2, 2, 2)},
            {0x273, new CommandType("MedalGive", 1, 2)},
            {0x274, new CommandType("FunfestMissionStart", 0)},
            {0x275, new CommandType("CMD_275", 3, 1, 2, 2)},
            {0x276, new CommandType("FunfestMissionBroadcast", 2, 2, 2)},
            {0x277, new CommandType("FunfestActorDelete", 0)},
            {0x278, new CommandType("FunfestDispSalesmanMessage", 3, 2, 2, 2)},
            {0x279, new CommandType("FunfestBGMReturn", 0)},
            {0x27A, new CommandType("FunfestGetGenericInfo", 2, 2, 2)},
            {0x27B, new CommandType("FunfestGetItemExchangeInfo", 4, 2, 2, 2, 2)},
            {0x27C, new CommandType("FunfestGetItemSaleInfo", 2, 2, 2)},
            {0x27D, new CommandType("FunfestGetPokemonQuizInfo", 3, 2, 2, 2)},
            {0x27E, new CommandType("FunfestGetPokemonQuizSpecies", 2, 2, 2)},
            {0x27F, new CommandType("FunfestGetPokemonQuizBogusSpecies", 2, 2, 2)},
            {0x284, new CommandType("CallPlaceSelect", 2, 2, 2)},
            {0x285, new CommandType("CallWordSetPokeNameInput", 3, 2, 2, 2)},
            {0x28A, new CommandType("MapChangeFlyWarp", 2, 2, 2)},
            {0x28B, new CommandType("CallEntralinkWarpOut", 0)},
            {0x28E, new CommandType("CMD_28E", 1, 2)},
            {0x28F, new CommandType("CMD_28F", 1, 2)},
            {0x290, new CommandType("WordSetRivalName", 1, 1)},
            {0x291, new CommandType("CMD_291", 1, 2)},
            {0x292, new CommandType("CMD_292", 1, 2)},
            {0x293, new CommandType("HiddenHollowCallWarpIn", 1, 2)},
            {0x294, new CommandType("HiddenHollowGetParam", 2, 2, 2)},
            {0x295, new CommandType("HiddenHollowReset", 0)},
            {0x296, new CommandType("HiddenHollowCallWarpOut", 0)},
            {0x297, new CommandType("CallWildBattleEx", 4, 2, 2, 2, 2)},
            {0x298, new CommandType("CMD_298", 2, 1, 2)},
            {0x299, new CommandType("WordSetLoadAbility", 2, 1, 2)},
            {0x29A, new CommandType("WordSetLoadNature", 2, 1, 2)},
            {0x29B, new CommandType("WordSetLoadJoinAvenueName", 1, 1)},
            {0x29E, new CommandType("MedalGetFieldEffectID", 2, 2, 2)},
            {0x29F, new CommandType("MedalDiscover", 1, 2)},
            {0x2A0, new CommandType("MedalIsObtained", 2, 2, 2)},
            {0x2A1, new CommandType("MedalGetMostCompleteCategory", 1, 2)},
            {0x2A2, new CommandType("CMD_2A2", 0)},
            {0x2A3, new CommandType("CMD_2A3", 0)},
            {0x2A4, new CommandType("CMD_2A4", 0)},
            {0x2A5, new CommandType("CMD_2A5", 1, 2)},
            {0x2A6, new CommandType("CMD_2A6", 0)},
            {0x2A7, new CommandType("CMD_2A7", 1, 2)},
            {0x2A8, new CommandType("CMD_2A8", 0)},
            {0x2A9, new CommandType("CMD_2A9", 0)},
            {0x2AC, new CommandType("PlayerPlayLeafPileStuck", 0)},
            {0x2AE, new CommandType("CMD_2AE", 1, 2)},
            {0x2AF, new CommandType("GameGetDifficulty", 1, 2)},
            {0x2B0, new CommandType("CallUnovaLinkKeyUnlock", 1, 2)},
            {0x2B1, new CommandType("CMD_2B1", 1, 2)},
            {0x2B2, new CommandType("CMD_2B2", 2, 2, 2)},
            {0x2B3, new CommandType("CMD_2B3", 2, 2, 2)},
            {0x2B4, new CommandType("CMD_2B4", 2, 2, 2)},
            {0x2B5, new CommandType("CMD_2B5", 2, 2, 2)},
            {0x2B6, new CommandType("CMD_2B6", 2, 2, 2)},
            {0x2B7, new CommandType("CMD_2B7", 1, 2)},
            {0x2B8, new CommandType("PedometerStart", 0)},
            {0x2B9, new CommandType("PedometerEnd", 0)},
            {0x2BA, new CommandType("PedometerGet", 1, 2)},
            {0x2BB, new CommandType("Gym0601FanAmbienceStart", 0)},
            {0x2BC, new CommandType("TVCheckCommercial", 2, 2, 2)},
            {0x2BD, new CommandType("TVGenCommercialMsgID", 1, 2)},
            {0x2BE, new CommandType("ActorMoveLinear", 5, 2, 2, 2, 2, 2)},
            {0x2C0, new CommandType("FieldTradeGetSpecies", 2, 2, 2)},
            {0x2C2, new CommandType("RepelRearm", 1, 2)},
            {0x2C3, new CommandType("CMD_2C3", 1, 2)},
            {0x2C4, new CommandType("CMD_2C4", 0)},
            {0x2C5, new CommandType("CMD_2C5", 1, 2)},
            {0x2C6, new CommandType("JoinAvenueStoreStart", 0)},
            {0x2C7, new CommandType("JoinAvenueStoreEnd", 0)},
            {0x2CA, new CommandType("CallHappyPhraseInput", 1, 2)},
            {0x2CB, new CommandType("CMD_2CB", 1, 2)},
            {0x2CF, new CommandType("PokeDexCheckHabitatList", 4, 2, 2, 2, 2)},
            {0x2D0, new CommandType("PokeDexEnableHabitatList", 0)},
            {0x2D1, new CommandType("CMD_2D1", 1, 2)},
            {0x2D2, new CommandType("FieldOpenRestoreLCD", 0)},
            {0x2D3, new CommandType("CMD_2D3", 2, 2, 2)},
            {0x2D4, new CommandType("ItemGetTMCount", 1, 2)},
            {0x2D5, new CommandType("CMD_2D5", 2, 2, 2)},
            {0x2D7, new CommandType("CMD_2D7", 2, 2, 2)},
            {0x2D8, new CommandType("CMD_2D8", 1, 2)},
            {0x2D9, new CommandType("CMD_2D9", 2, 2, 2)},
            {0x2DA, new CommandType("CMD_2DA", 1, 2)},
            {0x2DB, new CommandType("UnityTowerSetFloor", 2, 2, 2)},
            {0x2DC, new CommandType("UnityTowerInitVisitorMessage", 3, 2, 2, 2)},
            {0x2DD, new CommandType("UnityTowerGetVisitorCount", 1, 2)},
            {0x2DE, new CommandType("UnityTowerSetHobby", 1, 2)},
            {0x2DF, new CommandType("UnityTowerGetHobby", 1, 2)},
            {0x2E1, new CommandType("MedalDiscoverInitial", 0)},
            {0x2E3, new CommandType("LensFlareRequest", 0)},
            {0x2E5, new CommandType("HiddenHollowSet", 4, 2, 2, 2, 2)},
            {0x2E6, new CommandType("HiddenHollowCreateEvents", 0)},
            {0x2E8, new CommandType("CMD_2E8", 2, 2, 2)},
            {0x2E9, new CommandType("CMD_2E9", 2, 2, 2)},
            {0x2EA, new CommandType("PokePartyAddNPoke", 6, 2, 2, 2, 2, 2, 2)},
            {0x2ED, new CommandType("CMD_2ED", 2, 2, 2)},
            {0x2EE, new CommandType("MusicalIsPropOwned", 2, 2, 2)},
            {0x2EF, new CommandType("MusicalGetOwnedPropCount", 1, 2)},
            {0x2F1, new CommandType("CMD_2F1", 1, 2)},
            {0x2F2, new CommandType("CMD_2F2", 2, 2, 2)},
        };

        internal static Dictionary<int, CommandType> bw1CommandList = new Dictionary<int, CommandType>()
        {
            {0x0, new CommandType("VMNop", 0)},
            {0x1, new CommandType("VMNop2", 0)},
            {0x2, new CommandType("VMHalt", 0)},
            {0x3, new CommandType("VMSleep", 1, 2)},
            {0x4, new CommandType("VMCall", 1, 4)},
            {0x5, new CommandType("VMReturn", 0)},
            {0x6, new CommandType("DebugPrint", 1, 2)},
            {0x7, new CommandType("DebugStack", 1, 2)},
            {0x8, new CommandType("StackPushConst", 1, 2)},
            {0x9, new CommandType("StackPush", 1, 2)},
            {0xA, new CommandType("StackPop", 1, 2)},
            {0xB, new CommandType("StackDiscard", 0)},
            {0xC, new CommandType("StackAdd", 0)},
            {0xD, new CommandType("StackSub", 0)},
            {0xE, new CommandType("StackMul", 0)},
            {0xF, new CommandType("StackDiv", 0)},
            {0x10, new CommandType("StackPushFlag", 1, 2)},
            {0x11, new CommandType("StackCmp", 1, 2)},
            {0x12, new CommandType("WorkAnd", 2, 2, 2)},
            {0x13, new CommandType("WorkOr", 2, 2, 2)},
            {0x14, new CommandType("VMRegSet8", 2, 1, 1)},
            {0x15, new CommandType("VMRegSet32", 2, 1, 4)},
            {0x16, new CommandType("VMRegMov", 2, 1, 1)},
            {0x17, new CommandType("VMRegCmp8", 2, 1, 1)},
            {0x18, new CommandType("VMRegCmpConst8", 2, 1, 1)},
            {0x19, new CommandType("WorkCmpConst", 2, 2, 2)},
            {0x1A, new CommandType("WorkCmpWork", 2, 2, 2)},
            {0x1B, new CommandType("RTCallGlobalAsync", 1, 2)},
            {0x1C, new CommandType("RTCallGlobal", 1, 2)},
            {0x1D, new CommandType("RTEndGlobal", 0)},
            {0x1E, new CommandType("VMJump", 1, 4)},
            {0x1F, new CommandType("VMJumpIf", 2, 1, 4)},
            {0x20, new CommandType("VMCallIf", 2, 1, 4)},
            {0x21, new CommandType("RTReserveScript", 1, 2)},
            {0x22, new CommandType("FieldGetContinueFlag", 1, 2)},
            {0x23, new CommandType("FlagSet", 1, 2)},
            {0x24, new CommandType("FlagReset", 1, 2)},
            {0x25, new CommandType("FlagGet", 2, 2, 2)},
            {0x26, new CommandType("WorkAdd", 2, 2, 2)},
            {0x27, new CommandType("WorkSub", 2, 2, 2)},
            {0x28, new CommandType("WorkSetConst", 2, 2, 2)},
            {0x29, new CommandType("WorkGet", 2, 2, 2)},
            {0x2A, new CommandType("WorkSet", 2, 2, 2)},
            {0x2B, new CommandType("WorkMul", 2, 2, 2)},
            {0x2C, new CommandType("WorkDiv", 2, 2, 2)},
            {0x2D, new CommandType("WorkMod", 2, 2, 2)},
            {0x2E, new CommandType("ActorPauseAll", 0)},
            {0x2F, new CommandType("ActorUnlockAll", 0)},
            {0x30, new CommandType("RTFinishSubEvents", 0)},
            {0x31, new CommandType("InputWaitAB", 0)},
            {0x32, new CommandType("InputWaitLast", 0)},
            {0x33, new CommandType("MsgSetAutoscrolls", 1, 2)},
            {0x34, new CommandType("MsgSystem", 2, 2, 2)},
            {0x35, new CommandType("MsgSystemAsync", 2, 2, 2)},
            {0x36, new CommandType("MsgSystemClose", 0)},
            {0x37, new CommandType("MsgSetLoadingSpinner", 1, 2)},
            {0x38, new CommandType("MsgInfo", 2, 2, 1)},
            {0x39, new CommandType("MsgInfoClose", 0)},
            {0x3A, new CommandType("MsgMulti", 4, 2, 2, 2, 2)},
            {0x3B, new CommandType("MsgWinCloseNo", 1, 2)},
            {0x3C, new CommandType("MsgActorEx", 5, 2, 2, 2, 2, 2)},
            {0x3D, new CommandType("MsgActor", 4, 2, 2, 2, 2)},
            {0x3E, new CommandType("MsgActorClose", 0)},
            {0x3F, new CommandType("MsgWinCloseAll", 0)},
            {0x40, new CommandType("MoneyWinDisp", 2, 2, 2)},
            {0x41, new CommandType("MoneyWinClose", 0)},
            {0x42, new CommandType("MoneyWinUpdate", 0)},
            {0x43, new CommandType("MsgPlaceSign", 2, 2, 2)},
            {0x44, new CommandType("MsgPlaceSignClose", 0)},
            {0x45, new CommandType("MsgCheckerBG", 4, 2, 1, 1, 2)},
            {0x46, new CommandType("MsgCheckerBGClose", 0)},
            {0x47, new CommandType("YesNoWin", 1, 2)},
            {0x48, new CommandType("MsgActorGendered", 6, 2, 2, 2, 2, 2, 2)},
            {0x49, new CommandType("MsgActorVersioned", 6, 2, 2, 2, 2, 2, 2)},
            {0x4A, new CommandType("MsgScream", 2, 2, 1)},
            {0x4B, new CommandType("MsgWaitAdvance", 0)},
            {0x4C, new CommandType("WordSetPlayerName", 1, 1)},
            {0x4D, new CommandType("WordSetItemName", 2, 1, 2)},
            {0x4E, new CommandType("WordSetItemNameEx", 4, 1, 2, 2, 1)},
            {0x4F, new CommandType("WordSetItemNameWithArticle", 2, 1, 2)},
            {0x50, new CommandType("WordSetTMMoveName", 2, 1, 2)},
            {0x51, new CommandType("WordSetMoveName", 2, 1, 2)},
            {0x52, new CommandType("WordSetItemPocketName", 2, 1, 2)},
            {0x53, new CommandType("WordSetPartyPokeSpecies", 2, 1, 2)},
            {0x54, new CommandType("WordSetPartyPokeName", 2, 1, 2)},
            {0x55, new CommandType("WordSetDaycarePokeSpecies", 2, 1, 2)},
            {0x56, new CommandType("WordSetPokeTypeName", 2, 1, 2)},
            {0x57, new CommandType("WordSetPokeSpecies", 2, 1, 2)},
            {0x58, new CommandType("WordSetPokeSpeciesWithArticle", 2, 1, 2)},
            {0x59, new CommandType("WordSetPlaceName", 2, 1, 2)},
            {0x5A, new CommandType("WordSetTrendName", 2, 1, 2)},
            {0x5B, new CommandType("WordSetDaycarePokeName", 2, 1, 2)},
            {0x5C, new CommandType("WordSetNumber", 3, 1, 2, 2)},
            {0x5D, new CommandType("WordSetMusicalInfo", 3, 1, 1, 2)},
            {0x5E, new CommandType("WordSetCountry", 2, 1, 2)},
            {0x5F, new CommandType("WordSetHobbyName", 2, 1, 2)},
            {0x60, new CommandType("WordSetPassPowerName", 2, 1, 2)},
            {0x61, new CommandType("WordSetTrainerClassName", 2, 1, 2)},
            {0x62, new CommandType("WordSetTrainerClassNameWithArticle", 2, 1, 2)},
            {0x63, new CommandType("WordSetSurveyAnswer", 2, 1, 2)},
            {0x64, new CommandType("ActorCmdExec", 2, 2, 4)},
            {0x65, new CommandType("ActorCmdWait", 0)},
            {0x66, new CommandType("ActorGetMoveCode", 2, 2, 2)},
            {0x67, new CommandType("ActorGetGPos", 3, 2, 2, 2)},
            {0x68, new CommandType("PlayerGetGPos", 2, 2, 2)},
            {0x69, new CommandType("ActorNew", 6, 2, 2, 2, 2, 2, 2)},
            {0x6A, new CommandType("ActorGetSpawnFlag", 2, 2, 2)},
            {0x6B, new CommandType("ActorAdd", 1, 2)},
            {0x6C, new CommandType("ActorDelete", 1, 2)},
            {0x6D, new CommandType("ActorSetGPos", 5, 2, 2, 2, 2, 2)},
            {0x6E, new CommandType("PlayerGetDir", 1, 2)},
            {0x6F, new CommandType("PlayerGetActorInFront", 2, 2, 2)},
            {0x70, new CommandType("ActorFindByGPos", 5, 2, 2, 2, 2, 2)},
            {0x71, new CommandType("PlayerGetRailPos", 3, 2, 2, 2)},
            {0x72, new CommandType("ActorGetRailPos", 4, 2, 2, 2, 2)},
            {0x73, new CommandType("ActorSetMoveCode", 2, 2, 2)},
            {0x74, new CommandType("ActorSetEyeToEye", 0)},
            {0x75, new CommandType("PlayerSetSpecialSequence", 1, 2)},
            {0x76, new CommandType("PlayerMoveToYAsync", 4, 2, 2, 2, 2)},
            {0x77, new CommandType("PlayerTurnByTrigger", 0)},
            {0x78, new CommandType("PlayerGetExState", 1, 2)},
            {0x79, new CommandType("ActorGetUserParam", 3, 2, 2, 2)},
            {0x7A, new CommandType("ActorPlayRailSlipdown", 1, 2)},
            {0x7B, new CommandType("ActorJumpToGPos", 4, 2, 2, 2, 2)},
            {0x7C, new CommandType("PlayerSetRailPos", 3, 2, 2, 2)},
            {0x7D, new CommandType("ActorSetRailPos", 4, 2, 2, 2, 2)},
            {0x7E, new CommandType("ActorPlayTeleportSeq", 1, 2)},
            {0x7F, new CommandType("TrainerEyeGetTrainerID", 2, 2, 2)},
            {0x80, new CommandType("TrainerEyeEventInit", 1, 2)},
            {0x81, new CommandType("TrainerEyeEventStart", 0)},
            {0x82, new CommandType("TrainerEyeGetActorID", 2, 2, 2)},
            {0x83, new CommandType("TrainerEyeGetBattleType", 1, 2)},
            {0x84, new CommandType("ActorGetTrainerID", 1, 2)},
            {0x85, new CommandType("CallTrainerBattle", 3, 2, 2, 2)},
            {0x86, new CommandType("CallTrainerMultiBattle", 4, 2, 2, 2, 2)},
            {0x87, new CommandType("TrainerEyeSayMessage", 3, 2, 2, 2)},
            {0x88, new CommandType("TrainerEyeGetMessageTypes", 3, 2, 2, 2)},
            {0x8A, new CommandType("TrainerGetBattleType", 2, 2, 2)},
            {0x8B, new CommandType("TrainerBGMPlayPush", 1, 2)},
            {0x8C, new CommandType("CallTrainerLose", 0)},
            {0x8D, new CommandType("TrainerBattleIsVictory", 1, 2)},
            {0x8E, new CommandType("CallTrainerBattleEnd", 0)},
            {0x92, new CommandType("TrainerGetFieldAction", 2, 2, 2)},
            {0x93, new CommandType("TrainerGetPrizeItem", 2, 2, 2)},
            {0x94, new CommandType("CallTradedPokemonBattle", 4, 2, 2, 2, 2)},
            {0x95, new CommandType("TrainerFlagSet", 1, 2)},
            {0x96, new CommandType("TrainerFlagReset", 1, 2)},
            {0x97, new CommandType("TrainerFlagGet", 2, 2, 2)},
            {0x98, new CommandType("BGMPlay", 1, 2)},
            {0x9B, new CommandType("BGMIsPlaying", 2, 2, 2)},
            {0x9E, new CommandType("BGMChangeMap", 0)},
            {0x9F, new CommandType("BGMPlayPush", 1, 2)},
            {0xA0, new CommandType("BGMWait", 0)},
            {0xA1, new CommandType("BGMPush", 1, 2)},
            {0xA2, new CommandType("BGMPop", 2, 2, 2)},
            {0xA3, new CommandType("ISSSwitchEnable", 1, 2)},
            {0xA4, new CommandType("ISSSwitchDisable", 1, 2)},
            {0xA5, new CommandType("ISSSwitchQuery", 2, 2, 2)},
            {0xA6, new CommandType("SEPlay", 1, 2)},
            {0xA7, new CommandType("SEStop", 0)},
            {0xA8, new CommandType("SEWait", 0)},
            {0xA9, new CommandType("MEPlay", 1, 2)},
            {0xAA, new CommandType("MEWait", 0)},
            {0xAB, new CommandType("PVPlay", 2, 2, 2)},
            {0xAC, new CommandType("PVWait", 0)},
            {0xAD, new CommandType("ListMenuInitCommon", 5, 1, 1, 2, 1, 2)},
            {0xAE, new CommandType("ListMenuInitTL", 5, 1, 1, 2, 1, 2)},
            {0xAF, new CommandType("ListMenuAdd", 3, 2, 2, 2)},
            {0xB0, new CommandType("ListMenuShow", 0)},
            {0xB1, new CommandType("ListMenuShow2", 0)},
            {0xB2, new CommandType("ListMenuInitTR", 5, 1, 1, 2, 1, 2)},
            {0xB3, new CommandType("FadeEx", 4, 2, 2, 2, 2)},
            {0xB4, new CommandType("FadeExWait", 0)},
            {0xB5, new CommandType("ItemAdd", 3, 2, 2, 2)},
            {0xB6, new CommandType("ItemSub", 3, 2, 2, 2)},
            {0xB7, new CommandType("ItemCheckSpace", 3, 2, 2, 2)},
            {0xB8, new CommandType("ItemCheckCount", 3, 2, 2, 2)},
            {0xB9, new CommandType("ItemGetCount", 2, 2, 2)},
            {0xBA, new CommandType("ItemIsTMHM", 2, 2, 2)},
            {0xBB, new CommandType("ItemGetPocket", 2, 2, 2)},
            {0xBC, new CommandType("PhenomenonGetItemID", 1, 2)},
            {0xBD, new CommandType("ItemGetClass", 2, 2, 2)},
            {0xBE, new CommandType("MapChangeFake", 4, 2, 2, 2, 2)},
            {0xBF, new CommandType("MapChangeWarpPad", 4, 2, 2, 2, 2)},
            {0xC0, new CommandType("MapChangeWarpRail", 5, 2, 2, 2, 2, 2)},
            {0xC1, new CommandType("MapChangeQuicksand", 3, 2, 2, 2)},
            {0xC2, new CommandType("MapChangeWarp", 4, 2, 2, 2, 2)},
            {0xC3, new CommandType("MapChangeUnionRoom", 0)},
            {0xC4, new CommandType("MapChangeCore", 5, 2, 2, 2, 2, 2)},
            {0xC5, new CommandType("HMCallSurf", 0)},
            {0xC6, new CommandType("HMCallWaterfall", 1, 2)},
            {0xC7, new CommandType("HMCallCut", 0)},
            {0xC8, new CommandType("HMCallDiving", 1, 2)},
            {0xC9, new CommandType("CMD_C9", 0)},
            {0xCA, new CommandType("CMD_CA", 1, 2)},
            {0xCB, new CommandType("Random", 2, 2, 2)},
            {0xCC, new CommandType("RTGetTextFile", 1, 2)},
            {0xCD, new CommandType("RTCGetDayPart", 1, 2)},
            {0xCE, new CommandType("CMD_CE", 1, 2)},
            {0xCF, new CommandType("RTCGetWeekDay", 1, 2)},
            {0xD0, new CommandType("RTCGetDate", 2, 2, 2)},
            {0xD1, new CommandType("RTCGetTime", 2, 2, 2)},
            {0xD2, new CommandType("RTCGetSeason", 1, 2)},
            {0xD3, new CommandType("FieldGetZoneID", 1, 2)},
            {0xD4, new CommandType("TrainerCardGetBirthDate", 2, 2, 2)},
            {0xD5, new CommandType("TrainerCardHasBadge", 2, 2, 2)},
            {0xD6, new CommandType("TrainerCardAddBadge", 1, 2)},
            {0xD7, new CommandType("TrainerCardGetBadgeCount", 1, 2)},
            {0xD8, new CommandType("MapReplaceIsEventSet", 2, 2, 2)},
            {0xD9, new CommandType("FieldSetTeleportZone", 1, 2)},
            {0xDA, new CommandType("MapReplaceSetEvent", 3, 2, 2, 2)},
            {0xDB, new CommandType("FieldSetNextZoneHere", 0)},
            {0xDC, new CommandType("FieldSetNextZone", 5, 2, 2, 2, 2, 2)},
            {0xDD, new CommandType("PokeDexGetCount", 2, 2, 2)},
            {0xDE, new CommandType("PokeDexRegist", 2, 2, 2)},
            {0xDF, new CommandType("PokeDexIsRegist", 3, 2, 2, 2)},
            {0xE0, new CommandType("GameGetVersion", 1, 2)},
            {0xE1, new CommandType("TrainerCardGetSex", 1, 2)},
            {0xE2, new CommandType("SaveDataCheckRequired", 1, 2)},
            {0xE3, new CommandType("PlayerEnableRunningShoes", 0)},
            {0xE4, new CommandType("CMD_E4", 1, 2)},
            {0xE5, new CommandType("CMD_E5", 2, 2, 2)},
            {0xE6, new CommandType("CMD_E6", 2, 2, 2)},
            {0xE7, new CommandType("ActivateRelocator", 1, 2)},
            {0xE8, new CommandType("CMD_E8", 2, 2, 2)},
            {0xE9, new CommandType("CMD_E9", 1, 2)},
            {0xEA, new CommandType("HOFCheckIntegrity", 1, 2)},
            {0xEB, new CommandType("DayCareCheckSpawnFlag", 1, 2)},
            {0xEC, new CommandType("DayCareBreed", 0)},
            {0xED, new CommandType("DayCareResetSeed", 0)},
            {0xEE, new CommandType("DayCareGetPkmCount", 1, 2)},
            {0xEF, new CommandType("DayCareCalcEggSpawnChance", 1, 2)},
            {0xF0, new CommandType("DayCareDeposit", 1, 2)},
            {0xF1, new CommandType("DayCareWithdraw", 1, 2)},
            {0xF2, new CommandType("DayCareGetSpecies", 2, 2, 2)},
            {0xF3, new CommandType("DayCareGetForme", 2, 2, 2)},
            {0xF4, new CommandType("DayCareCalcNewLevel", 2, 2, 2)},
            {0xF5, new CommandType("DayCareCalcLevelGain", 2, 2, 2)},
            {0xF6, new CommandType("DayCareCalcWithdrawCost", 2, 2, 2)},
            {0xF7, new CommandType("DayCareCallPokeSelect", 1, 2)},
            {0xF8, new CommandType("DayCareGetSex", 2, 2, 2)},
            {0xF9, new CommandType("MoneyAdd", 1, 2)},
            {0xFA, new CommandType("MoneySub", 1, 2)},
            {0xFB, new CommandType("MoneyCheck", 2, 2, 2)},
            {0xFC, new CommandType("PokePartySetHappiness", 2, 2, 2)},
            {0xFD, new CommandType("PokePartyAdjustHappiness", 3, 2, 2, 2)},
            {0xFE, new CommandType("PokePartyGetSpecies", 2, 2, 2)},
            {0xFF, new CommandType("PokePartyGetForme", 2, 2, 2)},
            {0x100, new CommandType("PokePartyCheckPokerus", 1, 2)},
            {0x101, new CommandType("PokePartyIsFullHP", 2, 2, 2)},
            {0x102, new CommandType("PokePartyIsEgg", 2, 2, 2)},
            {0x103, new CommandType("PokePartyGetCount", 2, 2, 2)},
            {0x104, new CommandType("PokePartyRecoverAll", 0)},
            {0x105, new CommandType("CallPokeNameInput", 3, 2, 2, 2)},
            {0x106, new CommandType("CallEggHatch", 1, 2)},
            {0x107, new CommandType("CallPokeSelect", 4, 2, 2, 2, 2)},
            {0x108, new CommandType("PokePartyGetMoveCount", 2, 2, 2)},
            {0x109, new CommandType("CallPokeMoveReplace", 4, 2, 2, 2, 2)},
            {0x10A, new CommandType("PokePartyGetMove", 3, 2, 2, 2)},
            {0x10B, new CommandType("PokePartyLearnMove", 3, 2, 2, 2)},
            {0x10C, new CommandType("PokePartyAdd", 4, 2, 2, 2, 2)},
            {0x10D, new CommandType("PokePartyGetMemberByType", 2, 2, 2)},
            {0x10E, new CommandType("PokePartyAddEx", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x10F, new CommandType("PokePartyAddEgg", 3, 2, 2, 2)},
            {0x110, new CommandType("PokePartyGetParam", 3, 2, 2, 2)},
            {0x111, new CommandType("PokePartySetIV", 3, 2, 2, 2)},
            {0x112, new CommandType("PokePartyGetEVTotal", 2, 2, 2)},
            {0x113, new CommandType("PokePartyIsOriginGame", 2, 2, 2)},
            {0x114, new CommandType("PokePartyGetCountBySpecies", 2, 2, 2)},
            {0x115, new CommandType("PokePartyHasMove", 3, 2, 2, 2)},
            {0x116, new CommandType("PokePartyHasMoveAny", 2, 2, 2)},
            {0x117, new CommandType("PokePartySetForme", 2, 2, 2)},
            {0x118, new CommandType("PokePartyFind", 3, 2, 2, 2)},
            {0x119, new CommandType("PokePartyIsFromWhiteForest", 3, 2, 2, 2)},
            {0x11A, new CommandType("PokePartyGetMetDate", 4, 2, 2, 2, 2)},
            {0x11B, new CommandType("PokePartyGetTypes", 3, 2, 2, 2)},
            {0x11C, new CommandType("PokePartyChangeRotomForme", 3, 2, 2, 2)},
            {0x11D, new CommandType("TrainerCardSetFavePokemon", 1, 2)},
            {0x11E, new CommandType("TrainerCardSaveGymVictoryParty", 1, 2)},
            {0x11F, new CommandType("WordSetGymVictoryParty", 2, 2, 2)},
            {0x120, new CommandType("FieldTradeSavePokemon", 2, 2, 2)},
            {0x121, new CommandType("BoxGetCount", 2, 2, 2)},
            {0x122, new CommandType("BoxAdd", 4, 2, 2, 2, 2)},
            {0x123, new CommandType("BoxAddEx", 9, 2, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x124, new CommandType("BMHndAnmPlay", 2, 2, 2)},
            {0x125, new CommandType("BMPlayHOFMachineSeq", 0)},
            {0x126, new CommandType("BMChangeMdlID", 4, 2, 2, 2, 2)},
            {0x127, new CommandType("BMCreateHandleByGPos", 4, 2, 2, 2, 2)},
            {0x128, new CommandType("BMReleaseHandle", 1, 2)},
            {0x129, new CommandType("BMHndAudioVisualAnmPlay", 2, 2, 2)},
            {0x12A, new CommandType("BMHndAnmWait", 1, 2)},
            {0x12B, new CommandType("BMAnmPlayInv", 3, 2, 2, 2)},
            {0x12C, new CommandType("BMHndAnmPause", 1, 2)},
            {0x12D, new CommandType("BMSetVisible", 4, 2, 2, 2, 2)},
            {0x12E, new CommandType("BMAnmPlay", 3, 2, 2, 2)},
            {0x12F, new CommandType("PokecenPlayHealingSequence", 1, 2)},
            {0x130, new CommandType("PokecenPCOpen", 0)},
            {0x131, new CommandType("PokecenPCIdle", 0)},
            {0x132, new CommandType("PokecenPCClose", 1, 2)},
            {0x134, new CommandType("CasteliaRushInit", 0)},
            {0x135, new CommandType("FieldSetWeather", 1, 2)},
            {0x136, new CommandType("SaveDataWrite", 1, 2)},
            {0x137, new CommandType("SaveDataGetStatus", 3, 2, 2, 2)},
            {0x138, new CommandType("GameCommDisconnect", 1, 2)},
            {0x139, new CommandType("GameCommGetStatus", 1, 2)},
            {0x13A, new CommandType("GameCommCheckDSiWiFi", 1, 2)},
            {0x13B, new CommandType("CMD_13b", 1, 2)},
            {0x13C, new CommandType("CMD_13c", 0)},
            {0x13D, new CommandType("CMD_13d", 0)},
            {0x13E, new CommandType("CMD_13e", 0)},
            {0x13F, new CommandType("EVCameraInit", 0)},
            {0x140, new CommandType("EVCameraEnd", 0)},
            {0x141, new CommandType("EVCameraUnbind", 0)},
            {0x142, new CommandType("EVCameraRebind", 0)},
            {0x143, new CommandType("EVCameraMoveTo", 7, 2, 2, 4, 4, 4, 4, 2)},
            {0x144, new CommandType("EVCameraReturn", 1, 2)},
            {0x145, new CommandType("EVCameraWait", 0)},
            {0x146, new CommandType("EVCameraMoveToCommon", 2, 2, 2)},
            {0x147, new CommandType("EVCameraMoveToDefault", 1, 2)},
            {0x148, new CommandType("EVCameraShake", 8, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x149, new CommandType("CallFriendlyShopBuy", 2, 2, 2)},
            {0x14A, new CommandType("FieldOpen", 0)},
            {0x14B, new CommandType("FieldClose", 0)},
            {0x14C, new CommandType("RTFreeUserHeap", 0)},
            {0x14D, new CommandType("CallRecordSystem", 2, 2, 2)},
            {0x14E, new CommandType("CallBag", 3, 2, 2, 2)},
            {0x14F, new CommandType("CallPC", 2, 2, 2)},
            {0x150, new CommandType("CallMailbox", 1, 2)},
            {0x151, new CommandType("CallPokedexDiploma", 2, 2, 2)},
            {0x152, new CommandType("CallGeonet", 0)},
            {0x153, new CommandType("CallPoke3Select", 1, 2)},
            {0x154, new CommandType("Call3DDemo", 2, 2, 2)},
            {0x155, new CommandType("CallXTransceiver", 2, 2, 2)},
            {0x156, new CommandType("CallGameClear", 1, 2)},
            {0x157, new CommandType("CMD_157", 0)},
            {0x158, new CommandType("CMD_158", 2, 2, 2)},
            {0x159, new CommandType("CMD_159", 0)},
            {0x15A, new CommandType("CMD_15a", 4, 2, 2, 2, 2)},
            {0x15B, new CommandType("CMD_15b", 1, 2)},
            {0x15C, new CommandType("CMD_15c", 0)},
            {0x15D, new CommandType("NetConnectWiFiClub", 0)},
            {0x15E, new CommandType("NetConnectGTS", 0)},
            {0x15F, new CommandType("CMD_15F", 0)},
            {0x160, new CommandType("NetConnectWiFiBattle", 2, 2, 2)},
            {0x161, new CommandType("NetConnectBattleVideo", 1, 2)},
            {0x162, new CommandType("NetConnectGTSNegotiation", 0)},
            {0x163, new CommandType("CMD_163", 2, 2, 1)},
            {0x164, new CommandType("CMD_164", 1, 2)},
            {0x165, new CommandType("CMD_165", 3, 1, 2, 2)},
            {0x166, new CommandType("CMD_166", 3, 2, 1, 2)},
            {0x167, new CommandType("CMD_167", 2, 2, 1)},
            {0x168, new CommandType("CMD_168", 1, 2)},
            {0x169, new CommandType("CMD_169", 3, 1, 2, 2)},
            {0x16A, new CommandType("CMD_16a", 3, 2, 1, 2)},
            {0x16B, new CommandType("CMD_16b", 4, 2, 2, 2, 2)},
            {0x16C, new CommandType("CMD_16c", 1, 2)},
            {0x16D, new CommandType("CMD_16d", 1, 2)},
            {0x16E, new CommandType("CMD_16e", 2, 2, 2)},
            {0x16F, new CommandType("CMD_16f", 0)},
            {0x170, new CommandType("CMD_170", 1, 2)},
            {0x171, new CommandType("CMD_171", 0)},
            {0x172, new CommandType("CMD_172", 1, 2)},
            {0x173, new CommandType("CMD_173", 1, 2)},
            {0x174, new CommandType("CMD_174", 3, 2, 2, 2)},
            {0x175, new CommandType("CMD_175", 1, 2)},
            {0x176, new CommandType("CMD_176", 1, 2)},
            {0x177, new CommandType("CMD_177", 0)},
            {0x178, new CommandType("CallWildBattle", 3, 2, 2, 2)},
            {0x179, new CommandType("CallWildBattleEnd", 0)},
            {0x17A, new CommandType("CallWildLose", 0)},
            {0x17B, new CommandType("WildBattleIsVictory", 1, 2)},
            {0x17C, new CommandType("WildBattleGetResult", 1, 2)},
            {0x17D, new CommandType("CMD_17d", 1, 2)},
            {0x17E, new CommandType("CMD_17e", 1, 2)},
            {0x17F, new CommandType("CMD_17f", 2, 2, 2)},
            {0x180, new CommandType("CMD_180", 1, 2)},
            {0x181, new CommandType("CMD_181", 1, 2)},
            {0x182, new CommandType("CMD_182", 1, 2)},
            {0x183, new CommandType("CMD_183", 0)},
            {0x184, new CommandType("CMD_184", 1, 2)},
            {0x185, new CommandType("CMD_185", 1, 2)},
            {0x186, new CommandType("CMD_186", 1, 2)},
            {0x187, new CommandType("CMD_187", 1, 2)},
            {0x188, new CommandType("CMD_188", 1, 2)},
            {0x189, new CommandType("CMD_189", 1, 2)},
            {0x18A, new CommandType("CMD_18a", 0)},
            {0x18B, new CommandType("CMD_18b", 0)},
            {0x18C, new CommandType("CMD_18c", 2, 2, 2)},
            {0x18D, new CommandType("CMD_18d", 1, 2)},
            {0x18E, new CommandType("CMD_18e", 1, 2)},
            {0x18F, new CommandType("CMD_18f", 8, 2, 2, 2, 2, 2, 2, 2, 2)},
            {0x190, new CommandType("CMD_190", 1, 2)},
            {0x191, new CommandType("CMD_191", 1, 2)},
            {0x192, new CommandType("CMD_192", 1, 2)},
            {0x193, new CommandType("CMD_193", 1, 2)},
            {0x194, new CommandType("CMD_194", 1, 2)},
            {0x195, new CommandType("CMD_195", 1, 2)},
            {0x196, new CommandType("CMD_196", 0)},
            {0x197, new CommandType("CMD_197", 1, 2)},
            {0x198, new CommandType("CMD_198", 1, 2)},
            {0x199, new CommandType("CMD_199", 1, 2)},
            {0x19A, new CommandType("CMD_19a", 2, 2, 2)},
            {0x19B, new CommandType("CMD_19b", 1, 2)},
            {0x19C, new CommandType("CMD_19c", 1, 2)},
            {0x19D, new CommandType("CMD_19d", 1, 2)},
            {0x19E, new CommandType("CMD_19e", 1, 2)},
            {0x19F, new CommandType("PlayFieldEffect", 1, 2)},
            {0x1A0, new CommandType("PlayHMCutInEffect", 1, 2)},
            {0x1A1, new CommandType("PlayAlderFlyEffect", 1, 2)},
            {0x1A2, new CommandType("CMD_1a2", 1, 2)},
            {0x1A3, new CommandType("CMD_1a3", 1, 2)},
            {0x1A4, new CommandType("CMD_1a4", 1, 2)},
            {0x1A5, new CommandType("CMD_1a5", 0)},
            {0x1A6, new CommandType("CMD_1a6", 0)},
            {0x1A7, new CommandType("CMD_1a7", 2, 2, 2)},
            {0x1A8, new CommandType("CMD_1a8", 0)},
            {0x1A9, new CommandType("CMD_1a9", 4, 2, 2, 2, 2)},
            {0x1AA, new CommandType("CMD_1aa", 4, 2, 2, 2, 2)},
            {0x1AB, new CommandType("CMD_1ab", 1, 2)},
            {0x1AC, new CommandType("CMD_1ac", 0)},
            {0x1AD, new CommandType("CMD_1ad", 0)},
            {0x1AE, new CommandType("CMD_1ae", 0)},
            {0x1AF, new CommandType("CMD_1af", 0)},
            {0x1B0, new CommandType("CMD_1b0", 2, 2, 2)},
            {0x1B1, new CommandType("CMD_1b1", 0)},
            {0x1B2, new CommandType("CMD_1b2", 1, 2)},
            {0x1B3, new CommandType("CMD_1b3", 2, 2, 2)},
            {0x1B4, new CommandType("CMD_1b4", 2, 2, 2)},
            {0x1B5, new CommandType("CMD_1b5", 0)},
            {0x1B6, new CommandType("CMD_1b6", 0)},
            {0x1B7, new CommandType("CMD_1b7", 0)},
            {0x1B8, new CommandType("CMD_1b8", 0)},
            {0x1B9, new CommandType("CMD_1b9", 2, 2, 2)},
            {0x1BA, new CommandType("CMD_1ba", 2, 2, 2)},
            {0x1BB, new CommandType("CMD_1bb", 0)},
            {0x1BC, new CommandType("CMD_1bc", 1, 2)},
            {0x1BD, new CommandType("CMD_1bd", 2, 2, 2)},
            {0x1BE, new CommandType("FieldTradeStart", 2, 2, 2)},
            {0x1BF, new CommandType("FieldTradeCheck", 3, 2, 2, 2)},
            {0x1C1, new CommandType("ElevatorSetTablePtr", 1, 4)},
            {0x1C2, new CommandType("ElevatorBuildListMenu", 0)},
            {0x1C3, new CommandType("ElevatorChangeMap", 1, 2)},
            {0x1C4, new CommandType("PokeDexIsComplete", 2, 2, 2)},
            {0x1C5, new CommandType("PokeDexGetEvaluationParams", 4, 2, 2, 2, 2)},
            {0x1C6, new CommandType("PokeDexGiveNational", 0)},
            {0x1C7, new CommandType("PokeDexHaveNational", 1, 2)},
            {0x1C8, new CommandType("PokeDexEnable", 0)},
            {0x1C9, new CommandType("CMD_1c9", 2, 2, 2)},
            {0x1CA, new CommandType("CMD_1ca", 0)},
            {0x1CB, new CommandType("CMD_1cb", 1, 4)},
            {0x1CC, new CommandType("CMD_1cc", 0)},
            {0x1CD, new CommandType("CMD_1cd", 1, 2)},
            {0x1CE, new CommandType("CMD_1ce", 2, 2, 2)},
            {0x1CF, new CommandType("CMD_1cf", 3, 2, 2, 2)},
            {0x1D0, new CommandType("CMD_1d0", 4, 2, 2, 2, 2)},
            {0x1D1, new CommandType("CMD_1d1", 1, 2)},
            {0x1D2, new CommandType("CMD_1d2", 3, 2, 2, 2)},
            {0x1D3, new CommandType("CMD_1d3", 3, 2, 2, 2)},
            {0x1D4, new CommandType("CMD_1d4", 0)},
            {0x1D5, new CommandType("CMD_1d5", 0)},
            {0x1D6, new CommandType("CMD_1d6", 1, 2)},
            {0x1D7, new CommandType("ObjInitProxyGPos", 4, 2, 2, 2, 2)},
            {0x1D8, new CommandType("ObjInitWarpGPos", 4, 2, 2, 2, 2)},
            {0x1D9, new CommandType("ObjInitNPCGPos", 5, 2, 2, 2, 2, 2)},
            {0x1DA, new CommandType("CallPhraseSelect", 4, 2, 2, 2, 2)},
            {0x1DB, new CommandType("CMD_1db", 1, 2)},
            {0x1DC, new CommandType("CMD_1dc", 2, 2, 2)},
            {0x1DD, new CommandType("CMD_1dd", 2, 2, 2)},
            {0x1DE, new CommandType("CMD_1de", 2, 2, 2)},
            {0x1DF, new CommandType("CMD_1df", 3, 2, 2, 2)},
            {0x1E0, new CommandType("CMD_1e0", 4, 2, 2, 2, 2)},
            {0x1E1, new CommandType("CMD_1e1", 1, 2)},
            {0x1E2, new CommandType("CMD_1e2", 0)},
            {0x1E3, new CommandType("CMD_1e3", 1, 2)},
            {0x1E4, new CommandType("CMD_1e4", 6, 2, 2, 2, 2, 2, 2)},
            {0x1E5, new CommandType("CMD_1e5", 0)},
            {0x1E6, new CommandType("CMD_1e6", 0)},
            {0x1E7, new CommandType("CMD_1e7", 0)},
            {0x1E8, new CommandType("CMD_1e8", 2, 2, 2)},
            {0x1E9, new CommandType("CMD_1e9", 2, 2, 2)},
            {0x1EA, new CommandType("CMD_1ea", 4, 2, 2, 2, 2)},
            {0x1EB, new CommandType("CMD_1eb", 4, 2, 2, 2, 2)},
            {0x1EC, new CommandType("CMD_1ec", 5, 2, 2, 2, 2, 2)},
            {0x1ED, new CommandType("CMD_1ed", 4, 2, 2, 2, 2)},
            {0x1EE, new CommandType("CMD_1ee", 2, 2, 2)},
            {0x1EF, new CommandType("CMD_1ef", 2, 2, 2)},
            {0x1F0, new CommandType("CMD_1f0", 2, 2, 2)},
            {0x1F1, new CommandType("CMD_1f1", 2, 2, 2)},
            {0x1F2, new CommandType("CMD_1f2", 0)},
            {0x1F3, new CommandType("CMD_1f3", 1, 2)},
            {0x1F4, new CommandType("CMD_1f4", 0)},
            {0x1F5, new CommandType("CMD_1f5", 0)},
            {0x1F6, new CommandType("CMD_1f6", 3, 2, 2, 2)},
            {0x1F7, new CommandType("CMD_1f7", 6, 2, 2, 2, 2, 2, 2)},
            {0x1F8, new CommandType("CMD_1f8", 0)},
            {0x1F9, new CommandType("CMD_1f9", 0)},
            {0x1FA, new CommandType("CMD_1fa", 0)},
            {0x1FB, new CommandType("CMD_1fb", 2, 2, 2)},
            {0x1FC, new CommandType("CMD_1fc", 3, 2, 2, 2)},
            {0x1FD, new CommandType("CMD_1fd", 4, 2, 2, 2, 2)},
            {0x1FE, new CommandType("CMD_1fe", 0)},
            {0x1FF, new CommandType("CMD_1ff", 1, 2)},
            {0x200, new CommandType("CMD_200", 3, 2, 2, 2)},
            {0x201, new CommandType("CMD_201", 2, 2, 2)},
            {0x202, new CommandType("CMD_202", 1, 2)},
            {0x203, new CommandType("CMD_203", 1, 2)},
            {0x204, new CommandType("CMD_204", 1, 2)},
            {0x205, new CommandType("CMD_205", 1, 2)},
            {0x206, new CommandType("CMD_206", 1, 2)},
            {0x207, new CommandType("CMD_207", 2, 2, 2)},
            {0x208, new CommandType("CMD_208", 1, 2)},
            {0x209, new CommandType("CMD_209", 4, 2, 2, 2, 2)},
            {0x20A, new CommandType("CMD_20a", 4, 2, 2, 2, 2)},
            {0x20B, new CommandType("CMD_20b", 1, 2)},
            {0x20C, new CommandType("CMD_20c", 4, 2, 2, 2, 2)},
            {0x20D, new CommandType("CMD_20d", 4, 2, 2, 2, 2)},
            {0x20E, new CommandType("CMD_20e", 6, 2, 2, 2, 2, 2, 2)},
            {0x20F, new CommandType("CMD_20f", 3, 2, 2, 2)},
            {0x210, new CommandType("CMD_210", 1, 2)},
            {0x211, new CommandType("CMD_211", 1, 2)},
            {0x212, new CommandType("CMD_212", 3, 2, 2, 2)},
            {0x213, new CommandType("CMD_213", 1, 2)},
            {0x214, new CommandType("CMD_214", 4, 2, 2, 2, 2)},
            {0x215, new CommandType("CMD_215", 2, 2, 2)},
            {0x216, new CommandType("CMD_216", 2, 2, 2)},
            {0x217, new CommandType("CMD_217", 1, 2)},
            {0x218, new CommandType("CMD_218", 2, 2, 2)},
            {0x219, new CommandType("CMD_219", 2, 2, 2)},
            {0x21A, new CommandType("CMD_21a", 2, 2, 2)},
            {0x21B, new CommandType("CMD_21b", 0)},
            {0x21C, new CommandType("CMD_21c", 2, 2, 2)},
            {0x21D, new CommandType("CMD_21d", 1, 2)},
            {0x21E, new CommandType("CMD_21e", 1, 2)},
            {0x21F, new CommandType("CMD_21f", 0)},
            {0x220, new CommandType("PokePartyFindEx", 4, 2, 2, 2, 2)},
            {0x221, new CommandType("CMD_221", 6, 2, 2, 2, 2, 2, 2)},
            {0x222, new CommandType("CMD_222", 4, 2, 2, 2, 2)},
            {0x223, new CommandType("CMD_223", 1, 2)},
            {0x224, new CommandType("CMD_224", 1, 2)},
            {0x225, new CommandType("CMD_225", 3, 2, 2, 2)},
            {0x226, new CommandType("CMD_226", 1, 2)},
            {0x227, new CommandType("CMD_227", 4, 2, 2, 2, 2)},
            {0x228, new CommandType("CMD_228", 2, 2, 2)},
            {0x229, new CommandType("CMD_229", 2, 2, 2)},
            {0x22A, new CommandType("CMD_22a", 1, 2)},
            {0x22B, new CommandType("CMD_22b", 2, 2, 2)},
            {0x22D, new CommandType("CMD_22d", 1, 2)},
            {0x22E, new CommandType("CMD_22e", 1, 2)},
            {0x22F, new CommandType("CMD_22f", 2, 2, 2)},
            {0x230, new CommandType("CMD_230", 2, 2, 2)},
            {0x231, new CommandType("CMD_231", 1, 2)},
            {0x232, new CommandType("CMD_232", 2, 2, 2)},
            {0x233, new CommandType("CMD_233", 2, 2, 2)},
            {0x234, new CommandType("CMD_234", 2, 2, 2)},
            {0x235, new CommandType("CMD_235", 2, 2, 2)},
            {0x236, new CommandType("CMD_236", 4, 2, 2, 2, 2)},
            {0x237, new CommandType("CMD_237", 2, 2, 2)},
            {0x238, new CommandType("CMD_238", 1, 2)},
            {0x239, new CommandType("CMD_239", 1, 2)},
            {0x23A, new CommandType("CMD_23a", 2, 2, 2)},
            {0x23B, new CommandType("CMD_23b", 2, 2, 2)},
            {0x23C, new CommandType("CMD_23c", 0)},
            {0x23D, new CommandType("CMD_23d", 2, 2, 2)},
            {0x23E, new CommandType("CMD_23e", 2, 2, 2)},
            {0x23F, new CommandType("CMD_23f", 0)},
            {0x240, new CommandType("CMD_240", 1, 2)},
            {0x241, new CommandType("CMD_241", 1, 2)},
            {0x242, new CommandType("CMD_242", 2, 2, 2)},
            {0x243, new CommandType("CMD_243", 2, 2, 2)},
            {0x244, new CommandType("CMD_244", 1, 2)},
            {0x245, new CommandType("CMD_245", 2, 2, 2)},
            {0x246, new CommandType("CMD_246", 1, 2)},
            {0x247, new CommandType("CMD_247", 5, 2, 2, 2, 2, 2)},
            {0x248, new CommandType("CMD_248", 2, 2, 2)},
            {0x249, new CommandType("CMD_249", 4, 2, 2, 2, 2)},
            {0x24A, new CommandType("CMD_24a", 1, 2)},
            {0x24B, new CommandType("CMD_24b", 1, 2)},
            {0x24C, new CommandType("CMD_24c", 1, 2)},
            {0x24D, new CommandType("CMD_24d", 0)},
            {0x24E, new CommandType("CMD_24e", 2, 2, 2)},
            {0x24F, new CommandType("CMD_24f", 6, 2, 2, 2, 2, 2, 2)},
            {0x250, new CommandType("CMD_250", 5, 2, 2, 2, 2, 2)},
            {0x251, new CommandType("CMD_251", 3, 2, 2, 2)},
            {0x252, new CommandType("CMD_252", 0)},
            {0x253, new CommandType("CMD_253", 2, 2, 2)},
            {0x254, new CommandType("CMD_254", 1, 2)},
            {0x255, new CommandType("CMD_255", 6, 2, 2, 2, 2, 2, 2)},
            {0x256, new CommandType("CMD_256", 3, 2, 2, 2)},
            {0x257, new CommandType("CMD_257", 0)},
            {0x258, new CommandType("CMD_258", 0)},
            {0x259, new CommandType("CMD_259", 1, 2)},
            {0x25A, new CommandType("CMD_25a", 1, 2)},
            {0x25B, new CommandType("CMD_25b", 0)},
            {0x25C, new CommandType("CMD_25c", 6, 2, 2, 2, 2, 2, 2)},
            {0x25D, new CommandType("CMD_25d", 1, 2)},
            {0x25E, new CommandType("CMD_25e", 0)},
            {0x25F, new CommandType("CMD_25f", 0)},
            {0x260, new CommandType("CMD_260", 0)},
        };

        internal static Dictionary<int, Dictionary<int, CommandType>> bw2OverlayCommands = new Dictionary<int, Dictionary<int, CommandType>>()
        {
            { 50, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 2, 2, 2)},
                {0x3EA, new CommandType("c0x3EA", 0)},
                {0x3EB, new CommandType("c0x3EB", 4, 2, 2, 2, 2)},
            } },
            { 51, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 1, 2)},
                {0x3E9, new CommandType("c0x3E9", 0)},
                {0x3EA, new CommandType("c0x3EA", 2, 2, 2)},
                {0x3EB, new CommandType("c0x3EB", 2, 2, 2)},
                {0x3EC, new CommandType("c0x3EC", 4, 2, 2, 2, 2)},
                {0x3ED, new CommandType("c0x3ED", 0)},
            } },
            { 52, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 1, 2)},
                {0x3E9, new CommandType("c0x3E9", 1, 2)},
                {0x3EA, new CommandType("c0x3EA", 1, 2)},
                {0x3EB, new CommandType("c0x3EB", 1, 2)},
                {0x3EC, new CommandType("c0x3EC", 0)},
                {0x3ED, new CommandType("c0x3ED", 1, 2)},
                {0x3EE, new CommandType("c0x3EE", 0)},
                {0x3EF, new CommandType("c0x3EF", 0)},
                {0x3F0, new CommandType("c0x3F0", 2, 2, 2)},
                {0x3F1, new CommandType("c0x3F1", 0)},
                {0x3F2, new CommandType("c0x3F2", 1, 2)},
                {0x3F3, new CommandType("c0x3F3", 1, 2)},
                {0x3F4, new CommandType("c0x3F4", 0)},
                {0x3F5, new CommandType("c0x3F5", 0)},
                {0x3F6, new CommandType("c0x3F6", 1, 2)},
                {0x3F7, new CommandType("c0x3F7", 0)},
                {0x3F8, new CommandType("c0x3F8", 0)},
                {0x3F9, new CommandType("c0x3F9", 0)},
                {0x3FA, new CommandType("c0x3FA", 0)},
                {0x3FB, new CommandType("c0x3FB", 1, 2)},
                {0x3FC, new CommandType("c0x3FC", 1, 2)},
                {0x3FD, new CommandType("c0x3FD", 0)},
            } },
            { 53, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 2, 1, 2)},
            } },
            { 54, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 1, 2)},
            } },
            { 55, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 0)},
                {0x3EA, new CommandType("c0x3EA", 1, 2)},
                {0x3EB, new CommandType("c0x3EB", 1, 2)},
                {0x3EC, new CommandType("c0x3EC", 1, 2)},
                {0x3ED, new CommandType("c0x3ED", 0)},
                {0x3EE, new CommandType("c0x3EE", 2, 2, 2)},
                {0x3EF, new CommandType("c0x3EF", 2, 2, 2)},
                {0x3F0, new CommandType("c0x3F0", 1, 2)},
                {0x3F1, new CommandType("c0x3F1", 1, 2)},
                {0x3F2, new CommandType("c0x3F2", 1, 2)},
                {0x3F3, new CommandType("c0x3F3", 1, 2)},
                {0x3F4, new CommandType("c0x3F4", 1, 2)},
                {0x3F5, new CommandType("c0x3F5", 1, 2)},
                {0x3F6, new CommandType("c0x3F6", 1, 2)},
                {0x3F7, new CommandType("c0x3F7", 1, 2)},
                {0x3F8, new CommandType("c0x3F8", 1, 2)},
                {0x3F9, new CommandType("c0x3F9", 0)},
                {0x3FA, new CommandType("c0x3FA", 3, 2, 2, 2)},
                {0x3FB, new CommandType("c0x3FB", 1, 2)},
                {0x3FC, new CommandType("c0x3FC", 1, 2)},
                {0x3FE, new CommandType("c0x3FE", 0)},
                {0x3FF, new CommandType("c0x3FF", 3, 2, 2, 2)},
                {0x400, new CommandType("c0x400", 1, 2)},
                {0x401, new CommandType("c0x401", 1, 2)},
                {0x402, new CommandType("c0x402", 2, 2, 2)},
                {0x403, new CommandType("c0x403", 1, 2)},
                {0x404, new CommandType("c0x404", 1, 2)},
                {0x405, new CommandType("c0x405", 0)},
                {0x406, new CommandType("c0x406", 0)},
                {0x407, new CommandType("c0x407", 0)},
                {0x408, new CommandType("c0x408", 1, 2)},
                {0x409, new CommandType("c0x409", 2, 2, 2)},
                {0x40A, new CommandType("c0x40A", 1, 2)},
                {0x40B, new CommandType("c0x40B", 0)},
                {0x40C, new CommandType("c0x40C", 0)},
                {0x40D, new CommandType("c0x40D", 0)},
                {0x40E, new CommandType("c0x40E", 1, 2)},
                {0x40F, new CommandType("c0x40F", 0)},
                {0x410, new CommandType("c0x410", 1, 2)},
                {0x411, new CommandType("c0x411", 2, 2, 2)},
                {0x412, new CommandType("c0x412", 1, 2)},
                {0x413, new CommandType("c0x413", 1, 2)},
                {0x414, new CommandType("c0x414", 2, 1, 2)},
                {0x415, new CommandType("c0x415", 0)},
                {0x416, new CommandType("c0x416", 1, 2)},
                {0x417, new CommandType("c0x417", 1, 2)},
                {0x418, new CommandType("c0x418", 1, 2)},
                {0x419, new CommandType("c0x419", 2, 2, 2)},
                {0x41A, new CommandType("c0x41A", 1, 2)},
                {0x41B, new CommandType("c0x41B", 2, 2, 2)},
                {0x41C, new CommandType("c0x41C", 1, 2)},
                {0x41D, new CommandType("c0x41D", 2, 2, 2)},
                {0x41E, new CommandType("c0x41E", 0)},
                {0x41F, new CommandType("c0x41F", 2, 2, 2)},
                {0x420, new CommandType("c0x420", 1, 2)},
                {0x421, new CommandType("c0x421", 1, 2)},
                {0x422, new CommandType("c0x422", 1, 2)},
                {0x423, new CommandType("c0x423", 1, 2)},
                {0x424, new CommandType("c0x424", 1, 2)},
            } },
            { 58, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 0)},
                {0x3EA, new CommandType("c0x3EA", 2, 2, 2)},
                {0x3EB, new CommandType("c0x3EB", 2, 2, 2)},
                {0x3EC, new CommandType("c0x3EC", 2, 2, 2)},
                {0x3ED, new CommandType("c0x3ED", 2, 2, 2)},
                {0x3EE, new CommandType("c0x3EE", 1, 2)},
                {0x3EF, new CommandType("c0x3EF", 4, 2, 2, 2, 2)},
                {0x3F0, new CommandType("c0x3F0", 3, 2, 2, 2)},
                {0x3F1, new CommandType("c0x3F1", 4, 2, 2, 2, 2)},
                {0x3F2, new CommandType("c0x3F2", 3, 2, 2, 2)},
                {0x3F3, new CommandType("c0x3F3", 0)},
                {0x3F4, new CommandType("c0x3F4", 0)},
                {0x3F5, new CommandType("c0x3F5", 1, 2)},
                {0x3F6, new CommandType("c0x3F6", 1, 2)},
                {0x3F7, new CommandType("c0x3F7", 3, 2, 2, 2)},
                {0x3F8, new CommandType("c0x3F8", 3, 2, 2, 2)},
                {0x3F9, new CommandType("c0x3F9", 3, 2, 2, 2)},
                {0x3FA, new CommandType("c0x3FA", 4, 2, 2, 2, 2)},
                {0x3FB, new CommandType("c0x3FB", 3, 2, 2, 2)},
                {0x3FC, new CommandType("c0x3FC", 0)},
                {0x3FD, new CommandType("c0x3FD", 2, 2, 2)},
                {0x3FE, new CommandType("c0x3FE", 5, 2, 2, 2, 2, 2)},
                {0x3FF, new CommandType("c0x3FF", 2, 2, 2)},
                {0x400, new CommandType("c0x400", 4, 2, 2, 2, 2)},
                {0x401, new CommandType("c0x401", 1, 2)},
                {0x402, new CommandType("c0x402", 0)},
                {0x403, new CommandType("c0x403", 2, 2, 2)},
                {0x404, new CommandType("c0x404", 2, 2, 2)},
                {0x405, new CommandType("c0x405", 3, 2, 2, 2)},
                {0x406, new CommandType("c0x406", 2, 2, 2)},
                {0x407, new CommandType("c0x407", 2, 2, 2)},
                {0x408, new CommandType("c0x408", 3, 2, 2, 2)},
                {0x409, new CommandType("c0x409", 3, 2, 2, 2)},
                {0x40A, new CommandType("c0x40A", 3, 2, 2, 2)},
                {0x40B, new CommandType("c0x40B", 3, 2, 2, 2)},
                {0x40C, new CommandType("c0x40C", 1, 1)},
                {0x40D, new CommandType("c0x40D", 1, 2)},
                {0x40E, new CommandType("c0x40E", 2, 2, 2)},
                {0x40F, new CommandType("c0x40F", 1, 2)},
            } },
            { 61, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 3, 2, 2, 2)},
                {0x3EB, new CommandType("c0x3EB", 0)},
                {0x3EC, new CommandType("c0x3EC", 1, 2)},
                {0x3ED, new CommandType("c0x3ED", 1, 2)},
                {0x3EE, new CommandType("c0x3EE", 1, 2)},
                {0x3EF, new CommandType("c0x3EF", 0)},
                {0x3F0, new CommandType("c0x3F0", 1, 2)},
                {0x3F1, new CommandType("c0x3F1", 2, 2, 2)},
                {0x3F2, new CommandType("c0x3F2", 1, 2)},
                {0x3F3, new CommandType("c0x3F3", 2, 2, 2)},
                {0x3F4, new CommandType("c0x3F4", 1, 2)},
                {0x3F5, new CommandType("c0x3F5", 1, 2)},
                {0x3F6, new CommandType("c0x3F6", 2, 2, 2)},
                {0x3F7, new CommandType("c0x3F7", 2, 2, 2)},
                {0x3F8, new CommandType("c0x3F8", 2, 2, 2)},
                {0x3F9, new CommandType("c0x3F9", 1, 2)},
                {0x3FA, new CommandType("c0x3FA", 2, 2, 2)},
                {0x3FB, new CommandType("c0x3FB", 1, 2)},
                {0x3FC, new CommandType("c0x3FC", 1, 2)},
                {0x3FD, new CommandType("c0x3FD", 1, 2)},
                {0x3FE, new CommandType("c0x3FE", 2, 2, 2)},
                {0x3FF, new CommandType("c0x3FF", 1, 2)},
                {0x400, new CommandType("c0x400", 3, 2, 2, 2)},
                {0x401, new CommandType("c0x401", 0)},
                {0x402, new CommandType("c0x402", 2, 2, 2)},
                {0x403, new CommandType("c0x403", 2, 2, 2)},
                {0x404, new CommandType("c0x404", 1, 2)},
                {0x405, new CommandType("c0x405", 1, 2)},
                {0x406, new CommandType("c0x406", 0)},
                {0x407, new CommandType("c0x407", 1, 2)},
            } },
            { 62, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 3, 2, 2, 2)},
                {0x3E9, new CommandType("c0x3E9", 2, 2, 2)},
                {0x3EA, new CommandType("c0x3EA", 1, 2)},
                {0x3EB, new CommandType("c0x3EB", 2, 2, 2)},
                {0x3EC, new CommandType("c0x3EC", 1, 2)},
                {0x3ED, new CommandType("c0x3ED", 2, 1, 2)},
                {0x3EE, new CommandType("c0x3EE", 3, 2, 2, 2)},
                {0x3EF, new CommandType("c0x3EF", 2, 2, 2)},
                {0x3F0, new CommandType("c0x3F0", 1, 2)},
                {0x3F1, new CommandType("c0x3F1", 2, 2, 2)},
                {0x3F2, new CommandType("c0x3F2", 2, 2, 2)},
                {0x3F3, new CommandType("c0x3F3", 0)},
                {0x3F4, new CommandType("c0x3F4", 0)},
                {0x3F5, new CommandType("c0x3F5", 1, 2)},
                {0x3F6, new CommandType("c0x3F6", 1, 2)},
                {0x3F7, new CommandType("c0x3F7", 3, 2, 2, 2)},
                {0x3F8, new CommandType("c0x3F8", 1, 2)},
                {0x3F9, new CommandType("c0x3F9", 2, 2, 2)},
                {0x3FA, new CommandType("c0x3FA", 2, 2, 2)},
                {0x3FB, new CommandType("c0x3FB", 3, 2, 2, 2)},
                {0x3FC, new CommandType("c0x3FC", 2, 2, 2)},
                {0x3FD, new CommandType("c0x3FD", 1, 2)},
                {0x3FE, new CommandType("c0x3FE", 1, 2)},
                {0x3FF, new CommandType("c0x3FF", 1, 2)},
                {0x400, new CommandType("c0x400", 4, 2, 2, 2, 2)},
                {0x401, new CommandType("c0x401", 2, 2, 2)},
                {0x402, new CommandType("c0x402", 0)},
            } },
            { 63, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 1, 2)},
                {0x3E9, new CommandType("c0x3E9", 0)},
            } },
            { 64, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 1, 2)},
                {0x3E9, new CommandType("c0x3E9", 2, 2, 2)},
                {0x3EA, new CommandType("c0x3EA", 1, 2)},
                {0x3EB, new CommandType("c0x3EB", 1, 2)},
                {0x3EC, new CommandType("c0x3EC", 1, 2)},
                {0x3ED, new CommandType("c0x3ED", 1, 2)},
                {0x3EE, new CommandType("c0x3EE", 2, 2, 2)},
                {0x3EF, new CommandType("c0x3EF", 1, 2)},
                {0x3F0, new CommandType("c0x3F0", 1, 2)},
            } },
            { 65, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 1, 2)},
                {0x3E9, new CommandType("c0x3E9", 2, 2, 2)},
                {0x3ED, new CommandType("c0x3ED", 2, 2, 2)},
                {0x3EE, new CommandType("c0x3EE", 0)},
                {0x3EF, new CommandType("c0x3EF", 0)},
                {0x3F0, new CommandType("c0x3F0", 2, 2, 2)},
                {0x3F1, new CommandType("c0x3F1", 1, 2)},
            } },
            { 66, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 0)},
                {0x3E9, new CommandType("c0x3E9", 1, 2)},
                {0x3EA, new CommandType("c0x3EA", 1, 2)},
                {0x3EB, new CommandType("c0x3EB", 1, 2)},
                {0x3EC, new CommandType("c0x3EC", 1, 2)},
                {0x3ED, new CommandType("c0x3ED", 0)},
                {0x3EE, new CommandType("c0x3EE", 3, 2, 2, 2)},
                {0x3EF, new CommandType("c0x3EF", 0)},
                {0x3F1, new CommandType("c0x3F1", 0)},
                {0x3F2, new CommandType("c0x3F2", 1, 2)},
                {0x3F3, new CommandType("c0x3F3", 0)},
                {0x3F4, new CommandType("c0x3F4", 0)},
                {0x3F5, new CommandType("c0x3F5", 6, 2, 2, 2, 2, 2, 2)},
                {0x3F6, new CommandType("c0x3F6", 3, 2, 2, 2)},
                {0x3F7, new CommandType("c0x3F7", 0)},
                {0x3F8, new CommandType("c0x3F8", 0)},
            } },
            { 67, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 3, 2, 2, 2)},
                {0x3E9, new CommandType("c0x3E9", 4, 2, 2, 2, 2)},
            } },
            { 68, new Dictionary<int, CommandType>() {
                {0x3E8, new CommandType("c0x3E8", 1, 2)},
                {0x3E9, new CommandType("c0x3E9", 1, 2)},
                {0x3EA, new CommandType("c0x3EA", 1, 2)},
                {0x3EB, new CommandType("c0x3EB", 1, 2)},
                {0x3EC, new CommandType("c0x3EC", 0)},
                {0x3ED, new CommandType("c0x3ED", 1, 2)},
                {0x3EE, new CommandType("c0x3EE", 0)},
                {0x3EF, new CommandType("c0x3EF", 0)},
            } },
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

    internal struct CommandDefinition
    {
        public string name;
    }
}
