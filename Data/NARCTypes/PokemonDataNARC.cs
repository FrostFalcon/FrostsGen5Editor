﻿using System;
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
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class PokemonDataNARC : NARC
    {
        public List<PokemonEntry> pokemon;

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
            pokemon = new List<PokemonEntry>();

            pos = pointerStartAddress;

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
                    p.spriteID = 685 + pokemon[p.nameID].formSpritesStart + formNum;
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
                    if (poke.spriteID < fileSystem.pokemonSpritesNarc.sprites.Count) poke.sprite = fileSystem.pokemonSpritesNarc.sprites[poke.spriteID].GetFrontSprite();
                }
            });
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
            foreach (PokemonEntry p in pokemon)
            {
                newByteData.AddRange(p.bytes);
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += p.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
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

        public byte evYeildHP;
        public byte evYeildAttack;
        public byte evYeildDefense;
        public byte evYeildSpAtt;
        public byte evYeildSpDef;
        public byte evYeildSpeed;

        public short heldItem1;
        public short heldItem2;
        public short heldItem3;

        public byte unknownAt27;
        public byte[] unknownsFrom36To39;
        public byte[] unknownsAfter52;

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

        public PokemonEntry(byte[] bytes)
        {
            this.bytes = bytes;

            if (MainEditor.RomType == RomType.BW2) ReadDataBW2();
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

            evYeildHP = (byte)(bytes[10] & 0b_11);
            evYeildAttack = (byte)((bytes[10] & 0b_1100) >> 2);
            evYeildDefense = (byte)((bytes[10] & 0b_11_0000) >> 4);
            evYeildSpeed = (byte)((bytes[10] & 0b_1100_0000) >> 6);
            evYeildSpAtt = (byte)(bytes[11] & 0b_11);
            evYeildSpDef = (byte)((bytes[11] & 0b_1100) >> 2);

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

            unknownsFrom36To39 = new byte[4];
            for (int i = 0; i < 4; i++) unknownsFrom36To39[i] = bytes[36 + i];

            TMs = new bool[101];
            for (int i = 0; i < 101; i++)
            {
                int pos = 40 + i / 8;
                int bit = i % 8;
                TMs[i] = (bytes[pos] & (1 << bit)) != 0;
            }

            unknownsAfter52 = new byte[bytes.Length - 53];
            for (int i = 0; i < unknownsAfter52.Length; i++) unknownsAfter52[i] = bytes[53 + i];
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

                bytes[10] = (byte)(evYeildHP + (evYeildAttack << 2) + (evYeildDefense << 4) + (evYeildSpeed << 6));
                bytes[11] = (byte)(evYeildSpAtt + (evYeildSpDef << 2));

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

                for (int i = 0; i < 4; i++) bytes[36 + i] = unknownsFrom36To39[i];

                //Reset TM bytes
                for (int i = 40; i < 53; i++) bytes[i] = 0;
                //Add bits
                for (int i = 0; i < 101; i++)
                {
                    int pos = 40 + i / 8;
                    int bit = i % 8;

                    if (TMs[i]) bytes[pos] += (byte)(1 << bit);
                }

                for (int i = 0; i < unknownsAfter52.Length; i++) bytes[53 + i] = unknownsAfter52[i];

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
            if (MainEditor.pokemonSpritesNarc != null) sprite = MainEditor.pokemonSpritesNarc.sprites[spriteID].GetFrontSprite();
        }
    }
}
