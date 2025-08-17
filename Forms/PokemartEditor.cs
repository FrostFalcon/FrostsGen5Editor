﻿using NewEditor.Data;
using NewEditor.Data.NARCTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewEditor.Forms
{
    public partial class PokemartEditor : Form
    {
        public PokemartEditor()
        {
            InitializeComponent();

            shopIDDropdown.Items.AddRange(MainEditor.pokemartNarc.shops.ToArray());
            itemIDDropdown.Items.AddRange(MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text.ToArray());
        }

        private void shopIDDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (shopIDDropdown.SelectedItem is PokemartEntry shop)
            {
                itemListBox.Items.Clear();
                foreach (int item in shop.items)
                {
                    itemListBox.Items.Add(MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text[item]);
                }
                if (itemListBox.Items.Count > 0) itemListBox.SelectedIndex = 0;
            }
        }

        private void itemListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemIDDropdown.SelectedItem = itemListBox.SelectedItem;
            if (itemIDDropdown.SelectedIndex < 0) return;
            priceNumberBox.Value = MainEditor.itemDataNarc.items[itemIDDropdown.SelectedIndex].BuyPrice;
        }

        private void setItemButton_Click(object sender, EventArgs e)
        {
            if (itemListBox.SelectedIndex != -1 && itemIDDropdown.SelectedIndex != -1)
            {
                itemListBox.Items[itemListBox.SelectedIndex] = itemIDDropdown.SelectedItem;
            }
        }

        private void addItemButton_Click(object sender, EventArgs e)
        {
            if (itemListBox.Items.Count >= 250)
            {
                MessageBox.Show("Can't add any more items to this shop.");
                return;
            }
            if (itemIDDropdown.SelectedIndex != -1)
            {
                int pos = itemListBox.SelectedIndex + 1;
                itemListBox.Items.Insert(pos, itemIDDropdown.SelectedItem);
                itemListBox.SelectedIndex = pos;
            }
        }

        private void removeItemButton_Click(object sender, EventArgs e)
        {
            if (itemListBox.SelectedIndex != -1)
            {
                int pos = itemListBox.SelectedIndex;
                itemListBox.Items.RemoveAt(pos);
                if (pos != itemListBox.Items.Count)
                    itemListBox.SelectedIndex = pos;
                else if (itemListBox.Items.Count > 0)
                    itemListBox.SelectedIndex = pos - 1;
            }
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            if (itemListBox.SelectedIndex > 0)
            {
                int pos = itemListBox.SelectedIndex;
                string item = itemListBox.SelectedItem as string;
                itemListBox.Items.RemoveAt(pos);
                itemListBox.Items.Insert(pos - 1, item);
                itemListBox.SelectedIndex = pos - 1;
            }
        }
        private void moveDownButton_Click(object sender, EventArgs e)
        {
            if (itemListBox.SelectedIndex != -1 && itemListBox.SelectedIndex < itemListBox.Items.Count - 1)
            {
                int pos = itemListBox.SelectedIndex;
                string item = itemListBox.SelectedItem as string;
                itemListBox.Items.RemoveAt(pos);
                itemListBox.Items.Insert(pos + 1, item);
                itemListBox.SelectedIndex = pos + 1;
            }
        }

        private void applyShopButton_Click(object sender, EventArgs e)
        {
            if (shopIDDropdown.SelectedItem is PokemartEntry shop)
            {
                shop.items = new List<int>();
                foreach (string str in itemListBox.Items)
                {
                    shop.items.Add(MainEditor.textNarc.textFiles[VersionConstants.ItemNameTextFileID].text.IndexOf(str));
                }
                shop.Apply();

                int id = MainEditor.pokemartNarc.shops.IndexOf(shop);
                if (id >= 0 && id < MainEditor.pokemartItemCountNarc.itemCounts.Count)
                {
                    MainEditor.pokemartItemCountNarc.itemCounts[id] = (byte)shop.items.Count;
                }

                statusText.Text = "Saved shop data - " + DateTime.Now.StatusText();
            }
        }

        private void setPriceButton_Click(object sender, EventArgs e)
        {
            if (priceNumberBox.Value % 10 != 0)
            {
                MessageBox.Show("Item prices must be a multiple of 10.");
                return;
            }

            MainEditor.itemDataNarc.items[itemIDDropdown.SelectedIndex].BuyPrice = (int)priceNumberBox.Value;

            statusText.Text = "Set item price - " + DateTime.Now.StatusText();
        }

        private void itemIDDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemIDDropdown.SelectedIndex < 0) return;
            priceNumberBox.Value = MainEditor.itemDataNarc.items[itemIDDropdown.SelectedIndex].BuyPrice;
        }
    }
}
