
namespace NewEditor.Forms
{
    partial class PatchMaker
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
            this.label1 = new System.Windows.Forms.Label();
            this.changesListBox = new System.Windows.Forms.CheckedListBox();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.detailsTextBox = new System.Windows.Forms.RichTextBox();
            this.savePatchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Changes Found:";
            // 
            // changesListBox
            // 
            this.changesListBox.FormattingEnabled = true;
            this.changesListBox.Location = new System.Drawing.Point(15, 30);
            this.changesListBox.Name = "changesListBox";
            this.changesListBox.Size = new System.Drawing.Size(160, 242);
            this.changesListBox.TabIndex = 1;
            this.changesListBox.SelectedIndexChanged += new System.EventHandler(this.changesListBox_SelectedIndexChanged);
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Location = new System.Drawing.Point(181, 32);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(0, 16);
            this.sizeLabel.TabIndex = 2;
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Location = new System.Drawing.Point(184, 65);
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.Size = new System.Drawing.Size(268, 160);
            this.detailsTextBox.TabIndex = 3;
            this.detailsTextBox.Text = "";
            // 
            // savePatchButton
            // 
            this.savePatchButton.Location = new System.Drawing.Point(184, 240);
            this.savePatchButton.Name = "savePatchButton";
            this.savePatchButton.Size = new System.Drawing.Size(100, 32);
            this.savePatchButton.TabIndex = 4;
            this.savePatchButton.Text = "Save Patch";
            this.savePatchButton.UseVisualStyleBackColor = true;
            this.savePatchButton.Click += new System.EventHandler(this.savePatchButton_Click);
            // 
            // PatchMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 281);
            this.Controls.Add(this.savePatchButton);
            this.Controls.Add(this.detailsTextBox);
            this.Controls.Add(this.sizeLabel);
            this.Controls.Add(this.changesListBox);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PatchMaker";
            this.Text = "Create Patch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox changesListBox;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.RichTextBox detailsTextBox;
        private System.Windows.Forms.Button savePatchButton;
    }
}