namespace TTG_Tools
{
    partial class ArchiveUnpacker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveUnpacker));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.FileBrowseBtn = new System.Windows.Forms.Button();
            this.ResourceDirBrowse = new System.Windows.Forms.Button();
            this.filePathTB = new System.Windows.Forms.TextBox();
            this.dirPathTB = new System.Windows.Forms.TextBox();
            this.filePathLabel = new System.Windows.Forms.Label();
            this.dirPathLabel = new System.Windows.Forms.Label();
            this.unpackBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.filterLabel = new System.Windows.Forms.Label();
            this.fileFormatCB = new System.Windows.Forms.ComboBox();
            this.encKeyList = new System.Windows.Forms.Label();
            this.encKeyListCB = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.customKeyTB = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 82);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(784, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // FileBrowseBtn
            // 
            this.FileBrowseBtn.Location = new System.Drawing.Point(764, 10);
            this.FileBrowseBtn.Name = "FileBrowseBtn";
            this.FileBrowseBtn.Size = new System.Drawing.Size(30, 23);
            this.FileBrowseBtn.TabIndex = 1;
            this.FileBrowseBtn.Text = "...";
            this.FileBrowseBtn.UseVisualStyleBackColor = true;
            this.FileBrowseBtn.Click += new System.EventHandler(this.FileBrowseBtn_Click);
            // 
            // ResourceDirBrowse
            // 
            this.ResourceDirBrowse.Location = new System.Drawing.Point(764, 40);
            this.ResourceDirBrowse.Name = "ResourceDirBrowse";
            this.ResourceDirBrowse.Size = new System.Drawing.Size(30, 23);
            this.ResourceDirBrowse.TabIndex = 2;
            this.ResourceDirBrowse.Text = "...";
            this.ResourceDirBrowse.UseVisualStyleBackColor = true;
            this.ResourceDirBrowse.Click += new System.EventHandler(this.ResourceDirBrowse_Click);
            // 
            // filePathTB
            // 
            this.filePathTB.Location = new System.Drawing.Point(238, 12);
            this.filePathTB.Name = "filePathTB";
            this.filePathTB.Size = new System.Drawing.Size(503, 20);
            this.filePathTB.TabIndex = 3;
            // 
            // dirPathTB
            // 
            this.dirPathTB.Location = new System.Drawing.Point(238, 42);
            this.dirPathTB.Name = "dirPathTB";
            this.dirPathTB.Size = new System.Drawing.Size(503, 20);
            this.dirPathTB.TabIndex = 4;
            // 
            // filePathLabel
            // 
            this.filePathLabel.AutoSize = true;
            this.filePathLabel.Location = new System.Drawing.Point(173, 15);
            this.filePathLabel.Name = "filePathLabel";
            this.filePathLabel.Size = new System.Drawing.Size(50, 13);
            this.filePathLabel.TabIndex = 5;
            this.filePathLabel.Text = "File path:";
            // 
            // dirPathLabel
            // 
            this.dirPathLabel.AutoSize = true;
            this.dirPathLabel.Location = new System.Drawing.Point(176, 45);
            this.dirPathLabel.Name = "dirPathLabel";
            this.dirPathLabel.Size = new System.Drawing.Size(47, 13);
            this.dirPathLabel.TabIndex = 6;
            this.dirPathLabel.Text = "Dir path:";
            // 
            // unpackBtn
            // 
            this.unpackBtn.Location = new System.Drawing.Point(719, 125);
            this.unpackBtn.Name = "unpackBtn";
            this.unpackBtn.Size = new System.Drawing.Size(75, 23);
            this.unpackBtn.TabIndex = 7;
            this.unpackBtn.Text = "Unpack";
            this.unpackBtn.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 165);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(784, 472);
            this.dataGridView1.TabIndex = 8;
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(33, 10);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(77, 13);
            this.filterLabel.TabIndex = 9;
            this.filterLabel.Text = "File format filter";
            // 
            // fileFormatCB
            // 
            this.fileFormatCB.FormattingEnabled = true;
            this.fileFormatCB.Location = new System.Drawing.Point(12, 37);
            this.fileFormatCB.Name = "fileFormatCB";
            this.fileFormatCB.Size = new System.Drawing.Size(118, 21);
            this.fileFormatCB.TabIndex = 10;
            // 
            // encKeyList
            // 
            this.encKeyList.AutoSize = true;
            this.encKeyList.Location = new System.Drawing.Point(348, 129);
            this.encKeyList.Name = "encKeyList";
            this.encKeyList.Size = new System.Drawing.Size(95, 13);
            this.encKeyList.TabIndex = 11;
            this.encKeyList.Text = "Encryption key list:";
            // 
            // encKeyListCB
            // 
            this.encKeyListCB.FormattingEnabled = true;
            this.encKeyListCB.Location = new System.Drawing.Point(450, 125);
            this.encKeyListCB.Name = "encKeyListCB";
            this.encKeyListCB.Size = new System.Drawing.Size(263, 21);
            this.encKeyListCB.TabIndex = 12;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(15, 130);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(81, 17);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "Custom key";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // customKeyTB
            // 
            this.customKeyTB.Location = new System.Drawing.Point(102, 127);
            this.customKeyTB.Name = "customKeyTB";
            this.customKeyTB.Size = new System.Drawing.Size(240, 20);
            this.customKeyTB.TabIndex = 14;
            // 
            // ArchiveUnpacker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 649);
            this.Controls.Add(this.customKeyTB);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.encKeyListCB);
            this.Controls.Add(this.encKeyList);
            this.Controls.Add(this.fileFormatCB);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.unpackBtn);
            this.Controls.Add(this.dirPathLabel);
            this.Controls.Add(this.filePathLabel);
            this.Controls.Add(this.dirPathTB);
            this.Controls.Add(this.filePathTB);
            this.Controls.Add(this.ResourceDirBrowse);
            this.Controls.Add(this.FileBrowseBtn);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ArchiveUnpacker";
            this.Text = "Archive unpacker";
            this.Load += new System.EventHandler(this.ArchiveUnpacker_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button FileBrowseBtn;
        private System.Windows.Forms.Button ResourceDirBrowse;
        private System.Windows.Forms.TextBox filePathTB;
        private System.Windows.Forms.TextBox dirPathTB;
        private System.Windows.Forms.Label filePathLabel;
        private System.Windows.Forms.Label dirPathLabel;
        private System.Windows.Forms.Button unpackBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.ComboBox fileFormatCB;
        private System.Windows.Forms.Label encKeyList;
        private System.Windows.Forms.ComboBox encKeyListCB;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox customKeyTB;
    }
}