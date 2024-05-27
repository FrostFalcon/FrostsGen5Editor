
namespace NewEditor.Forms
{
    partial class TypeSwapEditor
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numCyclesBox = new System.Windows.Forms.NumericUpDown();
            this.cycleButton = new System.Windows.Forms.Button();
            this.normalButton = new System.Windows.Forms.Button();
            this.oppositesButton = new System.Windows.Forms.Button();
            this.randomButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.randomEffectivenessButton = new System.Windows.Forms.Button();
            this.normalEffectivenessNumberBox = new System.Windows.Forms.NumericUpDown();
            this.superEffectiveNumberBox = new System.Windows.Forms.NumericUpDown();
            this.notVeryEffectiveNumberBox = new System.Windows.Forms.NumericUpDown();
            this.noEffectNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.forcedSENumberBox = new System.Windows.Forms.NumericUpDown();
            this.forcedNVENumberBox = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCyclesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.normalEffectivenessNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superEffectiveNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notVeryEffectiveNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noEffectNumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.forcedSENumberBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.forcedNVENumberBox)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.numCyclesBox);
            this.groupBox1.Controls.Add(this.cycleButton);
            this.groupBox1.Controls.Add(this.normalButton);
            this.groupBox1.Controls.Add(this.oppositesButton);
            this.groupBox1.Controls.Add(this.randomButton);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 80);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Presets";
            // 
            // numCyclesBox
            // 
            this.numCyclesBox.Location = new System.Drawing.Point(370, 35);
            this.numCyclesBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numCyclesBox.Name = "numCyclesBox";
            this.numCyclesBox.Size = new System.Drawing.Size(40, 22);
            this.numCyclesBox.TabIndex = 5;
            // 
            // cycleButton
            // 
            this.cycleButton.Location = new System.Drawing.Point(420, 30);
            this.cycleButton.Name = "cycleButton";
            this.cycleButton.Size = new System.Drawing.Size(80, 30);
            this.cycleButton.TabIndex = 4;
            this.cycleButton.Text = "Cycle";
            this.cycleButton.UseVisualStyleBackColor = true;
            this.cycleButton.Click += new System.EventHandler(this.CycleElements);
            // 
            // normalButton
            // 
            this.normalButton.Location = new System.Drawing.Point(220, 30);
            this.normalButton.Name = "normalButton";
            this.normalButton.Size = new System.Drawing.Size(80, 30);
            this.normalButton.TabIndex = 2;
            this.normalButton.Text = "Normalize";
            this.normalButton.UseVisualStyleBackColor = true;
            this.normalButton.Click += new System.EventHandler(this.NormalizeElements);
            // 
            // oppositesButton
            // 
            this.oppositesButton.Location = new System.Drawing.Point(120, 30);
            this.oppositesButton.Name = "oppositesButton";
            this.oppositesButton.Size = new System.Drawing.Size(80, 30);
            this.oppositesButton.TabIndex = 1;
            this.oppositesButton.Text = "Opposites";
            this.oppositesButton.UseVisualStyleBackColor = true;
            this.oppositesButton.Click += new System.EventHandler(this.OppositeElements);
            // 
            // randomButton
            // 
            this.randomButton.Location = new System.Drawing.Point(20, 30);
            this.randomButton.Name = "randomButton";
            this.randomButton.Size = new System.Drawing.Size(80, 30);
            this.randomButton.TabIndex = 0;
            this.randomButton.Text = "Random";
            this.randomButton.UseVisualStyleBackColor = true;
            this.randomButton.Click += new System.EventHandler(this.RandomizeElements);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(12, 519);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(100, 30);
            this.ApplyButton.TabIndex = 6;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyTypeSwap);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 516);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Once applied, changes cannot be undone.\r\nMake sure you back up your rom files.\r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // randomEffectivenessButton
            // 
            this.randomEffectivenessButton.Location = new System.Drawing.Point(632, 499);
            this.randomEffectivenessButton.Name = "randomEffectivenessButton";
            this.randomEffectivenessButton.Size = new System.Drawing.Size(140, 50);
            this.randomEffectivenessButton.TabIndex = 8;
            this.randomEffectivenessButton.Text = "Randomize Type Effectiveness";
            this.randomEffectivenessButton.UseVisualStyleBackColor = true;
            this.randomEffectivenessButton.Visible = false;
            this.randomEffectivenessButton.Click += new System.EventHandler(this.randomEffectivenessButton_Click);
            // 
            // normalEffectivenessNumberBox
            // 
            this.normalEffectivenessNumberBox.Location = new System.Drawing.Point(712, 356);
            this.normalEffectivenessNumberBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.normalEffectivenessNumberBox.Name = "normalEffectivenessNumberBox";
            this.normalEffectivenessNumberBox.Size = new System.Drawing.Size(60, 22);
            this.normalEffectivenessNumberBox.TabIndex = 6;
            this.normalEffectivenessNumberBox.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.normalEffectivenessNumberBox.Visible = false;
            // 
            // superEffectiveNumberBox
            // 
            this.superEffectiveNumberBox.Location = new System.Drawing.Point(712, 384);
            this.superEffectiveNumberBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.superEffectiveNumberBox.Name = "superEffectiveNumberBox";
            this.superEffectiveNumberBox.Size = new System.Drawing.Size(60, 22);
            this.superEffectiveNumberBox.TabIndex = 9;
            this.superEffectiveNumberBox.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.superEffectiveNumberBox.Visible = false;
            // 
            // notVeryEffectiveNumberBox
            // 
            this.notVeryEffectiveNumberBox.Location = new System.Drawing.Point(712, 328);
            this.notVeryEffectiveNumberBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.notVeryEffectiveNumberBox.Name = "notVeryEffectiveNumberBox";
            this.notVeryEffectiveNumberBox.Size = new System.Drawing.Size(60, 22);
            this.notVeryEffectiveNumberBox.TabIndex = 10;
            this.notVeryEffectiveNumberBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.notVeryEffectiveNumberBox.Visible = false;
            // 
            // noEffectNumberBox
            // 
            this.noEffectNumberBox.Location = new System.Drawing.Point(712, 300);
            this.noEffectNumberBox.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.noEffectNumberBox.Name = "noEffectNumberBox";
            this.noEffectNumberBox.Size = new System.Drawing.Size(60, 22);
            this.noEffectNumberBox.TabIndex = 11;
            this.noEffectNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.noEffectNumberBox.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(640, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "No Effect:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(591, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Not Very Effective:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(572, 358);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Normal Effectiveness:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(607, 386);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Super Effective:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label5.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(709, 274);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 18);
            this.label6.TabIndex = 16;
            this.label6.Text = "Weight";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(560, 460);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 16);
            this.label7.TabIndex = 20;
            this.label7.Text = "Forced super effectives:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label7.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(547, 432);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(157, 16);
            this.label8.TabIndex = 19;
            this.label8.Text = "Forced not very effectives:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label8.Visible = false;
            // 
            // forcedSENumberBox
            // 
            this.forcedSENumberBox.Location = new System.Drawing.Point(712, 458);
            this.forcedSENumberBox.Maximum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.forcedSENumberBox.Name = "forcedSENumberBox";
            this.forcedSENumberBox.Size = new System.Drawing.Size(60, 22);
            this.forcedSENumberBox.TabIndex = 18;
            this.forcedSENumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.forcedSENumberBox.Visible = false;
            // 
            // forcedNVENumberBox
            // 
            this.forcedNVENumberBox.Location = new System.Drawing.Point(712, 430);
            this.forcedNVENumberBox.Maximum = new decimal(new int[] {
            17,
            0,
            0,
            0});
            this.forcedNVENumberBox.Name = "forcedNVENumberBox";
            this.forcedNVENumberBox.Size = new System.Drawing.Size(60, 22);
            this.forcedNVENumberBox.TabIndex = 17;
            this.forcedNVENumberBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.forcedNVENumberBox.Visible = false;
            // 
            // TypeSwapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.forcedSENumberBox);
            this.Controls.Add(this.forcedNVENumberBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.noEffectNumberBox);
            this.Controls.Add(this.notVeryEffectiveNumberBox);
            this.Controls.Add(this.superEffectiveNumberBox);
            this.Controls.Add(this.normalEffectivenessNumberBox);
            this.Controls.Add(this.randomEffectivenessButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TypeSwapEditor";
            this.Text = "TypeSwapEditor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numCyclesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.normalEffectivenessNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superEffectiveNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notVeryEffectiveNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noEffectNumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.forcedSENumberBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.forcedNVENumberBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numCyclesBox;
        private System.Windows.Forms.Button cycleButton;
        private System.Windows.Forms.Button normalButton;
        private System.Windows.Forms.Button oppositesButton;
        private System.Windows.Forms.Button randomButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button randomEffectivenessButton;
        private System.Windows.Forms.NumericUpDown normalEffectivenessNumberBox;
        private System.Windows.Forms.NumericUpDown superEffectiveNumberBox;
        private System.Windows.Forms.NumericUpDown notVeryEffectiveNumberBox;
        private System.Windows.Forms.NumericUpDown noEffectNumberBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown forcedSENumberBox;
        private System.Windows.Forms.NumericUpDown forcedNVENumberBox;
    }
}