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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoPacker));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tsvFilesRB = new System.Windows.Forms.RadioButton();
            this.txtFilesRB = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.comboBox1.Location = new System.Drawing.Point(30, 36);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(436, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 116);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 26);
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
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(13, 162);
            this.listBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(1071, 388);
            this.listBox1.TabIndex = 2;
            // 
            // buttonDecrypt
            // 
            this.buttonDecrypt.Location = new System.Drawing.Point(337, 116);
            this.buttonDecrypt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonDecrypt.Name = "buttonDecrypt";
            this.buttonDecrypt.Size = new System.Drawing.Size(165, 26);
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
            this.groupBox1.Location = new System.Drawing.Point(474, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 97);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Some functions";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(375, 70);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(171, 20);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "Swizzle Nintendo Switch";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkIOS
            // 
            this.checkIOS.AutoSize = true;
            this.checkIOS.Location = new System.Drawing.Point(188, 71);
            this.checkIOS.Name = "checkIOS";
            this.checkIOS.Size = new System.Drawing.Size(149, 20);
            this.checkIOS.TabIndex = 14;
            this.checkIOS.Text = "iOS (for new games)";
            this.checkIOS.UseVisualStyleBackColor = true;
            this.checkIOS.CheckedChanged += new System.EventHandler(this.checkIOS_CheckedChanged);
            // 
            // CheckNewEngine
            // 
            this.CheckNewEngine.AutoSize = true;
            this.CheckNewEngine.Location = new System.Drawing.Point(375, 44);
            this.CheckNewEngine.Name = "CheckNewEngine";
            this.CheckNewEngine.Size = new System.Drawing.Size(182, 20);
            this.CheckNewEngine.TabIndex = 13;
            this.CheckNewEngine.Text = "Lua scripts for new engine";
            this.CheckNewEngine.UseVisualStyleBackColor = true;
            this.CheckNewEngine.CheckedChanged += new System.EventHandler(this.CheckNewEngine_CheckedChanged);
            // 
            // checkEncDDS
            // 
            this.checkEncDDS.AutoSize = true;
            this.checkEncDDS.Location = new System.Drawing.Point(375, 19);
            this.checkEncDDS.Name = "checkEncDDS";
            this.checkEncDDS.Size = new System.Drawing.Size(180, 20);
            this.checkEncDDS.TabIndex = 12;
            this.checkEncDDS.Text = "Encrypt DDS header only";
            this.checkEncDDS.UseVisualStyleBackColor = true;
            this.checkEncDDS.CheckedChanged += new System.EventHandler(this.checkEncDDS_CheckedChanged);
            // 
            // checkUnicode
            // 
            this.checkUnicode.AutoSize = true;
            this.checkUnicode.Location = new System.Drawing.Point(10, 71);
            this.checkUnicode.Name = "checkUnicode";
            this.checkUnicode.Size = new System.Drawing.Size(162, 20);
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
            this.comboBox2.Location = new System.Drawing.Point(163, 42);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(143, 24);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "Method encryption:";
            // 
            // checkEncLangdb
            // 
            this.checkEncLangdb.AutoSize = true;
            this.checkEncLangdb.Location = new System.Drawing.Point(10, 19);
            this.checkEncLangdb.Name = "checkEncLangdb";
            this.checkEncLangdb.Size = new System.Drawing.Size(291, 20);
            this.checkEncLangdb.TabIndex = 7;
            this.checkEncLangdb.Text = "Encrypt langdb/d3dtx (fully encrypt d3dtx file)";
            this.checkEncLangdb.UseVisualStyleBackColor = true;
            this.checkEncLangdb.CheckedChanged += new System.EventHandler(this.checkEncLangdb_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(435, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Blowfish keys for encryption some old files or new compressed archives:";
            // 
            // checkCustomKey
            // 
            this.checkCustomKey.AutoSize = true;
            this.checkCustomKey.Location = new System.Drawing.Point(13, 77);
            this.checkCustomKey.Name = "checkCustomKey";
            this.checkCustomKey.Size = new System.Drawing.Size(123, 20);
            this.checkCustomKey.TabIndex = 12;
            this.checkCustomKey.Text = "Set custom key:";
            this.checkCustomKey.UseVisualStyleBackColor = true;
            this.checkCustomKey.CheckedChanged += new System.EventHandler(this.checkCustomKey_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(149, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(317, 22);
            this.textBox1.TabIndex = 13;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tsvFilesRB);
            this.groupBox3.Controls.Add(this.txtFilesRB);
            this.groupBox3.Location = new System.Drawing.Point(510, 104);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(432, 52);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Save files in:";
            // 
            // tsvFilesRB
            // 
            this.tsvFilesRB.AutoSize = true;
            this.tsvFilesRB.Location = new System.Drawing.Point(158, 19);
            this.tsvFilesRB.Name = "tsvFilesRB";
            this.tsvFilesRB.Size = new System.Drawing.Size(199, 20);
            this.tsvFilesRB.TabIndex = 1;
            this.tsvFilesRB.TabStop = true;
            this.tsvFilesRB.Text = "tsv format (for Google tables)";
            this.tsvFilesRB.UseVisualStyleBackColor = true;
            this.tsvFilesRB.CheckedChanged += new System.EventHandler(this.tsvFilesRB_CheckedChanged);
            // 
            // txtFilesRB
            // 
            this.txtFilesRB.AutoSize = true;
            this.txtFilesRB.Location = new System.Drawing.Point(22, 19);
            this.txtFilesRB.Name = "txtFilesRB";
            this.txtFilesRB.Size = new System.Drawing.Size(80, 20);
            this.txtFilesRB.TabIndex = 0;
            this.txtFilesRB.TabStop = true;
            this.txtFilesRB.Text = "txt format";
            this.txtFilesRB.UseVisualStyleBackColor = true;
            this.txtFilesRB.CheckedChanged += new System.EventHandler(this.txtFilesRB_CheckedChanged);
            // 
            // AutoPacker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 559);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkCustomKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonDecrypt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MinimumSize = new System.Drawing.Size(547, 352);
            this.Name = "AutoPacker";
            this.Text = "Auto(De)Packer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoPacker_FormClosing);
            this.Load += new System.EventHandler(this.AutoPacker_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timer1;
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton tsvFilesRB;
        private System.Windows.Forms.RadioButton txtFilesRB;
        private System.Windows.Forms.CheckBox checkIOS;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}

