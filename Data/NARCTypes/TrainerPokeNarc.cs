using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEditor.Forms;

namespace NewEditor.Data.NARCTypes
{
    public class TrainerPokeNARC : NARC
    {
        public List<TrainerPokemonEntry> pokemonGroups;

        public override void ReadData()
        {
            base.ReadData();

            int pos = pointerStartAddress;
            int initialPosition = FileEntryStart;

            //Register data files
            pokemonGroups = new List<TrainerPokemonEntry>();

            //Populate data types
            Dictionary<int, int> formNames = new Dictionary<int, int>();
            for (int i = 0; i < numFileEntries; i++)
            {
                int start = HelperFunctions.ReadInt(byteData, pos);
                int end = HelperFunctions.ReadInt(byteData, pos + 4);
                byte[] bytes = new byte[end - start];

                for (int j = 0; j < end - start; j++) bytes[j] = byteData[initialPosition + start + j];

                TrainerPokemonEntry p = new TrainerPokemonEntry(bytes, fileSystem.trainerNarc.trainers[i]);
                pokemonGroups.Add(p);

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
            foreach (TrainerPokemonEntry p in pokemonGroups)
            {
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
                totalSize += p.bytes.Length;
                newByteData.InsertRange(pPos, BitConverter.GetBytes(totalSize));
                pPos += 4;
            }
            foreach (TrainerPokemonEntry p in pokemonGroups)
            {
                newByteData.AddRange(p.bytes);
            }

            byteData = newByteData.ToArray();

            FixHeaders(pokemonGroups.Count);

            base.WriteData();
        }
    }

    public class TrainerPokemonEntry
    {
        public byte[] bytes;

        public List<TrainerPokemon> pokemon;
        public TrainerEntry parentTrainer;

        public TrainerPokemonEntry(byte[] bytes, TrainerEntry parentTrainer)
        {
            this.bytes = bytes;
            this.parentTrainer = parentTrainer;
            parentTrainer.pokemon = this;

            ReadData();
        }

        internal void ReadData()
        {
            pokemon = new List<TrainerPokemon>();

            for (int i = 0; i < parentTrainer.numPokemon; i++)
            {
                int pos = i * parentTrainer.TrPokeEntryLength;

                TrainerPokemon p = new TrainerPokemon();
                p.IVs = bytes[pos];
                p.gender = (byte)(bytes[pos + 1] & 3);
                p.ability = (byte)(bytes[pos + 1] >> 4);
                p.level = bytes[pos + 2];
                p.nature = bytes[pos + 3];
                p.pokemonID = (short)HelperFunctions.ReadShort(bytes, pos + 4);
                p.form = (short)HelperFunctions.ReadShort(bytes, pos + 6);

                if (parentTrainer.heldItems) p.heldItem = (short)HelperFunctions.ReadShort(bytes, pos + 8);

                p.moves = new short[4];
                if (parentTrainer.uniqueMoves)
                {
                    if (parentTrainer.heldItems) pos += 2;
                    for (int j = 0; j < 4; j++) p.moves[j] = (short)HelperFunctions.ReadShort(bytes, pos + 8 + (j * 2));
                }

                pokemon.Add(p);
            }
        }

        public void ApplyData()
        {
            //Fix pokemon number with parent value
            while (parentTrainer.numPokemon > pokemon.Count) pokemon.Add(new TrainerPokemon() { pokemonID = 1, level = 1, moves = new short[4] });
            while (parentTrainer.numPokemon < pokemon.Count) pokemon.RemoveAt(pokemon.Count - 1);

            List<byte> newBytes = new List<byte>();

            for (int i = 0; i < pokemon.Count; i++)
            {
                TrainerPokemon p = pokemon[i];

                newBytes.Add(p.IVs);
                newBytes.Add((byte)(p.gender + (p.ability << 4)));
                newBytes.Add(p.level);
                newBytes.Add(p.nature);
                newBytes.AddRange(BitConverter.GetBytes(p.pokemonID));
                newBytes.AddRange(BitConverter.GetBytes(p.form));

                if (parentTrainer.heldItems) newBytes.AddRange(BitConverter.GetBytes(p.heldItem));

                if (parentTrainer.uniqueMoves)
                {
                    for (int j = 0; j < 4; j++) newBytes.AddRange(BitConverter.GetBytes(p.moves[j]));
                }
            }

            if (pokemon.Count == 0) newBytes.AddRange(new byte[6]);

            bytes = newBytes.ToArray();
        }
    }

    public class TrainerPokemon
    {
        public byte IVs;
        public byte level;
        public byte nature;
        public short pokemonID;
        public short form;
        public byte ability;
        public byte gender;
        public short heldItem;
        public short[] moves;

        public TrainerPokemon Clone()
        {
            TrainerPokemon p = (TrainerPokemon)MemberwiseClone();
            p.moves = new List<short>(moves).ToArray();
            return p;
        }

        public override string ToString()
        {
            return pokemonID < MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text.Count ? MainEditor.textNarc.textFiles[VersionConstants.PokemonNameTextFileID].text[pokemonID] : "Name not found";
        }
    }
}
