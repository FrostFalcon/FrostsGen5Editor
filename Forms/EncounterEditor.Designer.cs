
namespace NewEditor.Forms
{
    partial class EncounterEditor
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
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.encounterSlotToPokemonEditorButton = new System.Windows.Forms.Button();
            this.pasteEncounterGroupButton = new System.Windows.Forms.Button();
            this.copyEncounterGroupButton = new System.Windows.Forms.Button();
            this.copyGrassToDarkGrassButton = new System.Windows.Forms.Button();
            this.encounterGroupApplyRouteButton = new System.Windows.Forms.Button();
            this.removeEncounterGroupButton = new System.Windows.Forms.Button();
            this.addEncounterGroupButton = new System.Windows.Forms.Button();
            this.encounterPokemonNameDropDown = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.encounterSlotApplyButton = new System.Windows.Forms.Button();
            this.routeEncounterTypesPanel = new System.Windows.Forms.Panel();
            this.encounterGroupListBox = new System.Windows.Forms.ListBox();
            this.encounterGroupRateNumber = new System.Windows.Forms.NumericUpDown();
            this.encounterRouteNameDropdown = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.encounterGroupMaxLvNumber = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.encounterGroupFormNumber = new System.Windows.Forms.NumericUpDown();
            this.encounterGroupMinLvNumber = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.toggleSeasonsButton = new System.Windows.Forms.Button();
            this.routeEncounterTypesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupRateNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupMaxLvNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupFormNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupMinLvNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(10, 150);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(107, 20);
            this.radioButton7.TabIndex = 21;
            this.radioButton7.Text = "Ripple Fishing";
            this.radioButton7.UseVisualStyleBackColor = true;
            this.radioButton7.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(10, 130);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(67, 20);
            this.radioButton6.TabIndex = 20;
            this.radioButton6.Text = "Fishing";
            this.radioButton6.UseVisualStyleBackColor = true;
            this.radioButton6.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(10, 110);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(110, 20);
            this.radioButton5.TabIndex = 19;
            this.radioButton5.Text = "Rippling Water";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(10, 90);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(48, 20);
            this.radioButton4.TabIndex = 18;
            this.radioButton4.Text = "Surf";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(10, 50);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(111, 20);
            this.radioButton3.TabIndex = 17;
            this.radioButton3.Text = "Rustling Grass";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(10, 30);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(91, 20);
            this.radioButton2.TabIndex = 16;
            this.radioButton2.Text = "Dark Grass";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(10, 10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(100, 20);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Grass / Land";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.ChangeRouteEncounterType);
            // 
            // encounterSlotToPokemonEditorButton
            // 
            this.encounterSlotToPokemonEditorButton.Location = new System.Drawing.Point(592, 159);
            this.encounterSlotToPokemonEditorButton.Name = "encounterSlotToPokemonEditorButton";
            this.encounterSlotToPokemonEditorButton.Size = new System.Drawing.Size(60, 25);
            this.encounterSlotToPokemonEditorButton.TabIndex = 52;
            this.encounterSlotToPokemonEditorButton.Text = "View";
            this.encounterSlotToPokemonEditorButton.UseVisualStyleBackColor = true;
            this.encounterSlotToPokemonEditorButton.Click += new System.EventHandler(this.ViewPokemonButton);
            // 
            // pasteEncounterGroupButton
            // 
            this.pasteEncounterGroupButton.Enabled = false;
            this.pasteEncounterGroupButton.Location = new System.Drawing.Point(262, 117);
            this.pasteEncounterGroupButton.Name = "pasteEncounterGroupButton";
            this.pasteEncounterGroupButton.Size = new System.Drawing.Size(70, 25);
            this.pasteEncounterGroupButton.TabIndex = 51;
            this.pasteEncounterGroupButton.Text = "Paste";
            this.pasteEncounterGroupButton.UseVisualStyleBackColor = true;
            this.pasteEncounterGroupButton.Click += new System.EventHandler(this.pasteEncounterGroupButton_Click);
            // 
            // copyEncounterGroupButton
            // 
            this.copyEncounterGroupButton.Enabled = false;
            this.copyEncounterGroupButton.Location = new System.Drawing.Point(171, 117);
            this.copyEncounterGroupButton.Name = "copyEncounterGroupButton";
            this.copyEncounterGroupButton.Size = new System.Drawing.Size(70, 25);
            this.copyEncounterGroupButton.TabIndex = 50;
            this.copyEncounterGroupButton.Text = "Copy";
            this.copyEncounterGroupButton.UseVisualStyleBackColor = true;
            this.copyEncounterGroupButton.Click += new System.EventHandler(this.copyEncounterGroupButton_Click);
            // 
            // copyGrassToDarkGrassButton
            // 
            this.copyGrassToDarkGrassButton.Enabled = false;
            this.copyGrassToDarkGrassButton.Location = new System.Drawing.Point(21, 113);
            this.copyGrassToDarkGrassButton.Name = "copyGrassToDarkGrassButton";
            this.copyGrassToDarkGrassButton.Size = new System.Drawing.Size(140, 30);
            this.copyGrassToDarkGrassButton.TabIndex = 49;
            this.copyGrassToDarkGrassButton.Text = "Grass -> Dark Grass";
            this.copyGrassToDarkGrassButton.UseVisualStyleBackColor = true;
            this.copyGrassToDarkGrassButton.Click += new System.EventHandler(this.copyGrassToDarkGrassButton_Click);
            // 
            // encounterGroupApplyRouteButton
            // 
            this.encounterGroupApplyRouteButton.Enabled = false;
            this.encounterGroupApplyRouteButton.Location = new System.Drawing.Point(260, 10);
            this.encounterGroupApplyRouteButton.Name = "encounterGroupApplyRouteButton";
            this.encounterGroupApplyRouteButton.Size = new System.Drawing.Size(100, 28);
            this.encounterGroupApplyRouteButton.TabIndex = 48;
            this.encounterGroupApplyRouteButton.Text = "Apply Route";
            this.encounterGroupApplyRouteButton.UseVisualStyleBackColor = true;
            this.encounterGroupApplyRouteButton.Click += new System.EventHandler(this.encounterGroupApplyRouteButton_Click);
            // 
            // removeEncounterGroupButton
            // 
            this.removeEncounterGroupButton.Enabled = false;
            this.removeEncounterGroupButton.Location = new System.Drawing.Point(261, 303);
            this.removeEncounterGroupButton.Name = "removeEncounterGroupButton";
            this.removeEncounterGroupButton.Size = new System.Drawing.Size(70, 25);
            this.removeEncounterGroupButton.TabIndex = 47;
            this.removeEncounterGroupButton.Text = "Remove";
            this.removeEncounterGroupButton.UseVisualStyleBackColor = true;
            this.removeEncounterGroupButton.Click += new System.EventHandler(this.removeEncounterGroupButton_Click);
            // 
            // addEncounterGroupButton
            // 
            this.addEncounterGroupButton.Enabled = false;
            this.addEncounterGroupButton.Location = new System.Drawing.Point(171, 303);
            this.addEncounterGroupButton.Name = "addEncounterGroupButton";
            this.addEncounterGroupButton.Size = new System.Drawing.Size(70, 25);
            this.addEncounterGroupButton.TabIndex = 46;
            this.addEncounterGroupButton.Text = "Add";
            this.addEncounterGroupButton.UseVisualStyleBackColor = true;
            this.addEncounterGroupButton.Click += new System.EventHandler(this.addEncounterGroupButton_Click);
            // 
            // encounterPokemonNameDropDown
            // 
            this.encounterPokemonNameDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.encounterPokemonNameDropDown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.encounterPokemonNameDropDown.Enabled = false;
            this.encounterPokemonNameDropDown.FormattingEnabled = true;
            this.encounterPokemonNameDropDown.Location = new System.Drawing.Point(421, 160);
            this.encounterPokemonNameDropDown.Name = "encounterPokemonNameDropDown";
            this.encounterPokemonNameDropDown.Size = new System.Drawing.Size(150, 24);
            this.encounterPokemonNameDropDown.TabIndex = 45;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(348, 163);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 16);
            this.label7.TabIndex = 44;
            this.label7.Text = "Pokemon:";
            // 
            // encounterSlotApplyButton
            // 
            this.encounterSlotApplyButton.Enabled = false;
            this.encounterSlotApplyButton.Location = new System.Drawing.Point(351, 279);
            this.encounterSlotApplyButton.Name = "encounterSlotApplyButton";
            this.encounterSlotApplyButton.Size = new System.Drawing.Size(100, 25);
            this.encounterSlotApplyButton.TabIndex = 43;
            this.encounterSlotApplyButton.Text = "Apply Slot";
            this.encounterSlotApplyButton.UseVisualStyleBackColor = true;
            this.encounterSlotApplyButton.Click += new System.EventHandler(this.encounterSlotApplyButton_Click);
            // 
            // routeEncounterTypesPanel
            // 
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton7);
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton6);
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton5);
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton4);
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton3);
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton2);
            this.routeEncounterTypesPanel.Controls.Add(this.radioButton1);
            this.routeEncounterTypesPanel.Enabled = false;
            this.routeEncounterTypesPanel.Location = new System.Drawing.Point(21, 149);
            this.routeEncounterTypesPanel.Name = "routeEncounterTypesPanel";
            this.routeEncounterTypesPanel.Size = new System.Drawing.Size(140, 191);
            this.routeEncounterTypesPanel.TabIndex = 42;
            // 
            // encounterGroupListBox
            // 
            this.encounterGroupListBox.Enabled = false;
            this.encounterGroupListBox.FormattingEnabled = true;
            this.encounterGroupListBox.ItemHeight = 16;
            this.encounterGroupListBox.Location = new System.Drawing.Point(171, 149);
            this.encounterGroupListBox.Name = "encounterGroupListBox";
            this.encounterGroupListBox.Size = new System.Drawing.Size(160, 148);
            this.encounterGroupListBox.TabIndex = 33;
            this.encounterGroupListBox.SelectedIndexChanged += new System.EventHandler(this.encounterGroupListBox_SelectedIndexChanged);
            // 
            // encounterGroupRateNumber
            // 
            this.encounterGroupRateNumber.Enabled = false;
            this.encounterGroupRateNumber.Location = new System.Drawing.Point(408, 241);
            this.encounterGroupRateNumber.Name = "encounterGroupRateNumber";
            this.encounterGroupRateNumber.Size = new System.Drawing.Size(40, 22);
            this.encounterGroupRateNumber.TabIndex = 41;
            // 
            // encounterRouteNameDropdown
            // 
            this.encounterRouteNameDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.encounterRouteNameDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.encounterRouteNameDropdown.FormattingEnabled = true;
            this.encounterRouteNameDropdown.Location = new System.Drawing.Point(12, 12);
            this.encounterRouteNameDropdown.Name = "encounterRouteNameDropdown";
            this.encounterRouteNameDropdown.Size = new System.Drawing.Size(240, 24);
            this.encounterRouteNameDropdown.TabIndex = 32;
            this.encounterRouteNameDropdown.SelectedIndexChanged += new System.EventHandler(this.encounterRouteNameDropdown_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(348, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = "Form:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(471, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 35;
            this.label4.Text = "Min Lv:";
            // 
            // encounterGroupMaxLvNumber
            // 
            this.encounterGroupMaxLvNumber.Enabled = false;
            this.encounterGroupMaxLvNumber.Location = new System.Drawing.Point(531, 242);
            this.encounterGroupMaxLvNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.encounterGroupMaxLvNumber.Name = "encounterGroupMaxLvNumber";
            this.encounterGroupMaxLvNumber.Size = new System.Drawing.Size(40, 22);
            this.encounterGroupMaxLvNumber.TabIndex = 40;
            this.encounterGroupMaxLvNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.encounterGroupMaxLvNumber.ValueChanged += new System.EventHandler(this.KeepMinLevelUnderMaxLevel);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(348, 244);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 16);
            this.label6.TabIndex = 37;
            this.label6.Text = "Rate:";
            // 
            // encounterGroupFormNumber
            // 
            this.encounterGroupFormNumber.Enabled = false;
            this.encounterGroupFormNumber.Location = new System.Drawing.Point(408, 203);
            this.encounterGroupFormNumber.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.encounterGroupFormNumber.Name = "encounterGroupFormNumber";
            this.encounterGroupFormNumber.Size = new System.Drawing.Size(40, 22);
            this.encounterGroupFormNumber.TabIndex = 38;
            // 
            // encounterGroupMinLvNumber
            // 
            this.encounterGroupMinLvNumber.Enabled = false;
            this.encounterGroupMinLvNumber.Location = new System.Drawing.Point(531, 203);
            this.encounterGroupMinLvNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.encounterGroupMinLvNumber.Name = "encounterGroupMinLvNumber";
            this.encounterGroupMinLvNumber.Size = new System.Drawing.Size(40, 22);
            this.encounterGroupMinLvNumber.TabIndex = 39;
            this.encounterGroupMinLvNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.encounterGroupMinLvNumber.ValueChanged += new System.EventHandler(this.KeepMaxLevelOverMinLevel);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(471, 245);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 16);
            this.label5.TabIndex = 36;
            this.label5.Text = "Max Lv:";
            // 
            // toggleSeasonsButton
            // 
            this.toggleSeasonsButton.Enabled = false;
            this.toggleSeasonsButton.Location = new System.Drawing.Point(12, 42);
            this.toggleSeasonsButton.Name = "toggleSeasonsButton";
            this.toggleSeasonsButton.Size = new System.Drawing.Size(120, 30);
            this.toggleSeasonsButton.TabIndex = 53;
            this.toggleSeasonsButton.Text = "Add Seasons";
            this.toggleSeasonsButton.UseVisualStyleBackColor = true;
            this.toggleSeasonsButton.Click += new System.EventHandler(this.toggleSeasonsButton_Click);
            // 
            // EncounterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 361);
            this.Controls.Add(this.toggleSeasonsButton);
            this.Controls.Add(this.encounterSlotToPokemonEditorButton);
            this.Controls.Add(this.pasteEncounterGroupButton);
            this.Controls.Add(this.copyEncounterGroupButton);
            this.Controls.Add(this.copyGrassToDarkGrassButton);
            this.Controls.Add(this.encounterGroupApplyRouteButton);
            this.Controls.Add(this.removeEncounterGroupButton);
            this.Controls.Add(this.addEncounterGroupButton);
            this.Controls.Add(this.encounterPokemonNameDropDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.encounterSlotApplyButton);
            this.Controls.Add(this.routeEncounterTypesPanel);
            this.Controls.Add(this.encounterGroupListBox);
            this.Controls.Add(this.encounterGroupRateNumber);
            this.Controls.Add(this.encounterRouteNameDropdown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.encounterGroupMaxLvNumber);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.encounterGroupFormNumber);
            this.Controls.Add(this.encounterGroupMinLvNumber);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "EncounterEditor";
            this.Text = "Encounter Editor";
            this.routeEncounterTypesPanel.ResumeLayout(false);
            this.routeEncounterTypesPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupRateNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupMaxLvNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupFormNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.encounterGroupMinLvNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button encounterSlotToPokemonEditorButton;
        private System.Windows.Forms.Button pasteEncounterGroupButton;
        private System.Windows.Forms.Button copyEncounterGroupButton;
        private System.Windows.Forms.Button copyGrassToDarkGrassButton;
        private System.Windows.Forms.Button encounterGroupApplyRouteButton;
        private System.Windows.Forms.Button removeEncounterGroupButton;
        private System.Windows.Forms.Button addEncounterGroupButton;
        private System.Windows.Forms.ComboBox encounterPokemonNameDropDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button encounterSlotApplyButton;
        private System.Windows.Forms.Panel routeEncounterTypesPanel;
        private System.Windows.Forms.ListBox encounterGroupListBox;
        private System.Windows.Forms.NumericUpDown encounterGroupRateNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown encounterGroupMaxLvNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown encounterGroupFormNumber;
        private System.Windows.Forms.NumericUpDown encounterGroupMinLvNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        public System.Windows.Forms.ComboBox encounterRouteNameDropdown;
        private System.Windows.Forms.Button toggleSeasonsButton;
    }
}