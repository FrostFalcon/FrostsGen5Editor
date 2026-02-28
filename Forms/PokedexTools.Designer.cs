
namespace NewEditor.Forms
{
    partial class PokedexTools
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
            this.statusText = new System.Windows.Forms.Label();
            this.saveRegionalButton = new System.Windows.Forms.Button();
            this.regionalDexListBox = new System.Windows.Forms.ListBox();
            this.regionalGroupBox = new System.Windows.Forms.GroupBox();
            this.regionalNameDropdown = new System.Windows.Forms.ComboBox();
            this.addRegionalButton = new System.Windows.Forms.Button();
            this.clearRegionalButton = new System.Windows.Forms.Button();
            this.removeRegionalButton = new System.Windows.Forms.Button();
            this.moveRegionalDownButton = new System.Windows.Forms.Button();
            this.moveRegionalUpButton = new System.Windows.Forms.Button();
            this.syncHabitatsButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.regionalGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(28, 346);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 16);
            this.statusText.TabIndex = 54;
            // 
            // saveRegionalButton
            // 
            this.saveRegionalButton.Location = new System.Drawing.Point(334, 315);
            this.saveRegionalButton.Name = "saveRegionalButton";
            this.saveRegionalButton.Size = new System.Drawing.Size(80, 30);
            this.saveRegionalButton.TabIndex = 55;
            this.saveRegionalButton.Text = "Save";
            this.saveRegionalButton.UseVisualStyleBackColor = true;
            this.saveRegionalButton.Click += new System.EventHandler(this.saveRegionalButton_Click);
            // 
            // regionalDexListBox
            // 
            this.regionalDexListBox.FormattingEnabled = true;
            this.regionalDexListBox.ItemHeight = 16;
            this.regionalDexListBox.Location = new System.Drawing.Point(6, 21);
            this.regionalDexListBox.Name = "regionalDexListBox";
            this.regionalDexListBox.Size = new System.Drawing.Size(196, 324);
            this.regionalDexListBox.TabIndex = 56;
            // 
            // regionalGroupBox
            // 
            this.regionalGroupBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.regionalGroupBox.Controls.Add(this.regionalNameDropdown);
            this.regionalGroupBox.Controls.Add(this.addRegionalButton);
            this.regionalGroupBox.Controls.Add(this.clearRegionalButton);
            this.regionalGroupBox.Controls.Add(this.removeRegionalButton);
            this.regionalGroupBox.Controls.Add(this.moveRegionalDownButton);
            this.regionalGroupBox.Controls.Add(this.moveRegionalUpButton);
            this.regionalGroupBox.Controls.Add(this.saveRegionalButton);
            this.regionalGroupBox.Controls.Add(this.regionalDexListBox);
            this.regionalGroupBox.Location = new System.Drawing.Point(12, 12);
            this.regionalGroupBox.Name = "regionalGroupBox";
            this.regionalGroupBox.Size = new System.Drawing.Size(420, 360);
            this.regionalGroupBox.TabIndex = 85;
            this.regionalGroupBox.TabStop = false;
            this.regionalGroupBox.Text = "Regional Dex";
            // 
            // regionalNameDropdown
            // 
            this.regionalNameDropdown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.regionalNameDropdown.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.regionalNameDropdown.FormattingEnabled = true;
            this.regionalNameDropdown.Location = new System.Drawing.Point(208, 189);
            this.regionalNameDropdown.Name = "regionalNameDropdown";
            this.regionalNameDropdown.Size = new System.Drawing.Size(127, 24);
            this.regionalNameDropdown.TabIndex = 91;
            // 
            // addRegionalButton
            // 
            this.addRegionalButton.Location = new System.Drawing.Point(208, 219);
            this.addRegionalButton.Name = "addRegionalButton";
            this.addRegionalButton.Size = new System.Drawing.Size(80, 30);
            this.addRegionalButton.TabIndex = 90;
            this.addRegionalButton.Text = "Add After";
            this.addRegionalButton.UseVisualStyleBackColor = true;
            this.addRegionalButton.Click += new System.EventHandler(this.addRegionalButton_Click);
            // 
            // clearRegionalButton
            // 
            this.clearRegionalButton.Location = new System.Drawing.Point(334, 279);
            this.clearRegionalButton.Name = "clearRegionalButton";
            this.clearRegionalButton.Size = new System.Drawing.Size(80, 30);
            this.clearRegionalButton.TabIndex = 89;
            this.clearRegionalButton.Text = "Clear";
            this.clearRegionalButton.UseVisualStyleBackColor = true;
            this.clearRegionalButton.Click += new System.EventHandler(this.clearRegionalButton_Click);
            // 
            // removeRegionalButton
            // 
            this.removeRegionalButton.Location = new System.Drawing.Point(208, 315);
            this.removeRegionalButton.Name = "removeRegionalButton";
            this.removeRegionalButton.Size = new System.Drawing.Size(80, 30);
            this.removeRegionalButton.TabIndex = 88;
            this.removeRegionalButton.Text = "Remove";
            this.removeRegionalButton.UseVisualStyleBackColor = true;
            this.removeRegionalButton.Click += new System.EventHandler(this.removeRegionalButton_Click);
            // 
            // moveRegionalDownButton
            // 
            this.moveRegionalDownButton.Location = new System.Drawing.Point(208, 285);
            this.moveRegionalDownButton.Name = "moveRegionalDownButton";
            this.moveRegionalDownButton.Size = new System.Drawing.Size(24, 24);
            this.moveRegionalDownButton.TabIndex = 87;
            this.moveRegionalDownButton.Text = "v";
            this.moveRegionalDownButton.UseVisualStyleBackColor = true;
            this.moveRegionalDownButton.Click += new System.EventHandler(this.moveRegionalDownButton_Click);
            // 
            // moveRegionalUpButton
            // 
            this.moveRegionalUpButton.Location = new System.Drawing.Point(208, 255);
            this.moveRegionalUpButton.Name = "moveRegionalUpButton";
            this.moveRegionalUpButton.Size = new System.Drawing.Size(24, 24);
            this.moveRegionalUpButton.TabIndex = 86;
            this.moveRegionalUpButton.Text = "^";
            this.moveRegionalUpButton.UseVisualStyleBackColor = true;
            this.moveRegionalUpButton.Click += new System.EventHandler(this.moveRegionalUpButton_Click);
            // 
            // syncHabitatsButton
            // 
            this.syncHabitatsButton.Location = new System.Drawing.Point(542, 12);
            this.syncHabitatsButton.Name = "syncHabitatsButton";
            this.syncHabitatsButton.Size = new System.Drawing.Size(120, 48);
            this.syncHabitatsButton.TabIndex = 86;
            this.syncHabitatsButton.Text = "Sync habitat list with encounters";
            this.syncHabitatsButton.UseVisualStyleBackColor = true;
            this.syncHabitatsButton.Click += new System.EventHandler(this.syncHabitatsButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(10, 376);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 16);
            this.statusLabel.TabIndex = 87;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(525, 360);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 32);
            this.label1.TabIndex = 88;
            this.label1.Text = "(More features may be\r\nadded in the future)";
            // 
            // PokedexTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 401);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.syncHabitatsButton);
            this.Controls.Add(this.regionalGroupBox);
            this.Controls.Add(this.statusText);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "PokedexTools";
            this.Text = "Pokedex Tools";
            this.regionalGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label statusText;
        private System.Windows.Forms.Button saveRegionalButton;
        private System.Windows.Forms.ListBox regionalDexListBox;
        private System.Windows.Forms.GroupBox regionalGroupBox;
        private System.Windows.Forms.Button removeRegionalButton;
        private System.Windows.Forms.Button moveRegionalDownButton;
        private System.Windows.Forms.Button moveRegionalUpButton;
        private System.Windows.Forms.Button addRegionalButton;
        private System.Windows.Forms.Button clearRegionalButton;
        private System.Windows.Forms.ComboBox regionalNameDropdown;
        private System.Windows.Forms.Button syncHabitatsButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label label1;
    }
}