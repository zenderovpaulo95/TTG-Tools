namespace TTG_Tools
{
    partial class AutoPacker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoPacker));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonDecrypt = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkIOS = new System.Windows.Forms.CheckBox();
            this.CheckNewEngine = new System.Windows.Forms.CheckBox();
            this.checkEncDDS = new System.Windows.Forms.CheckBox();
            this.checkUnicode = new System.Windows.Forms.CheckBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkEncLangdb = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkCustomKey = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DisplayMember = "0";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Before The Wolf Among Us",
            "Sam & Max 201",
            "Sam & Max 202",
            "Sam & Max 203",
            "Sam & Max 204",
            "Sam & Max 205",
            "After The Wolf Among Us"});
            this.comboBox1.Location = new System.Drawing.Point(22, 63);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(328, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(24, 128);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(124, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "Encrypt, Pack, Import";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(10, 171);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(760, 368);
            this.listBox1.TabIndex = 2;
            // 
            // buttonDecrypt
            // 
            this.buttonDecrypt.Location = new System.Drawing.Point(209, 128);
            this.buttonDecrypt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonDecrypt.Name = "buttonDecrypt";
            this.buttonDecrypt.Size = new System.Drawing.Size(124, 21);
            this.buttonDecrypt.TabIndex = 1;
            this.buttonDecrypt.Text = "Decrypt, Export";
            this.buttonDecrypt.UseVisualStyleBackColor = true;
            this.buttonDecrypt.Click += new System.EventHandler(this.buttonDecrypt_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.checkIOS);
            this.groupBox1.Controls.Add(this.CheckNewEngine);
            this.groupBox1.Controls.Add(this.checkEncDDS);
            this.groupBox1.Controls.Add(this.checkUnicode);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkEncLangdb);
            this.groupBox1.Location = new System.Drawing.Point(356, 35);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(413, 114);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Some functions";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(250, 17);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(142, 17);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Swizzle Nintendo Switch";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkIOS
            // 
            this.checkIOS.AutoSize = true;
            this.checkIOS.Location = new System.Drawing.Point(250, 38);
            this.checkIOS.Margin = new System.Windows.Forms.Padding(2);
            this.checkIOS.Name = "checkIOS";
            this.checkIOS.Size = new System.Drawing.Size(121, 17);
            this.checkIOS.TabIndex = 14;
            this.checkIOS.Text = "iOS (for new games)";
            this.checkIOS.UseVisualStyleBackColor = true;
            this.checkIOS.CheckedChanged += new System.EventHandler(this.checkIOS_CheckedChanged);
            // 
            // CheckNewEngine
            // 
            this.CheckNewEngine.AutoSize = true;
            this.CheckNewEngine.Location = new System.Drawing.Point(8, 86);
            this.CheckNewEngine.Margin = new System.Windows.Forms.Padding(2);
            this.CheckNewEngine.Name = "CheckNewEngine";
            this.CheckNewEngine.Size = new System.Drawing.Size(150, 17);
            this.CheckNewEngine.TabIndex = 13;
            this.CheckNewEngine.Text = "Lua scripts for new engine";
            this.CheckNewEngine.UseVisualStyleBackColor = true;
            this.CheckNewEngine.CheckedChanged += new System.EventHandler(this.CheckNewEngine_CheckedChanged);
            // 
            // checkEncDDS
            // 
            this.checkEncDDS.AutoSize = true;
            this.checkEncDDS.Location = new System.Drawing.Point(8, 38);
            this.checkEncDDS.Margin = new System.Windows.Forms.Padding(2);
            this.checkEncDDS.Name = "checkEncDDS";
            this.checkEncDDS.Size = new System.Drawing.Size(146, 17);
            this.checkEncDDS.TabIndex = 12;
            this.checkEncDDS.Text = "Encrypt DDS header only";
            this.checkEncDDS.UseVisualStyleBackColor = true;
            this.checkEncDDS.CheckedChanged += new System.EventHandler(this.checkEncDDS_CheckedChanged);
            // 
            // checkUnicode
            // 
            this.checkUnicode.AutoSize = true;
            this.checkUnicode.Location = new System.Drawing.Point(250, 61);
            this.checkUnicode.Margin = new System.Windows.Forms.Padding(2);
            this.checkUnicode.Name = "checkUnicode";
            this.checkUnicode.Size = new System.Drawing.Size(132, 17);
            this.checkUnicode.TabIndex = 11;
            this.checkUnicode.Text = "Unicode support is on.";
            this.checkUnicode.UseVisualStyleBackColor = true;
            this.checkUnicode.CheckedChanged += new System.EventHandler(this.checkUnicode_CheckedChanged);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Versions 2-6",
            "Versions 7-9"});
            this.comboBox2.Location = new System.Drawing.Point(120, 61);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(2);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(108, 21);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Method encryption:";
            // 
            // checkEncLangdb
            // 
            this.checkEncLangdb.AutoSize = true;
            this.checkEncLangdb.Location = new System.Drawing.Point(8, 15);
            this.checkEncLangdb.Margin = new System.Windows.Forms.Padding(2);
            this.checkEncLangdb.Name = "checkEncLangdb";
            this.checkEncLangdb.Size = new System.Drawing.Size(238, 17);
            this.checkEncLangdb.TabIndex = 7;
            this.checkEncLangdb.Text = "Encrypt langdb/d3dtx (fully encrypt d3dtx file)";
            this.checkEncLangdb.UseVisualStyleBackColor = true;
            this.checkEncLangdb.CheckedChanged += new System.EventHandler(this.checkEncLangdb_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Blowfish keys for encryption some old files or new compressed archives:";
            // 
            // checkCustomKey
            // 
            this.checkCustomKey.AutoSize = true;
            this.checkCustomKey.Location = new System.Drawing.Point(10, 97);
            this.checkCustomKey.Margin = new System.Windows.Forms.Padding(2);
            this.checkCustomKey.Name = "checkCustomKey";
            this.checkCustomKey.Size = new System.Drawing.Size(102, 17);
            this.checkCustomKey.TabIndex = 12;
            this.checkCustomKey.Text = "Set custom key:";
            this.checkCustomKey.UseVisualStyleBackColor = true;
            this.checkCustomKey.CheckedChanged += new System.EventHandler(this.checkCustomKey_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 95);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(239, 20);
            this.textBox1.TabIndex = 13;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(780, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // AutoPacker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 556);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkCustomKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonDecrypt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MinimumSize = new System.Drawing.Size(414, 293);
            this.Name = "AutoPacker";
            this.Text = "Auto(De)Packer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoPacker_FormClosing);
            this.Load += new System.EventHandler(this.AutoPacker_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonDecrypt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkEncLangdb;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkUnicode;
        private System.Windows.Forms.CheckBox CheckNewEngine;
        private System.Windows.Forms.CheckBox checkEncDDS;
        private System.Windows.Forms.CheckBox checkCustomKey;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkIOS;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}

