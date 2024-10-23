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
            this.pokemonIconBox = new System.Windows.Forms.PictureBox();
            this.paletteIDNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.iconTypeDropdown = new System.Windows.Forms.ComboBox();
            this.saveIconButton = new System.Windows.Forms.Button();
            this.importIconButton = new System.Windows.Forms.Button();
            this.setIconPaletteButton = new System.Windows.Forms.Button();
            this.saveIconPaletteButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonIconBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIDNumberBox)).BeginInit();
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
            // pokemonIconBox
            // 
            this.pokemonIconBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pokemonIconBox.Location = new System.Drawing.Point(908, 209);
            this.pokemonIconBox.Name = "pokemonIconBox";
            this.pokemonIconBox.Size = new System.Drawing.Size(64, 128);
            this.pokemonIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pokemonIconBox.TabIndex = 12;
            this.pokemonIconBox.TabStop = false;
            // 
            // paletteIDNumberBox
            // 
            this.paletteIDNumberBox.Location = new System.Drawing.Point(862, 209);
            this.paletteIDNumberBox.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.paletteIDNumberBox.Name = "paletteIDNumberBox";
            this.paletteIDNumberBox.Size = new System.Drawing.Size(40, 22);
            this.paletteIDNumberBox.TabIndex = 13;
            this.paletteIDNumberBox.ValueChanged += new System.EventHandler(this.paletteIDNumberBox_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(804, 211);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Palette:";
            // 
            // iconTypeDropdown
            // 
            this.iconTypeDropdown.FormattingEnabled = true;
            this.iconTypeDropdown.Items.AddRange(new object[] {
            "Male Icon",
            "Female Icon"});
            this.iconTypeDropdown.Location = new System.Drawing.Point(812, 179);
            this.iconTypeDropdown.Name = "iconTypeDropdown";
            this.iconTypeDropdown.Size = new System.Drawing.Size(160, 24);
            this.iconTypeDropdown.TabIndex = 15;
            this.iconTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.iconTypeDropdown_SelectedIndexChanged);
            // 
            // saveIconButton
            // 
            this.saveIconButton.Location = new System.Drawing.Point(756, 343);
            this.saveIconButton.Name = "saveIconButton";
            this.saveIconButton.Size = new System.Drawing.Size(100, 35);
            this.saveIconButton.TabIndex = 16;
            this.saveIconButton.Text = "Save Icon";
            this.saveIconButton.UseVisualStyleBackColor = true;
            this.saveIconButton.Click += new System.EventHandler(this.saveIconButton_Click);
            // 
            // importIconButton
            // 
            this.importIconButton.Location = new System.Drawing.Point(872, 343);
            this.importIconButton.Name = "importIconButton";
            this.importIconButton.Size = new System.Drawing.Size(100, 35);
            this.importIconButton.TabIndex = 17;
            this.importIconButton.Text = "Import Icon";
            this.importIconButton.UseVisualStyleBackColor = true;
            this.importIconButton.Click += new System.EventHandler(this.importIconButton_Click);
            // 
            // setIconPaletteButton
            // 
            this.setIconPaletteButton.Location = new System.Drawing.Point(817, 237);
            this.setIconPaletteButton.Name = "setIconPaletteButton";
            this.setIconPaletteButton.Size = new System.Drawing.Size(85, 30);
            this.setIconPaletteButton.TabIndex = 18;
            this.setIconPaletteButton.Text = "Set Palette";
            this.setIconPaletteButton.UseVisualStyleBackColor = true;
            this.setIconPaletteButton.Click += new System.EventHandler(this.setIconPaletteButton_Click);
            // 
            // saveIconPaletteButton
            // 
            this.saveIconPaletteButton.Location = new System.Drawing.Point(807, 273);
            this.saveIconPaletteButton.Name = "saveIconPaletteButton";
            this.saveIconPaletteButton.Size = new System.Drawing.Size(95, 30);
            this.saveIconPaletteButton.TabIndex = 19;
            this.saveIconPaletteButton.Text = "Save Palette";
            this.saveIconPaletteButton.UseVisualStyleBackColor = true;
            this.saveIconPaletteButton.Click += new System.EventHandler(this.saveIconPaletteButton_Click);
            // 
            // PaletteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(984, 721);
            this.Controls.Add(this.saveIconPaletteButton);
            this.Controls.Add(this.setIconPaletteButton);
            this.Controls.Add(this.importIconButton);
            this.Controls.Add(this.saveIconButton);
            this.Controls.Add(this.iconTypeDropdown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.paletteIDNumberBox);
            this.Controls.Add(this.pokemonIconBox);
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
            ((System.ComponentModel.ISupportInitialize)(this.pokemonIconBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIDNumberBox)).EndInit();
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
        private System.Windows.Forms.PictureBox pokemonIconBox;
        private System.Windows.Forms.NumericUpDown paletteIDNumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox iconTypeDropdown;
        private System.Windows.Forms.Button saveIconButton;
        private System.Windows.Forms.Button importIconButton;
        private System.Windows.Forms.Button setIconPaletteButton;
        private System.Windows.Forms.Button saveIconPaletteButton;
    }
}