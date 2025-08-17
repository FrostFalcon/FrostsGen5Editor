
namespace NewEditor.Forms
{
    partial class FileExplorer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExplorer));
            this.fileTree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.fileText = new System.Windows.Forms.Label();
            this.importButton = new System.Windows.Forms.Button();
            this.extractButton = new System.Windows.Forms.Button();
            this.compressOverlayButton = new System.Windows.Forms.Button();
            this.packNarcButton = new System.Windows.Forms.Button();
            this.unpackNarcButton = new System.Windows.Forms.Button();
            this.statusText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // fileTree
            // 
            this.fileTree.ImageIndex = 0;
            this.fileTree.ImageList = this.imageList1;
            this.fileTree.Location = new System.Drawing.Point(12, 12);
            this.fileTree.Name = "fileTree";
            this.fileTree.PathSeparator = "/";
            this.fileTree.SelectedImageIndex = 0;
            this.fileTree.Size = new System.Drawing.Size(260, 437);
            this.fileTree.TabIndex = 0;
            this.fileTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.fileTree_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder.png");
            this.imageList1.Images.SetKeyName(1, "Narc.png");
            this.imageList1.Images.SetKeyName(2, "file.png");
            // 
            // fileText
            // 
            this.fileText.AutoSize = true;
            this.fileText.Location = new System.Drawing.Point(278, 12);
            this.fileText.Name = "fileText";
            this.fileText.Size = new System.Drawing.Size(0, 16);
            this.fileText.TabIndex = 1;
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(492, 417);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(80, 32);
            this.importButton.TabIndex = 2;
            this.importButton.Text = "Import";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // extractButton
            // 
            this.extractButton.Location = new System.Drawing.Point(406, 417);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(80, 32);
            this.extractButton.TabIndex = 3;
            this.extractButton.Text = "Extract";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // compressOverlayButton
            // 
            this.compressOverlayButton.Location = new System.Drawing.Point(472, 379);
            this.compressOverlayButton.Name = "compressOverlayButton";
            this.compressOverlayButton.Size = new System.Drawing.Size(100, 32);
            this.compressOverlayButton.TabIndex = 4;
            this.compressOverlayButton.Text = "Compress";
            this.compressOverlayButton.UseVisualStyleBackColor = true;
            this.compressOverlayButton.Visible = false;
            this.compressOverlayButton.Click += new System.EventHandler(this.compressOverlayButton_Click);
            // 
            // packNarcButton
            // 
            this.packNarcButton.Location = new System.Drawing.Point(492, 379);
            this.packNarcButton.Name = "packNarcButton";
            this.packNarcButton.Size = new System.Drawing.Size(80, 32);
            this.packNarcButton.TabIndex = 5;
            this.packNarcButton.Text = "Pack";
            this.packNarcButton.UseVisualStyleBackColor = true;
            this.packNarcButton.Visible = false;
            this.packNarcButton.Click += new System.EventHandler(this.packNarcButton_Click);
            // 
            // unpackNarcButton
            // 
            this.unpackNarcButton.Location = new System.Drawing.Point(406, 379);
            this.unpackNarcButton.Name = "unpackNarcButton";
            this.unpackNarcButton.Size = new System.Drawing.Size(80, 32);
            this.unpackNarcButton.TabIndex = 6;
            this.unpackNarcButton.Text = "Unpack";
            this.unpackNarcButton.UseVisualStyleBackColor = true;
            this.unpackNarcButton.Visible = false;
            this.unpackNarcButton.Click += new System.EventHandler(this.unpackNarcButton_Click);
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(9, 456);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(0, 16);
            this.statusText.TabIndex = 7;
            // 
            // FileExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 481);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.unpackNarcButton);
            this.Controls.Add(this.packNarcButton);
            this.Controls.Add(this.compressOverlayButton);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.importButton);
            this.Controls.Add(this.fileText);
            this.Controls.Add(this.fileTree);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FileExplorer";
            this.Text = "NDS File System";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView fileTree;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label fileText;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.Button compressOverlayButton;
        private System.Windows.Forms.Button packNarcButton;
        private System.Windows.Forms.Button unpackNarcButton;
        private System.Windows.Forms.Label statusText;
    }
}