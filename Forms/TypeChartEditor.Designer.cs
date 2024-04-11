namespace NewEditor.Forms
{
    partial class TypeChartEditor
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
            this.label2 = new System.Windows.Forms.Label();
            this.tableAddressNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.recalculateAddressButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tableAddressNumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(478, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Defender";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 327);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Attacker";
            // 
            // tableAddressNumberBox
            // 
            this.tableAddressNumberBox.Location = new System.Drawing.Point(108, 12);
            this.tableAddressNumberBox.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.tableAddressNumberBox.Name = "tableAddressNumberBox";
            this.tableAddressNumberBox.Size = new System.Drawing.Size(120, 22);
            this.tableAddressNumberBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Table Address:";
            // 
            // recalculateAddressButton
            // 
            this.recalculateAddressButton.Location = new System.Drawing.Point(12, 40);
            this.recalculateAddressButton.Name = "recalculateAddressButton";
            this.recalculateAddressButton.Size = new System.Drawing.Size(100, 32);
            this.recalculateAddressButton.TabIndex = 4;
            this.recalculateAddressButton.Text = "Recalculate";
            this.recalculateAddressButton.UseVisualStyleBackColor = true;
            this.recalculateAddressButton.Click += new System.EventHandler(this.recalculateAddressButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(832, 14);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(100, 32);
            this.applyButton.TabIndex = 5;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // TypeChartEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 601);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.recalculateAddressButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableAddressNumberBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TypeChartEditor";
            this.Text = "Pokepatcher";
            ((System.ComponentModel.ISupportInitialize)(this.tableAddressNumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown tableAddressNumberBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button recalculateAddressButton;
        private System.Windows.Forms.Button applyButton;
    }
}