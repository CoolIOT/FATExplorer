namespace FATExplorer
{
    partial class FATExplorer
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
            this.treeViewDirectory = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeViewDirectory
            // 
            this.treeViewDirectory.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.treeViewDirectory.Location = new System.Drawing.Point(12, 12);
            this.treeViewDirectory.Name = "treeViewDirectory";
            this.treeViewDirectory.Size = new System.Drawing.Size(577, 563);
            this.treeViewDirectory.TabIndex = 0;
            // 
            // FATExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 587);
            this.Controls.Add(this.treeViewDirectory);
            this.Name = "FATExplorer";
            this.Text = "FATExplorer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewDirectory;
    }
}

