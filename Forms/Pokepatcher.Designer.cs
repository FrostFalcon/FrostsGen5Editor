namespace NewEditor.Forms
{
    partial class Pokepatcher
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
            this.pokemonListbox = new System.Windows.Forms.ListBox();
            this.pokeNameDropdown = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.addPokeButton = new System.Windows.Forms.Button();
            this.removePokeButton = new System.Windows.Forms.Button();
            this.savePatchButton = new System.Windows.Forms.Button();
            this.loadPatchButton = new System.Windows.Forms.Button();
            this.randomIDTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pokemonListbox
            // 
            this.pokemonListbox.FormattingEnabled = true;
            this.pokemonListbox.ItemHeight = 16;
            this.pokemonListbox.Location = new System.Drawing.Point(16, 73);
            this.pokemonListbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pokemonListbox.Name = "pokemonListbox";
            this.pokemonListbox.Size = new System.Drawing.Size(140, 228);
            this.pokemonListbox.TabIndex = 0;
            // 
            // pokeNameDropdown
            // 
            this.pokeNameDropdown.FormattingEnabled = true;
            this.pokeNameDropdown.Location = new System.Drawing.Point(16, 42);
            this.pokeNameDropdown.Name = "pokeNameDropdown";
            this.pokeNameDropdown.Size = new System.Drawing.Size(140, 24);
            this.pokeNameDropdown.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.randomIDTextBox);
            this.groupBox1.Controls.Add(this.savePatchButton);
            this.groupBox1.Controls.Add(this.removePokeButton);
            this.groupBox1.Controls.Add(this.addPokeButton);
            this.groupBox1.Controls.Add(this.pokeNameDropdown);
            this.groupBox1.Controls.Add(this.pokemonListbox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 320);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pokepatch Creation";
            // 
            // addPokeButton
            // 
            this.addPokeButton.Location = new System.Drawing.Point(162, 40);
            this.addPokeButton.Name = "addPokeButton";
            this.addPokeButton.Size = new System.Drawing.Size(80, 26);
            this.addPokeButton.TabIndex = 2;
            this.addPokeButton.Text = "Add";
            this.addPokeButton.UseVisualStyleBackColor = true;
            this.addPokeButton.Click += new System.EventHandler(this.addPokeButton_Click);
            // 
            // removePokeButton
            // 
            this.removePokeButton.Location = new System.Drawing.Point(162, 73);
            this.removePokeButton.Name = "removePokeButton";
            this.removePokeButton.Size = new System.Drawing.Size(80, 26);
            this.removePokeButton.TabIndex = 3;
            this.removePokeButton.Text = "Remove";
            this.removePokeButton.UseVisualStyleBackColor = true;
            this.removePokeButton.Click += new System.EventHandler(this.removePokeButton_Click);
            // 
            // savePatchButton
            // 
            this.savePatchButton.Location = new System.Drawing.Point(162, 269);
            this.savePatchButton.Name = "savePatchButton";
            this.savePatchButton.Size = new System.Drawing.Size(100, 32);
            this.savePatchButton.TabIndex = 4;
            this.savePatchButton.Text = "Save Patch";
            this.savePatchButton.UseVisualStyleBackColor = true;
            this.savePatchButton.Click += new System.EventHandler(this.savePatchButton_Click);
            // 
            // loadPatchButton
            // 
            this.loadPatchButton.Location = new System.Drawing.Point(360, 12);
            this.loadPatchButton.Name = "loadPatchButton";
            this.loadPatchButton.Size = new System.Drawing.Size(120, 40);
            this.loadPatchButton.TabIndex = 5;
            this.loadPatchButton.Text = "Load Pokepatch";
            this.loadPatchButton.UseVisualStyleBackColor = true;
            this.loadPatchButton.Click += new System.EventHandler(this.loadPatchButton_Click);
            // 
            // randomIDTextBox
            // 
            this.randomIDTextBox.Location = new System.Drawing.Point(163, 243);
            this.randomIDTextBox.Name = "randomIDTextBox";
            this.randomIDTextBox.Size = new System.Drawing.Size(99, 22);
            this.randomIDTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(160, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "RandomizerID";
            // 
            // Pokepatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 341);
            this.Controls.Add(this.loadPatchButton);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Pokepatcher";
            this.Text = "Pokepatcher";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox pokemonListbox;
        private System.Windows.Forms.ComboBox pokeNameDropdown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button savePatchButton;
        private System.Windows.Forms.Button removePokeButton;
        private System.Windows.Forms.Button addPokeButton;
        private System.Windows.Forms.Button loadPatchButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox randomIDTextBox;
    }
}