using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class HabitatListNARC : NARC
    {
        public List<HabitatListEntry> lists;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            lists = new List<HabitatListEntry>();

            //Populate data types
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                HabitatListEntry m = new HabitatListEntry(bytes) { nameID = i };
                lists.Add(m);

                pos += 8;
            }
        }

        public override void WriteData()
        {
            if (fileSystem.encounterNarc != null)
            {
                for (int i = 0; i < lists.Count; i++)
                {
                    bool seasons = false;
                    lists[i].pokemon = new List<HabitatListPokemon>();
                    Dictionary<short, byte[]> map = new Dictionary<short, byte[]>();
                    foreach (int pool in VersionConstants.habitatListEntries[i])
                    {
                        for (int s = 0; s < 4; s++)
                        {
                            EncounterEntry ee = (s == 0 ? fileSystem.encounterNarc.mainEncounterPools[pool] : fileSystem.encounterNarc.subEncounterPools.FirstOrDefault(p => p.parentPool == fileSystem.encounterNarc.mainEncounterPools[pool] && p.season == s)) ?? fileSystem.encounterNarc.mainEncounterPools[pool];
                            foreach (List<EncounterSlot> slots in ee.groupedLandSlots)
                            {
                                foreach (EncounterSlot slot in slots)
                                {
                                    if (!map.ContainsKey(slot.pokemonID)) map.Add(slot.pokemonID, new byte[26]);
                                    map[slot.pokemonID][s * 3] = 1;
                                }
                            }
                            int fish = 0;
                            foreach (List<EncounterSlot> slots in ee.groupedWaterSlots)
                            {
                                foreach (EncounterSlot slot in slots)
                                {
                                    if (!map.ContainsKey(slot.pokemonID)) map.Add(slot.pokemonID, new byte[26]);
                                    map[slot.pokemonID][s * 3 + (fish >= 2 ? 2 : 1)] = 1;
                                }
                                fish++;
                            }
                            if (!fileSystem.encounterNarc.mainEncounterPools.Contains(ee)) seasons = true;
                        }
                    }
                    while (map.Count > 30) map.Remove(map.Keys.ToList().Last());
                    map.OrderBy(k => k.Key);
                    foreach (var kv in map)
                    {
                        lists[i].pokemon.Add(new HabitatListPokemon() { species = kv.Key, flags = kv.Value });
                    }
                    if (seasons)
                    {
                        lists[i].bytes[0] = 1;
                        lists[i].bytes[1] = 1;
                        lists[i].bytes[2] = 1;
                    }
                    else
                    {
                        lists[i].bytes[0] = 0;
                        lists[i].bytes[1] = 0;
                        lists[i].bytes[2] = 0;
                    }
                    lists[i].ApplyData();
                }
            }

            List<byte> newByteData = new List<byte>();
            List<byte> oldByteData = new List<byte>(byteData);

            newByteData.AddRange(oldByteData.GetRange(0, pointerStartAddress));
            newByteData.AddRange(oldByteData.GetRange(BTNFPosition, FileEntryStart - BTNFPosition));

            //Write Files
            int totalSize = 0;
            int pPos = pointerStartAddress;
            foreach (HabitatListEntry i in lists)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += i.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (HabitatListEntry i in lists)
            {
                newByteData.AddRange(i.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(lists.Count);

            base.WriteData();
        }
    }

    public class HabitatListEntry
    {
        public byte[] bytes;
        public int nameID;

        public List<HabitatListPokemon> pokemon;

        public HabitatListEntry(byte[] bytes)
        {
            this.bytes = bytes;
            ReadData();
        }

        public void ReadData()
        {
            int count = HelperFunctions.ReadShort(bytes, 8);
            pokemon = new List<HabitatListPokemon>();
            for (int i = 0; i < count; i++)
            {
                int pos = 10 + 28 * i;
                byte[] flags = new byte[26];
                for (int j = 0; j < 26; j++) flags[j] = bytes[pos + 2 + j];
                pokemon.Add(new HabitatListPokemon() { species = (short)HelperFunctions.ReadShort(bytes, pos), flags = flags });
            }
        }

        public void ApplyData()
        {
            HelperFunctions.WriteShort(bytes, 8, pokemon.Count);
            List<byte> newBytes = new List<byte>();
            for (int i = 0; i < 10; i++) newBytes.Add(bytes[i]);

            foreach (HabitatListPokemon poke in pokemon)
            {
                newBytes.AddRange(BitConverter.GetBytes(poke.species));
                newBytes.AddRange(poke.flags);
            }
            bytes = newBytes.ToArray();
        }

        public override string ToString()
        {
            return nameID.ToString();
        }
    }

    public class HabitatListPokemon
    {
        public short species;
        public byte[] flags;

        public HabitatListPokemon() { }

        public HabitatListPokemon(int species)
        {
            this.species = (short)species;
            this.flags = new byte[26];
        }

        public void SetFlag(bool value, int season, int type)
        {
            flags[season * 3 + type] = (byte)(value ? 1 : 0);
        }
    }
}
