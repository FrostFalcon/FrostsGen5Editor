
namespace NewEditor.Forms
{
    partial class ScriptEditor
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
            this.scriptFileDropdown = new System.Windows.Forms.ComboBox();
            this.sequenceIDNumberBox = new System.Windows.Forms.NumericUpDown();
            this.commandTypeDropdown = new System.Windows.Forms.ComboBox();
            this.commandsListBox = new System.Windows.Forms.ListBox();
            this.sequenceCountLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.copySequenceButton = new System.Windows.Forms.Button();
            this.pasteSequenceButton = new System.Windows.Forms.Button();
            this.addCommandButton = new System.Windows.Forms.Button();
            this.removeCommandButton = new System.Windows.Forms.Button();
            this.addBeforeBubble = new System.Windows.Forms.RadioButton();
            this.addAfterBubble = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.applyCommandButton = new System.Windows.Forms.Button();
            this.addScriptButton = new System.Windows.Forms.Button();
            this.rawDataTextBox = new System.Windows.Forms.RichTextBox();
            this.applyRawDataButton = new System.Windows.Forms.Button();
            this.locateByteButton = new System.Windows.Forms.Button();
            this.cutsceneIDButton = new System.Windows.Forms.Button();
            this.byteNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.readScriptFileButton = new System.Windows.Forms.Button();
            this.exportScriptFileButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sequenceIDNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // scriptFileDropdown
            // 
            this.scriptFileDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.scriptFileDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.scriptFileDropdown.FormattingEnabled = true;
            this.scriptFileDropdown.Location = new System.Drawing.Point(14, 16);
            this.scriptFileDropdown.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.scriptFileDropdown.Name = "scriptFileDropdown";
            this.scriptFileDropdown.Size = new System.Drawing.Size(193, 24);
            this.scriptFileDropdown.TabIndex = 3;
            this.scriptFileDropdown.SelectedIndexChanged += new System.EventHandler(this.LoadScriptFile);
            // 
            // sequenceIDNumberBox
            // 
            this.sequenceIDNumberBox.Location = new System.Drawing.Point(14, 48);
            this.sequenceIDNumberBox.Name = "sequenceIDNumberBox";
            this.sequenceIDNumberBox.Size = new System.Drawing.Size(60, 22);
            this.sequenceIDNumberBox.TabIndex = 4;
            this.sequenceIDNumberBox.ValueChanged += new System.EventHandler(this.LoadSequence);
            // 
            // commandTypeDropdown
            // 
            this.commandTypeDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.commandTypeDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.commandTypeDropdown.FormattingEnabled = true;
            this.commandTypeDropdown.Location = new System.Drawing.Point(435, 160);
            this.commandTypeDropdown.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.commandTypeDropdown.Name = "commandTypeDropdown";
            this.commandTypeDropdown.Size = new System.Drawing.Size(193, 24);
            this.commandTypeDropdown.TabIndex = 5;
            this.commandTypeDropdown.SelectedIndexChanged += new System.EventHandler(this.ChangeCommandDropdown);
            // 
            // commandsListBox
            // 
            this.commandsListBox.FormattingEnabled = true;
            this.commandsListBox.ItemHeight = 16;
            this.commandsListBox.Location = new System.Drawing.Point(15, 160);
            this.commandsListBox.Name = "commandsListBox";
            this.commandsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.commandsListBox.Size = new System.Drawing.Size(330, 212);
            this.commandsListBox.TabIndex = 6;
            this.commandsListBox.SelectedIndexChanged += new System.EventHandler(this.ChangeSelectedListboxCommand);
            this.commandsListBox.DoubleClick += new System.EventHandler(this.DoubleClickListBox);
            // 
            // sequenceCountLabel
            // 
            this.sequenceCountLabel.AutoSize = true;
            this.sequenceCountLabel.Location = new System.Drawing.Point(80, 52);
            this.sequenceCountLabel.Name = "sequenceCountLabel";
            this.sequenceCountLabel.Size = new System.Drawing.Size(22, 16);
            this.sequenceCountLabel.TabIndex = 7;
            this.sequenceCountLabel.Text = "/ 0";
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(15, 500);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(120, 40);
            this.saveButton.TabIndex = 69;
            this.saveButton.Text = "Apply Sequence";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.ApplySequence);
            // 
            // copySequenceButton
            // 
            this.copySequenceButton.Location = new System.Drawing.Point(14, 120);
            this.copySequenceButton.Name = "copySequenceButton";
            this.copySequenceButton.Size = new System.Drawing.Size(80, 30);
            this.copySequenceButton.TabIndex = 79;
            this.copySequenceButton.Text = "Copy";
            this.copySequenceButton.UseVisualStyleBackColor = true;
            this.copySequenceButton.Click += new System.EventHandler(this.CopyCommandSequence);
            // 
            // pasteSequenceButton
            // 
            this.pasteSequenceButton.Enabled = false;
            this.pasteSequenceButton.Location = new System.Drawing.Point(105, 120);
            this.pasteSequenceButton.Name = "pasteSequenceButton";
            this.pasteSequenceButton.Size = new System.Drawing.Size(80, 30);
            this.pasteSequenceButton.TabIndex = 80;
            this.pasteSequenceButton.Text = "Paste";
            this.pasteSequenceButton.UseVisualStyleBackColor = true;
            this.pasteSequenceButton.Click += new System.EventHandler(this.PasteCommandSequence);
            // 
            // addCommandButton
            // 
            this.addCommandButton.Location = new System.Drawing.Point(14, 380);
            this.addCommandButton.Name = "addCommandButton";
            this.addCommandButton.Size = new System.Drawing.Size(80, 30);
            this.addCommandButton.TabIndex = 81;
            this.addCommandButton.Text = "Add";
            this.addCommandButton.UseVisualStyleBackColor = true;
            this.addCommandButton.Click += new System.EventHandler(this.AddCommandToSequence);
            // 
            // removeCommandButton
            // 
            this.removeCommandButton.Location = new System.Drawing.Point(105, 380);
            this.removeCommandButton.Name = "removeCommandButton";
            this.removeCommandButton.Size = new System.Drawing.Size(80, 30);
            this.removeCommandButton.TabIndex = 82;
            this.removeCommandButton.Text = "Remove";
            this.removeCommandButton.UseVisualStyleBackColor = true;
            this.removeCommandButton.Click += new System.EventHandler(this.RemoveCommandFromSequence);
            // 
            // addBeforeBubble
            // 
            this.addBeforeBubble.AutoSize = true;
            this.addBeforeBubble.Location = new System.Drawing.Point(20, 415);
            this.addBeforeBubble.Name = "addBeforeBubble";
            this.addBeforeBubble.Size = new System.Drawing.Size(62, 20);
            this.addBeforeBubble.TabIndex = 83;
            this.addBeforeBubble.Text = "Before";
            this.addBeforeBubble.UseVisualStyleBackColor = true;
            // 
            // addAfterBubble
            // 
            this.addAfterBubble.AutoSize = true;
            this.addAfterBubble.Checked = true;
            this.addAfterBubble.Location = new System.Drawing.Point(20, 435);
            this.addAfterBubble.Name = "addAfterBubble";
            this.addAfterBubble.Size = new System.Drawing.Size(52, 20);
            this.addAfterBubble.TabIndex = 84;
            this.addAfterBubble.TabStop = true;
            this.addAfterBubble.Text = "After";
            this.addAfterBubble.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(360, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 85;
            this.label1.Text = "Command:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(352, 220);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 86;
            this.label2.Text = "Parameters:";
            // 
            // applyCommandButton
            // 
            this.applyCommandButton.Location = new System.Drawing.Point(355, 280);
            this.applyCommandButton.Name = "applyCommandButton";
            this.applyCommandButton.Size = new System.Drawing.Size(120, 40);
            this.applyCommandButton.TabIndex = 87;
            this.applyCommandButton.Text = "Apply Command";
            this.applyCommandButton.UseVisualStyleBackColor = true;
            this.applyCommandButton.Click += new System.EventHandler(this.ApplyCommand);
            // 
            // addScriptButton
            // 
            this.addScriptButton.Location = new System.Drawing.Point(127, 45);
            this.addScriptButton.Name = "addScriptButton";
            this.addScriptButton.Size = new System.Drawing.Size(80, 30);
            this.addScriptButton.TabIndex = 88;
            this.addScriptButton.Text = "Add Script";
            this.addScriptButton.UseVisualStyleBackColor = true;
            this.addScriptButton.Click += new System.EventHandler(this.addScriptButton_Click);
            // 
            // rawDataTextBox
            // 
            this.rawDataTextBox.Enabled = false;
            this.rawDataTextBox.Location = new System.Drawing.Point(520, 400);
            this.rawDataTextBox.Name = "rawDataTextBox";
            this.rawDataTextBox.Size = new System.Drawing.Size(400, 140);
            this.rawDataTextBox.TabIndex = 89;
            this.rawDataTextBox.Text = "";
            this.rawDataTextBox.Click += new System.EventHandler(this.rawDataTextBox_Click);
            // 
            // applyRawDataButton
            // 
            this.applyRawDataButton.Enabled = false;
            this.applyRawDataButton.Location = new System.Drawing.Point(394, 500);
            this.applyRawDataButton.Name = "applyRawDataButton";
            this.applyRawDataButton.Size = new System.Drawing.Size(120, 40);
            this.applyRawDataButton.TabIndex = 90;
            this.applyRawDataButton.Text = "Apply Raw Data";
            this.applyRawDataButton.UseVisualStyleBackColor = true;
            this.applyRawDataButton.Click += new System.EventHandler(this.ApplyRawData);
            // 
            // locateByteButton
            // 
            this.locateByteButton.Location = new System.Drawing.Point(800, 364);
            this.locateByteButton.Name = "locateByteButton";
            this.locateByteButton.Size = new System.Drawing.Size(121, 30);
            this.locateByteButton.TabIndex = 91;
            this.locateByteButton.Text = "Locate Command";
            this.locateByteButton.UseVisualStyleBackColor = true;
            this.locateByteButton.Click += new System.EventHandler(this.locateByteButton_Click);
            // 
            // cutsceneIDButton
            // 
            this.cutsceneIDButton.Location = new System.Drawing.Point(800, 12);
            this.cutsceneIDButton.Name = "cutsceneIDButton";
            this.cutsceneIDButton.Size = new System.Drawing.Size(120, 30);
            this.cutsceneIDButton.TabIndex = 92;
            this.cutsceneIDButton.Text = "Cutscene IDs";
            this.cutsceneIDButton.UseVisualStyleBackColor = true;
            this.cutsceneIDButton.Visible = false;
            this.cutsceneIDButton.Click += new System.EventHandler(this.cutsceneIDButton_Click);
            // 
            // byteNumberBox
            // 
            this.byteNumberBox.Location = new System.Drawing.Point(520, 369);
            this.byteNumberBox.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.byteNumberBox.Name = "byteNumberBox";
            this.byteNumberBox.Size = new System.Drawing.Size(78, 22);
            this.byteNumberBox.TabIndex = 93;
            this.byteNumberBox.ValueChanged += new System.EventHandler(this.byteNumberBox_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(605, 372);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 16);
            this.label3.TabIndex = 94;
            this.label3.Text = "Byte number";
            // 
            // readScriptFileButton
            // 
            this.readScriptFileButton.Enabled = false;
            this.readScriptFileButton.Location = new System.Drawing.Point(220, 15);
            this.readScriptFileButton.Name = "readScriptFileButton";
            this.readScriptFileButton.Size = new System.Drawing.Size(120, 40);
            this.readScriptFileButton.TabIndex = 95;
            this.readScriptFileButton.Text = "Read Script File";
            this.readScriptFileButton.UseVisualStyleBackColor = true;
            this.readScriptFileButton.Click += new System.EventHandler(this.ReadScriptFile);
            // 
            // exportScriptFileButton
            // 
            this.exportScriptFileButton.Enabled = false;
            this.exportScriptFileButton.Location = new System.Drawing.Point(350, 15);
            this.exportScriptFileButton.Name = "exportScriptFileButton";
            this.exportScriptFileButton.Size = new System.Drawing.Size(120, 40);
            this.exportScriptFileButton.TabIndex = 96;
            this.exportScriptFileButton.Text = "Export Script File";
            this.exportScriptFileButton.UseVisualStyleBackColor = true;
            this.exportScriptFileButton.Click += new System.EventHandler(this.ExportScriptFile);
            // 
            // ScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(933, 554);
            this.Controls.Add(this.exportScriptFileButton);
            this.Controls.Add(this.readScriptFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.byteNumberBox);
            this.Controls.Add(this.cutsceneIDButton);
            this.Controls.Add(this.locateByteButton);
            this.Controls.Add(this.applyRawDataButton);
            this.Controls.Add(this.rawDataTextBox);
            this.Controls.Add(this.addScriptButton);
            this.Controls.Add(this.applyCommandButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addAfterBubble);
            this.Controls.Add(this.addBeforeBubble);
            this.Controls.Add(this.removeCommandButton);
            this.Controls.Add(this.addCommandButton);
            this.Controls.Add(this.copySequenceButton);
            this.Controls.Add(this.pasteSequenceButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.sequenceCountLabel);
            this.Controls.Add(this.commandsListBox);
            this.Controls.Add(this.commandTypeDropdown);
            this.Controls.Add(this.sequenceIDNumberBox);
            this.Controls.Add(this.scriptFileDropdown);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ScriptEditor";
            this.Text = "ScriptEditor";
            ((System.ComponentModel.ISupportInitialize)(this.sequenceIDNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown sequenceIDNumberBox;
        private System.Windows.Forms.ComboBox commandTypeDropdown;
        private System.Windows.Forms.ListBox commandsListBox;
        private System.Windows.Forms.Label sequenceCountLabel;
        private System.Windows.Forms.Button saveButton;
        public System.Windows.Forms.ComboBox scriptFileDropdown;
        private System.Windows.Forms.Button copySequenceButton;
        private System.Windows.Forms.Button pasteSequenceButton;
        private System.Windows.Forms.Button addCommandButton;
        private System.Windows.Forms.Button removeCommandButton;
        private System.Windows.Forms.RadioButton addBeforeBubble;
        private System.Windows.Forms.RadioButton addAfterBubble;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button applyCommandButton;
        private System.Windows.Forms.Button addScriptButton;
        private System.Windows.Forms.RichTextBox rawDataTextBox;
        private System.Windows.Forms.Button applyRawDataButton;
        private System.Windows.Forms.Button locateByteButton;
        private System.Windows.Forms.Button cutsceneIDButton;
        private System.Windows.Forms.NumericUpDown byteNumberBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button readScriptFileButton;
        private System.Windows.Forms.Button exportScriptFileButton;
    }
}