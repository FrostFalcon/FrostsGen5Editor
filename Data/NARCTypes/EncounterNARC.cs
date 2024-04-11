using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class EncounterNARC : NARC
    {
        public List<EncounterEntry> encounterPools
        {
            get
            {
                List<EncounterEntry> list = mainEncounterPools.Union(subEncounterPools).ToList();
                list.Sort((e1, e2) => e1.nameID == e2.nameID ? e1.season - e2.season : e1.nameID - e2.nameID);
                return list;
            }
        }

        public List<EncounterEntry> mainEncounterPools;
        public List<EncounterEntry> subEncounterPools;

        public override void ReadData()
        {
            base.ReadData();

            //Find first file instance
            int pos = numFileEntries * 8;
            while (pos < byteData.Length)
            {
                pos++;
                if (pos >= byteData.Length) return;
                if (byteData[pos] == 'B' && byteData[pos + 1] == 'T' && byteData[pos + 2] == 'N' && byteData[pos + 3] == 'F') break;
            }
            int initialPosition = pos + 24;

            //Register data files
            mainEncounterPools = new List<EncounterEntry>();
            subEncounterPools = new List<EncounterEntry>();

            pos = pointerStartAddress;

            //Populate data types
            int season = 0;
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                season = end - start > 240 ? 0 : -1;
                for (int j = start; j < end; j += 232)
                {
                    byte[] bytes = new byte[232];

                    for (int k = 0; k < 232; k++) bytes[k] = byteData[initialPosition + j + k];

                    EncounterEntry p = new EncounterEntry(bytes) { nameID = i, season = season };
                    season++;

                    if (j == start) mainEncounterPools.Add(p);
                    else
                    {
                        p.parentPool = mainEncounterPools[mainEncounterPools.Count - 1];
                        subEncounterPools.Add(p);
                    }
                }

                pos += 8;
            }
        }

        public override void WriteData()
        {
            List<byte> newByteData = new List<byte>();
            List<byte> oldByteData = new List<byte>(byteData);

            newByteData.AddRange(oldByteData.GetRange(0, pointerStartAddress));

            //Find start of file instances
            int pos = 0;
            while (pos < byteData.Length)
            {
                pos++;
                if (pos >= byteData.Length) return;
                if (byteData[pos] == 'B' && byteData[pos + 1] == 'T' && byteData[pos + 2] == 'N' && byteData[pos + 3] == 'F') break;
            }

            newByteData.AddRange(oldByteData.GetRange(pos, 24));

            //Write Files
            int totalSize = 0;
            int pPos = pointerStartAddress;
            foreach (EncounterEntry e in mainEncounterPools)
            {
                newByteData.AddRange(e.bytes);
                foreach (EncounterEntry e2 in subEncounterPools.Where(s => s.parentPool == e)) newByteData.AddRange(e2.bytes);

                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += e.bytes.Length;
                foreach (EncounterEntry e2 in subEncounterPools.Where(s => s.parentPool == e)) totalSize += e2.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }

            byteData = newByteData.ToArray();

            FixHeaders(mainEncounterPools.Count);

            base.WriteData();
        }
    }

    public class EncounterEntry
    {
        public byte[] bytes;
        public int nameID;
        public int season;
        public EncounterEntry parentPool;

        public List<EncounterSlot[]> landSlots;
        public List<EncounterSlot[]> waterSlots;
        public List<List<EncounterSlot>> groupedLandSlots;
        public List<List<EncounterSlot>> groupedWaterSlots;

        public EncounterEntry(byte[] bytes)
        {
            this.bytes = bytes;

            ReadData();
        }

        internal void ReadData()
        {
            landSlots = new List<EncounterSlot[]>();
            waterSlots = new List<EncounterSlot[]>();
            int loc = 8;

            for (int i = 0; i < 7; i++)
            {
                List<EncounterSlot[]> slotList;
                if (i <= 2) slotList = landSlots;
                else slotList = waterSlots;

                EncounterSlot[] slotAssignment = new EncounterSlot[12];
                if (i > 2) slotAssignment = new EncounterSlot[5];
                List<int> encounterRates = new List<int>() { 20, 20, 10, 10, 10, 10, 5, 5, 4, 4, 1, 1 };
                if (i > 2) encounterRates = new List<int>() { 60, 30, 5, 4, 1 };
                for (int x = 0; x < slotAssignment.Length; x++)
                {
                    slotAssignment[x] = new EncounterSlot((short)HelperFunctions.ReadShort(bytes, loc), 0, bytes[loc + 2], bytes[loc + 3], encounterRates[x]);
                    while (slotAssignment[x].pokemonID > 2048)
                    {
                        slotAssignment[x].pokemonID -= 2048;
                        slotAssignment[x].pokemonForm++;
                    }
                    loc += 4;
                }
                slotList.Add(slotAssignment);
            }

            EncounterSlotsToGroups();
        }

        /// <summary>
        /// Returns false if the encounter slots could not be assigned. Otherwise applies data as normal
        /// </summary>
        /// <returns></returns>
        public bool ApplyData()
        {
            for (int i = 0; i < 7; i++)
            {
                //Identify the list to use
                List<EncounterSlot> slotList = new List<EncounterSlot>();
                if (i <= 2) slotList = groupedLandSlots[i];
                else slotList = groupedWaterSlots[i - 3];

                //Make sure the rates add up to 100%
                int sum = 0;
                foreach (EncounterSlot slot in slotList.ToArray())
                {
                    if ((slot.pokemonID == 0 || slot.rate == 0) && slotList.Count > 1) slotList.Remove(slot);
                    else sum += slot.rate;
                }
                if (sum != 100)
                {
                    MessageBox.Show("One of the encounter rate distributions doesn't add up to 100%\nsum = " + sum + "%");
                    return false;
                }

                //Sort groups by rate
                slotList.Sort((e1, e2) => Math.Sign(e2.rate - e1.rate));
                //Setup slots
                EncounterSlot[] slotAssignment = new EncounterSlot[12];
                if (i > 2) slotAssignment = new EncounterSlot[5];
                List<int> encounterRates = new List<int>() { 20, 20, 10, 10, 10, 10, 5, 5, 4, 4, 1, 1 };
                if (i > 2) encounterRates = new List<int>() { 60, 30, 5, 4, 1 };

                //Assign Groups to slots
                foreach (EncounterSlot slot in slotList.ToArray())
                {
                    int remainingRate = slot.rate;
                    for (int n = 0; n < slotAssignment.Length; n++)
                    {
                        if (slotAssignment.Length == 12 && remainingRate == 8 && slotAssignment[8] == null && slotAssignment[9] == null)
                        {
                            slotAssignment[8] = new EncounterSlot(slot.pokemonID, slot.pokemonForm, slot.minLevel, slot.maxLevel, 4);
                            slotAssignment[9] = new EncounterSlot(slot.pokemonID, slot.pokemonForm, slot.minLevel, slot.maxLevel, 4);
                            remainingRate = 0;
                            break;
                        }
                        if (slotAssignment[n] == null && remainingRate >= encounterRates[n])
                        {
                            slotAssignment[n] = new EncounterSlot(slot.pokemonID, slot.pokemonForm, slot.minLevel, slot.maxLevel, encounterRates[n]);
                            remainingRate -= encounterRates[n];
                            if (remainingRate == 0) break;
                        }
                    }
                    if (remainingRate != 0)
                    {
                        MessageBox.Show("Could not find a suitable distribution for the encounter rates");
                        return false;
                    }
                }

                //Set the actual encounter set
                if (i <= 2) landSlots[i] = slotAssignment;
                else waterSlots[i - 3] = slotAssignment;
            }
            EncounterSlotsToGroups();

            int loc = 8;
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < landSlots[x].Length; y++)
                {
                    int id = landSlots[x][y].pokemonID + landSlots[x][y].pokemonForm * 2048;
                    HelperFunctions.WriteShort(bytes, loc, id);
                    bytes[loc + 2] = landSlots[x][y].minLevel;
                    bytes[loc + 3] = landSlots[x][y].maxLevel;
                    loc += 4;
                }
            }

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < waterSlots[x].Length; y++)
                {
                    int id = waterSlots[x][y].pokemonID + waterSlots[x][y].pokemonForm * 2048;
                    HelperFunctions.WriteShort(bytes, loc, id);
                    bytes[loc + 2] = waterSlots[x][y].minLevel;
                    bytes[loc + 3] = waterSlots[x][y].maxLevel;
                    loc += 4;
                }
            }

            return true;
        }

        public void EncounterSlotsToGroups()
        {
            groupedLandSlots = new List<List<EncounterSlot>>();
            groupedLandSlots.Add(new List<EncounterSlot>());
            groupedLandSlots.Add(new List<EncounterSlot>());
            groupedLandSlots.Add(new List<EncounterSlot>());
            for (int j = 0; j < 3; j++) foreach (EncounterSlot e in landSlots[j])
                {
                    bool foundGroup = false;
                    foreach (EncounterSlot e2 in groupedLandSlots[j]) if (e.pokemonID == e2.pokemonID && e.pokemonForm == e2.pokemonForm)
                        {
                            foundGroup = true;
                            e2.rate += e.rate;
                            e2.minLevel = Math.Min(e2.minLevel, e.minLevel);
                            e2.maxLevel = Math.Max(e2.maxLevel, e.maxLevel);
                            break;
                        }
                    if (!foundGroup)
                    {
                        groupedLandSlots[j].Add(new EncounterSlot(e.pokemonID, e.pokemonForm, e.minLevel, e.maxLevel, e.rate));
                    }
                }

            groupedWaterSlots = new List<List<EncounterSlot>>();
            groupedWaterSlots.Add(new List<EncounterSlot>());
            groupedWaterSlots.Add(new List<EncounterSlot>());
            groupedWaterSlots.Add(new List<EncounterSlot>());
            groupedWaterSlots.Add(new List<EncounterSlot>());
            for (int j = 0; j < 4; j++) foreach (EncounterSlot e in waterSlots[j])
                {
                    bool foundGroup = false;
                    foreach (EncounterSlot e2 in groupedWaterSlots[j]) if (e.pokemonID == e2.pokemonID && e.pokemonForm == e2.pokemonForm)
                        {
                            foundGroup = true;
                            e2.rate += e.rate;
                            e2.minLevel = Math.Min(e2.minLevel, e.minLevel);
                            e2.maxLevel = Math.Max(e2.maxLevel, e.maxLevel);
                            break;
                        }
                    if (!foundGroup)
                    {
                        groupedWaterSlots[j].Add(new EncounterSlot(e.pokemonID, e.pokemonForm, e.minLevel, e.maxLevel, e.rate));
                    }
                }
        }

        public override string ToString()
        {
            string str = (MainEditor.RomType == RomType.BW2 && nameID >= 0 && nameID < VersionConstants.BW2_RouteEnounterPoolNames.Count ? VersionConstants.BW2_RouteEnounterPoolNames[nameID] : "Name not found");
            if (season == 0) str += " (Spring)";
            if (season == 1) str += " (Summer)";
            if (season == 2) str += " (Autumn)";
            if (season == 3) str += " (Winter)";
            return str;
        }
    }

    public class EncounterSlot
    {
        public short pokemonID;
        public int pokemonForm;
        public byte minLevel;
        public byte maxLevel;
        public int rate;

        public EncounterSlot(short pokemonID, int pokemonForm, byte minLevel, byte maxLevel, int rate)
        {
            this.pokemonID = pokemonID;
            this.pokemonForm = pokemonForm;
            this.minLevel = minLevel;
            this.maxLevel = maxLevel;
            this.rate = rate;
        }

        public EncounterSlot Clone()
        {
            return new EncounterSlot(pokemonID, pokemonForm, minLevel, maxLevel, rate);
        }

        public override string ToString()
        {
            return (pokemonID < MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[pokemonID] : "Name not found") + " - " + rate + "%";
        }
    }
}
