
namespace NewEditor.Forms
{
    partial class TextViewer
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
            this.textBoxDisplay = new System.Windows.Forms.RichTextBox();
            this.fileNumComboBox = new System.Windows.Forms.ComboBox();
            this.storyTextRadioButton = new System.Windows.Forms.RadioButton();
            this.miscTextRadioButton = new System.Windows.Forms.RadioButton();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.saveTextFileButton = new System.Windows.Forms.Button();
            this.addLinesButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.selectedLineNumberBox = new System.Windows.Forms.NumericUpDown();
            this.lineCountLabel = new System.Windows.Forms.Label();
            this.statusText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.selectedLineNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Location = new System.Drawing.Point(17, 89);
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.Size = new System.Drawing.Size(915, 340);
            this.textBoxDisplay.TabIndex = 0;
            this.textBoxDisplay.Text = "";
            this.textBoxDisplay.WordWrap = false;
            this.textBoxDisplay.Click += new System.EventHandler(this.textBoxDisplay_Click);
            this.textBoxDisplay.TextChanged += new System.EventHandler(this.textBoxDisplay_TextChanged);
            // 
            // fileNumComboBox
            // 
            this.fileNumComboBox.FormattingEnabled = true;
            this.fileNumComboBox.Location = new System.Drawing.Point(12, 20);
            this.fileNumComboBox.Name = "fileNumComboBox";
            this.fileNumComboBox.Size = new System.Drawing.Size(69, 24);
            this.fileNumComboBox.TabIndex = 1;
            this.fileNumComboBox.SelectedIndexChanged += new System.EventHandler(this.LoadTextbox);
            // 
            // storyTextRadioButton
            // 
            this.storyTextRadioButton.AutoSize = true;
            this.storyTextRadioButton.Location = new System.Drawing.Point(840, 20);
            this.storyTextRadioButton.Name = "storyTextRadioButton";
            this.storyTextRadioButton.Size = new System.Drawing.Size(84, 20);
            this.storyTextRadioButton.TabIndex = 2;
            this.storyTextRadioButton.Text = "Story Text";
            this.storyTextRadioButton.UseVisualStyleBackColor = true;
            // 
            // miscTextRadioButton
            // 
            this.miscTextRadioButton.AutoSize = true;
            this.miscTextRadioButton.Checked = true;
            this.miscTextRadioButton.Location = new System.Drawing.Point(740, 20);
            this.miscTextRadioButton.Name = "miscTextRadioButton";
            this.miscTextRadioButton.Size = new System.Drawing.Size(81, 20);
            this.miscTextRadioButton.TabIndex = 3;
            this.miscTextRadioButton.TabStop = true;
            this.miscTextRadioButton.Text = "Misc Text";
            this.miscTextRadioButton.UseVisualStyleBackColor = true;
            this.miscTextRadioButton.CheckedChanged += new System.EventHandler(this.ChangeNarc);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(225, 20);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(150, 22);
            this.searchTextBox.TabIndex = 4;
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(144, 19);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 24);
            this.searchButton.TabIndex = 5;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.FilterFiles);
            // 
            // saveTextFileButton
            // 
            this.saveTextFileButton.Location = new System.Drawing.Point(600, 10);
            this.saveTextFileButton.Name = "saveTextFileButton";
            this.saveTextFileButton.Size = new System.Drawing.Size(120, 40);
            this.saveTextFileButton.TabIndex = 6;
            this.saveTextFileButton.Text = "Apply Text";
            this.saveTextFileButton.UseVisualStyleBackColor = true;
            this.saveTextFileButton.Click += new System.EventHandler(this.ApplyTextFile);
            // 
            // addLinesButton
            // 
            this.addLinesButton.Location = new System.Drawing.Point(480, 20);
            this.addLinesButton.Name = "addLinesButton";
            this.addLinesButton.Size = new System.Drawing.Size(100, 30);
            this.addLinesButton.TabIndex = 7;
            this.addLinesButton.Text = "Add Lines";
            this.addLinesButton.UseVisualStyleBackColor = true;
            this.addLinesButton.Visible = false;
            this.addLinesButton.Click += new System.EventHandler(this.addLinesButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Selected Line:";
            // 
            // selectedLineNumberBox
            // 
            this.selectedLineNumberBox.Location = new System.Drawing.Point(106, 58);
            this.selectedLineNumberBox.Name = "selectedLineNumberBox";
            this.selectedLineNumberBox.Size = new System.Drawing.Size(60, 22);
            this.selectedLineNumberBox.TabIndex = 9;
            this.selectedLineNumberBox.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // lineCountLabel
            // 
            this.lineCountLabel.AutoSize = true;
            this.lineCountLabel.Location = new System.Drawing.Point(172, 60);
            this.lineCountLabel.Name = "lineCountLabel";
            this.lineCountLabel.Size = new System.Drawing.Size(22, 16);
            this.lineCountLabel.TabIndex = 10;
            this.lineCountLabel.Text = "/ 0";
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(14, 436);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 16);
            this.statusText.TabIndex = 11;
            // 
            // TextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(944, 461);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.lineCountLabel);
            this.Controls.Add(this.selectedLineNumberBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addLinesButton);
            this.Controls.Add(this.saveTextFileButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.miscTextRadioButton);
            this.Controls.Add(this.storyTextRadioButton);
            this.Controls.Add(this.fileNumComboBox);
            this.Controls.Add(this.textBoxDisplay);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TextViewer";
            this.Text = "Text Editor";
            ((System.ComponentModel.ISupportInitialize)(this.selectedLineNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchButton;
        public System.Windows.Forms.ComboBox fileNumComboBox;
        public System.Windows.Forms.RadioButton storyTextRadioButton;
        public System.Windows.Forms.RadioButton miscTextRadioButton;
        private System.Windows.Forms.Button saveTextFileButton;
        public System.Windows.Forms.RichTextBox textBoxDisplay;
        private System.Windows.Forms.Button addLinesButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown selectedLineNumberBox;
        private System.Windows.Forms.Label lineCountLabel;
        private System.Windows.Forms.Label statusText;
    }
}