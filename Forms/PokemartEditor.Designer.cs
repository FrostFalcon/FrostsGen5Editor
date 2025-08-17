namespace NewEditor.Forms
{
    partial class PokemartEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.shopIDDropdown = new System.Windows.Forms.ComboBox();
            this.itemIDDropdown = new System.Windows.Forms.ComboBox();
            this.itemListBox = new System.Windows.Forms.ListBox();
            this.setItemButton = new System.Windows.Forms.Button();
            this.addItemButton = new System.Windows.Forms.Button();
            this.removeItemButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.applyShopButton = new System.Windows.Forms.Button();
            this.priceNumberBox = new System.Windows.Forms.NumericUpDown();
            this.setPriceButton = new System.Windows.Forms.Button();
            this.statusText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.priceNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // shopIDDropdown
            // 
            this.shopIDDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.shopIDDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.shopIDDropdown.FormattingEnabled = true;
            this.shopIDDropdown.Location = new System.Drawing.Point(14, 12);
            this.shopIDDropdown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.shopIDDropdown.Name = "shopIDDropdown";
            this.shopIDDropdown.Size = new System.Drawing.Size(256, 24);
            this.shopIDDropdown.TabIndex = 71;
            this.shopIDDropdown.SelectedIndexChanged += new System.EventHandler(this.shopIDDropdown_SelectedIndexChanged);
            // 
            // itemIDDropdown
            // 
            this.itemIDDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.itemIDDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.itemIDDropdown.FormattingEnabled = true;
            this.itemIDDropdown.Location = new System.Drawing.Point(208, 61);
            this.itemIDDropdown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.itemIDDropdown.Name = "itemIDDropdown";
            this.itemIDDropdown.Size = new System.Drawing.Size(166, 24);
            this.itemIDDropdown.TabIndex = 72;
            this.itemIDDropdown.SelectedIndexChanged += new System.EventHandler(this.itemIDDropdown_SelectedIndexChanged);
            // 
            // itemListBox
            // 
            this.itemListBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemListBox.FormattingEnabled = true;
            this.itemListBox.ItemHeight = 16;
            this.itemListBox.Location = new System.Drawing.Point(14, 61);
            this.itemListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.itemListBox.Name = "itemListBox";
            this.itemListBox.Size = new System.Drawing.Size(186, 228);
            this.itemListBox.TabIndex = 75;
            this.itemListBox.SelectedIndexChanged += new System.EventHandler(this.itemListBox_SelectedIndexChanged);
            // 
            // setItemButton
            // 
            this.setItemButton.Location = new System.Drawing.Point(208, 90);
            this.setItemButton.Name = "setItemButton";
            this.setItemButton.Size = new System.Drawing.Size(80, 24);
            this.setItemButton.TabIndex = 76;
            this.setItemButton.Text = "Set Item";
            this.setItemButton.UseVisualStyleBackColor = true;
            this.setItemButton.Click += new System.EventHandler(this.setItemButton_Click);
            // 
            // addItemButton
            // 
            this.addItemButton.Location = new System.Drawing.Point(294, 91);
            this.addItemButton.Name = "addItemButton";
            this.addItemButton.Size = new System.Drawing.Size(80, 24);
            this.addItemButton.TabIndex = 77;
            this.addItemButton.Text = "Add Item";
            this.addItemButton.UseVisualStyleBackColor = true;
            this.addItemButton.Click += new System.EventHandler(this.addItemButton_Click);
            // 
            // removeItemButton
            // 
            this.removeItemButton.Location = new System.Drawing.Point(208, 180);
            this.removeItemButton.Name = "removeItemButton";
            this.removeItemButton.Size = new System.Drawing.Size(100, 30);
            this.removeItemButton.TabIndex = 78;
            this.removeItemButton.Text = "Remove Item";
            this.removeItemButton.UseVisualStyleBackColor = true;
            this.removeItemButton.Click += new System.EventHandler(this.removeItemButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(208, 235);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(24, 24);
            this.moveUpButton.TabIndex = 79;
            this.moveUpButton.Text = "^";
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(208, 265);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(24, 24);
            this.moveDownButton.TabIndex = 80;
            this.moveDownButton.Text = "v";
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // applyShopButton
            // 
            this.applyShopButton.Location = new System.Drawing.Point(247, 259);
            this.applyShopButton.Name = "applyShopButton";
            this.applyShopButton.Size = new System.Drawing.Size(100, 30);
            this.applyShopButton.TabIndex = 81;
            this.applyShopButton.Text = "Apply Shop";
            this.applyShopButton.UseVisualStyleBackColor = true;
            this.applyShopButton.Click += new System.EventHandler(this.applyShopButton_Click);
            // 
            // priceNumberBox
            // 
            this.priceNumberBox.Location = new System.Drawing.Point(208, 132);
            this.priceNumberBox.Maximum = new decimal(new int[] {
            655350,
            0,
            0,
            0});
            this.priceNumberBox.Name = "priceNumberBox";
            this.priceNumberBox.Size = new System.Drawing.Size(80, 22);
            this.priceNumberBox.TabIndex = 82;
            // 
            // setPriceButton
            // 
            this.setPriceButton.Location = new System.Drawing.Point(294, 130);
            this.setPriceButton.Name = "setPriceButton";
            this.setPriceButton.Size = new System.Drawing.Size(80, 24);
            this.setPriceButton.TabIndex = 83;
            this.setPriceButton.Text = "Set Price";
            this.setPriceButton.UseVisualStyleBackColor = true;
            this.setPriceButton.Click += new System.EventHandler(this.setPriceButton_Click);
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(11, 296);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 16);
            this.statusText.TabIndex = 84;
            // 
            // PokemartEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 321);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.setPriceButton);
            this.Controls.Add(this.priceNumberBox);
            this.Controls.Add(this.applyShopButton);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.removeItemButton);
            this.Controls.Add(this.addItemButton);
            this.Controls.Add(this.setItemButton);
            this.Controls.Add(this.itemListBox);
            this.Controls.Add(this.itemIDDropdown);
            this.Controls.Add(this.shopIDDropdown);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PokemartEditor";
            this.Text = "Pokemart Editor";
            ((System.ComponentModel.ISupportInitialize)(this.priceNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox shopIDDropdown;
        private System.Windows.Forms.ComboBox itemIDDropdown;
        private System.Windows.Forms.ListBox itemListBox;
        private System.Windows.Forms.Button setItemButton;
        private System.Windows.Forms.Button addItemButton;
        private System.Windows.Forms.Button removeItemButton;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.Button applyShopButton;
        private System.Windows.Forms.NumericUpDown priceNumberBox;
        private System.Windows.Forms.Button setPriceButton;
        private System.Windows.Forms.Label statusText;
    }
}