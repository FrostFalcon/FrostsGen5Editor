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
            this.dumpFilesButton = new System.Windows.Forms.Button();
            this.dumpFilesPIDNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.spritesTab = new System.Windows.Forms.TabPage();
            this.rigCellsTab = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.rigFlagsTextBox = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.maleRigCheckBox = new System.Windows.Forms.RadioButton();
            this.femaleRigCheckBox = new System.Windows.Forms.RadioButton();
            this.selectedCellText = new System.Windows.Forms.Label();
            this.cellPreview = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cellSpriteYNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cellSpriteXNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cellHeightNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cellWidthNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cellYNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.cellXNumberBox = new System.Windows.Forms.NumericUpDown();
            this.applyRigCellButton = new System.Windows.Forms.Button();
            this.backRigCheckbox = new System.Windows.Forms.RadioButton();
            this.frontRigCheckBox = new System.Windows.Forms.RadioButton();
            this.rigCellsRender = new System.Windows.Forms.PictureBox();
            this.addSubCellButton = new System.Windows.Forms.Button();
            this.subCellHelpButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonIconBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIDNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dumpFilesPIDNumberBox)).BeginInit();
            this.mainTabs.SuspendLayout();
            this.spritesTab.SuspendLayout();
            this.rigCellsTab.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cellPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellSpriteYNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellSpriteXNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellHeightNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellWidthNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellYNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellXNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rigCellsRender)).BeginInit();
            this.SuspendLayout();
            // 
            // pokemonSpriteBox
            // 
            this.pokemonSpriteBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pokemonSpriteBox.Location = new System.Drawing.Point(6, 36);
            this.pokemonSpriteBox.Name = "pokemonSpriteBox";
            this.pokemonSpriteBox.Size = new System.Drawing.Size(512, 256);
            this.pokemonSpriteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pokemonSpriteBox.TabIndex = 0;
            this.pokemonSpriteBox.TabStop = false;
            // 
            // applyPaletteButton
            // 
            this.applyPaletteButton.Location = new System.Drawing.Point(71, 298);
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
            this.shinyCheckBox.Location = new System.Drawing.Point(6, 306);
            this.shinyCheckBox.Name = "shinyCheckBox";
            this.shinyCheckBox.Size = new System.Drawing.Size(59, 20);
            this.shinyCheckBox.TabIndex = 3;
            this.shinyCheckBox.Text = "Shiny";
            this.shinyCheckBox.UseVisualStyleBackColor = true;
            this.shinyCheckBox.CheckedChanged += new System.EventHandler(this.shinyCheckBox_CheckedChanged);
            // 
            // imageTypeDropdown
            // 
            this.imageTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageTypeDropdown.FormattingEnabled = true;
            this.imageTypeDropdown.Location = new System.Drawing.Point(6, 6);
            this.imageTypeDropdown.Name = "imageTypeDropdown";
            this.imageTypeDropdown.Size = new System.Drawing.Size(160, 24);
            this.imageTypeDropdown.TabIndex = 4;
            this.imageTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.imageTypeDropdown_SelectedIndexChanged);
            // 
            // saveImageButton
            // 
            this.saveImageButton.Location = new System.Drawing.Point(524, 36);
            this.saveImageButton.Name = "saveImageButton";
            this.saveImageButton.Size = new System.Drawing.Size(100, 35);
            this.saveImageButton.TabIndex = 5;
            this.saveImageButton.Text = "Save Image";
            this.saveImageButton.UseVisualStyleBackColor = true;
            this.saveImageButton.Click += new System.EventHandler(this.saveImageButton_Click);
            // 
            // importImageButton
            // 
            this.importImageButton.Location = new System.Drawing.Point(524, 77);
            this.importImageButton.Name = "importImageButton";
            this.importImageButton.Size = new System.Drawing.Size(100, 35);
            this.importImageButton.TabIndex = 6;
            this.importImageButton.Text = "Import Image";
            this.importImageButton.UseVisualStyleBackColor = true;
            this.importImageButton.Click += new System.EventHandler(this.importImageButton_Click);
            // 
            // replaceFileButton
            // 
            this.replaceFileButton.Location = new System.Drawing.Point(1077, 91);
            this.replaceFileButton.Name = "replaceFileButton";
            this.replaceFileButton.Size = new System.Drawing.Size(100, 35);
            this.replaceFileButton.TabIndex = 8;
            this.replaceFileButton.Text = "Replace File";
            this.replaceFileButton.UseVisualStyleBackColor = true;
            this.replaceFileButton.Click += new System.EventHandler(this.replaceFileButton_Click);
            // 
            // extractFileButton
            // 
            this.extractFileButton.Location = new System.Drawing.Point(1077, 50);
            this.extractFileButton.Name = "extractFileButton";
            this.extractFileButton.Size = new System.Drawing.Size(100, 35);
            this.extractFileButton.TabIndex = 7;
            this.extractFileButton.Text = "Extract File";
            this.extractFileButton.UseVisualStyleBackColor = true;
            this.extractFileButton.Click += new System.EventHandler(this.extractFileButton_Click);
            // 
            // fileIDDropdown
            // 
            this.fileIDDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.fileIDDropdown.Location = new System.Drawing.Point(1017, 12);
            this.fileIDDropdown.Name = "fileIDDropdown";
            this.fileIDDropdown.Size = new System.Drawing.Size(160, 24);
            this.fileIDDropdown.TabIndex = 9;
            // 
            // importPaletteButton
            // 
            this.importPaletteButton.Location = new System.Drawing.Point(418, 298);
            this.importPaletteButton.Name = "importPaletteButton";
            this.importPaletteButton.Size = new System.Drawing.Size(100, 35);
            this.importPaletteButton.TabIndex = 11;
            this.importPaletteButton.Text = "Import Palette";
            this.importPaletteButton.UseVisualStyleBackColor = true;
            this.importPaletteButton.Click += new System.EventHandler(this.importPaletteButton_Click);
            // 
            // savePaletteButton
            // 
            this.savePaletteButton.Location = new System.Drawing.Point(312, 298);
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
            this.pokemonIconBox.Location = new System.Drawing.Point(1113, 270);
            this.pokemonIconBox.Name = "pokemonIconBox";
            this.pokemonIconBox.Size = new System.Drawing.Size(64, 128);
            this.pokemonIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pokemonIconBox.TabIndex = 12;
            this.pokemonIconBox.TabStop = false;
            // 
            // paletteIDNumberBox
            // 
            this.paletteIDNumberBox.Location = new System.Drawing.Point(1067, 270);
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
            this.label1.Location = new System.Drawing.Point(1009, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "Palette:";
            // 
            // iconTypeDropdown
            // 
            this.iconTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iconTypeDropdown.FormattingEnabled = true;
            this.iconTypeDropdown.Items.AddRange(new object[] {
            "Male Icon",
            "Female Icon"});
            this.iconTypeDropdown.Location = new System.Drawing.Point(1017, 240);
            this.iconTypeDropdown.Name = "iconTypeDropdown";
            this.iconTypeDropdown.Size = new System.Drawing.Size(160, 24);
            this.iconTypeDropdown.TabIndex = 15;
            this.iconTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.iconTypeDropdown_SelectedIndexChanged);
            // 
            // saveIconButton
            // 
            this.saveIconButton.Location = new System.Drawing.Point(961, 404);
            this.saveIconButton.Name = "saveIconButton";
            this.saveIconButton.Size = new System.Drawing.Size(100, 35);
            this.saveIconButton.TabIndex = 16;
            this.saveIconButton.Text = "Save Icon";
            this.saveIconButton.UseVisualStyleBackColor = true;
            this.saveIconButton.Click += new System.EventHandler(this.saveIconButton_Click);
            // 
            // importIconButton
            // 
            this.importIconButton.Location = new System.Drawing.Point(1077, 404);
            this.importIconButton.Name = "importIconButton";
            this.importIconButton.Size = new System.Drawing.Size(100, 35);
            this.importIconButton.TabIndex = 17;
            this.importIconButton.Text = "Import Icon";
            this.importIconButton.UseVisualStyleBackColor = true;
            this.importIconButton.Click += new System.EventHandler(this.importIconButton_Click);
            // 
            // setIconPaletteButton
            // 
            this.setIconPaletteButton.Location = new System.Drawing.Point(1022, 298);
            this.setIconPaletteButton.Name = "setIconPaletteButton";
            this.setIconPaletteButton.Size = new System.Drawing.Size(85, 30);
            this.setIconPaletteButton.TabIndex = 18;
            this.setIconPaletteButton.Text = "Set Palette";
            this.setIconPaletteButton.UseVisualStyleBackColor = true;
            this.setIconPaletteButton.Click += new System.EventHandler(this.setIconPaletteButton_Click);
            // 
            // saveIconPaletteButton
            // 
            this.saveIconPaletteButton.Location = new System.Drawing.Point(1012, 334);
            this.saveIconPaletteButton.Name = "saveIconPaletteButton";
            this.saveIconPaletteButton.Size = new System.Drawing.Size(95, 30);
            this.saveIconPaletteButton.TabIndex = 19;
            this.saveIconPaletteButton.Text = "Save Palette";
            this.saveIconPaletteButton.UseVisualStyleBackColor = true;
            this.saveIconPaletteButton.Click += new System.EventHandler(this.saveIconPaletteButton_Click);
            // 
            // dumpFilesButton
            // 
            this.dumpFilesButton.Location = new System.Drawing.Point(1077, 132);
            this.dumpFilesButton.Name = "dumpFilesButton";
            this.dumpFilesButton.Size = new System.Drawing.Size(100, 35);
            this.dumpFilesButton.TabIndex = 20;
            this.dumpFilesButton.Text = "Dump Files";
            this.dumpFilesButton.UseVisualStyleBackColor = true;
            this.dumpFilesButton.Click += new System.EventHandler(this.dumpFilesButton_Click);
            // 
            // dumpFilesPIDNumberBox
            // 
            this.dumpFilesPIDNumberBox.Location = new System.Drawing.Point(1011, 139);
            this.dumpFilesPIDNumberBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.dumpFilesPIDNumberBox.Name = "dumpFilesPIDNumberBox";
            this.dumpFilesPIDNumberBox.Size = new System.Drawing.Size(60, 22);
            this.dumpFilesPIDNumberBox.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(953, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 16);
            this.label2.TabIndex = 22;
            this.label2.Text = "Dex ID:";
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.spritesTab);
            this.mainTabs.Controls.Add(this.rigCellsTab);
            this.mainTabs.Location = new System.Drawing.Point(12, 12);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(940, 720);
            this.mainTabs.TabIndex = 23;
            // 
            // spritesTab
            // 
            this.spritesTab.Controls.Add(this.imageTypeDropdown);
            this.spritesTab.Controls.Add(this.pokemonSpriteBox);
            this.spritesTab.Controls.Add(this.applyPaletteButton);
            this.spritesTab.Controls.Add(this.shinyCheckBox);
            this.spritesTab.Controls.Add(this.saveImageButton);
            this.spritesTab.Controls.Add(this.importImageButton);
            this.spritesTab.Controls.Add(this.savePaletteButton);
            this.spritesTab.Controls.Add(this.importPaletteButton);
            this.spritesTab.Location = new System.Drawing.Point(4, 25);
            this.spritesTab.Name = "spritesTab";
            this.spritesTab.Padding = new System.Windows.Forms.Padding(3);
            this.spritesTab.Size = new System.Drawing.Size(932, 691);
            this.spritesTab.TabIndex = 0;
            this.spritesTab.Text = "Sprites";
            this.spritesTab.UseVisualStyleBackColor = true;
            // 
            // rigCellsTab
            // 
            this.rigCellsTab.Controls.Add(this.subCellHelpButton);
            this.rigCellsTab.Controls.Add(this.addSubCellButton);
            this.rigCellsTab.Controls.Add(this.label9);
            this.rigCellsTab.Controls.Add(this.rigFlagsTextBox);
            this.rigCellsTab.Controls.Add(this.panel1);
            this.rigCellsTab.Controls.Add(this.selectedCellText);
            this.rigCellsTab.Controls.Add(this.cellPreview);
            this.rigCellsTab.Controls.Add(this.label7);
            this.rigCellsTab.Controls.Add(this.cellSpriteYNumberBox);
            this.rigCellsTab.Controls.Add(this.label8);
            this.rigCellsTab.Controls.Add(this.cellSpriteXNumberBox);
            this.rigCellsTab.Controls.Add(this.label5);
            this.rigCellsTab.Controls.Add(this.cellHeightNumberBox);
            this.rigCellsTab.Controls.Add(this.label6);
            this.rigCellsTab.Controls.Add(this.cellWidthNumberBox);
            this.rigCellsTab.Controls.Add(this.label4);
            this.rigCellsTab.Controls.Add(this.cellYNumberBox);
            this.rigCellsTab.Controls.Add(this.label3);
            this.rigCellsTab.Controls.Add(this.cellXNumberBox);
            this.rigCellsTab.Controls.Add(this.applyRigCellButton);
            this.rigCellsTab.Controls.Add(this.backRigCheckbox);
            this.rigCellsTab.Controls.Add(this.frontRigCheckBox);
            this.rigCellsTab.Controls.Add(this.rigCellsRender);
            this.rigCellsTab.Location = new System.Drawing.Point(4, 25);
            this.rigCellsTab.Name = "rigCellsTab";
            this.rigCellsTab.Size = new System.Drawing.Size(932, 691);
            this.rigCellsTab.TabIndex = 2;
            this.rigCellsTab.Text = "Rig Cells";
            this.rigCellsTab.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(784, 505);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 16);
            this.label9.TabIndex = 41;
            this.label9.Text = "Flags:";
            // 
            // rigFlagsTextBox
            // 
            this.rigFlagsTextBox.Location = new System.Drawing.Point(784, 524);
            this.rigFlagsTextBox.Name = "rigFlagsTextBox";
            this.rigFlagsTextBox.Size = new System.Drawing.Size(140, 152);
            this.rigFlagsTextBox.TabIndex = 40;
            this.rigFlagsTextBox.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.maleRigCheckBox);
            this.panel1.Controls.Add(this.femaleRigCheckBox);
            this.panel1.Location = new System.Drawing.Point(779, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(120, 60);
            this.panel1.TabIndex = 39;
            // 
            // maleRigCheckBox
            // 
            this.maleRigCheckBox.AutoSize = true;
            this.maleRigCheckBox.Checked = true;
            this.maleRigCheckBox.Location = new System.Drawing.Point(1, 6);
            this.maleRigCheckBox.Name = "maleRigCheckBox";
            this.maleRigCheckBox.Size = new System.Drawing.Size(53, 20);
            this.maleRigCheckBox.TabIndex = 37;
            this.maleRigCheckBox.TabStop = true;
            this.maleRigCheckBox.Text = "Male";
            this.maleRigCheckBox.UseVisualStyleBackColor = true;
            // 
            // femaleRigCheckBox
            // 
            this.femaleRigCheckBox.AutoSize = true;
            this.femaleRigCheckBox.Location = new System.Drawing.Point(1, 32);
            this.femaleRigCheckBox.Name = "femaleRigCheckBox";
            this.femaleRigCheckBox.Size = new System.Drawing.Size(68, 20);
            this.femaleRigCheckBox.TabIndex = 38;
            this.femaleRigCheckBox.Text = "Female";
            this.femaleRigCheckBox.UseVisualStyleBackColor = true;
            // 
            // selectedCellText
            // 
            this.selectedCellText.AutoSize = true;
            this.selectedCellText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedCellText.Location = new System.Drawing.Point(10, 415);
            this.selectedCellText.Name = "selectedCellText";
            this.selectedCellText.Size = new System.Drawing.Size(47, 16);
            this.selectedCellText.TabIndex = 36;
            this.selectedCellText.Text = "Cell 0:";
            // 
            // cellPreview
            // 
            this.cellPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.cellPreview.Location = new System.Drawing.Point(266, 420);
            this.cellPreview.Name = "cellPreview";
            this.cellPreview.Size = new System.Drawing.Size(512, 256);
            this.cellPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.cellPreview.TabIndex = 35;
            this.cellPreview.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(136, 526);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 16);
            this.label7.TabIndex = 34;
            this.label7.Text = "Sprite Y:";
            // 
            // cellSpriteYNumberBox
            // 
            this.cellSpriteYNumberBox.Location = new System.Drawing.Point(196, 524);
            this.cellSpriteYNumberBox.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.cellSpriteYNumberBox.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.cellSpriteYNumberBox.Name = "cellSpriteYNumberBox";
            this.cellSpriteYNumberBox.Size = new System.Drawing.Size(60, 22);
            this.cellSpriteYNumberBox.TabIndex = 33;
            this.cellSpriteYNumberBox.ValueChanged += new System.EventHandler(this.cellSpriteYNumberBox_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 526);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 32;
            this.label8.Text = "Sprite X:";
            // 
            // cellSpriteXNumberBox
            // 
            this.cellSpriteXNumberBox.Location = new System.Drawing.Point(70, 524);
            this.cellSpriteXNumberBox.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.cellSpriteXNumberBox.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.cellSpriteXNumberBox.Name = "cellSpriteXNumberBox";
            this.cellSpriteXNumberBox.Size = new System.Drawing.Size(60, 22);
            this.cellSpriteXNumberBox.TabIndex = 31;
            this.cellSpriteXNumberBox.ValueChanged += new System.EventHandler(this.cellSpriteXNumberBox_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(136, 486);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 16);
            this.label5.TabIndex = 30;
            this.label5.Text = "Height:";
            // 
            // cellHeightNumberBox
            // 
            this.cellHeightNumberBox.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellHeightNumberBox.Location = new System.Drawing.Point(196, 484);
            this.cellHeightNumberBox.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.cellHeightNumberBox.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellHeightNumberBox.Name = "cellHeightNumberBox";
            this.cellHeightNumberBox.Size = new System.Drawing.Size(60, 22);
            this.cellHeightNumberBox.TabIndex = 29;
            this.cellHeightNumberBox.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellHeightNumberBox.ValueChanged += new System.EventHandler(this.cellHeightNumberBox_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 486);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 16);
            this.label6.TabIndex = 28;
            this.label6.Text = "Width:";
            // 
            // cellWidthNumberBox
            // 
            this.cellWidthNumberBox.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellWidthNumberBox.Location = new System.Drawing.Point(70, 484);
            this.cellWidthNumberBox.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.cellWidthNumberBox.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellWidthNumberBox.Name = "cellWidthNumberBox";
            this.cellWidthNumberBox.Size = new System.Drawing.Size(60, 22);
            this.cellWidthNumberBox.TabIndex = 27;
            this.cellWidthNumberBox.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellWidthNumberBox.ValueChanged += new System.EventHandler(this.cellWidthNumberBox_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 446);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 16);
            this.label4.TabIndex = 26;
            this.label4.Text = "Cell Y:";
            // 
            // cellYNumberBox
            // 
            this.cellYNumberBox.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellYNumberBox.Location = new System.Drawing.Point(196, 444);
            this.cellYNumberBox.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.cellYNumberBox.Name = "cellYNumberBox";
            this.cellYNumberBox.Size = new System.Drawing.Size(60, 22);
            this.cellYNumberBox.TabIndex = 25;
            this.cellYNumberBox.ValueChanged += new System.EventHandler(this.cellYNumberBox_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 446);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Cell X:";
            // 
            // cellXNumberBox
            // 
            this.cellXNumberBox.Increment = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.cellXNumberBox.Location = new System.Drawing.Point(70, 444);
            this.cellXNumberBox.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.cellXNumberBox.Name = "cellXNumberBox";
            this.cellXNumberBox.Size = new System.Drawing.Size(60, 22);
            this.cellXNumberBox.TabIndex = 23;
            this.cellXNumberBox.ValueChanged += new System.EventHandler(this.cellXNumberBox_ValueChanged);
            // 
            // applyRigCellButton
            // 
            this.applyRigCellButton.Location = new System.Drawing.Point(13, 640);
            this.applyRigCellButton.Name = "applyRigCellButton";
            this.applyRigCellButton.Size = new System.Drawing.Size(120, 35);
            this.applyRigCellButton.TabIndex = 4;
            this.applyRigCellButton.Text = "Apply Rig Cells";
            this.applyRigCellButton.UseVisualStyleBackColor = true;
            this.applyRigCellButton.Click += new System.EventHandler(this.applyRigCellButton_Click);
            // 
            // backRigCheckbox
            // 
            this.backRigCheckbox.AutoSize = true;
            this.backRigCheckbox.Location = new System.Drawing.Point(780, 46);
            this.backRigCheckbox.Name = "backRigCheckbox";
            this.backRigCheckbox.Size = new System.Drawing.Size(78, 20);
            this.backRigCheckbox.TabIndex = 3;
            this.backRigCheckbox.Text = "Back Rig";
            this.backRigCheckbox.UseVisualStyleBackColor = true;
            // 
            // frontRigCheckBox
            // 
            this.frontRigCheckBox.AutoSize = true;
            this.frontRigCheckBox.Checked = true;
            this.frontRigCheckBox.Location = new System.Drawing.Point(780, 20);
            this.frontRigCheckBox.Name = "frontRigCheckBox";
            this.frontRigCheckBox.Size = new System.Drawing.Size(78, 20);
            this.frontRigCheckBox.TabIndex = 2;
            this.frontRigCheckBox.TabStop = true;
            this.frontRigCheckBox.Text = "Front Rig";
            this.frontRigCheckBox.UseVisualStyleBackColor = true;
            this.frontRigCheckBox.CheckedChanged += new System.EventHandler(this.frontrigCheckBox_CheckedChanged);
            // 
            // rigCellsRender
            // 
            this.rigCellsRender.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rigCellsRender.Location = new System.Drawing.Point(10, 20);
            this.rigCellsRender.Name = "rigCellsRender";
            this.rigCellsRender.Size = new System.Drawing.Size(768, 384);
            this.rigCellsRender.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.rigCellsRender.TabIndex = 1;
            this.rigCellsRender.TabStop = false;
            this.rigCellsRender.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rigCellsRender_MouseDown);
            this.rigCellsRender.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rigCellsRender_MouseMove);
            this.rigCellsRender.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rigCellsRender_MouseUp);
            // 
            // addSubCellButton
            // 
            this.addSubCellButton.Location = new System.Drawing.Point(13, 560);
            this.addSubCellButton.Name = "addSubCellButton";
            this.addSubCellButton.Size = new System.Drawing.Size(120, 30);
            this.addSubCellButton.TabIndex = 42;
            this.addSubCellButton.Text = "Add Sub Cell";
            this.addSubCellButton.UseVisualStyleBackColor = true;
            this.addSubCellButton.Click += new System.EventHandler(this.addSubCellButton_Click);
            // 
            // subCellHelpButton
            // 
            this.subCellHelpButton.Location = new System.Drawing.Point(140, 560);
            this.subCellHelpButton.Name = "subCellHelpButton";
            this.subCellHelpButton.Size = new System.Drawing.Size(30, 30);
            this.subCellHelpButton.TabIndex = 43;
            this.subCellHelpButton.Text = "?";
            this.subCellHelpButton.UseVisualStyleBackColor = true;
            this.subCellHelpButton.Click += new System.EventHandler(this.subCellHelpButton_Click);
            // 
            // PaletteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1184, 741);
            this.Controls.Add(this.mainTabs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.replaceFileButton);
            this.Controls.Add(this.dumpFilesButton);
            this.Controls.Add(this.saveIconPaletteButton);
            this.Controls.Add(this.dumpFilesPIDNumberBox);
            this.Controls.Add(this.extractFileButton);
            this.Controls.Add(this.setIconPaletteButton);
            this.Controls.Add(this.fileIDDropdown);
            this.Controls.Add(this.iconTypeDropdown);
            this.Controls.Add(this.importIconButton);
            this.Controls.Add(this.pokemonIconBox);
            this.Controls.Add(this.saveIconButton);
            this.Controls.Add(this.paletteIDNumberBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PaletteEditor";
            this.Text = "Sprite Editor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PaletteEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonIconBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paletteIDNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dumpFilesPIDNumberBox)).EndInit();
            this.mainTabs.ResumeLayout(false);
            this.spritesTab.ResumeLayout(false);
            this.spritesTab.PerformLayout();
            this.rigCellsTab.ResumeLayout(false);
            this.rigCellsTab.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cellPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellSpriteYNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellSpriteXNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellHeightNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellWidthNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellYNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellXNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rigCellsRender)).EndInit();
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
        private System.Windows.Forms.Button dumpFilesButton;
        private System.Windows.Forms.NumericUpDown dumpFilesPIDNumberBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl mainTabs;
        private System.Windows.Forms.TabPage spritesTab;
        private System.Windows.Forms.TabPage rigCellsTab;
        private System.Windows.Forms.PictureBox rigCellsRender;
        private System.Windows.Forms.RadioButton backRigCheckbox;
        private System.Windows.Forms.RadioButton frontRigCheckBox;
        private System.Windows.Forms.Button applyRigCellButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown cellXNumberBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown cellYNumberBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown cellSpriteYNumberBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown cellSpriteXNumberBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown cellHeightNumberBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown cellWidthNumberBox;
        private System.Windows.Forms.PictureBox cellPreview;
        private System.Windows.Forms.Label selectedCellText;
        private System.Windows.Forms.RadioButton femaleRigCheckBox;
        private System.Windows.Forms.RadioButton maleRigCheckBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox rigFlagsTextBox;
        private System.Windows.Forms.Button subCellHelpButton;
        private System.Windows.Forms.Button addSubCellButton;
    }
}