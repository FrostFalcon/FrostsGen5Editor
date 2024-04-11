namespace NewEditor.Forms
{
    partial class PaletteEditor
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
            this.pokemonSpriteBox = new System.Windows.Forms.PictureBox();
            this.applyPaletteButton = new System.Windows.Forms.Button();
            this.backSpriteBox = new System.Windows.Forms.PictureBox();
            this.shinyCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backSpriteBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pokemonSpriteBox
            // 
            this.pokemonSpriteBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pokemonSpriteBox.Location = new System.Drawing.Point(12, 12);
            this.pokemonSpriteBox.Name = "pokemonSpriteBox";
            this.pokemonSpriteBox.Size = new System.Drawing.Size(320, 320);
            this.pokemonSpriteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pokemonSpriteBox.TabIndex = 0;
            this.pokemonSpriteBox.TabStop = false;
            // 
            // applyPaletteButton
            // 
            this.applyPaletteButton.Location = new System.Drawing.Point(12, 338);
            this.applyPaletteButton.Name = "applyPaletteButton";
            this.applyPaletteButton.Size = new System.Drawing.Size(120, 35);
            this.applyPaletteButton.TabIndex = 1;
            this.applyPaletteButton.Text = "Apply Palette";
            this.applyPaletteButton.UseVisualStyleBackColor = true;
            this.applyPaletteButton.Click += new System.EventHandler(this.applyPaletteButton_Click);
            // 
            // backSpriteBox
            // 
            this.backSpriteBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.backSpriteBox.Location = new System.Drawing.Point(12, 379);
            this.backSpriteBox.Name = "backSpriteBox";
            this.backSpriteBox.Size = new System.Drawing.Size(320, 320);
            this.backSpriteBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.backSpriteBox.TabIndex = 2;
            this.backSpriteBox.TabStop = false;
            // 
            // shinyCheckBox
            // 
            this.shinyCheckBox.AutoSize = true;
            this.shinyCheckBox.Location = new System.Drawing.Point(339, 13);
            this.shinyCheckBox.Name = "shinyCheckBox";
            this.shinyCheckBox.Size = new System.Drawing.Size(59, 20);
            this.shinyCheckBox.TabIndex = 3;
            this.shinyCheckBox.Text = "Shiny";
            this.shinyCheckBox.UseVisualStyleBackColor = true;
            this.shinyCheckBox.CheckedChanged += new System.EventHandler(this.shinyCheckBox_CheckedChanged);
            // 
            // PaletteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 721);
            this.Controls.Add(this.shinyCheckBox);
            this.Controls.Add(this.backSpriteBox);
            this.Controls.Add(this.applyPaletteButton);
            this.Controls.Add(this.pokemonSpriteBox);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PaletteEditor";
            this.Text = "PaletteEditor";
            ((System.ComponentModel.ISupportInitialize)(this.pokemonSpriteBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backSpriteBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pokemonSpriteBox;
        private System.Windows.Forms.Button applyPaletteButton;
        private System.Windows.Forms.PictureBox backSpriteBox;
        private System.Windows.Forms.CheckBox shinyCheckBox;
    }
}