using NewEditor.Data.NARCTypes;
using NewEditor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewEditor.Data
{
    public static class TypeShuffler
    {
        static List<PokemonEntry> mons;
        static Dictionary<byte, byte> swapDictionary;
        static Random rand = new Random();

        static Dictionary<string, List<byte>> dominantColors = new Dictionary<string, List<byte>>()
        {
            { "Bulbasaur", new List<byte>() { 11, 12, 13, 14 } },
            { "Ivysaur", new List<byte>() { 2, 3, 4, 13, 14} },
            { "Venusaur", new List<byte>() { 5, 6, 7, 8, 12} },
            { "Charmander", new List<byte>() { } },
            { "Charmeleon", new List<byte>() { } },
            { "Charizard", new List<byte>() { 10, 11, 12} },
            { "Squirtle", new List<byte>() { } },
            { "Wartortle", new List<byte>() { } },
            { "Blastoise", new List<byte>() { 11, 12, 13, 14, 15} },
            { "Caterpie", new List<byte>() { } },
            { "Metapod", new List<byte>() { } },
            { "Butterfree", new List<byte>() { 8, 9, 10 } },
            { "Weedle", new List<byte>() { 6, 7, 8, 9, 10 } },
            { "Kakuna", new List<byte>() { 2, 3, 4, 5, 6, 7 } },
            { "Beedrill", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Pidgey", new List<byte>() { 11, 12, 13, 14 } },
            { "Pidgeotto", new List<byte>() { 9, 10, 11 } },
            { "Pidgeot", new List<byte>() { 12, 13, 14 } },
            { "Rattata", new List<byte>() { } },
            { "Raticate", new List<byte>() { } },
            { "Spearow", new List<byte>() { 10, 11, 12, 13, 14 } },
            { "Fearow", new List<byte>() { 8, 9, 10, 11 } },
            { "Ekans", new List<byte>() { } },
            { "Arbok", new List<byte>() { 9, 10, 11, 12 } },
            { "Pikachu", new List<byte>() { } },
            { "Raichu", new List<byte>() { } },
            { "Sandshrew", new List<byte>() { } },
            { "Sandslash", new List<byte>() { 11, 12, 13, 14 } },
            { "Nidoran F", new List<byte>() { } },
            { "Nidorina", new List<byte>() { } },
            { "Nidoqueen", new List<byte>() { 10, 11, 12, 13, 14} },
            { "Nidoran M", new List<byte>() { } },
            { "Nidorino", new List<byte>() { } },
            { "Nidoking", new List<byte>() { 7, 11, 12, 13, 14 } },
            { "Clefairy", new List<byte>() { 11, 12, 13, 14 } },
            { "Clefable", new List<byte>() { 4, 5, 6, 7, 8, 9, 10, 11, 14} },
            { "Vulpix", new List<byte>() { } },
            { "Ninetales", new List<byte>() { } },
            { "Jigglypuff", new List<byte>() { } },
            { "Wigglytuff", new List<byte>() { } },
            { "Zubat", new List<byte>() { 6, 7, 8, 9 } },
            { "Golbat", new List<byte>() { 5, 6, 7, 8 } },
            { "Oddish", new List<byte>() { 4, 5, 6, 7 } },
            { "Gloom", new List<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9} },
            { "Vileplume", new List<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } },
            { "Paras", new List<byte>() { 4, 5, 6, 7, 8 } },
            { "Parasect", new List<byte>() { 4, 5, 6, 7, 8, 9 } },
            { "Venonat", new List<byte>() { 4, 5, 6, 7 } },
            { "Venomoth", new List<byte>() { 11, 12, 13, 14, 15} },
            { "Diglett", new List<byte>() { } },
            { "Dugtrio", new List<byte>() { } },
            { "Meowth", new List<byte>() { } },
            { "Persian", new List<byte>() { } },
            { "Psyduck", new List<byte>() { } },
            { "Golduck", new List<byte>() { 3, 4, 5 } },
            { "Mankey", new List<byte>() { } },
            { "Primeape", new List<byte>() { } },
            { "Growlithe", new List<byte>() { } },
            { "Arcanine", new List<byte>() { } },
            { "Poliwag", new List<byte>() { } },
            { "Poliwhirl", new List<byte>() { } },
            { "Poliwrath", new List<byte>() { } },
            { "Abra", new List<byte>() { } },
            { "Kadabra", new List<byte>() { } },
            { "Alakazam", new List<byte>() { } },
            { "Machop", new List<byte>() { } },
            { "Machoke", new List<byte>() { } },
            { "Machamp", new List<byte>() { } },
            { "Bellsprout", new List<byte>() { 9, 10, 11, 12 } },
            { "Weepinbell", new List<byte>() { 12, 13, 14, 15 } },
            { "Victreebel", new List<byte>() { 12, 13, 14, 15 } },
            { "Tentacool", new List<byte>() { 8, 9, 10, 11 } },
            { "Tentacruel", new List<byte>() { 2, 3, 4, 5 } },
            { "Geodude", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Graveler", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Golem", new List<byte>() { 6, 7, 8, 9 } },
            { "Ponyta", new List<byte>() { 8, 9, 10, 11 } },
            { "Rapidash", new List<byte>() { 8, 9, 10, 11 } },
            { "Slowpoke", new List<byte>() { 9, 10, 11, 12, 13 } },
            { "Slowbro", new List<byte>() { 11, 12, 13, 14 } },
            { "Magnemite", new List<byte>() { 7, 8 } },
            { "Magneton", new List<byte>() { 7, 8 } },
            { "Farfetch'd", new List<byte>() { 11, 12, 13, 14 } },
            { "Doduo", new List<byte>() { 5, 6, 7, 8 } },
            { "Dodrio", new List<byte>() { 1, 2, 3, 4 } },
            { "Seel", new List<byte>() { 1, 2, 3 } },
            { "Dewgong", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Grimer", new List<byte>() { 6, 7, 8, 9, 10, 11 } },
            { "Muk", new List<byte>() { 6, 7, 8, 9, 10, 11, 12 } },
            { "Shellder", new List<byte>() { } },
            { "Cloyster", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Gastly", new List<byte>() { 10, 11, 12 } },
            { "Haunter", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Gengar", new List<byte>() { 10, 11, 12, 13, 14} },
            { "Onix", new List<byte>() { 10, 11, 12, 13, 14 } },
            { "Drowzee", new List<byte>() { 2, 3, 4, 5 } },
            { "Hypno", new List<byte>() { 10, 11, 12, 13 } },
            { "Krabby", new List<byte>() { 11, 12, 13, 14 } },
            { "Kingler", new List<byte>() { 1, 2, 3, 4 } },
            { "Voltorb", new List<byte>() { 9, 10, 11, 12, 13, 14} },
            { "Electrode", new List<byte>() { 7, 8, 9, 10, 11, 12, 13, 14 } },
            { "Exeggcute", new List<byte>() { 11, 12, 13, 14, 15 } },
            { "Exeggutor", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Cubone", new List<byte>() { 1, 2, 3, 4 } },
            { "Marowak", new List<byte>() { 12, 13, 14, 15 } },
            { "Hitmonlee", new List<byte>() { } },
            { "Hitmonchan", new List<byte>() { } },
            { "Lickitung", new List<byte>() { 10, 11, 12, 13, 14 } },
            { "Koffing", new List<byte>() { } },
            { "Weezing", new List<byte>() { } },
            { "Rhyhorn", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Rhydon", new List<byte>() { 11, 12, 13, 14 } },
            { "Chansey", new List<byte>() { } },
            { "Tangela", new List<byte>() { 1, 2, 3, 4 } },
            { "Kangaskhan", new List<byte>() { } },
            { "Horsea", new List<byte>() { } },
            { "Seadra", new List<byte>() { } },
            { "Goldeen", new List<byte>() { } },
            { "Seaking", new List<byte>() { 9, 10, 11, 12 } },
            { "Staryu", new List<byte>() { 13, 14, 15 } },
            { "Starmie", new List<byte>() { 12, 13, 14, 15 } },
            { "Mr. Mime", new List<byte>() { 1, 2, 3, 4 } },
            { "Scyther", new List<byte>() { 10, 11, 12, 13, 14 } },
            { "Jynx", new List<byte>() { 2, 3, 4, 5 } },
            { "Electabuzz", new List<byte>() { } },
            { "Magmar", new List<byte>() { } },
            { "Pinsir", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Tauros", new List<byte>() { } },
            { "Magikarp", new List<byte>() { } },
            { "Gyarados", new List<byte>() { 3, 4, 5, 6, 7 } },
            { "Lapras", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Ditto", new List<byte>() { } },
            { "Eevee", new List<byte>() { } },
            { "Vaporeon", new List<byte>() { } },
            { "Jolteon", new List<byte>() { } },
            { "Flareon", new List<byte>() { } },
            { "Porygon", new List<byte>() { } },
            { "Omanyte", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Omastar", new List<byte>() { 2, 3, 4, 5 } },
            { "Kabuto", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Kabutops", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Aerodactyl", new List<byte>() { 2, 3, 4, 5, 6, 7, 8, 9} },
            { "Snorlax", new List<byte>() { 2, 3, 4, 5 } },
            { "Articuno", new List<byte>() { 2, 3, 4, 5, 6, 7, 12 } },
            { "Zapdos", new List<byte>() { 9, 10, 11 } },
            { "Moltres", new List<byte>() { 6, 7, 8, 9 } },
            { "Dratini", new List<byte>() { } },
            { "Dragonair", new List<byte>() { } },
            { "Dragonite", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Mewtwo", new List<byte>() { } },
            { "Mew", new List<byte>() { } },
            { "Chikorita", new List<byte>() { 10, 11, 12, 13 } },
            { "Bayleef", new List<byte>() { 8, 9, 10, 11 } },
            { "Meganium", new List<byte>() { 7, 8, 9, 10 } },
            { "Cyndaquil", new List<byte>() { 10, 11, 12, 13 } },
            { "Quilava", new List<byte>() { 10, 11, 12, 13 } },
            { "Typhlosion", new List<byte>() { 10, 11, 12, 13 } },
            { "Totodile", new List<byte>() { 8, 9, 10, 11, 12 } },
            { "Croconaw", new List<byte>() { 8, 9, 10, 11 } },
            { "Feraligatr", new List<byte>() { 7, 8, 9, 10 } },
            { "Sentret", new List<byte>() { } },
            { "Furret", new List<byte>() { } },
            { "Hoothoot", new List<byte>() { 2, 3, 4, 5, 6, 7, 8 } },
            { "Noctowl", new List<byte>() { 7, 8, 9, 10 } },
            { "Ledyba", new List<byte>() { 2, 3, 4, 5 } },
            { "Ledian", new List<byte>() { 2, 3, 4, 5 } },
            { "Spinarak", new List<byte>() { 7, 8, 9, 10, 11, 12 } },
            { "Ariados", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Crobat", new List<byte>() { 2, 3, 4, 5 } },
            { "Chinchou", new List<byte>() { 1, 2, 3, 4, 5, 6 } },
            { "Lanturn", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Pichu", new List<byte>() { } },
            { "Cleffa", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Igglybuff", new List<byte>() { } },
            { "Togepi", new List<byte>() { } },
            { "Togetic", new List<byte>() { 9, 10, 11, 13, 14, 15 } },
            { "Natu", new List<byte>() { 9, 10, 11 } },
            { "Xatu", new List<byte>() { 9, 10, 11 } },
            { "Mareep", new List<byte>() { } },
            { "Flaaffy", new List<byte>() { } },
            { "Ampharos", new List<byte>() { } },
            { "Bellossom", new List<byte>() { } },
            { "Marill", new List<byte>() { 6, 7, 8, 9, 10, 11, 15 } },
            { "Azumarill", new List<byte>() { 6, 7, 8, 9, 10, 11, 15 } },
            { "Sudowoodo", new List<byte>() { } },
            { "Politoed", new List<byte>() { } },
            { "Hoppip", new List<byte>() { 2, 3, 4, 5 } },
            { "Skiploom", new List<byte>() { 2, 3, 4, 5 } },
            { "Jumpluff", new List<byte>() { 2, 3, 4, 5 } },
            { "Aipom", new List<byte>() { } },
            { "Sunkern", new List<byte>() { } },
            { "Sunflora", new List<byte>() { 3, 4, 5, 6, 7, 8, 9 } },
            { "Yanma", new List<byte>() { 1, 2, 3, 4, 5 } },
            { "Wooper", new List<byte>() { 8, 9, 10, 11 } },
            { "Quagsire", new List<byte>() { 3, 4, 5, 11} },
            { "Espeon", new List<byte>() { } },
            { "Umbreon", new List<byte>() { } },
            { "Murkrow", new List<byte>() { 1, 2, 3 } },
            { "Slowking", new List<byte>() { 2, 3, 4, 5, 6, 7, 13 } },
            { "Misdreavus", new List<byte>() { 9, 10, 11, 12, 13, 14 } },
            { "Unown", new List<byte>() { } },
            { "Wobbuffet", new List<byte>() { } },
            { "Girafarig", new List<byte>() { 3, 4, 5, 6 } },
            { "Pineco", new List<byte>() { } },
            { "Forretress", new List<byte>() { 10, 11, 12, 13 } },
            { "Dunsparce", new List<byte>() { 3, 4, 5, 6 } },
            { "Gligar", new List<byte>() { 3, 4, 5, 7 } },
            { "Steelix", new List<byte>() { 4, 5, 6, 7, 8 } },
            { "Snubbull", new List<byte>() { } },
            { "Granbull", new List<byte>() { } },
            { "Qwilfish", new List<byte>() { 3, 4, 5, 6, 7} },
            { "Scizor", new List<byte>() { 2, 3, 4, 5 } },
            { "Shuckle", new List<byte>() { 11, 12, 13, 14 } },
            { "Heracross", new List<byte>() { 2, 3, 4, 5 } },
            { "Sneasel", new List<byte>() { 7, 8, 9, 10 } },
            { "Teddiursa", new List<byte>() { } },
            { "Ursaring", new List<byte>() { } },
            { "Slugma", new List<byte>() { } },
            { "Magcargo", new List<byte>() { 11, 12, 13, 14 } },
            { "Swinub", new List<byte>() { 3, 4, 5, 6, 7 } },
            { "Piloswine", new List<byte>() { 2, 3, 4, 5, 6, 7 } },
            { "Corsola", new List<byte>() { 1, 2, 3, 4, 5, 6 } },
            { "Remoraid", new List<byte>() { } },
            { "Octillery", new List<byte>() { } },
            { "Delibird", new List<byte>() { 2, 3, 4, 5 } },
            { "Mantine", new List<byte>() { 1, 2, 3, 4 } },
            { "Skarmory", new List<byte>() { 8, 9, 10, 11 } },
            { "Houndour", new List<byte>() { 6, 7, 8, 9 } },
            { "Houndoom", new List<byte>() { 7, 8, 9, 10 } },
            { "Kingdra", new List<byte>() { 2, 3, 4, 5, 6 } },
            { "Phanpy", new List<byte>() { } },
            { "Donphan", new List<byte>() { } },
            { "Porygon2", new List<byte>() { } },
            { "Stantler", new List<byte>() { 11, 12 } },
            { "Smeargle", new List<byte>() { } },
            { "Tyrogue", new List<byte>() { } },
            { "Hitmontop", new List<byte>() { } },
            { "Smoochum", new List<byte>() { 2, 3, 4 } },
            { "Elekid", new List<byte>() { } },
            { "Magby", new List<byte>() { } },
            { "Miltank", new List<byte>() { } },
            { "Blissey", new List<byte>() { } },
            { "Raikou", new List<byte>() { } },
            { "Entei", new List<byte>() { } },
            { "Suicune", new List<byte>() { } },
            { "Larvitar", new List<byte>() { 7, 8, 9, 10 } },
            { "Pupitar", new List<byte>() { 2, 3, 4, 5 } },
            { "Tyranitar", new List<byte>() { 7, 8, 9, 10 } },
            { "Lugia", new List<byte>() { 7, 8, 9, 10, 13, 14, 15} },
            { "Ho-Oh", new List<byte>() { 10, 11, 12 } },
            { "Celebi", new List<byte>() { 5, 6, 7 } },
            { "Rowlet", new List<byte>() { } },
            { "Dartrix", new List<byte>() { } },
            { "Decidueye", new List<byte>() { } },
            { "Torchic", new List<byte>() { } },
            { "Combusken", new List<byte>() { } },
            { "Blaziken", new List<byte>() { } },
            { "Mudkip", new List<byte>() { } },
            { "Marshtomp", new List<byte>() { } },
            { "Swampert", new List<byte>() { } },
            { "Poochyena", new List<byte>() { } },
            { "Mightyena", new List<byte>() { } },
            { "Zigzagoon", new List<byte>() { } },
            { "Linoone", new List<byte>() { } },
            { "Wurmple", new List<byte>() { } },
            { "Silcoon", new List<byte>() { } },
            { "Beautifly", new List<byte>() { } },
            { "Cascoon", new List<byte>() { } },
            { "Dustox", new List<byte>() { } },
            { "Lotad", new List<byte>() { } },
            { "Lombre", new List<byte>() { } },
            { "Ludicolo", new List<byte>() { } },
            { "Seedot", new List<byte>() { } },
            { "Nuzleaf", new List<byte>() { } },
            { "Shiftry", new List<byte>() { } },
            { "Taillow", new List<byte>() { } },
            { "Swellow", new List<byte>() { } },
            { "Wingull", new List<byte>() { } },
            { "Pelipper", new List<byte>() { } },
            { "Ralts", new List<byte>() { } },
            { "Kirlia", new List<byte>() { } },
            { "Gardevoir", new List<byte>() { } },
            { "Surskit", new List<byte>() { } },
            { "Masquerain", new List<byte>() { } },
            { "Shroomish", new List<byte>() { } },
            { "Breloom", new List<byte>() { } },
            { "Slakoth", new List<byte>() { } },
            { "Vigoroth", new List<byte>() { } },
            { "Slaking", new List<byte>() { } },
            { "Nincada", new List<byte>() { } },
            { "Ninjask", new List<byte>() { } },
            { "Shedinja", new List<byte>() { } },
            { "Whismur", new List<byte>() { } },
            { "Loudred", new List<byte>() { } },
            { "Exploud", new List<byte>() { } },
            { "Makuhita", new List<byte>() { } },
            { "Hariyama", new List<byte>() { } },
            { "Azurill", new List<byte>() { } },
            { "Nosepass", new List<byte>() { } },
            { "Skitty", new List<byte>() { } },
            { "Delcatty", new List<byte>() { } },
            { "Sableye", new List<byte>() { } },
            { "Mawile", new List<byte>() { } },
            { "Aron", new List<byte>() { } },
            { "Lairon", new List<byte>() { } },
            { "Aggron", new List<byte>() { } },
            { "Meditite", new List<byte>() { } },
            { "Medicham", new List<byte>() { } },
            { "Electrike", new List<byte>() { } },
            { "Manectric", new List<byte>() { } },
            { "Plusle", new List<byte>() { } },
            { "Minun", new List<byte>() { } },
            { "Volbeat", new List<byte>() { } },
            { "Illumise", new List<byte>() { } },
            { "Roselia", new List<byte>() { } },
            { "Gulpin", new List<byte>() { } },
            { "Swalot", new List<byte>() { } },
            { "Carvanha", new List<byte>() { } },
            { "Sharpedo", new List<byte>() { } },
            { "Wailmer", new List<byte>() { } },
            { "Wailord", new List<byte>() { } },
            { "Numel", new List<byte>() { } },
            { "Camerupt", new List<byte>() { } },
            { "Torkoal", new List<byte>() { } },
            { "Spoink", new List<byte>() { } },
            { "Grumpig", new List<byte>() { } },
            { "Spinda", new List<byte>() { } },
            { "Trapinch", new List<byte>() { } },
            { "Vibrava", new List<byte>() { } },
            { "Flygon", new List<byte>() { } },
            { "Cacnea", new List<byte>() { } },
            { "Cacturne", new List<byte>() { } },
            { "Swablu", new List<byte>() { } },
            { "Altaria", new List<byte>() { } },
            { "Zangoose", new List<byte>() { } },
            { "Seviper", new List<byte>() { } },
            { "Lunatone", new List<byte>() { } },
            { "Solrock", new List<byte>() { } },
            { "Barboach", new List<byte>() { } },
            { "Whiscash", new List<byte>() { } },
            { "Corphish", new List<byte>() { } },
            { "Crawdaunt", new List<byte>() { } },
            { "Baltoy", new List<byte>() { } },
            { "Claydol", new List<byte>() { } },
            { "Lileep", new List<byte>() { } },
            { "Cradily", new List<byte>() { } },
            { "Anorith", new List<byte>() { } },
            { "Armaldo", new List<byte>() { } },
            { "Feebas", new List<byte>() { } },
            { "Milotic", new List<byte>() { } },
            { "Castform", new List<byte>() { } },
            { "Kecleon", new List<byte>() { } },
            { "Shuppet", new List<byte>() { } },
            { "Banette", new List<byte>() { } },
            { "Duskull", new List<byte>() { } },
            { "Dusclops", new List<byte>() { } },
            { "Tropius", new List<byte>() { } },
            { "Chimecho", new List<byte>() { } },
            { "Absol", new List<byte>() { } },
            { "Wynaut", new List<byte>() { } },
            { "Snorunt", new List<byte>() { } },
            { "Glalie", new List<byte>() { } },
            { "Spheal", new List<byte>() { } },
            { "Sealeo", new List<byte>() { } },
            { "Walrein", new List<byte>() { } },
            { "Clamperl", new List<byte>() { } },
            { "Huntail", new List<byte>() { } },
            { "Gorebyss", new List<byte>() { } },
            { "Relicanth", new List<byte>() { } },
            { "Luvdisc", new List<byte>() { } },
            { "Bagon", new List<byte>() { } },
            { "Shelgon", new List<byte>() { } },
            { "Salamence", new List<byte>() { } },
            { "Beldum", new List<byte>() { } },
            { "Metang", new List<byte>() { } },
            { "Metagross", new List<byte>() { } },
            { "Regirock", new List<byte>() { } },
            { "Regice", new List<byte>() { } },
            { "Registeel", new List<byte>() { } },
            { "Latias", new List<byte>() { } },
            { "Latios", new List<byte>() { } },
            { "Kyogre", new List<byte>() { } },
            { "Groudon", new List<byte>() { } },
            { "Rayquaza", new List<byte>() { } },
            { "Jirachi", new List<byte>() { } },
            { "Deoxys", new List<byte>() { } },
            { "Turtwig", new List<byte>() { } },
            { "Grotle", new List<byte>() { } },
            { "Torterra", new List<byte>() { } },
            { "Chimchar", new List<byte>() { } },
            { "Monferno", new List<byte>() { } },
            { "Infernape", new List<byte>() { } },
            { "Piplup", new List<byte>() { } },
            { "Prinplup", new List<byte>() { } },
            { "Empoleon", new List<byte>() { } },
            { "Starly", new List<byte>() { } },
            { "Staravia", new List<byte>() { } },
            { "Staraptor", new List<byte>() { } },
            { "Bidoof", new List<byte>() { } },
            { "Bibarel", new List<byte>() { } },
            { "Kricketot", new List<byte>() { } },
            { "Kricketune", new List<byte>() { } },
            { "Shinx", new List<byte>() { } },
            { "Luxio", new List<byte>() { } },
            { "Luxray", new List<byte>() { } },
            { "Budew", new List<byte>() { } },
            { "Roserade", new List<byte>() { } },
            { "Cranidos", new List<byte>() { } },
            { "Rampardos", new List<byte>() { } },
            { "Shieldon", new List<byte>() { } },
            { "Bastiodon", new List<byte>() { } },
            { "Burmy", new List<byte>() { } },
            { "Wormadam", new List<byte>() { } },
            { "Mothim", new List<byte>() { } },
            { "Combee", new List<byte>() { } },
            { "Vespiquen", new List<byte>() { } },
            { "Pachirisu", new List<byte>() { } },
            { "Buizel", new List<byte>() { } },
            { "Floatzel", new List<byte>() { } },
            { "Cherubi", new List<byte>() { } },
            { "Cherrim", new List<byte>() { } },
            { "Shellos", new List<byte>() { } },
            { "Gastrodon", new List<byte>() { } },
            { "Ambipom", new List<byte>() { } },
            { "Drifloon", new List<byte>() { } },
            { "Drifblim", new List<byte>() { } },
            { "Buneary", new List<byte>() { } },
            { "Lopunny", new List<byte>() { } },
            { "Mismagius", new List<byte>() { } },
            { "Honchkrow", new List<byte>() { } },
            { "Glameow", new List<byte>() { } },
            { "Purugly", new List<byte>() { } },
            { "Chingling", new List<byte>() { } },
            { "Stunky", new List<byte>() { } },
            { "Skuntank", new List<byte>() { } },
            { "Bronzor", new List<byte>() { } },
            { "Bronzong", new List<byte>() { } },
            { "Bonsly", new List<byte>() { } },
            { "Mime Jr.", new List<byte>() { } },
            { "Happiny", new List<byte>() { } },
            { "Chatot", new List<byte>() { } },
            { "Spiritomb", new List<byte>() { } },
            { "Gible", new List<byte>() { } },
            { "Gabite", new List<byte>() { } },
            { "Garchomp", new List<byte>() { } },
            { "Munchlax", new List<byte>() { } },
            { "Riolu", new List<byte>() { } },
            { "Lucario", new List<byte>() { } },
            { "Hippopotas", new List<byte>() { } },
            { "Hippowdon", new List<byte>() { } },
            { "Skorupi", new List<byte>() { } },
            { "Drapion", new List<byte>() { } },
            { "Croagunk", new List<byte>() { } },
            { "Toxicroak", new List<byte>() { } },
            { "Carnivine", new List<byte>() { } },
            { "Finneon", new List<byte>() { } },
            { "Lumineon", new List<byte>() { } },
            { "Mantyke", new List<byte>() { } },
            { "Snover", new List<byte>() { } },
            { "Abomasnow", new List<byte>() { } },
            { "Weavile", new List<byte>() { } },
            { "Magnezone", new List<byte>() { } },
            { "Lickilicky", new List<byte>() { } },
            { "Rhyperior", new List<byte>() { } },
            { "Tangrowth", new List<byte>() { } },
            { "Electivire", new List<byte>() { } },
            { "Magmortar", new List<byte>() { } },
            { "Togekiss", new List<byte>() { } },
            { "Yanmega", new List<byte>() { } },
            { "Leafeon", new List<byte>() { } },
            { "Glaceon", new List<byte>() { } },
            { "Gliscor", new List<byte>() { } },
            { "Mamoswine", new List<byte>() { } },
            { "Porygon-Z", new List<byte>() { } },
            { "Gallade", new List<byte>() { } },
            { "Probopass", new List<byte>() { } },
            { "Dusknoir", new List<byte>() { } },
            { "Froslass", new List<byte>() { } },
            { "Rotom", new List<byte>() { } },
            { "Uxie", new List<byte>() { } },
            { "Mesprit", new List<byte>() { } },
            { "Azelf", new List<byte>() { } },
            { "Dialga", new List<byte>() { } },
            { "Palkia", new List<byte>() { } },
            { "Heatran", new List<byte>() { } },
            { "Regigigas", new List<byte>() { } },
            { "Giratina", new List<byte>() { } },
            { "Cresselia", new List<byte>() { } },
            { "Phione", new List<byte>() { } },
            { "Manaphy", new List<byte>() { } },
            { "Darkrai", new List<byte>() { } },
            { "Shaymin", new List<byte>() { } },
            { "Arceus", new List<byte>() { } },
            { "Victini", new List<byte>() { } },
            { "Snivy", new List<byte>() { } },
            { "Servine", new List<byte>() { } },
            { "Serperior", new List<byte>() { } },
            { "Tepig", new List<byte>() { } },
            { "Pignite", new List<byte>() { } },
            { "Emboar", new List<byte>() { } },
            { "Oshawott", new List<byte>() { } },
            { "Dewott", new List<byte>() { } },
            { "Samurott", new List<byte>() { } },
            { "Patrat", new List<byte>() { } },
            { "Watchog", new List<byte>() { } },
            { "Lillipup", new List<byte>() { } },
            { "Herdier", new List<byte>() { } },
            { "Stoutland", new List<byte>() { } },
            { "Purrloin", new List<byte>() { } },
            { "Liepard", new List<byte>() { } },
            { "Pansage", new List<byte>() { } },
            { "Simisage", new List<byte>() { } },
            { "Pansear", new List<byte>() { } },
            { "Simisear", new List<byte>() { } },
            { "Panpour", new List<byte>() { } },
            { "Simipour", new List<byte>() { } },
            { "Munna", new List<byte>() { } },
            { "Musharna", new List<byte>() { } },
            { "Pidove", new List<byte>() { } },
            { "Tranquill", new List<byte>() { } },
            { "Unfezant", new List<byte>() { } },
            { "Blitzle", new List<byte>() { } },
            { "Zebstrika", new List<byte>() { } },
            { "Roggenrola", new List<byte>() { } },
            { "Boldore", new List<byte>() { } },
            { "Gigalith", new List<byte>() { } },
            { "Woobat", new List<byte>() { } },
            { "Swoobat", new List<byte>() { } },
            { "Drilbur", new List<byte>() { } },
            { "Excadrill", new List<byte>() { } },
            { "Audino", new List<byte>() { } },
            { "Timburr", new List<byte>() { } },
            { "Gurdurr", new List<byte>() { } },
            { "Conkeldurr", new List<byte>() { } },
            { "Tympole", new List<byte>() { } },
            { "Palpitoad", new List<byte>() { } },
            { "Seismitoad", new List<byte>() { } },
            { "Throh", new List<byte>() { } },
            { "Sawk", new List<byte>() { } },
            { "Sewaddle", new List<byte>() { } },
            { "Swadloon", new List<byte>() { } },
            { "Leavanny", new List<byte>() { } },
            { "Venipede", new List<byte>() { } },
            { "Whirlipede", new List<byte>() { } },
            { "Scolipede", new List<byte>() { } },
            { "Cottonee", new List<byte>() { } },
            { "Whimsicott", new List<byte>() { } },
            { "Petilil", new List<byte>() { } },
            { "Lilligant", new List<byte>() { } },
            { "Basculin", new List<byte>() { } },
            { "Sandile", new List<byte>() { } },
            { "Krokorok", new List<byte>() { } },
            { "Krookodile", new List<byte>() { } },
            { "Darumaka", new List<byte>() { } },
            { "Darmanitan", new List<byte>() { } },
            { "Maractus", new List<byte>() { } },
            { "Dwebble", new List<byte>() { } },
            { "Crustle", new List<byte>() { } },
            { "Scraggy", new List<byte>() { } },
            { "Scrafty", new List<byte>() { } },
            { "Sigilyph", new List<byte>() { } },
            { "Yamask", new List<byte>() { } },
            { "Cofagrigus", new List<byte>() { } },
            { "Tirtouga", new List<byte>() { } },
            { "Carracosta", new List<byte>() { } },
            { "Archen", new List<byte>() { } },
            { "Archeops", new List<byte>() { } },
            { "Trubbish", new List<byte>() { } },
            { "Garbodor", new List<byte>() { } },
            { "Zorua", new List<byte>() { } },
            { "Zoroark", new List<byte>() { } },
            { "Minccino", new List<byte>() { } },
            { "Cinccino", new List<byte>() { } },
            { "Gothita", new List<byte>() { } },
            { "Gothorita", new List<byte>() { } },
            { "Gothitelle", new List<byte>() { } },
            { "Solosis", new List<byte>() { } },
            { "Duosion", new List<byte>() { } },
            { "Reuniclus", new List<byte>() { } },
            { "Ducklett", new List<byte>() { } },
            { "Swanna", new List<byte>() { } },
            { "Vanillite", new List<byte>() { } },
            { "Vanillish", new List<byte>() { } },
            { "Vanilluxe", new List<byte>() { } },
            { "Deerling", new List<byte>() { } },
            { "Sawsbuck", new List<byte>() { } },
            { "Emolga", new List<byte>() { } },
            { "Karrablast", new List<byte>() { } },
            { "Escavalier", new List<byte>() { } },
            { "Foongus", new List<byte>() { } },
            { "Amoonguss", new List<byte>() { } },
            { "Frillish", new List<byte>() { } },
            { "Jellicent", new List<byte>() { } },
            { "Alomomola", new List<byte>() { } },
            { "Joltik", new List<byte>() { } },
            { "Galvantula", new List<byte>() { } },
            { "Ferroseed", new List<byte>() { } },
            { "Ferrothorn", new List<byte>() { } },
            { "Klink", new List<byte>() { } },
            { "Klang", new List<byte>() { } },
            { "Klinklang", new List<byte>() { } },
            { "Tynamo", new List<byte>() { } },
            { "Eelektrik", new List<byte>() { } },
            { "Eelektross", new List<byte>() { } },
            { "Elgyem", new List<byte>() { } },
            { "Beheeyem", new List<byte>() { } },
            { "Litwick", new List<byte>() { } },
            { "Lampent", new List<byte>() { } },
            { "Chandelure", new List<byte>() { } },
            { "Axew", new List<byte>() { } },
            { "Fraxure", new List<byte>() { } },
            { "Haxorus", new List<byte>() { } },
            { "Cubchoo", new List<byte>() { } },
            { "Beartic", new List<byte>() { } },
            { "Cryogonal", new List<byte>() { } },
            { "Shelmet", new List<byte>() { } },
            { "Accelgor", new List<byte>() { } },
            { "Stunfisk", new List<byte>() { } },
            { "Mienfoo", new List<byte>() { } },
            { "Mienshao", new List<byte>() { } },
            { "Druddigon", new List<byte>() { } },
            { "Golett", new List<byte>() { } },
            { "Golurk", new List<byte>() { } },
            { "Pawniard", new List<byte>() { } },
            { "Bisharp", new List<byte>() { } },
            { "Bouffalant", new List<byte>() { } },
            { "Rufflet", new List<byte>() { } },
            { "Braviary", new List<byte>() { } },
            { "Vullaby", new List<byte>() { } },
            { "Mandibuzz", new List<byte>() { } },
            { "Heatmor", new List<byte>() { } },
            { "Durant", new List<byte>() { } },
            { "Deino", new List<byte>() { } },
            { "Zweilous", new List<byte>() { } },
            { "Hydreigon", new List<byte>() { } },
            { "Larvesta", new List<byte>() { } },
            { "Volcarona", new List<byte>() { } },
            { "Cobalion", new List<byte>() { } },
            { "Terrakion", new List<byte>() { } },
            { "Virizion", new List<byte>() { } },
            { "Tornadus", new List<byte>() { } },
            { "Thundurus", new List<byte>() { } },
            { "Reshiram", new List<byte>() { } },
            { "Zekrom", new List<byte>() { } },
            { "Landorus", new List<byte>() { } },
            { "Kyurem", new List<byte>() { } },
            { "Keldeo", new List<byte>() { } },
            { "Meloetta", new List<byte>() { } },
            { "Genesect", new List<byte>() { } },
            { "Deoxys (1)", new List<byte>() { } },
            { "Deoxys (2)", new List<byte>() { } },
            { "Deoxys (3)", new List<byte>() { } },
            { "Wormadam (1)", new List<byte>() { } },
            { "Wormadam (2)", new List<byte>() { } },
            { "Shaymin (1)", new List<byte>() { } },
            { "Giratina (1)", new List<byte>() { } },
            { "Rotom (1)", new List<byte>() { } },
            { "Rotom (2)", new List<byte>() { } },
            { "Rotom (3)", new List<byte>() { } },
            { "Rotom (4)", new List<byte>() { } },
            { "Rotom (5)", new List<byte>() { } },
            { "Castform (1)", new List<byte>() { } },
            { "Castform (2)", new List<byte>() { } },
            { "Castform (3)", new List<byte>() { } },
            { "Basculin (1)", new List<byte>() { } },
            { "Darmanitan (1)", new List<byte>() { } },
            { "Meloetta (1)", new List<byte>() { } },
            { "Kyurem (1)", new List<byte>() { } },
            { "Kyurem (2)", new List<byte>() { } },
            { "Keldeo (1)", new List<byte>() { } },
            { "Tornadus (1)", new List<byte>() { } },
            { "Thundurus (1)", new List<byte>() { } },
            { "Landorus (1)", new List<byte>() { } },
        };

        static Dictionary<int, (int, float, float)> colorDictionary = new Dictionary<int, (int, float, float)>()
        {
            { 0, (-1, 0.5f, 1) },
            { 1, (0, 1, 1) },
            { 2, (230, 0.3f, 2) },
            { 3, (280, 1, 1) },
            { 4, (40, 0.5f, 1) },
            { 5, (40, 0.5f, 0.5f) },
            { 6, (80, 1, 1) },
            { 7, (260, 0.5f, 0.75f) },
            { 8, (-1, 0.1f, 2) },
            { 9, (20, 2, 1) },
            { 10, (200, 1, 1) },
            { 11, (120, 1, 1) },
            { 12, (50, 1, 1) },
            { 13, (300, 1, 1) },
            { 14, (180, 0.5f, 1) },
            { 15, (260, 1, 2) },
            { 16, (-1, 0, 0.5f) },
            { 17, (300, 0.5f, 2) },
        };

        public static void Shuffle()
        {
            mons = new List<PokemonEntry>(MainEditor.pokemonDataNarc.pokemon);
            mons.RemoveAt(0);
            mons.Sort((p1, p2) => p1.baseStatTotal - p2.baseStatTotal);

            while (mons.Count > 0)
            {
                swapDictionary = MakeSwapDictionary();
                ApplyPokemon(mons[0]);
            }
        }

        static void ApplyPokemon(PokemonEntry pk)
        {
            mons.Remove(pk);
            if (pk.type2 != pk.type1)
            {
                int old1 = pk.type1;
                int old2 = pk.type2;

                bool type1 = false;
                if (pk.type1 == 0)
                {
                    pk.type1 = swapDictionary[0];
                    type1 = true;
                }
                else pk.type2 = swapDictionary[pk.type2];
                if (old1 != pk.type1 || old2 != pk.type2) ColorSwap(pk, MainEditor.pokemonSpritesNarc.sprites[pk.spriteID], type1 ? pk.type1 : pk.type2);
            }
            pk.ApplyData();

            if (pk.evolutions == null) return;

            for (int i = 0; i < pk.evolutions.methods.Length; i++)
            {
                if (pk.evolutions.methods[i].newPokemonID != 0)
                {
                    ApplyPokemon(MainEditor.pokemonDataNarc.pokemon[pk.evolutions.methods[i].newPokemonID]);
                }
            }
        }

        static void ColorSwap(PokemonEntry pk, PokemonSpriteEntry sprite, int type)
        {
            //Find dominant color
            int num = MainEditor.pokemonSpritesNarc.sprites.IndexOf(sprite);
            if (!dominantColors.ContainsKey(pk.Name)) return;

            HSV[] palette = new HSV[16];
            for (int i = 0; i < 16; i++) palette[i] = ColorToHSV(sprite.palette[i]);

            //Change colors

            for (int i = 1; i < 16; i++)
            {
                if (dominantColors[pk.Name].Contains((byte)i))
                {
                    if (colorDictionary[type].Item1 != -1) palette[i].hue = colorDictionary[type].Item1;
                    palette[i].saturation = Math.Min(palette[i].saturation * colorDictionary[type].Item2, 1);
                    palette[i].value *= Math.Min(palette[i].value * colorDictionary[type].Item3, 1);

                    sprite.palette[i] = ColorFromHSV(palette[i]);
                }
            }

            sprite.ApplyData();
        }

        static Dictionary<byte, byte> MakeSwapDictionary()
        {
            Dictionary<byte, byte> dict = new Dictionary<byte, byte>();

            int max = 17;
            if (MainEditor.textNarc.textFiles[VersionConstants.TypeNameTextFileID].text.Contains("Fairy")) max++;

            for (int i = 0; i < max; i++)
            {
                int pick = 0;
                do
                {
                    pick = rand.Next(max);
                } while (pick == i);

                dict.Add((byte)i, (byte)pick);
            }

            return dict;
        }

        static bool HuesNearby(HSV hsv1, HSV hsv2, int threshold)
        {
            double h1 = hsv1.hue;
            double h2 = hsv2.hue;
            if (h2 >= 270 && h1 <= 90) h2 -= 360;
            if (h1 >= 270 && h2 <= 90) h1 -= 360;
            return Math.Abs(h2 - h1) <= threshold;
        }

        static HSV ColorToHSV(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            return new HSV(color.GetHue(), (max == 0) ? 0 : 1d - (1d * min / max), max / 255d);
        }

        class HSV
        {
            public double hue;
            public double saturation;
            public double value;

            public HSV(double hue, double saturation, double value)
            {
                this.hue = hue;
                this.saturation = saturation;
                this.value = value;
            }
        }

        static Color ColorFromHSV(HSV hsv)
        {
            int hi = Convert.ToInt32(Math.Floor(hsv.hue / 60)) % 6;
            double f = hsv.hue / 60 - Math.Floor(hsv.hue / 60);

            hsv.value = hsv.value * 255;
            int v = Convert.ToInt32(hsv.value);
            int p = Convert.ToInt32(hsv.value * (1 - hsv.saturation));
            int q = Convert.ToInt32(hsv.value * (1 - f * hsv.saturation));
            int t = Convert.ToInt32(hsv.value * (1 - (1 - f) * hsv.saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }
    }
}
