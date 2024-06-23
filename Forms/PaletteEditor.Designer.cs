namespace NewEditor.Forms
{
    partial class PaletteEditor
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
            this.pokemonSpriteBox = new System.Windows.Forms.PictureBox();
            this.applyPaletteButton = new System.Windows.Forms.Button();
            this.shinyCheckBox = new System.Windows.Forms.CheckBox();
            this.imageTypeDropdown = new System.Windows.Forms.ComboBox();
            this.saveImageButton = new System.Windows.Forms.Button();
            this.importImageButton = new System.Windows.Forms.Button();
            this.replaceFileButton = new System.Windows.Forms.Button();
            this.extractFileButton = new System.Windows.Forms.Button();
            this.fileIDDropdown = new System.Windows.Forms.ComboBox();
            this.importPaletteButton = new System.Windows.Forms.Button();
            this.savePaletteButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pokemonSpriteBox
            // 
            this.pokemonSpriteBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pokemonSpriteBox.Location = new System.Drawing.Point(12, 42);
            this.pokemonSpriteBox.Name = "pokemonSpriteBox";
            this.pokemonSpriteBox.Size = new System.Drawing.Size(512, 256);
            this.pokemonSpriteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pokemonSpriteBox.TabIndex = 0;
            this.pokemonSpriteBox.TabStop = false;
            // 
            // applyPaletteButton
            // 
            this.applyPaletteButton.Location = new System.Drawing.Point(77, 304);
            this.applyPaletteButton.Name = "applyPaletteButton";
            this.applyPaletteButton.Size = new System.Drawing.Size(120, 35);
            this.applyPaletteButton.TabIndex = 1;
            this.applyPaletteButton.Text = "Apply Palette";
            this.applyPaletteButton.UseVisualStyleBackColor = true;
            this.applyPaletteButton.Click += new System.EventHandler(this.applyPaletteButton_Click);
            // 
            // shinyCheckBox
            // 
            this.shinyCheckBox.AutoSize = true;
            this.shinyCheckBox.Location = new System.Drawing.Point(12, 312);
            this.shinyCheckBox.Name = "shinyCheckBox";
            this.shinyCheckBox.Size = new System.Drawing.Size(59, 20);
            this.shinyCheckBox.TabIndex = 3;
            this.shinyCheckBox.Text = "Shiny";
            this.shinyCheckBox.UseVisualStyleBackColor = true;
            this.shinyCheckBox.CheckedChanged += new System.EventHandler(this.shinyCheckBox_CheckedChanged);
            // 
            // imageTypeDropdown
            // 
            this.imageTypeDropdown.FormattingEnabled = true;
            this.imageTypeDropdown.Location = new System.Drawing.Point(12, 12);
            this.imageTypeDropdown.Name = "imageTypeDropdown";
            this.imageTypeDropdown.Size = new System.Drawing.Size(160, 24);
            this.imageTypeDropdown.TabIndex = 4;
            this.imageTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.imageTypeDropdown_SelectedIndexChanged);
            // 
            // saveImageButton
            // 
            this.saveImageButton.Location = new System.Drawing.Point(530, 42);
            this.saveImageButton.Name = "saveImageButton";
            this.saveImageButton.Size = new System.Drawing.Size(100, 35);
            this.saveImageButton.TabIndex = 5;
            this.saveImageButton.Text = "Save Image";
            this.saveImageButton.UseVisualStyleBackColor = true;
            this.saveImageButton.Click += new System.EventHandler(this.saveImageButton_Click);
            // 
            // importImageButton
            // 
            this.importImageButton.Location = new System.Drawing.Point(530, 83);
            this.importImageButton.Name = "importImageButton";
            this.importImageButton.Size = new System.Drawing.Size(100, 35);
            this.importImageButton.TabIndex = 6;
            this.importImageButton.Text = "Import Image";
            this.importImageButton.UseVisualStyleBackColor = true;
            this.importImageButton.Click += new System.EventHandler(this.importImageButton_Click);
            // 
            // replaceFileButton
            // 
            this.replaceFileButton.Location = new System.Drawing.Point(872, 83);
            this.replaceFileButton.Name = "replaceFileButton";
            this.replaceFileButton.Size = new System.Drawing.Size(100, 35);
            this.replaceFileButton.TabIndex = 8;
            this.replaceFileButton.Text = "Replace File";
            this.replaceFileButton.UseVisualStyleBackColor = true;
            this.replaceFileButton.Click += new System.EventHandler(this.replaceFileButton_Click);
            // 
            // extractFileButton
            // 
            this.extractFileButton.Location = new System.Drawing.Point(872, 42);
            this.extractFileButton.Name = "extractFileButton";
            this.extractFileButton.Size = new System.Drawing.Size(100, 35);
            this.extractFileButton.TabIndex = 7;
            this.extractFileButton.Text = "Extract File";
            this.extractFileButton.UseVisualStyleBackColor = true;
            this.extractFileButton.Click += new System.EventHandler(this.extractFileButton_Click);
            // 
            // fileIDDropdown
            // 
            this.fileIDDropdown.FormattingEnabled = true;
            this.fileIDDropdown.Items.AddRange(new object[] {
            "Front Male Sprite",
            "Front Female Sprite",
            "Front Male Rig",
            "Front Female Rig",
            "Front RECN",
            "Front RNAN",
            "Front RCMN",
            "Front RAMN",
            "Front Rig Cells",
            "Back Male Sprite",
            "Back Female Sprite",
            "Back Male Rig",
            "Back Female Rig",
            "Back RECN",
            "Back RNAN",
            "Back RCMN",
            "Back RAMN",
            "Back Rig Cells",
            "Palette",
            "Shiny Palette"});
            this.fileIDDropdown.Location = new System.Drawing.Point(706, 48);
            this.fileIDDropdown.Name = "fileIDDropdown";
            this.fileIDDropdown.Size = new System.Drawing.Size(160, 24);
            this.fileIDDropdown.TabIndex = 9;
            // 
            // importPaletteButton
            // 
            this.importPaletteButton.Location = new System.Drawing.Point(424, 304);
            this.importPaletteButton.Name = "importPaletteButton";
            this.importPaletteButton.Size = new System.Drawing.Size(100, 35);
            this.importPaletteButton.TabIndex = 11;
            this.importPaletteButton.Text = "Import Palette";
            this.importPaletteButton.UseVisualStyleBackColor = true;
            this.importPaletteButton.Click += new System.EventHandler(this.importPaletteButton_Click);
            // 
            // savePaletteButton
            // 
            this.savePaletteButton.Location = new System.Drawing.Point(318, 304);
            this.savePaletteButton.Name = "savePaletteButton";
            this.savePaletteButton.Size = new System.Drawing.Size(100, 35);
            this.savePaletteButton.TabIndex = 10;
            this.savePaletteButton.Text = "Save Palette";
            this.savePaletteButton.UseVisualStyleBackColor = true;
            this.savePaletteButton.Click += new System.EventHandler(this.savePaletteButton_Click);
            // 
            // PaletteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 721);
            this.Controls.Add(this.importPaletteButton);
            this.Controls.Add(this.savePaletteButton);
            this.Controls.Add(this.fileIDDropdown);
            this.Controls.Add(this.replaceFileButton);
            this.Controls.Add(this.extractFileButton);
            this.Controls.Add(this.importImageButton);
            this.Controls.Add(this.saveImageButton);
            this.Controls.Add(this.imageTypeDropdown);
            this.Controls.Add(this.shinyCheckBox);
            this.Controls.Add(this.applyPaletteButton);
            this.Controls.Add(this.pokemonSpriteBox);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PaletteEditor";
            this.Text = "PaletteEditor";
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pokemonSpriteBox;
        private System.Windows.Forms.Button applyPaletteButton;
        private System.Windows.Forms.CheckBox shinyCheckBox;
        private System.Windows.Forms.ComboBox imageTypeDropdown;
        private System.Windows.Forms.Button saveImageButton;
        private System.Windows.Forms.Button importImageButton;
        private System.Windows.Forms.Button replaceFileButton;
        private System.Windows.Forms.Button extractFileButton;
        private System.Windows.Forms.ComboBox fileIDDropdown;
        private System.Windows.Forms.Button importPaletteButton;
        private System.Windows.Forms.Button savePaletteButton;
    }
}