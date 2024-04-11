
namespace NewEditor.Forms
{
    partial class RandomMovesEditor
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
            this.totalMovesNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.moveSpacingNumberBox = new System.Windows.Forms.NumericUpDown();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.stabRatioNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.rngSeedNumberBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.totalMovesNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveSpacingNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stabRatioNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rngSeedNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // totalMovesNumberBox
            // 
            this.totalMovesNumberBox.Location = new System.Drawing.Point(142, 16);
            this.totalMovesNumberBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.totalMovesNumberBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.totalMovesNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.totalMovesNumberBox.Name = "totalMovesNumberBox";
            this.totalMovesNumberBox.Size = new System.Drawing.Size(47, 22);
            this.totalMovesNumberBox.TabIndex = 0;
            this.totalMovesNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(-18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Moves per Pokemon:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(-18, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Move Spacing:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // moveSpacingNumberBox
            // 
            this.moveSpacingNumberBox.Location = new System.Drawing.Point(142, 46);
            this.moveSpacingNumberBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.moveSpacingNumberBox.Name = "moveSpacingNumberBox";
            this.moveSpacingNumberBox.Size = new System.Drawing.Size(47, 22);
            this.moveSpacingNumberBox.TabIndex = 2;
            this.moveSpacingNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(12, 129);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 30);
            this.ApplyButton.TabIndex = 7;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyRandomMoves);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(-18, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Force STAB chance:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // stabRatioNumberBox
            // 
            this.stabRatioNumberBox.Location = new System.Drawing.Point(142, 76);
            this.stabRatioNumberBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.stabRatioNumberBox.Name = "stabRatioNumberBox";
            this.stabRatioNumberBox.Size = new System.Drawing.Size(47, 22);
            this.stabRatioNumberBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(236, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Seed:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rngSeedNumberBox
            // 
            this.rngSeedNumberBox.Location = new System.Drawing.Point(292, 16);
            this.rngSeedNumberBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rngSeedNumberBox.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.rngSeedNumberBox.Minimum = new decimal(new int[] {
            2000000000,
            0,
            0,
            -2147483648});
            this.rngSeedNumberBox.Name = "rngSeedNumberBox";
            this.rngSeedNumberBox.Size = new System.Drawing.Size(140, 22);
            this.rngSeedNumberBox.TabIndex = 12;
            // 
            // RandomMovesEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 171);
            this.Controls.Add(this.rngSeedNumberBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.stabRatioNumberBox);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.moveSpacingNumberBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.totalMovesNumberBox);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RandomMovesEditor";
            this.Text = "Random Moves Editor";
            ((System.ComponentModel.ISupportInitialize)(this.totalMovesNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveSpacingNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stabRatioNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rngSeedNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown totalMovesNumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown moveSpacingNumberBox;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown stabRatioNumberBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown rngSeedNumberBox;
    }
}