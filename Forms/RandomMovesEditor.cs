using NewEditor.Data;
using NewEditor.Data.NARCTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Forms
{
    public partial class RandomMovesEditor : Form
    {
        public RandomMovesEditor()
        {
            InitializeComponent();
        }

        private void ApplyRandomMoves(object sender, EventArgs e)
        {
            Random random = rngSeedNumberBox.Value != 0 ? new Random((int)rngSeedNumberBox.Value) : new Random();

            List<MoveDataEntry> moveData = MainEditor.moveDataNarc.moves;

            foreach (PokemonEntry pk in MainEditor.pokemonDataNarc.pokemon)
            {
                LevelUpMoveset l = pk.levelUpMoves;

                if (l == null || l.moves == null) continue;

                l.moves.Clear();

                int move = random.Next(moveData.Count - 1) + 1;
                int randNum = random.Next(100);
                while (moveData[move].damageType == 0 || moveData[move].category == 9 || (randNum < stabRatioNumberBox.Value && !(moveData[move].element == pk.type1 || moveData[move].element == pk.type2))) move = random.Next(moveData.Count);
                l.moves.Add(new LevelUpMoveSlot((short)move, 1));

                move = random.Next(moveData.Count);
                while (moveData[move].damageType != 0) move = random.Next(moveData.Count);
                l.moves.Add(new LevelUpMoveSlot((short)move, 1));

                for (int i = 1 + (int)moveSpacingNumberBox.Value; i <= 100 && l.moves.Count < totalMovesNumberBox.Value; i += (int)moveSpacingNumberBox.Value)
                {
                    move = random.Next(moveData.Count);
                    randNum = random.Next(100);
                    while (moveData[move].category == 9 || (moveData[move].damageType != 0 && randNum < stabRatioNumberBox.Value && !(moveData[move].element == pk.type1 || moveData[move].element == pk.type2))) move = random.Next(moveData.Count);
                    l.moves.Add(new LevelUpMoveSlot((short)move, (short)i));
                }

                pk.ApplyData();
            }
        }
    }
}
