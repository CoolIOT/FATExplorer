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
            this.comboBoxPartitions = new System.Windows.Forms.ComboBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewDirectory
            // 
            this.treeViewDirectory.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(171)))), ((int)(((byte)(171)))), ((int)(((byte)(171)))));
            this.treeViewDirectory.Location = new System.Drawing.Point(12, 46);
            this.treeViewDirectory.Name = "treeViewDirectory";
            this.treeViewDirectory.Size = new System.Drawing.Size(577, 529);
            this.treeViewDirectory.TabIndex = 0;
            // 
            // comboBoxPartitions
            // 
            this.comboBoxPartitions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPartitions.FormattingEnabled = true;
            this.comboBoxPartitions.Location = new System.Drawing.Point(13, 13);
            this.comboBoxPartitions.Name = "comboBoxPartitions";
            this.comboBoxPartitions.Size = new System.Drawing.Size(484, 21);
            this.comboBoxPartitions.TabIndex = 1;
            this.comboBoxPartitions.TabStop = false;
            this.comboBoxPartitions.SelectedIndexChanged += new System.EventHandler(this.comboBoxPartitions_SelectedIndexChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(514, 11);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 24);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "&Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // FATExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 587);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.comboBoxPartitions);
            this.Controls.Add(this.treeViewDirectory);
            this.Name = "FATExplorer";
            this.Text = "FATExplorer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewDirectory;
        private System.Windows.Forms.ComboBox comboBoxPartitions;
        private System.Windows.Forms.Button buttonRefresh;
    }
}

