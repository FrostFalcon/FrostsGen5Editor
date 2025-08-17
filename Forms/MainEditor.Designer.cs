
namespace NewEditor.Forms
{
    partial class MainEditor
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
            this.components = new System.ComponentModel.Container();
            this.openRomButton = new System.Windows.Forms.Button();
            this.romTypeText = new System.Windows.Forms.Label();
            this.romNameText = new System.Windows.Forms.Label();
            this.saveRomButton = new System.Windows.Forms.Button();
            this.openTextViewerButton = new System.Windows.Forms.Button();
            this.openPokemonEditorButton = new System.Windows.Forms.Button();
            this.openOverworldEditorButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.littleCupButton = new System.Windows.Forms.Button();
            this.rotationBattleButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.stsFormatDropdown = new System.Windows.Forms.ComboBox();
            this.tripleBattleButton = new System.Windows.Forms.Button();
            this.rogueModeButton = new System.Windows.Forms.Button();
            this.openPresetMoveEditorButton = new System.Windows.Forms.Button();
            this.openTypeSwapEditorButton = new System.Windows.Forms.Button();
            this.typeShuffleButton = new System.Windows.Forms.Button();
            this.openMoveEditorButton = new System.Windows.Forms.Button();
            this.openScriptEditorButton = new System.Windows.Forms.Button();
            this.openTrainerEditorButton = new System.Windows.Forms.Button();
            this.openEncounterEditorButton = new System.Windows.Forms.Button();
            this.taskProgressBar = new System.Windows.Forms.ProgressBar();
            this.replaceNarcButton = new System.Windows.Forms.Button();
            this.narcToReplaceNumberBox = new System.Windows.Forms.NumericUpDown();
            this.autoLoadButton = new System.Windows.Forms.Button();
            this.createPatchButton = new System.Windows.Forms.Button();
            this.applyPatchButton = new System.Windows.Forms.Button();
            this.reducePatchSizeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.selectivePatchTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.selectivePatchTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.openShopEditorButton = new System.Windows.Forms.Button();
            this.replaceSoundButton = new System.Windows.Forms.Button();
            this.replaceIconButton = new System.Windows.Forms.Button();
            this.replaceSoundID = new System.Windows.Forms.NumericUpDown();
            this.replaceIconID = new System.Windows.Forms.NumericUpDown();
            this.openOverlayEditorButton = new System.Windows.Forms.Button();
            this.dumpRomButton = new System.Windows.Forms.Button();
            this.replaceOverlayID = new System.Windows.Forms.NumericUpDown();
            this.replaceOverlayButton = new System.Windows.Forms.Button();
            this.loadFromFolderButton = new System.Windows.Forms.Button();
            this.pokepatcherButton = new System.Windows.Forms.Button();
            this.typeChartEditorButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OpenFileExplorerButton = new System.Windows.Forms.Button();
            this.openGrottoEditorButton = new System.Windows.Forms.Button();
            this.openXPCurveEditorButton = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.replaceMapID = new System.Windows.Forms.NumericUpDown();
            this.replaceMapButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.themeDropdown = new System.Windows.Forms.ComboBox();
            this.statusText = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.narcToReplaceNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.replaceSoundID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.replaceIconID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.replaceOverlayID)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.replaceMapID)).BeginInit();
            this.SuspendLayout();
            // 
            // openRomButton
            // 
            this.openRomButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.openRomButton.Location = new System.Drawing.Point(552, 14);
            this.openRomButton.Name = "openRomButton";
            this.openRomButton.Size = new System.Drawing.Size(100, 40);
            this.openRomButton.TabIndex = 1;
            this.openRomButton.Text = "Open Rom";
            this.openRomButton.UseVisualStyleBackColor = true;
            this.openRomButton.Click += new System.EventHandler(this.OpenRomButton);
            // 
            // romTypeText
            // 
            this.romTypeText.AutoSize = true;
            this.romTypeText.Font = new System.Drawing.Font("Arial", 9.75F);
            this.romTypeText.Location = new System.Drawing.Point(12, 38);
            this.romTypeText.Name = "romTypeText";
            this.romTypeText.Size = new System.Drawing.Size(73, 16);
            this.romTypeText.TabIndex = 4;
            this.romTypeText.Text = "Rom Type: ";
            // 
            // romNameText
            // 
            this.romNameText.AutoSize = true;
            this.romNameText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.romNameText.Location = new System.Drawing.Point(12, 12);
            this.romNameText.MaximumSize = new System.Drawing.Size(600, 20);
            this.romNameText.Name = "romNameText";
            this.romNameText.Size = new System.Drawing.Size(42, 16);
            this.romNameText.TabIndex = 3;
            this.romNameText.Text = "Rom: ";
            // 
            // saveRomButton
            // 
            this.saveRomButton.Enabled = false;
            this.saveRomButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.saveRomButton.Location = new System.Drawing.Point(552, 64);
            this.saveRomButton.Name = "saveRomButton";
            this.saveRomButton.Size = new System.Drawing.Size(100, 40);
            this.saveRomButton.TabIndex = 6;
            this.saveRomButton.Text = "Save Rom";
            this.saveRomButton.UseVisualStyleBackColor = true;
            this.saveRomButton.Click += new System.EventHandler(this.SaveRomButton);
            // 
            // openTextViewerButton
            // 
            this.openTextViewerButton.Location = new System.Drawing.Point(10, 17);
            this.openTextViewerButton.Name = "openTextViewerButton";
            this.openTextViewerButton.Size = new System.Drawing.Size(120, 32);
            this.openTextViewerButton.TabIndex = 7;
            this.openTextViewerButton.Text = "Text Editor";
            this.openTextViewerButton.UseVisualStyleBackColor = true;
            this.openTextViewerButton.Click += new System.EventHandler(this.OpenTextViewer);
            // 
            // openPokemonEditorButton
            // 
            this.openPokemonEditorButton.Location = new System.Drawing.Point(10, 95);
            this.openPokemonEditorButton.Name = "openPokemonEditorButton";
            this.openPokemonEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openPokemonEditorButton.TabIndex = 8;
            this.openPokemonEditorButton.Text = "Pokemon Editor";
            this.openPokemonEditorButton.UseVisualStyleBackColor = true;
            this.openPokemonEditorButton.Click += new System.EventHandler(this.OpenPokemonEditor);
            // 
            // openOverworldEditorButton
            // 
            this.openOverworldEditorButton.Location = new System.Drawing.Point(136, 135);
            this.openOverworldEditorButton.Name = "openOverworldEditorButton";
            this.openOverworldEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openOverworldEditorButton.TabIndex = 9;
            this.openOverworldEditorButton.Text = "Overworld Editor";
            this.openOverworldEditorButton.UseVisualStyleBackColor = true;
            this.openOverworldEditorButton.Click += new System.EventHandler(this.OpenOverworldEditor);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.littleCupButton);
            this.groupBox1.Controls.Add(this.rotationBattleButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.stsFormatDropdown);
            this.groupBox1.Controls.Add(this.tripleBattleButton);
            this.groupBox1.Controls.Add(this.rogueModeButton);
            this.groupBox1.Controls.Add(this.openPresetMoveEditorButton);
            this.groupBox1.Controls.Add(this.openTypeSwapEditorButton);
            this.groupBox1.Location = new System.Drawing.Point(508, 249);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 240);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom Game Modes";
            // 
            // littleCupButton
            // 
            this.littleCupButton.Location = new System.Drawing.Point(150, 110);
            this.littleCupButton.Name = "littleCupButton";
            this.littleCupButton.Size = new System.Drawing.Size(100, 32);
            this.littleCupButton.TabIndex = 20;
            this.littleCupButton.Text = "Little Cup";
            this.littleCupButton.UseVisualStyleBackColor = true;
            this.littleCupButton.Click += new System.EventHandler(this.littleCupButton_Click);
            // 
            // rotationBattleButton
            // 
            this.rotationBattleButton.Location = new System.Drawing.Point(15, 70);
            this.rotationBattleButton.Name = "rotationBattleButton";
            this.rotationBattleButton.Size = new System.Drawing.Size(120, 32);
            this.rotationBattleButton.TabIndex = 18;
            this.rotationBattleButton.Text = "Rotation Battle";
            this.rotationBattleButton.UseVisualStyleBackColor = true;
            this.rotationBattleButton.Click += new System.EventHandler(this.rotationBattleButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "StS Format:";
            // 
            // stsFormatDropdown
            // 
            this.stsFormatDropdown.FormattingEnabled = true;
            this.stsFormatDropdown.Items.AddRange(new object[] {
            "Single",
            "Double",
            "Triple",
            "Rotation"});
            this.stsFormatDropdown.Location = new System.Drawing.Point(130, 156);
            this.stsFormatDropdown.Name = "stsFormatDropdown";
            this.stsFormatDropdown.Size = new System.Drawing.Size(120, 24);
            this.stsFormatDropdown.TabIndex = 16;
            // 
            // tripleBattleButton
            // 
            this.tripleBattleButton.Location = new System.Drawing.Point(150, 70);
            this.tripleBattleButton.Name = "tripleBattleButton";
            this.tripleBattleButton.Size = new System.Drawing.Size(100, 32);
            this.tripleBattleButton.TabIndex = 15;
            this.tripleBattleButton.Text = "Triple Battle";
            this.tripleBattleButton.UseVisualStyleBackColor = true;
            this.tripleBattleButton.Click += new System.EventHandler(this.tripleBattleButton_Click);
            // 
            // rogueModeButton
            // 
            this.rogueModeButton.Location = new System.Drawing.Point(130, 190);
            this.rogueModeButton.Name = "rogueModeButton";
            this.rogueModeButton.Size = new System.Drawing.Size(120, 32);
            this.rogueModeButton.TabIndex = 14;
            this.rogueModeButton.Text = "Slay the Spoink";
            this.rogueModeButton.UseVisualStyleBackColor = true;
            this.rogueModeButton.Click += new System.EventHandler(this.rogueModeButton_Click);
            // 
            // openPresetMoveEditorButton
            // 
            this.openPresetMoveEditorButton.Location = new System.Drawing.Point(150, 30);
            this.openPresetMoveEditorButton.Name = "openPresetMoveEditorButton";
            this.openPresetMoveEditorButton.Size = new System.Drawing.Size(100, 32);
            this.openPresetMoveEditorButton.TabIndex = 12;
            this.openPresetMoveEditorButton.Text = "Preset Moves";
            this.openPresetMoveEditorButton.UseVisualStyleBackColor = true;
            this.openPresetMoveEditorButton.Click += new System.EventHandler(this.OpenPresetMoveEditor);
            // 
            // openTypeSwapEditorButton
            // 
            this.openTypeSwapEditorButton.Location = new System.Drawing.Point(15, 30);
            this.openTypeSwapEditorButton.Name = "openTypeSwapEditorButton";
            this.openTypeSwapEditorButton.Size = new System.Drawing.Size(100, 32);
            this.openTypeSwapEditorButton.TabIndex = 11;
            this.openTypeSwapEditorButton.Text = "Type Swap";
            this.openTypeSwapEditorButton.UseVisualStyleBackColor = true;
            this.openTypeSwapEditorButton.Click += new System.EventHandler(this.OpenTypeSwapEditor);
            // 
            // typeShuffleButton
            // 
            this.typeShuffleButton.Location = new System.Drawing.Point(11, 54);
            this.typeShuffleButton.Name = "typeShuffleButton";
            this.typeShuffleButton.Size = new System.Drawing.Size(100, 32);
            this.typeShuffleButton.TabIndex = 19;
            this.typeShuffleButton.Text = "Type Shuffle";
            this.typeShuffleButton.UseVisualStyleBackColor = true;
            this.typeShuffleButton.Visible = false;
            this.typeShuffleButton.Click += new System.EventHandler(this.ApplyTypeShuffle);
            // 
            // openMoveEditorButton
            // 
            this.openMoveEditorButton.Location = new System.Drawing.Point(10, 55);
            this.openMoveEditorButton.Name = "openMoveEditorButton";
            this.openMoveEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openMoveEditorButton.TabIndex = 11;
            this.openMoveEditorButton.Text = "Move Editor";
            this.openMoveEditorButton.UseVisualStyleBackColor = true;
            this.openMoveEditorButton.Click += new System.EventHandler(this.OpenMoveEditor);
            // 
            // openScriptEditorButton
            // 
            this.openScriptEditorButton.Location = new System.Drawing.Point(136, 95);
            this.openScriptEditorButton.Name = "openScriptEditorButton";
            this.openScriptEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openScriptEditorButton.TabIndex = 12;
            this.openScriptEditorButton.Text = "Script Editor";
            this.openScriptEditorButton.UseVisualStyleBackColor = true;
            this.openScriptEditorButton.Click += new System.EventHandler(this.OpenScriptEditor);
            // 
            // openTrainerEditorButton
            // 
            this.openTrainerEditorButton.Location = new System.Drawing.Point(10, 135);
            this.openTrainerEditorButton.Name = "openTrainerEditorButton";
            this.openTrainerEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openTrainerEditorButton.TabIndex = 14;
            this.openTrainerEditorButton.Text = "Trainer Editor";
            this.openTrainerEditorButton.UseVisualStyleBackColor = true;
            this.openTrainerEditorButton.Click += new System.EventHandler(this.OpenTrainerEditor);
            // 
            // openEncounterEditorButton
            // 
            this.openEncounterEditorButton.Location = new System.Drawing.Point(262, 135);
            this.openEncounterEditorButton.Name = "openEncounterEditorButton";
            this.openEncounterEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openEncounterEditorButton.TabIndex = 15;
            this.openEncounterEditorButton.Text = "Encounter Editor";
            this.openEncounterEditorButton.UseVisualStyleBackColor = true;
            this.openEncounterEditorButton.Click += new System.EventHandler(this.OpenEncounterEditor);
            // 
            // taskProgressBar
            // 
            this.taskProgressBar.Location = new System.Drawing.Point(100, 72);
            this.taskProgressBar.Name = "taskProgressBar";
            this.taskProgressBar.Size = new System.Drawing.Size(120, 25);
            this.taskProgressBar.TabIndex = 17;
            this.taskProgressBar.Visible = false;
            // 
            // replaceNarcButton
            // 
            this.replaceNarcButton.Location = new System.Drawing.Point(263, 184);
            this.replaceNarcButton.Name = "replaceNarcButton";
            this.replaceNarcButton.Size = new System.Drawing.Size(120, 32);
            this.replaceNarcButton.TabIndex = 14;
            this.replaceNarcButton.Text = "Replace NARC";
            this.replaceNarcButton.UseVisualStyleBackColor = true;
            this.replaceNarcButton.Click += new System.EventHandler(this.replaceNarcButton_Click);
            // 
            // narcToReplaceNumberBox
            // 
            this.narcToReplaceNumberBox.Location = new System.Drawing.Point(183, 190);
            this.narcToReplaceNumberBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.narcToReplaceNumberBox.Name = "narcToReplaceNumberBox";
            this.narcToReplaceNumberBox.Size = new System.Drawing.Size(60, 22);
            this.narcToReplaceNumberBox.TabIndex = 18;
            // 
            // autoLoadButton
            // 
            this.autoLoadButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.autoLoadButton.Location = new System.Drawing.Point(602, 119);
            this.autoLoadButton.Name = "autoLoadButton";
            this.autoLoadButton.Size = new System.Drawing.Size(130, 35);
            this.autoLoadButton.TabIndex = 19;
            this.autoLoadButton.Text = "Enable Auto Load";
            this.autoLoadButton.UseVisualStyleBackColor = true;
            this.autoLoadButton.Click += new System.EventHandler(this.EnableAutoLoad);
            // 
            // createPatchButton
            // 
            this.createPatchButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.createPatchButton.Location = new System.Drawing.Point(10, 21);
            this.createPatchButton.Name = "createPatchButton";
            this.createPatchButton.Size = new System.Drawing.Size(100, 40);
            this.createPatchButton.TabIndex = 20;
            this.createPatchButton.Text = "Create Patch";
            this.createPatchButton.UseVisualStyleBackColor = true;
            this.createPatchButton.Click += new System.EventHandler(this.createPatchButton_Click);
            // 
            // applyPatchButton
            // 
            this.applyPatchButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.applyPatchButton.Location = new System.Drawing.Point(10, 67);
            this.applyPatchButton.Name = "applyPatchButton";
            this.applyPatchButton.Size = new System.Drawing.Size(100, 40);
            this.applyPatchButton.TabIndex = 21;
            this.applyPatchButton.Text = "Apply Patch";
            this.applyPatchButton.UseVisualStyleBackColor = true;
            this.applyPatchButton.Click += new System.EventHandler(this.applyPatchButton_Click);
            // 
            // selectivePatchTextBox
            // 
            this.selectivePatchTextBox.Enabled = false;
            this.selectivePatchTextBox.Location = new System.Drawing.Point(120, 21);
            this.selectivePatchTextBox.Name = "selectivePatchTextBox";
            this.selectivePatchTextBox.Size = new System.Drawing.Size(100, 22);
            this.selectivePatchTextBox.TabIndex = 23;
            this.selectivePatchTooltip.SetToolTip(this.selectivePatchTextBox, "By entering numbers separated by a space, a patch file will be created using only" +
        " the NARC files listed.");
            this.selectivePatchTextBox.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F);
            this.label2.Location = new System.Drawing.Point(16, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "Selective Patch:";
            this.label2.Visible = false;
            // 
            // openShopEditorButton
            // 
            this.openShopEditorButton.Location = new System.Drawing.Point(136, 55);
            this.openShopEditorButton.Name = "openShopEditorButton";
            this.openShopEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openShopEditorButton.TabIndex = 26;
            this.openShopEditorButton.Text = "Shop Editor";
            this.openShopEditorButton.UseVisualStyleBackColor = true;
            this.openShopEditorButton.Click += new System.EventHandler(this.OpenShopEditor);
            // 
            // replaceSoundButton
            // 
            this.replaceSoundButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.replaceSoundButton.Location = new System.Drawing.Point(263, 148);
            this.replaceSoundButton.Name = "replaceSoundButton";
            this.replaceSoundButton.Size = new System.Drawing.Size(120, 30);
            this.replaceSoundButton.TabIndex = 29;
            this.replaceSoundButton.Text = "Replace Sound";
            this.replaceSoundButton.UseVisualStyleBackColor = true;
            this.replaceSoundButton.Click += new System.EventHandler(this.replaceSoundButton_Click);
            // 
            // replaceIconButton
            // 
            this.replaceIconButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.replaceIconButton.Location = new System.Drawing.Point(263, 112);
            this.replaceIconButton.Name = "replaceIconButton";
            this.replaceIconButton.Size = new System.Drawing.Size(120, 30);
            this.replaceIconButton.TabIndex = 30;
            this.replaceIconButton.Text = "Replace Icon";
            this.replaceIconButton.UseVisualStyleBackColor = true;
            this.replaceIconButton.Click += new System.EventHandler(this.replaceIconButton_Click);
            // 
            // replaceSoundID
            // 
            this.replaceSoundID.Location = new System.Drawing.Point(183, 152);
            this.replaceSoundID.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.replaceSoundID.Name = "replaceSoundID";
            this.replaceSoundID.Size = new System.Drawing.Size(60, 22);
            this.replaceSoundID.TabIndex = 31;
            // 
            // replaceIconID
            // 
            this.replaceIconID.Location = new System.Drawing.Point(183, 117);
            this.replaceIconID.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.replaceIconID.Name = "replaceIconID";
            this.replaceIconID.Size = new System.Drawing.Size(60, 22);
            this.replaceIconID.TabIndex = 32;
            // 
            // openOverlayEditorButton
            // 
            this.openOverlayEditorButton.Enabled = false;
            this.openOverlayEditorButton.Location = new System.Drawing.Point(11, 138);
            this.openOverlayEditorButton.Name = "openOverlayEditorButton";
            this.openOverlayEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openOverlayEditorButton.TabIndex = 33;
            this.openOverlayEditorButton.Text = "Arm Editor";
            this.openOverlayEditorButton.UseVisualStyleBackColor = true;
            this.openOverlayEditorButton.Visible = false;
            this.openOverlayEditorButton.Click += new System.EventHandler(this.openOverlayEditorButton_Click);
            // 
            // dumpRomButton
            // 
            this.dumpRomButton.Enabled = false;
            this.dumpRomButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.dumpRomButton.Location = new System.Drawing.Point(672, 64);
            this.dumpRomButton.Name = "dumpRomButton";
            this.dumpRomButton.Size = new System.Drawing.Size(100, 40);
            this.dumpRomButton.TabIndex = 34;
            this.dumpRomButton.Text = "Dump Rom";
            this.dumpRomButton.UseVisualStyleBackColor = true;
            this.dumpRomButton.Click += new System.EventHandler(this.dumpRomButton_Click);
            // 
            // replaceOverlayID
            // 
            this.replaceOverlayID.Location = new System.Drawing.Point(183, 81);
            this.replaceOverlayID.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.replaceOverlayID.Name = "replaceOverlayID";
            this.replaceOverlayID.Size = new System.Drawing.Size(60, 22);
            this.replaceOverlayID.TabIndex = 36;
            // 
            // replaceOverlayButton
            // 
            this.replaceOverlayButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.replaceOverlayButton.Location = new System.Drawing.Point(263, 76);
            this.replaceOverlayButton.Name = "replaceOverlayButton";
            this.replaceOverlayButton.Size = new System.Drawing.Size(120, 30);
            this.replaceOverlayButton.TabIndex = 35;
            this.replaceOverlayButton.Text = "Replace Overlay";
            this.replaceOverlayButton.UseVisualStyleBackColor = true;
            this.replaceOverlayButton.Click += new System.EventHandler(this.replaceOverlayButton_Click);
            // 
            // loadFromFolderButton
            // 
            this.loadFromFolderButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.loadFromFolderButton.Location = new System.Drawing.Point(672, 14);
            this.loadFromFolderButton.Name = "loadFromFolderButton";
            this.loadFromFolderButton.Size = new System.Drawing.Size(100, 40);
            this.loadFromFolderButton.TabIndex = 37;
            this.loadFromFolderButton.Text = "Load Folder";
            this.loadFromFolderButton.UseVisualStyleBackColor = true;
            this.loadFromFolderButton.Click += new System.EventHandler(this.LoadRomFromFolder);
            // 
            // pokepatcherButton
            // 
            this.pokepatcherButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.pokepatcherButton.Location = new System.Drawing.Point(11, 92);
            this.pokepatcherButton.Name = "pokepatcherButton";
            this.pokepatcherButton.Size = new System.Drawing.Size(100, 40);
            this.pokepatcherButton.TabIndex = 38;
            this.pokepatcherButton.Text = "Pokepatcher";
            this.pokepatcherButton.UseVisualStyleBackColor = true;
            this.pokepatcherButton.Visible = false;
            this.pokepatcherButton.Click += new System.EventHandler(this.pokepatherButton_Click);
            // 
            // typeChartEditorButton
            // 
            this.typeChartEditorButton.Location = new System.Drawing.Point(11, 181);
            this.typeChartEditorButton.Name = "typeChartEditorButton";
            this.typeChartEditorButton.Size = new System.Drawing.Size(120, 32);
            this.typeChartEditorButton.TabIndex = 39;
            this.typeChartEditorButton.Text = "Type Chart Editor";
            this.typeChartEditorButton.UseVisualStyleBackColor = true;
            this.typeChartEditorButton.Visible = false;
            this.typeChartEditorButton.Click += new System.EventHandler(this.typeChartEditorButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Controls.Add(this.createPatchButton);
            this.groupBox2.Controls.Add(this.applyPatchButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 179);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(120, 119);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Patching";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Controls.Add(this.OpenFileExplorerButton);
            this.groupBox3.Controls.Add(this.openGrottoEditorButton);
            this.groupBox3.Controls.Add(this.openXPCurveEditorButton);
            this.groupBox3.Controls.Add(this.openTextViewerButton);
            this.groupBox3.Controls.Add(this.openPokemonEditorButton);
            this.groupBox3.Controls.Add(this.openMoveEditorButton);
            this.groupBox3.Controls.Add(this.openTrainerEditorButton);
            this.groupBox3.Controls.Add(this.openScriptEditorButton);
            this.groupBox3.Controls.Add(this.openOverworldEditorButton);
            this.groupBox3.Controls.Add(this.openEncounterEditorButton);
            this.groupBox3.Controls.Add(this.openShopEditorButton);
            this.groupBox3.Location = new System.Drawing.Point(12, 304);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(394, 185);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Editors";
            // 
            // OpenFileExplorerButton
            // 
            this.OpenFileExplorerButton.Location = new System.Drawing.Point(136, 17);
            this.OpenFileExplorerButton.Name = "OpenFileExplorerButton";
            this.OpenFileExplorerButton.Size = new System.Drawing.Size(120, 32);
            this.OpenFileExplorerButton.TabIndex = 29;
            this.OpenFileExplorerButton.Text = "File Explorer";
            this.OpenFileExplorerButton.UseVisualStyleBackColor = true;
            this.OpenFileExplorerButton.Click += new System.EventHandler(this.OpenFileExplorer);
            // 
            // openGrottoEditorButton
            // 
            this.openGrottoEditorButton.Location = new System.Drawing.Point(262, 95);
            this.openGrottoEditorButton.Name = "openGrottoEditorButton";
            this.openGrottoEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openGrottoEditorButton.TabIndex = 28;
            this.openGrottoEditorButton.Text = "Grotto Editor";
            this.openGrottoEditorButton.UseVisualStyleBackColor = true;
            this.openGrottoEditorButton.Click += new System.EventHandler(this.OpenHiddenGrottoEditor);
            // 
            // openXPCurveEditorButton
            // 
            this.openXPCurveEditorButton.Location = new System.Drawing.Point(262, 55);
            this.openXPCurveEditorButton.Name = "openXPCurveEditorButton";
            this.openXPCurveEditorButton.Size = new System.Drawing.Size(120, 32);
            this.openXPCurveEditorButton.TabIndex = 27;
            this.openXPCurveEditorButton.Text = "Exp Curve Editor";
            this.openXPCurveEditorButton.UseVisualStyleBackColor = true;
            this.openXPCurveEditorButton.Click += new System.EventHandler(this.openXPCurveEditorButton_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox4.Controls.Add(this.replaceMapID);
            this.groupBox4.Controls.Add(this.typeShuffleButton);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.replaceMapButton);
            this.groupBox4.Controls.Add(this.selectivePatchTextBox);
            this.groupBox4.Controls.Add(this.pokepatcherButton);
            this.groupBox4.Controls.Add(this.replaceNarcButton);
            this.groupBox4.Controls.Add(this.openOverlayEditorButton);
            this.groupBox4.Controls.Add(this.taskProgressBar);
            this.groupBox4.Controls.Add(this.narcToReplaceNumberBox);
            this.groupBox4.Controls.Add(this.typeChartEditorButton);
            this.groupBox4.Controls.Add(this.replaceSoundButton);
            this.groupBox4.Controls.Add(this.replaceIconButton);
            this.groupBox4.Controls.Add(this.replaceOverlayID);
            this.groupBox4.Controls.Add(this.replaceSoundID);
            this.groupBox4.Controls.Add(this.replaceOverlayButton);
            this.groupBox4.Controls.Add(this.replaceIconID);
            this.groupBox4.Location = new System.Drawing.Point(191, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(389, 229);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Experimental (BW2 Only)";
            this.groupBox4.Visible = false;
            // 
            // replaceMapID
            // 
            this.replaceMapID.Location = new System.Drawing.Point(183, 45);
            this.replaceMapID.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.replaceMapID.Name = "replaceMapID";
            this.replaceMapID.Size = new System.Drawing.Size(60, 22);
            this.replaceMapID.TabIndex = 41;
            // 
            // replaceMapButton
            // 
            this.replaceMapButton.Font = new System.Drawing.Font("Arial", 9.75F);
            this.replaceMapButton.Location = new System.Drawing.Point(263, 40);
            this.replaceMapButton.Name = "replaceMapButton";
            this.replaceMapButton.Size = new System.Drawing.Size(120, 30);
            this.replaceMapButton.TabIndex = 40;
            this.replaceMapButton.Text = "Replace Map";
            this.replaceMapButton.UseVisualStyleBackColor = true;
            this.replaceMapButton.Click += new System.EventHandler(this.replaceMapButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.label1.Location = new System.Drawing.Point(12, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 43;
            this.label1.Text = "Theme: ";
            // 
            // themeDropdown
            // 
            this.themeDropdown.FormattingEnabled = true;
            this.themeDropdown.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.themeDropdown.Location = new System.Drawing.Point(65, 65);
            this.themeDropdown.Name = "themeDropdown";
            this.themeDropdown.Size = new System.Drawing.Size(120, 24);
            this.themeDropdown.TabIndex = 21;
            this.themeDropdown.SelectedIndexChanged += new System.EventHandler(this.ChangeTheme);
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(9, 496);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(113, 16);
            this.statusText.TabIndex = 44;
            this.statusText.Text = "Ready to load rom";
            // 
            // MainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(784, 521);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.themeDropdown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.loadFromFolderButton);
            this.Controls.Add(this.dumpRomButton);
            this.Controls.Add(this.autoLoadButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.saveRomButton);
            this.Controls.Add(this.romTypeText);
            this.Controls.Add(this.romNameText);
            this.Controls.Add(this.openRomButton);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainEditor";
            this.Text = "Frost\'s Ultimate Gen 5 Editor 4.0";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.narcToReplaceNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.replaceSoundID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.replaceIconID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.replaceOverlayID)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.replaceMapID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openRomButton;
        private System.Windows.Forms.Label romTypeText;
        private System.Windows.Forms.Label romNameText;
        private System.Windows.Forms.Button saveRomButton;
        private System.Windows.Forms.Button openTextViewerButton;
        private System.Windows.Forms.Button openPokemonEditorButton;
        private System.Windows.Forms.Button openOverworldEditorButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button openTypeSwapEditorButton;
        private System.Windows.Forms.Button openMoveEditorButton;
        private System.Windows.Forms.Button openScriptEditorButton;
        private System.Windows.Forms.Button openPresetMoveEditorButton;
        private System.Windows.Forms.Button openTrainerEditorButton;
        private System.Windows.Forms.Button openEncounterEditorButton;
        public System.Windows.Forms.ProgressBar taskProgressBar;
        private System.Windows.Forms.Button replaceNarcButton;
        private System.Windows.Forms.NumericUpDown narcToReplaceNumberBox;
        private System.Windows.Forms.Button autoLoadButton;
        private System.Windows.Forms.Button createPatchButton;
        private System.Windows.Forms.Button applyPatchButton;
        private System.Windows.Forms.ToolTip reducePatchSizeToolTip;
        private System.Windows.Forms.Button rogueModeButton;
        private System.Windows.Forms.Button tripleBattleButton;
        private System.Windows.Forms.TextBox selectivePatchTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip selectivePatchTooltip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox stsFormatDropdown;
        private System.Windows.Forms.Button openShopEditorButton;
        private System.Windows.Forms.Button rotationBattleButton;
        private System.Windows.Forms.Button replaceSoundButton;
        private System.Windows.Forms.Button replaceIconButton;
        private System.Windows.Forms.NumericUpDown replaceSoundID;
        private System.Windows.Forms.NumericUpDown replaceIconID;
        private System.Windows.Forms.Button openOverlayEditorButton;
        private System.Windows.Forms.Button typeShuffleButton;
        private System.Windows.Forms.Button dumpRomButton;
        private System.Windows.Forms.NumericUpDown replaceOverlayID;
        private System.Windows.Forms.Button replaceOverlayButton;
        private System.Windows.Forms.Button loadFromFolderButton;
        private System.Windows.Forms.Button pokepatcherButton;
        private System.Windows.Forms.Button typeChartEditorButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown replaceMapID;
        private System.Windows.Forms.Button replaceMapButton;
        private System.Windows.Forms.Button littleCupButton;
        private System.Windows.Forms.Button openXPCurveEditorButton;
        private System.Windows.Forms.Button openGrottoEditorButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox themeDropdown;
        private System.Windows.Forms.Button OpenFileExplorerButton;
        private System.Windows.Forms.Label statusText;
    }
}

