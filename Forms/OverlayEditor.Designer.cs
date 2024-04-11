
namespace NewEditor.Forms
{
    partial class OverlayEditor
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
            this.decompressButton = new System.Windows.Forms.Button();
            this.saveOverlayButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.selectedLineNumberBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.selectedLineNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDisplay
            // 
            this.textBoxDisplay.Location = new System.Drawing.Point(20, 89);
            this.textBoxDisplay.Name = "textBoxDisplay";
            this.textBoxDisplay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.textBoxDisplay.Size = new System.Drawing.Size(480, 340);
            this.textBoxDisplay.TabIndex = 0;
            this.textBoxDisplay.Text = "";
            this.textBoxDisplay.WordWrap = false;
            this.textBoxDisplay.Click += new System.EventHandler(this.textBoxDisplay_Click);
            // 
            // fileNumComboBox
            // 
            this.fileNumComboBox.FormattingEnabled = true;
            this.fileNumComboBox.Location = new System.Drawing.Point(12, 20);
            this.fileNumComboBox.Name = "fileNumComboBox";
            this.fileNumComboBox.Size = new System.Drawing.Size(100, 24);
            this.fileNumComboBox.TabIndex = 1;
            // 
            // decompressButton
            // 
            this.decompressButton.Location = new System.Drawing.Point(136, 16);
            this.decompressButton.Name = "decompressButton";
            this.decompressButton.Size = new System.Drawing.Size(100, 30);
            this.decompressButton.TabIndex = 5;
            this.decompressButton.Text = "Decompress";
            this.decompressButton.UseVisualStyleBackColor = true;
            this.decompressButton.Click += new System.EventHandler(this.DecompressOverlay);
            // 
            // saveOverlayButton
            // 
            this.saveOverlayButton.Location = new System.Drawing.Point(420, 52);
            this.saveOverlayButton.Name = "saveOverlayButton";
            this.saveOverlayButton.Size = new System.Drawing.Size(80, 32);
            this.saveOverlayButton.TabIndex = 6;
            this.saveOverlayButton.Text = "Save";
            this.saveOverlayButton.UseVisualStyleBackColor = true;
            this.saveOverlayButton.Click += new System.EventHandler(this.ApplyOverlay);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Current Address:";
            // 
            // selectedLineNumberBox
            // 
            this.selectedLineNumberBox.Location = new System.Drawing.Point(120, 58);
            this.selectedLineNumberBox.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.selectedLineNumberBox.Name = "selectedLineNumberBox";
            this.selectedLineNumberBox.Size = new System.Drawing.Size(60, 22);
            this.selectedLineNumberBox.TabIndex = 9;
            this.selectedLineNumberBox.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // OverlayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 441);
            this.Controls.Add(this.selectedLineNumberBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveOverlayButton);
            this.Controls.Add(this.decompressButton);
            this.Controls.Add(this.fileNumComboBox);
            this.Controls.Add(this.textBoxDisplay);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "OverlayEditor";
            this.Text = "TextViewer";
            ((System.ComponentModel.ISupportInitialize)(this.selectedLineNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button decompressButton;
        public System.Windows.Forms.ComboBox fileNumComboBox;
        private System.Windows.Forms.Button saveOverlayButton;
        public System.Windows.Forms.RichTextBox textBoxDisplay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown selectedLineNumberBox;
    }
}