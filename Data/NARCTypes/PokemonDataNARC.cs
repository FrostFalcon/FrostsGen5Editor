using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class PokemonDataNARC : NARC
    {
        public List<PokemonEntry> pokemon;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            pokemon = new List<PokemonEntry>();

            //Populate data types
            int nameID = 0;
            Dictionary<int, int> formNames = new Dictionary<int, int>();
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                PokemonEntry p = new PokemonEntry(bytes) { nameID = nameID, spriteID = i };
                pokemon.Add(p);

                //Assign name and handle alt forms
                if (formNames.ContainsKey(i))
                {
                    p.nameID = formNames[i];
                    p.spriteID = p.nameID;
                }
                if (p.numberOfForms > 1 && p.formsStart != 0)
                {
                    for (int j = 0; j < p.numberOfForms - 1; j++) formNames.Add(p.formsStart + j, nameID);
                }
                if (p.numberOfForms > 1 && p.formSpritesStart == 0 && p.nameID != 201 && p.nameID != 493)
                {
                    int formNum = i - pokemon[p.nameID].formsStart;
                    p.formID = formNum + 1;
                    p.spriteID = (MainEditor.RomType == RomType.BW2 ? 685 : 652) + pokemon[p.nameID].formSpritesStart + formNum;
                }
                nameID++;

                pos += 8;
            }

            if (MainEditor.RomType == RomType.BW2) GetSprites();
        }

        async void GetSprites()
        {
            await Task.Run(() =>
            {
                while (fileSystem.pokemonSpritesNarc == null) Thread.Sleep(10);

                foreach (PokemonEntry poke in pokemon.ToArray())
                {
                    while (fileSystem.pokemonSpritesNarc == null || fileSystem.pokemonSpritesNarc.sprites == null) Thread.Sleep(10);
                    if (poke.spriteID < fileSystem.pokemonSpritesNarc.sprites.Count)
                    {
                        try
                        {
                            poke.sprite = fileSystem.pokemonSpritesNarc.sprites[poke.spriteID].GetSprite();
                        }
                        catch
                        {

                        }
                    }
                }
            });
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
            foreach (PokemonEntry p in pokemon)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += p.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (PokemonEntry p in pokemon)
            {
                newByteData.AddRange(p.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(pokemon.Count);

            base.WriteData();
        }
    }

    public class PokemonEntry
    {
        public byte[] bytes;
        public int nameID;
        public int spriteID;
        public int formID;
        public Bitmap sprite;

        public LevelUpMoveset levelUpMoves;
        public EvolutionDataEntry evolutions;

        public byte baseHP;
        public byte baseAttack;
        public byte baseDefense;
        public byte baseSpAtt;
        public byte baseSpDef;
        public byte baseSpeed;
        public int baseStatTotal => baseHP + baseAttack + baseDefense + baseSpAtt + baseSpDef + baseSpeed;

        public byte type1;
        public byte type2;
        
        public byte catchRate;
        public byte evolutionStage;

        public byte evYieldHP;
        public byte evYieldAttack;
        public byte evYieldDefense;
        public byte evYieldSpAtt;
        public byte evYieldSpDef;
        public byte evYieldSpeed;

        public short heldItem1;
        public short heldItem2;
        public short heldItem3;

        public byte unknownAt27;
        public short height;
        public short weight;

        public byte genderRatio;
        public byte hatchCounter;
        public byte baseHappiness;
        public byte levelRate;
        public short xpYield;

        public byte eggGroup1;
        public byte eggGroup2;

        public byte ability1;
        public byte ability2;
        public byte ability3;

        public byte numberOfForms;
        public short formsStart;
        public short formSpritesStart;
        public byte pokedexColor;

        public bool[] TMs;
        public bool[] miscTutors;
        public bool[] driftveilTutors;
        public bool[] lentimasTutors;
        public bool[] humilauTutors;
        public bool[] nacreneTutors;

        public PokemonEntry(byte[] bytes)
        {
            this.bytes = bytes;

            if (MainEditor.RomType == RomType.BW2) ReadDataBW2();
            else if (MainEditor.RomType == RomType.BW1) ReadDataBW1();
            else ReadDataHGSS();
        }

        internal void ReadDataBW2()
        {
            baseHP = bytes[0];
            baseAttack = bytes[1];
            baseDefense = bytes[2];
            baseSpAtt = bytes[4];
            baseSpDef = bytes[5];
            baseSpeed = bytes[3];

            type1 = bytes[6];
            type2 = bytes[7];

            catchRate = bytes[8];
            evolutionStage = bytes[9];

            evYieldHP = (byte)(bytes[10] & 0b_11);
            evYieldAttack = (byte)((bytes[10] & 0b_1100) >> 2);
            evYieldDefense = (byte)((bytes[10] & 0b_11_0000) >> 4);
            evYieldSpeed = (byte)((bytes[10] & 0b_1100_0000) >> 6);
            evYieldSpAtt = (byte)(bytes[11] & 0b_11);
            evYieldSpDef = (byte)((bytes[11] & 0b_1100) >> 2);

            heldItem1 = (short)HelperFunctions.ReadShort(bytes, 12);
            heldItem2 = (short)HelperFunctions.ReadShort(bytes, 14);
            heldItem3 = (short)HelperFunctions.ReadShort(bytes, 16);

            genderRatio = bytes[18];

            hatchCounter = bytes[19];
            baseHappiness = bytes[20];
            levelRate = bytes[21];
            eggGroup1 = bytes[22];
            eggGroup2 = bytes[23];

            ability1 = bytes[24];
            ability2 = bytes[25];
            ability3 = bytes[26];

            unknownAt27 = bytes[27];

            formsStart = (short)HelperFunctions.ReadShort(bytes, 28);
            formSpritesStart = (short)HelperFunctions.ReadShort(bytes, 30);
            numberOfForms = bytes[32];
            pokedexColor = bytes[33];
            xpYield = (short)HelperFunctions.ReadShort(bytes, 34);

            height = (short)HelperFunctions.ReadShort(bytes, 36);
            weight = (short)HelperFunctions.ReadShort(bytes, 38);

            TMs = new bool[101];
            for (int i = 0; i < 101; i++)
            {
                int pos = 40 + i / 8;
                int bit = i % 8;
                TMs[i] = (bytes[pos] & (1 << bit)) != 0;
            }

            miscTutors = new bool[7];
            for (int i = 0; i < 7; i++)
            {
                int pos = 56 + i / 8;
                int bit = i % 8;
                miscTutors[i] = (bytes[pos] & (1 << bit)) != 0;
            }
            driftveilTutors = new bool[15];
            for (int i = 0; i < 15; i++)
            {
                int pos = 60 + i / 8;
                int bit = i % 8;
                driftveilTutors[i] = (bytes[pos] & (1 << bit)) != 0;
            }
            lentimasTutors = new bool[17];
            for (int i = 0; i < 17; i++)
            {
                int pos = 64 + i / 8;
                int bit = i % 8;
                lentimasTutors[i] = (bytes[pos] & (1 << bit)) != 0;
            }
            humilauTutors = new bool[13];
            for (int i = 0; i < 13; i++)
            {
                int pos = 68 + i / 8;
                int bit = i % 8;
                humilauTutors[i] = (bytes[pos] & (1 << bit)) != 0;
            }
            nacreneTutors = new bool[15];
            for (int i = 0; i < 15; i++)
            {
                int pos = 72 + i / 8;
                int bit = i % 8;
                nacreneTutors[i] = (bytes[pos] & (1 << bit)) != 0;
            }
        }

        internal void ReadDataBW1()
        {
            baseHP = bytes[0];
            baseAttack = bytes[1];
            baseDefense = bytes[2];
            baseSpAtt = bytes[4];
            baseSpDef = bytes[5];
            baseSpeed = bytes[3];

            type1 = bytes[6];
            type2 = bytes[7];

            catchRate = bytes[8];
            evolutionStage = bytes[9];

            evYieldHP = (byte)(bytes[10] & 0b_11);
            evYieldAttack = (byte)((bytes[10] & 0b_1100) >> 2);
            evYieldDefense = (byte)((bytes[10] & 0b_11_0000) >> 4);
            evYieldSpeed = (byte)((bytes[10] & 0b_1100_0000) >> 6);
            evYieldSpAtt = (byte)(bytes[11] & 0b_11);
            evYieldSpDef = (byte)((bytes[11] & 0b_1100) >> 2);

            heldItem1 = (short)HelperFunctions.ReadShort(bytes, 12);
            heldItem2 = (short)HelperFunctions.ReadShort(bytes, 14);
            heldItem3 = (short)HelperFunctions.ReadShort(bytes, 16);

            genderRatio = bytes[18];

            hatchCounter = bytes[19];
            baseHappiness = bytes[20];
            levelRate = bytes[21];
            eggGroup1 = bytes[22];
            eggGroup2 = bytes[23];

            ability1 = bytes[24];
            ability2 = bytes[25];
            ability3 = bytes[26];

            unknownAt27 = bytes[27];

            formsStart = (short)HelperFunctions.ReadShort(bytes, 28);
            formSpritesStart = (short)HelperFunctions.ReadShort(bytes, 30);
            numberOfForms = bytes[32];
            pokedexColor = bytes[33];
            xpYield = (short)HelperFunctions.ReadShort(bytes, 34);

            height = (short)HelperFunctions.ReadShort(bytes, 36);
            weight = (short)HelperFunctions.ReadShort(bytes, 38);

            TMs = new bool[101];
            for (int i = 0; i < 101; i++)
            {
                int pos = 40 + i / 8;
                int bit = i % 8;
                TMs[i] = (bytes[pos] & (1 << bit)) != 0;
            }

            if (bytes.Length == 60)
            {
                miscTutors = new bool[7];
                for (int i = 0; i < 7; i++)
                {
                    int bit = i % 8;
                    miscTutors[i] = (bytes[56] & (1 << bit)) != 0;
                }
            }
        }

        internal void ReadDataHGSS()
        {
            baseHP = bytes[0];
            baseAttack = bytes[1];
            baseDefense = bytes[2];
            baseSpAtt = bytes[4];
            baseSpDef = bytes[5];
            baseSpeed = bytes[3];

            type1 = bytes[6];
            type2 = bytes[7];
        }

        public void ApplyData()
        {
            //Don't handle the last file
            if (bytes.Length > 200) return;

            if (MainEditor.RomType == RomType.BW2)
            {
                bytes[0] = baseHP;
                bytes[1] = baseAttack;
                bytes[2] = baseDefense;
                bytes[4] = baseSpAtt;
                bytes[5] = baseSpDef;
                bytes[3] = baseSpeed;

                bytes[6] = type1;
                bytes[7] = type2;

                bytes[8] = catchRate;
                bytes[9] = evolutionStage;

                bytes[10] = (byte)(evYieldHP + (evYieldAttack << 2) + (evYieldDefense << 4) + (evYieldSpeed << 6));
                bytes[11] = (byte)(evYieldSpAtt + (evYieldSpDef << 2));

                HelperFunctions.WriteShort(bytes, 12, heldItem1);
                HelperFunctions.WriteShort(bytes, 14, heldItem2);
                HelperFunctions.WriteShort(bytes, 16, heldItem3);

                bytes[18] = genderRatio;

                bytes[19] = hatchCounter;
                bytes[20] = baseHappiness;
                bytes[21] = levelRate;
                bytes[22] = eggGroup1;
                bytes[23] = eggGroup2;

                bytes[24] = ability1;
                bytes[25] = ability2;
                bytes[26] = ability3;

                bytes[27] = unknownAt27;

                HelperFunctions.WriteShort(bytes, 28, formsStart);
                HelperFunctions.WriteShort(bytes, 30, formSpritesStart);
                bytes[32] = numberOfForms;
                bytes[33] = pokedexColor;
                HelperFunctions.WriteShort(bytes, 34, xpYield);

                HelperFunctions.WriteShort(bytes, 36, height);
                HelperFunctions.WriteShort(bytes, 38, weight);

                //Reset TM bytes
                for (int i = 40; i < 53; i++) bytes[i] = 0;
                //Add bits
                for (int i = 0; i < 101; i++)
                {
                    int pos = 40 + i / 8;
                    int bit = i % 8;

                    if (TMs[i]) bytes[pos] += (byte)(1 << bit);
                }

                //Tutors
                for (int i = 56; i < 76; i++) bytes[i] = 0;
                for (int i = 0; i < miscTutors.Length; i++)
                {
                    int pos = 56 + i / 8;
                    int bit = i % 8;
                    if (miscTutors[i]) bytes[pos] += (byte)(1 << bit);
                }
                for (int i = 0; i < driftveilTutors.Length; i++)
                {
                    int pos = 60 + i / 8;
                    int bit = i % 8;
                    if (driftveilTutors[i]) bytes[pos] += (byte)(1 << bit);
                }
                for (int i = 0; i < lentimasTutors.Length; i++)
                {
                    int pos = 64 + i / 8;
                    int bit = i % 8;
                    if (lentimasTutors[i]) bytes[pos] += (byte)(1 << bit);
                }

                for (int i = 0; i < humilauTutors.Length; i++)
                {
                    int pos = 68 + i / 8;
                    int bit = i % 8;
                    if (humilauTutors[i]) bytes[pos] += (byte)(1 << bit);
                }

                for (int i = 0; i < nacreneTutors.Length; i++)
                {
                    int pos = 72 + i / 8;
                    int bit = i % 8;
                    if (nacreneTutors[i]) bytes[pos] += (byte)(1 << bit);
                }

                if (levelUpMoves != null) levelUpMoves.ApplyData();
                if (evolutions != null) evolutions.ApplyData();
            }
            else if (MainEditor.RomType == RomType.BW1)
            {
                bytes[0] = baseHP;
                bytes[1] = baseAttack;
                bytes[2] = baseDefense;
                bytes[4] = baseSpAtt;
                bytes[5] = baseSpDef;
                bytes[3] = baseSpeed;

                bytes[6] = type1;
                bytes[7] = type2;

                bytes[8] = catchRate;
                bytes[9] = evolutionStage;

                bytes[10] = (byte)(evYieldHP + (evYieldAttack << 2) + (evYieldDefense << 4) + (evYieldSpeed << 6));
                bytes[11] = (byte)(evYieldSpAtt + (evYieldSpDef << 2));

                HelperFunctions.WriteShort(bytes, 12, heldItem1);
                HelperFunctions.WriteShort(bytes, 14, heldItem2);
                HelperFunctions.WriteShort(bytes, 16, heldItem3);

                bytes[18] = genderRatio;

                bytes[19] = hatchCounter;
                bytes[20] = baseHappiness;
                bytes[21] = levelRate;
                bytes[22] = eggGroup1;
                bytes[23] = eggGroup2;

                bytes[24] = ability1;
                bytes[25] = ability2;
                bytes[26] = ability3;

                bytes[27] = unknownAt27;

                HelperFunctions.WriteShort(bytes, 28, formsStart);
                HelperFunctions.WriteShort(bytes, 30, formSpritesStart);
                bytes[32] = numberOfForms;
                bytes[33] = pokedexColor;
                HelperFunctions.WriteShort(bytes, 34, xpYield);

                HelperFunctions.WriteShort(bytes, 36, height);
                HelperFunctions.WriteShort(bytes, 38, weight);

                //Reset TM bits
                for (int i = 40; i < 53; i++) bytes[i] = 0;
                //Add bits
                for (int i = 0; i < 101; i++)
                {
                    int pos = 40 + i / 8;
                    int bit = i % 8;

                    if (TMs[i]) bytes[pos] += (byte)(1 << bit);
                }

                if (bytes.Length == 60)
                {
                    //Tutors
                    for (int i = 56; i < 60; i++) bytes[i] = 0;
                    for (int i = 0; i < miscTutors.Length; i++)
                    {
                        int pos = 56 + i / 8;
                        int bit = i % 8;
                        if (miscTutors[i]) bytes[pos] += (byte)(1 << bit);
                    }
                }

                if (levelUpMoves != null) levelUpMoves.ApplyData();
                if (evolutions != null) evolutions.ApplyData();
            }
            else
            {
                bytes[0] = baseHP;
                bytes[1] = baseAttack;
                bytes[2] = baseDefense;
                bytes[4] = baseSpAtt;
                bytes[5] = baseSpDef;
                bytes[3] = baseSpeed;

                bytes[6] = type1;
                bytes[7] = type2;
            }
        }

        public string Name
        {
            get
            {
                string str = nameID < MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[nameID] + (formID != 0 ? $" ({formID})" : "") : "Name not found";
                str = str.Replace("⑮", " F").Replace("⑭", " M");
                return str;
            }
        }

        public override string ToString()
        {
            return Name + " - " + nameID;
        }

        public void RetrieveSprite()
        {
            if (MainEditor.pokemonSpritesNarc != null) sprite = MainEditor.pokemonSpritesNarc.sprites[spriteID].GetSprite();
        }
    }
}
