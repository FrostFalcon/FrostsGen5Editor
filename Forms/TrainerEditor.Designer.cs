
namespace NewEditor.Forms
{
    partial class TrainerEditor
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
            this.trainerNameDropdown = new System.Windows.Forms.ComboBox();
            this.trainerDataGroup = new System.Windows.Forms.GroupBox();
            this.editTrainerAIButton = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.aiNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.prizeMoneyNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.prizeItemDropdown = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.item4Dropdown = new System.Windows.Forms.ComboBox();
            this.item3Dropdown = new System.Windows.Forms.ComboBox();
            this.item2Dropdown = new System.Windows.Forms.ComboBox();
            this.item1Dropdown = new System.Windows.Forms.ComboBox();
            this.numPokemonBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.battleTypeDropdown = new System.Windows.Forms.ComboBox();
            this.healerCheckBox = new System.Windows.Forms.CheckBox();
            this.uniqueMovesCheckBox = new System.Windows.Forms.CheckBox();
            this.heldItemsCheckBox = new System.Windows.Forms.CheckBox();
            this.pokemonGroupBox = new System.Windows.Forms.GroupBox();
            this.movePokemonDownButton = new System.Windows.Forms.Button();
            this.movePokemonUpButton = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.pokemonHeldItemDropdown = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pokemonAbilityDropdown = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pokemonGenderDropdown = new System.Windows.Forms.ComboBox();
            this.pokemonIVsNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pokemonLevelNumberBox = new System.Windows.Forms.NumericUpDown();
            this.pokemonMove4Dropdown = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pokemonMove3Dropdown = new System.Windows.Forms.ComboBox();
            this.pokemonFormNumberBox = new System.Windows.Forms.NumericUpDown();
            this.pokemonMove2Dropdown = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pokemonMove1Dropdown = new System.Windows.Forms.ComboBox();
            this.pokemonListBox = new System.Windows.Forms.ListBox();
            this.pokemonIDDropdown = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            this.addTrainerButton = new System.Windows.Forms.Button();
            this.dialogueGroup = new System.Windows.Forms.GroupBox();
            this.trDialogueTextBox = new System.Windows.Forms.TextBox();
            this.dialogueTypeDropdown = new System.Windows.Forms.ComboBox();
            this.trainerNameTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.trainerClassDropdown = new System.Windows.Forms.ComboBox();
            this.trainerDataGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aiNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.prizeMoneyNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPokemonBox)).BeginInit();
            this.pokemonGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonIVsNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonLevelNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonFormNumberBox)).BeginInit();
            this.dialogueGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // trainerNameDropdown
            // 
            this.trainerNameDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.trainerNameDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.trainerNameDropdown.FormattingEnabled = true;
            this.trainerNameDropdown.Location = new System.Drawing.Point(12, 12);
            this.trainerNameDropdown.Name = "trainerNameDropdown";
            this.trainerNameDropdown.Size = new System.Drawing.Size(200, 24);
            this.trainerNameDropdown.TabIndex = 2;
            this.trainerNameDropdown.SelectedIndexChanged += new System.EventHandler(this.LoadTrainerIntoEditor);
            // 
            // trainerDataGroup
            // 
            this.trainerDataGroup.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.trainerDataGroup.Controls.Add(this.trainerClassDropdown);
            this.trainerDataGroup.Controls.Add(this.editTrainerAIButton);
            this.trainerDataGroup.Controls.Add(this.label16);
            this.trainerDataGroup.Controls.Add(this.aiNumberBox);
            this.trainerDataGroup.Controls.Add(this.label6);
            this.trainerDataGroup.Controls.Add(this.prizeMoneyNumberBox);
            this.trainerDataGroup.Controls.Add(this.label5);
            this.trainerDataGroup.Controls.Add(this.label4);
            this.trainerDataGroup.Controls.Add(this.prizeItemDropdown);
            this.trainerDataGroup.Controls.Add(this.label3);
            this.trainerDataGroup.Controls.Add(this.item4Dropdown);
            this.trainerDataGroup.Controls.Add(this.item3Dropdown);
            this.trainerDataGroup.Controls.Add(this.item2Dropdown);
            this.trainerDataGroup.Controls.Add(this.item1Dropdown);
            this.trainerDataGroup.Controls.Add(this.numPokemonBox);
            this.trainerDataGroup.Controls.Add(this.label2);
            this.trainerDataGroup.Controls.Add(this.label1);
            this.trainerDataGroup.Controls.Add(this.battleTypeDropdown);
            this.trainerDataGroup.Controls.Add(this.healerCheckBox);
            this.trainerDataGroup.Controls.Add(this.uniqueMovesCheckBox);
            this.trainerDataGroup.Controls.Add(this.heldItemsCheckBox);
            this.trainerDataGroup.Enabled = false;
            this.trainerDataGroup.Location = new System.Drawing.Point(12, 80);
            this.trainerDataGroup.Name = "trainerDataGroup";
            this.trainerDataGroup.Size = new System.Drawing.Size(420, 260);
            this.trainerDataGroup.TabIndex = 73;
            this.trainerDataGroup.TabStop = false;
            this.trainerDataGroup.Text = "Trainer Data";
            // 
            // editTrainerAIButton
            // 
            this.editTrainerAIButton.Location = new System.Drawing.Point(106, 221);
            this.editTrainerAIButton.Name = "editTrainerAIButton";
            this.editTrainerAIButton.Size = new System.Drawing.Size(24, 24);
            this.editTrainerAIButton.TabIndex = 97;
            this.editTrainerAIButton.Text = ">";
            this.editTrainerAIButton.UseVisualStyleBackColor = true;
            this.editTrainerAIButton.Click += new System.EventHandler(this.editTrainerAIButton_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(87, 16);
            this.label16.TabIndex = 84;
            this.label16.Text = "Trainer Class:";
            // 
            // aiNumberBox
            // 
            this.aiNumberBox.Location = new System.Drawing.Point(40, 222);
            this.aiNumberBox.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.aiNumberBox.Name = "aiNumberBox";
            this.aiNumberBox.Size = new System.Drawing.Size(60, 22);
            this.aiNumberBox.TabIndex = 82;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 225);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 16);
            this.label6.TabIndex = 81;
            this.label6.Text = "AI:";
            // 
            // prizeMoneyNumberBox
            // 
            this.prizeMoneyNumberBox.Location = new System.Drawing.Point(280, 222);
            this.prizeMoneyNumberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.prizeMoneyNumberBox.Name = "prizeMoneyNumberBox";
            this.prizeMoneyNumberBox.Size = new System.Drawing.Size(40, 22);
            this.prizeMoneyNumberBox.TabIndex = 80;
            this.prizeMoneyNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(188, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 16);
            this.label5.TabIndex = 79;
            this.label5.Text = "Prize Money:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(202, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 16);
            this.label4.TabIndex = 77;
            this.label4.Text = "Prize Item:";
            // 
            // prizeItemDropdown
            // 
            this.prizeItemDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.prizeItemDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.prizeItemDropdown.FormattingEnabled = true;
            this.prizeItemDropdown.Location = new System.Drawing.Point(280, 186);
            this.prizeItemDropdown.Name = "prizeItemDropdown";
            this.prizeItemDropdown.Size = new System.Drawing.Size(100, 24);
            this.prizeItemDropdown.TabIndex = 76;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(277, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 75;
            this.label3.Text = "Items:";
            // 
            // item4Dropdown
            // 
            this.item4Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.item4Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.item4Dropdown.FormattingEnabled = true;
            this.item4Dropdown.Location = new System.Drawing.Point(280, 141);
            this.item4Dropdown.Name = "item4Dropdown";
            this.item4Dropdown.Size = new System.Drawing.Size(120, 24);
            this.item4Dropdown.TabIndex = 74;
            // 
            // item3Dropdown
            // 
            this.item3Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.item3Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.item3Dropdown.FormattingEnabled = true;
            this.item3Dropdown.Location = new System.Drawing.Point(280, 111);
            this.item3Dropdown.Name = "item3Dropdown";
            this.item3Dropdown.Size = new System.Drawing.Size(120, 24);
            this.item3Dropdown.TabIndex = 73;
            // 
            // item2Dropdown
            // 
            this.item2Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.item2Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.item2Dropdown.FormattingEnabled = true;
            this.item2Dropdown.Location = new System.Drawing.Point(280, 81);
            this.item2Dropdown.Name = "item2Dropdown";
            this.item2Dropdown.Size = new System.Drawing.Size(120, 24);
            this.item2Dropdown.TabIndex = 72;
            // 
            // item1Dropdown
            // 
            this.item1Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.item1Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.item1Dropdown.FormattingEnabled = true;
            this.item1Dropdown.Location = new System.Drawing.Point(280, 51);
            this.item1Dropdown.Name = "item1Dropdown";
            this.item1Dropdown.Size = new System.Drawing.Size(120, 24);
            this.item1Dropdown.TabIndex = 71;
            // 
            // numPokemonBox
            // 
            this.numPokemonBox.Location = new System.Drawing.Point(150, 52);
            this.numPokemonBox.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numPokemonBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPokemonBox.Name = "numPokemonBox";
            this.numPokemonBox.Size = new System.Drawing.Size(40, 22);
            this.numPokemonBox.TabIndex = 57;
            this.numPokemonBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 16);
            this.label2.TabIndex = 54;
            this.label2.Text = "Number of Pokemon:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 16);
            this.label1.TabIndex = 53;
            this.label1.Text = "Battle Type:";
            // 
            // battleTypeDropdown
            // 
            this.battleTypeDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.battleTypeDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.battleTypeDropdown.FormattingEnabled = true;
            this.battleTypeDropdown.Items.AddRange(new object[] {
            "Single Battle",
            "Double Battle",
            "Triple Battle",
            "Rotation Battle"});
            this.battleTypeDropdown.Location = new System.Drawing.Point(90, 81);
            this.battleTypeDropdown.Name = "battleTypeDropdown";
            this.battleTypeDropdown.Size = new System.Drawing.Size(100, 24);
            this.battleTypeDropdown.TabIndex = 52;
            // 
            // healerCheckBox
            // 
            this.healerCheckBox.AutoSize = true;
            this.healerCheckBox.Location = new System.Drawing.Point(12, 185);
            this.healerCheckBox.Name = "healerCheckBox";
            this.healerCheckBox.Size = new System.Drawing.Size(63, 20);
            this.healerCheckBox.TabIndex = 5;
            this.healerCheckBox.Text = "Healer";
            this.healerCheckBox.UseVisualStyleBackColor = true;
            // 
            // uniqueMovesCheckBox
            // 
            this.uniqueMovesCheckBox.AutoSize = true;
            this.uniqueMovesCheckBox.Location = new System.Drawing.Point(12, 155);
            this.uniqueMovesCheckBox.Name = "uniqueMovesCheckBox";
            this.uniqueMovesCheckBox.Size = new System.Drawing.Size(107, 20);
            this.uniqueMovesCheckBox.TabIndex = 4;
            this.uniqueMovesCheckBox.Text = "Unique Moves";
            this.uniqueMovesCheckBox.UseVisualStyleBackColor = true;
            // 
            // heldItemsCheckBox
            // 
            this.heldItemsCheckBox.AutoSize = true;
            this.heldItemsCheckBox.Location = new System.Drawing.Point(12, 125);
            this.heldItemsCheckBox.Name = "heldItemsCheckBox";
            this.heldItemsCheckBox.Size = new System.Drawing.Size(88, 20);
            this.heldItemsCheckBox.TabIndex = 3;
            this.heldItemsCheckBox.Text = "Held Items";
            this.heldItemsCheckBox.UseVisualStyleBackColor = true;
            // 
            // pokemonGroupBox
            // 
            this.pokemonGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pokemonGroupBox.Controls.Add(this.movePokemonDownButton);
            this.pokemonGroupBox.Controls.Add(this.movePokemonUpButton);
            this.pokemonGroupBox.Controls.Add(this.label15);
            this.pokemonGroupBox.Controls.Add(this.pokemonHeldItemDropdown);
            this.pokemonGroupBox.Controls.Add(this.label14);
            this.pokemonGroupBox.Controls.Add(this.pokemonAbilityDropdown);
            this.pokemonGroupBox.Controls.Add(this.label13);
            this.pokemonGroupBox.Controls.Add(this.pokemonGenderDropdown);
            this.pokemonGroupBox.Controls.Add(this.pokemonIVsNumberBox);
            this.pokemonGroupBox.Controls.Add(this.label11);
            this.pokemonGroupBox.Controls.Add(this.label10);
            this.pokemonGroupBox.Controls.Add(this.pokemonLevelNumberBox);
            this.pokemonGroupBox.Controls.Add(this.pokemonMove4Dropdown);
            this.pokemonGroupBox.Controls.Add(this.label9);
            this.pokemonGroupBox.Controls.Add(this.pokemonMove3Dropdown);
            this.pokemonGroupBox.Controls.Add(this.pokemonFormNumberBox);
            this.pokemonGroupBox.Controls.Add(this.pokemonMove2Dropdown);
            this.pokemonGroupBox.Controls.Add(this.label8);
            this.pokemonGroupBox.Controls.Add(this.pokemonMove1Dropdown);
            this.pokemonGroupBox.Controls.Add(this.pokemonListBox);
            this.pokemonGroupBox.Controls.Add(this.pokemonIDDropdown);
            this.pokemonGroupBox.Enabled = false;
            this.pokemonGroupBox.Location = new System.Drawing.Point(440, 80);
            this.pokemonGroupBox.Name = "pokemonGroupBox";
            this.pokemonGroupBox.Size = new System.Drawing.Size(530, 260);
            this.pokemonGroupBox.TabIndex = 83;
            this.pokemonGroupBox.TabStop = false;
            this.pokemonGroupBox.Text = "Pokemon";
            // 
            // movePokemonDownButton
            // 
            this.movePokemonDownButton.Location = new System.Drawing.Point(50, 125);
            this.movePokemonDownButton.Name = "movePokemonDownButton";
            this.movePokemonDownButton.Size = new System.Drawing.Size(30, 20);
            this.movePokemonDownButton.TabIndex = 96;
            this.movePokemonDownButton.Text = "v";
            this.movePokemonDownButton.UseVisualStyleBackColor = true;
            this.movePokemonDownButton.Click += new System.EventHandler(this.movePokemonDownButton_Click);
            // 
            // movePokemonUpButton
            // 
            this.movePokemonUpButton.Location = new System.Drawing.Point(10, 125);
            this.movePokemonUpButton.Name = "movePokemonUpButton";
            this.movePokemonUpButton.Size = new System.Drawing.Size(30, 20);
            this.movePokemonUpButton.TabIndex = 95;
            this.movePokemonUpButton.Text = "^";
            this.movePokemonUpButton.UseVisualStyleBackColor = true;
            this.movePokemonUpButton.Click += new System.EventHandler(this.movePokemonUpButton_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(180, 219);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 16);
            this.label15.TabIndex = 93;
            this.label15.Text = "Held Item:";
            // 
            // pokemonHeldItemDropdown
            // 
            this.pokemonHeldItemDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonHeldItemDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonHeldItemDropdown.FormattingEnabled = true;
            this.pokemonHeldItemDropdown.Location = new System.Drawing.Point(250, 215);
            this.pokemonHeldItemDropdown.Name = "pokemonHeldItemDropdown";
            this.pokemonHeldItemDropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonHeldItemDropdown.TabIndex = 92;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(245, 60);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 16);
            this.label14.TabIndex = 91;
            this.label14.Text = "Ability:";
            // 
            // pokemonAbilityDropdown
            // 
            this.pokemonAbilityDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonAbilityDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonAbilityDropdown.FormattingEnabled = true;
            this.pokemonAbilityDropdown.Location = new System.Drawing.Point(295, 56);
            this.pokemonAbilityDropdown.Name = "pokemonAbilityDropdown";
            this.pokemonAbilityDropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonAbilityDropdown.TabIndex = 90;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 219);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 16);
            this.label13.TabIndex = 89;
            this.label13.Text = "Gender:";
            // 
            // pokemonGenderDropdown
            // 
            this.pokemonGenderDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonGenderDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonGenderDropdown.FormattingEnabled = true;
            this.pokemonGenderDropdown.Items.AddRange(new object[] {
            "Any",
            "Male",
            "Female"});
            this.pokemonGenderDropdown.Location = new System.Drawing.Point(70, 215);
            this.pokemonGenderDropdown.Name = "pokemonGenderDropdown";
            this.pokemonGenderDropdown.Size = new System.Drawing.Size(80, 24);
            this.pokemonGenderDropdown.TabIndex = 88;
            // 
            // pokemonIVsNumberBox
            // 
            this.pokemonIVsNumberBox.Location = new System.Drawing.Point(40, 181);
            this.pokemonIVsNumberBox.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.pokemonIVsNumberBox.Name = "pokemonIVsNumberBox";
            this.pokemonIVsNumberBox.Size = new System.Drawing.Size(60, 22);
            this.pokemonIVsNumberBox.TabIndex = 84;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 184);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 16);
            this.label11.TabIndex = 83;
            this.label11.Text = "IVs:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(387, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 16);
            this.label10.TabIndex = 87;
            this.label10.Text = "Moves:";
            // 
            // pokemonLevelNumberBox
            // 
            this.pokemonLevelNumberBox.Location = new System.Drawing.Point(190, 57);
            this.pokemonLevelNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.pokemonLevelNumberBox.Name = "pokemonLevelNumberBox";
            this.pokemonLevelNumberBox.Size = new System.Drawing.Size(40, 22);
            this.pokemonLevelNumberBox.TabIndex = 86;
            this.pokemonLevelNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pokemonMove4Dropdown
            // 
            this.pokemonMove4Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonMove4Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonMove4Dropdown.FormattingEnabled = true;
            this.pokemonMove4Dropdown.Location = new System.Drawing.Point(390, 215);
            this.pokemonMove4Dropdown.Name = "pokemonMove4Dropdown";
            this.pokemonMove4Dropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonMove4Dropdown.TabIndex = 86;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(140, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 16);
            this.label9.TabIndex = 85;
            this.label9.Text = "Level:";
            // 
            // pokemonMove3Dropdown
            // 
            this.pokemonMove3Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonMove3Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonMove3Dropdown.FormattingEnabled = true;
            this.pokemonMove3Dropdown.Location = new System.Drawing.Point(390, 185);
            this.pokemonMove3Dropdown.Name = "pokemonMove3Dropdown";
            this.pokemonMove3Dropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonMove3Dropdown.TabIndex = 85;
            // 
            // pokemonFormNumberBox
            // 
            this.pokemonFormNumberBox.Location = new System.Drawing.Point(330, 21);
            this.pokemonFormNumberBox.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.pokemonFormNumberBox.Name = "pokemonFormNumberBox";
            this.pokemonFormNumberBox.Size = new System.Drawing.Size(40, 22);
            this.pokemonFormNumberBox.TabIndex = 84;
            this.pokemonFormNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pokemonMove2Dropdown
            // 
            this.pokemonMove2Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonMove2Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonMove2Dropdown.FormattingEnabled = true;
            this.pokemonMove2Dropdown.Location = new System.Drawing.Point(390, 155);
            this.pokemonMove2Dropdown.Name = "pokemonMove2Dropdown";
            this.pokemonMove2Dropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonMove2Dropdown.TabIndex = 84;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(280, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 16);
            this.label8.TabIndex = 83;
            this.label8.Text = "Form:";
            // 
            // pokemonMove1Dropdown
            // 
            this.pokemonMove1Dropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonMove1Dropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonMove1Dropdown.FormattingEnabled = true;
            this.pokemonMove1Dropdown.Location = new System.Drawing.Point(390, 125);
            this.pokemonMove1Dropdown.Name = "pokemonMove1Dropdown";
            this.pokemonMove1Dropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonMove1Dropdown.TabIndex = 83;
            // 
            // pokemonListBox
            // 
            this.pokemonListBox.FormattingEnabled = true;
            this.pokemonListBox.ItemHeight = 16;
            this.pokemonListBox.Location = new System.Drawing.Point(10, 20);
            this.pokemonListBox.Name = "pokemonListBox";
            this.pokemonListBox.Size = new System.Drawing.Size(120, 100);
            this.pokemonListBox.TabIndex = 0;
            this.pokemonListBox.SelectedIndexChanged += new System.EventHandler(this.LoadPokemonIntoEditor);
            // 
            // pokemonIDDropdown
            // 
            this.pokemonIDDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.pokemonIDDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pokemonIDDropdown.FormattingEnabled = true;
            this.pokemonIDDropdown.Location = new System.Drawing.Point(140, 20);
            this.pokemonIDDropdown.Name = "pokemonIDDropdown";
            this.pokemonIDDropdown.Size = new System.Drawing.Size(120, 24);
            this.pokemonIDDropdown.TabIndex = 83;
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Unown-A - 0",
            "Unown-B - 1",
            "Unown-C - 2",
            "Unown-D - 3",
            "Unown-E - 4",
            "Unown-F - 5",
            "Unown-G - 6",
            "Unown-H - 7",
            "Unown-I - 8",
            "Unown-J - 9",
            "Unown-K - 10",
            "Unown-L - 11",
            "Unown-M - 12",
            "Unown-N - 13",
            "Unown-O - 14",
            "Unown-P - 15",
            "Unown-Q - 16",
            "Unown-R - 17",
            "Unown-S - 18",
            "Unown-T - 19",
            "Unown-U - 20",
            "Unown-V - 21",
            "Unown-W - 22",
            "Unown-X - 23",
            "Unown-Y - 24",
            "Unown-Z - 25",
            "Unown-! - 26",
            "Unown-? - 27",
            "",
            "Castform-Normal - 0",
            "Castform-Sunny - 1",
            "Castform-Rainy - 2",
            "Castform-Snowy - 3",
            "",
            "Deoxys-Normal - 0",
            "Deoxys-Attack - 1",
            "Deoxys-Defense - 2",
            "Deoxys-Speed - 3",
            "",
            "Burmy-Plant Cloak - 0",
            "Burmy-Sandy Cloak - 1",
            "Burmy-Trash Cloak - 2",
            "",
            "Wormadam-Plant Cloak - 0",
            "Wormadam-Sandy Cloak - 1",
            "Wormadam-Trash Cloak - 2",
            "",
            "Cherrim-Overcast - 0",
            "Cherrim-Sunshine - 1",
            "",
            "Shellos-West Sea - 0",
            "Shellos-East Sea - 1",
            "",
            "Gastrodon-West Sea - 0",
            "Gastrodon-East Sea - 1",
            "",
            "Rotom-Normal - 0",
            "Rotom-Heat - 1",
            "Rotom-Wash - 2",
            "Rotom-Frost - 3",
            "Rotom-Fan - 4",
            "Rotom-Mow - 5",
            "",
            "Giratina-Altered - 0",
            "Giratina-Origin - 1",
            "",
            "Shaymin-Land - 0",
            "Shaymin-Sky - 1",
            "",
            "Arceus-Normal - 0",
            "Arceus-Fighting - 1",
            "Arceus-Flying - 2",
            "Arceus-Poison - 3",
            "Arceus-Ground - 4",
            "Arceus-Rock - 5",
            "Arceus-Bug - 6",
            "Arceus-Ghost - 7",
            "Arceus-Steel - 8",
            "Arceus-Fire - 9",
            "Arceus-Water - 10",
            "Arceus-Grass - 11",
            "Arceus-Electric - 12",
            "Arceus-Psychic - 13",
            "Arceus-Ice - 14",
            "Arceus-Dragon - 15",
            "Arceus-Dark - 16",
            "",
            "Basculin-Red-Striped - 0",
            "Basculin-Blue-Striped - 1",
            "",
            "Darmanitan-Standard Mode - 0",
            "Darmanitan-Zen Mode - 1",
            "",
            "Deerling-Spring - 0",
            "Deerling-Summer - 1",
            "Deerling-Autumn - 2",
            "Deerling-Winter - 3",
            "",
            "Sawsbuck-Spring - 0",
            "Sawsbuck-Summer - 1",
            "Sawsbuck-Autumn - 2",
            "Sawsbuck-Winter - 3",
            "",
            "Tornadus-Incarnate - 0",
            "Tornadus-Therian - 1",
            "",
            "Thundurus-Incarnate - 0",
            "Thundurus-Therian - 1",
            "",
            "Landorus-Incarnate - 0",
            "Landorus-Therian - 1",
            "",
            "Kyurem-Normal - 0",
            "Kyurem-White - 1",
            "Kyurem-Black - 2",
            "",
            "Keldeo-Usual - 0",
            "Keldeo-Resolution - 1",
            "",
            "Meloetta-Aria - 0",
            "Meloetta-Pirouette - 1",
            "",
            "Genesect-Normal - 0",
            "Genesect-Water - 1",
            "Genesect-Electric - 2",
            "Genesect-Fire - 3",
            "Genesect-Ice - 4"});
            this.comboBox1.Location = new System.Drawing.Point(770, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(200, 24);
            this.comboBox1.TabIndex = 84;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(660, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 16);
            this.label7.TabIndex = 85;
            this.label7.Text = "Pokemon Forms:";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(220, 12);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(80, 30);
            this.applyButton.TabIndex = 94;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.ApplyTrainer);
            // 
            // addTrainerButton
            // 
            this.addTrainerButton.Location = new System.Drawing.Point(320, 12);
            this.addTrainerButton.Name = "addTrainerButton";
            this.addTrainerButton.Size = new System.Drawing.Size(120, 30);
            this.addTrainerButton.TabIndex = 95;
            this.addTrainerButton.Text = "Add Trainer";
            this.addTrainerButton.UseVisualStyleBackColor = true;
            this.addTrainerButton.Click += new System.EventHandler(this.addTrainerButton_Click);
            // 
            // dialogueGroup
            // 
            this.dialogueGroup.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.dialogueGroup.Controls.Add(this.trDialogueTextBox);
            this.dialogueGroup.Controls.Add(this.dialogueTypeDropdown);
            this.dialogueGroup.Enabled = false;
            this.dialogueGroup.Location = new System.Drawing.Point(12, 349);
            this.dialogueGroup.Name = "dialogueGroup";
            this.dialogueGroup.Size = new System.Drawing.Size(958, 80);
            this.dialogueGroup.TabIndex = 96;
            this.dialogueGroup.TabStop = false;
            this.dialogueGroup.Text = "Dialogue";
            // 
            // trDialogueTextBox
            // 
            this.trDialogueTextBox.Location = new System.Drawing.Point(280, 34);
            this.trDialogueTextBox.Name = "trDialogueTextBox";
            this.trDialogueTextBox.Size = new System.Drawing.Size(658, 22);
            this.trDialogueTextBox.TabIndex = 4;
            // 
            // dialogueTypeDropdown
            // 
            this.dialogueTypeDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.dialogueTypeDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.dialogueTypeDropdown.FormattingEnabled = true;
            this.dialogueTypeDropdown.Items.AddRange(new object[] {
            "Before Battle",
            "Defeated in Battle",
            "After Battle",
            "Before Double Battle",
            "Defeated in Double Battle",
            "After Double Battle",
            "Reject Double Battle",
            "Pre Double Battle 2",
            "Defeated in Double Battle 2",
            "After Double Battle 2",
            "Reject Double Battle 2",
            "----",
            "----",
            "Before Heal",
            "After Heal",
            "After Battle Item",
            "More Item",
            "After First Hit",
            "----",
            "Last Pokemon",
            "Last Pokemon Half Hp",
            "Before Triple Battle",
            "Defeated in Triple Battle",
            "After Triple Battle",
            "Reject Triple Battle"});
            this.dialogueTypeDropdown.Location = new System.Drawing.Point(15, 34);
            this.dialogueTypeDropdown.Name = "dialogueTypeDropdown";
            this.dialogueTypeDropdown.Size = new System.Drawing.Size(200, 24);
            this.dialogueTypeDropdown.TabIndex = 3;
            this.dialogueTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.dialogueTypeDropdown_SelectedIndexChanged);
            // 
            // trainerNameTextBox
            // 
            this.trainerNameTextBox.Enabled = false;
            this.trainerNameTextBox.Location = new System.Drawing.Point(60, 47);
            this.trainerNameTextBox.Name = "trainerNameTextBox";
            this.trainerNameTextBox.Size = new System.Drawing.Size(152, 22);
            this.trainerNameTextBox.TabIndex = 97;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 16);
            this.label12.TabIndex = 98;
            this.label12.Text = "Name:";
            // 
            // trainerClassDropdown
            // 
            this.trainerClassDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.trainerClassDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.trainerClassDropdown.FormattingEnabled = true;
            this.trainerClassDropdown.Location = new System.Drawing.Point(105, 22);
            this.trainerClassDropdown.Name = "trainerClassDropdown";
            this.trainerClassDropdown.Size = new System.Drawing.Size(151, 24);
            this.trainerClassDropdown.TabIndex = 98;
            // 
            // TrainerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(984, 441);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.trainerNameTextBox);
            this.Controls.Add(this.dialogueGroup);
            this.Controls.Add(this.addTrainerButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.pokemonGroupBox);
            this.Controls.Add(this.trainerDataGroup);
            this.Controls.Add(this.trainerNameDropdown);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TrainerEditor";
            this.Text = "Trainer Editor";
            this.trainerDataGroup.ResumeLayout(false);
            this.trainerDataGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aiNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.prizeMoneyNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPokemonBox)).EndInit();
            this.pokemonGroupBox.ResumeLayout(false);
            this.pokemonGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonIVsNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonLevelNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonFormNumberBox)).EndInit();
            this.dialogueGroup.ResumeLayout(false);
            this.dialogueGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox trainerNameDropdown;
        private System.Windows.Forms.GroupBox trainerDataGroup;
        private System.Windows.Forms.CheckBox healerCheckBox;
        private System.Windows.Forms.CheckBox uniqueMovesCheckBox;
        private System.Windows.Forms.CheckBox heldItemsCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox battleTypeDropdown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPokemonBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox item4Dropdown;
        private System.Windows.Forms.ComboBox item3Dropdown;
        private System.Windows.Forms.ComboBox item2Dropdown;
        private System.Windows.Forms.ComboBox item1Dropdown;
        private System.Windows.Forms.NumericUpDown prizeMoneyNumberBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox prizeItemDropdown;
        private System.Windows.Forms.NumericUpDown aiNumberBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox pokemonGroupBox;
        private System.Windows.Forms.ListBox pokemonListBox;
        private System.Windows.Forms.ComboBox pokemonIDDropdown;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown pokemonFormNumberBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown pokemonLevelNumberBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox pokemonMove4Dropdown;
        private System.Windows.Forms.ComboBox pokemonMove3Dropdown;
        private System.Windows.Forms.ComboBox pokemonMove2Dropdown;
        private System.Windows.Forms.ComboBox pokemonMove1Dropdown;
        private System.Windows.Forms.NumericUpDown pokemonIVsNumberBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox pokemonGenderDropdown;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox pokemonHeldItemDropdown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox pokemonAbilityDropdown;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button movePokemonDownButton;
        private System.Windows.Forms.Button movePokemonUpButton;
        private System.Windows.Forms.Button addTrainerButton;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox dialogueGroup;
        private System.Windows.Forms.ComboBox dialogueTypeDropdown;
        private System.Windows.Forms.TextBox trDialogueTextBox;
        private System.Windows.Forms.Button editTrainerAIButton;
        private System.Windows.Forms.TextBox trainerNameTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox trainerClassDropdown;
    }
}