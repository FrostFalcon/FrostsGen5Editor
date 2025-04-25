
namespace NewEditor.Forms
{
    partial class NewScriptEditor
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
            this.rawDataTextBox = new System.Windows.Forms.RichTextBox();
            this.applyRawDataButton = new System.Windows.Forms.Button();
            this.byteNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.readScriptFileButton = new System.Windows.Forms.Button();
            this.exportScriptFileButton = new System.Windows.Forms.Button();
            this.setupQuickBuildButton = new System.Windows.Forms.Button();
            this.quickBuildButton = new System.Windows.Forms.Button();
            this.quickBuildLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.commandNameSelection1 = new System.Windows.Forms.RadioButton();
            this.commandNameSelection2 = new System.Windows.Forms.RadioButton();
            this.loadedOverlayDropdown = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.byteNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // scriptFileDropdown
            // 
            this.scriptFileDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.scriptFileDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.scriptFileDropdown.FormattingEnabled = true;
            this.scriptFileDropdown.Location = new System.Drawing.Point(14, 20);
            this.scriptFileDropdown.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.scriptFileDropdown.Name = "scriptFileDropdown";
            this.scriptFileDropdown.Size = new System.Drawing.Size(120, 24);
            this.scriptFileDropdown.TabIndex = 3;
            this.scriptFileDropdown.SelectedIndexChanged += new System.EventHandler(this.LoadScriptFile);
            // 
            // rawDataTextBox
            // 
            this.rawDataTextBox.Enabled = false;
            this.rawDataTextBox.Location = new System.Drawing.Point(152, 309);
            this.rawDataTextBox.Name = "rawDataTextBox";
            this.rawDataTextBox.Size = new System.Drawing.Size(380, 140);
            this.rawDataTextBox.TabIndex = 89;
            this.rawDataTextBox.Text = "";
            this.rawDataTextBox.Click += new System.EventHandler(this.rawDataTextBox_Click);
            // 
            // applyRawDataButton
            // 
            this.applyRawDataButton.Enabled = false;
            this.applyRawDataButton.Location = new System.Drawing.Point(12, 409);
            this.applyRawDataButton.Name = "applyRawDataButton";
            this.applyRawDataButton.Size = new System.Drawing.Size(120, 40);
            this.applyRawDataButton.TabIndex = 90;
            this.applyRawDataButton.Text = "Apply Raw Data";
            this.applyRawDataButton.UseVisualStyleBackColor = true;
            this.applyRawDataButton.Click += new System.EventHandler(this.ApplyRawData);
            // 
            // byteNumberBox
            // 
            this.byteNumberBox.Location = new System.Drawing.Point(454, 281);
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
            this.label3.Location = new System.Drawing.Point(367, 283);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 16);
            this.label3.TabIndex = 94;
            this.label3.Text = "Byte number";
            // 
            // readScriptFileButton
            // 
            this.readScriptFileButton.Enabled = false;
            this.readScriptFileButton.Location = new System.Drawing.Point(14, 160);
            this.readScriptFileButton.Name = "readScriptFileButton";
            this.readScriptFileButton.Size = new System.Drawing.Size(120, 40);
            this.readScriptFileButton.TabIndex = 95;
            this.readScriptFileButton.Text = "Import Script File";
            this.readScriptFileButton.UseVisualStyleBackColor = true;
            this.readScriptFileButton.Click += new System.EventHandler(this.ReadScriptFile);
            // 
            // exportScriptFileButton
            // 
            this.exportScriptFileButton.Enabled = false;
            this.exportScriptFileButton.Location = new System.Drawing.Point(14, 205);
            this.exportScriptFileButton.Name = "exportScriptFileButton";
            this.exportScriptFileButton.Size = new System.Drawing.Size(120, 40);
            this.exportScriptFileButton.TabIndex = 96;
            this.exportScriptFileButton.Text = "Export Script File";
            this.exportScriptFileButton.UseVisualStyleBackColor = true;
            this.exportScriptFileButton.Click += new System.EventHandler(this.ExportScriptFile);
            // 
            // setupQuickBuildButton
            // 
            this.setupQuickBuildButton.Location = new System.Drawing.Point(200, 12);
            this.setupQuickBuildButton.Name = "setupQuickBuildButton";
            this.setupQuickBuildButton.Size = new System.Drawing.Size(100, 50);
            this.setupQuickBuildButton.TabIndex = 97;
            this.setupQuickBuildButton.Text = "Setup\r\nQuick Build";
            this.setupQuickBuildButton.UseVisualStyleBackColor = true;
            this.setupQuickBuildButton.Click += new System.EventHandler(this.setupQuickBuildButton_Click);
            // 
            // quickBuildButton
            // 
            this.quickBuildButton.Enabled = false;
            this.quickBuildButton.Location = new System.Drawing.Point(306, 12);
            this.quickBuildButton.Name = "quickBuildButton";
            this.quickBuildButton.Size = new System.Drawing.Size(100, 50);
            this.quickBuildButton.TabIndex = 98;
            this.quickBuildButton.Text = "Quick Build Rom";
            this.quickBuildButton.UseVisualStyleBackColor = true;
            this.quickBuildButton.Click += new System.EventHandler(this.quickBuildButton_Click);
            // 
            // quickBuildLabel
            // 
            this.quickBuildLabel.AutoSize = true;
            this.quickBuildLabel.Location = new System.Drawing.Point(200, 75);
            this.quickBuildLabel.Name = "quickBuildLabel";
            this.quickBuildLabel.Size = new System.Drawing.Size(0, 16);
            this.quickBuildLabel.TabIndex = 99;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 16);
            this.label1.TabIndex = 100;
            this.label1.Text = "Command Names:";
            // 
            // commandNameSelection1
            // 
            this.commandNameSelection1.AutoSize = true;
            this.commandNameSelection1.Checked = true;
            this.commandNameSelection1.Location = new System.Drawing.Point(20, 95);
            this.commandNameSelection1.Name = "commandNameSelection1";
            this.commandNameSelection1.Size = new System.Drawing.Size(102, 20);
            this.commandNameSelection1.TabIndex = 101;
            this.commandNameSelection1.TabStop = true;
            this.commandNameSelection1.Text = "Frost\'s Editor";
            this.commandNameSelection1.UseVisualStyleBackColor = true;
            this.commandNameSelection1.CheckedChanged += new System.EventHandler(this.commandNameSelection1_CheckedChanged);
            // 
            // commandNameSelection2
            // 
            this.commandNameSelection2.AutoSize = true;
            this.commandNameSelection2.Location = new System.Drawing.Point(20, 120);
            this.commandNameSelection2.Name = "commandNameSelection2";
            this.commandNameSelection2.Size = new System.Drawing.Size(87, 20);
            this.commandNameSelection2.TabIndex = 102;
            this.commandNameSelection2.Text = "Pokescript";
            this.commandNameSelection2.UseVisualStyleBackColor = true;
            this.commandNameSelection2.CheckedChanged += new System.EventHandler(this.commandNameSelection2_CheckedChanged);
            // 
            // loadedOverlayDropdown
            // 
            this.loadedOverlayDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.loadedOverlayDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.loadedOverlayDropdown.FormattingEnabled = true;
            this.loadedOverlayDropdown.Items.AddRange(new object[] {
            "None",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "58",
            "61",
            "62",
            "63",
            "64",
            "65",
            "66",
            "67",
            "68"});
            this.loadedOverlayDropdown.Location = new System.Drawing.Point(14, 255);
            this.loadedOverlayDropdown.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.loadedOverlayDropdown.Name = "loadedOverlayDropdown";
            this.loadedOverlayDropdown.Size = new System.Drawing.Size(120, 24);
            this.loadedOverlayDropdown.TabIndex = 103;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 104;
            this.label2.Text = "Loaded Overlay";
            // 
            // NewScriptEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 461);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.loadedOverlayDropdown);
            this.Controls.Add(this.commandNameSelection2);
            this.Controls.Add(this.commandNameSelection1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.quickBuildLabel);
            this.Controls.Add(this.quickBuildButton);
            this.Controls.Add(this.setupQuickBuildButton);
            this.Controls.Add(this.exportScriptFileButton);
            this.Controls.Add(this.readScriptFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.byteNumberBox);
            this.Controls.Add(this.applyRawDataButton);
            this.Controls.Add(this.rawDataTextBox);
            this.Controls.Add(this.scriptFileDropdown);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "NewScriptEditor";
            this.Text = "Script Editor";
            ((System.ComponentModel.ISupportInitialize)(this.byteNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ComboBox scriptFileDropdown;
        private System.Windows.Forms.RichTextBox rawDataTextBox;
        private System.Windows.Forms.Button applyRawDataButton;
        private System.Windows.Forms.NumericUpDown byteNumberBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button readScriptFileButton;
        private System.Windows.Forms.Button exportScriptFileButton;
        private System.Windows.Forms.Button setupQuickBuildButton;
        private System.Windows.Forms.Button quickBuildButton;
        private System.Windows.Forms.Label quickBuildLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton commandNameSelection1;
        private System.Windows.Forms.RadioButton commandNameSelection2;
        public System.Windows.Forms.ComboBox loadedOverlayDropdown;
        private System.Windows.Forms.Label label2;
    }
}