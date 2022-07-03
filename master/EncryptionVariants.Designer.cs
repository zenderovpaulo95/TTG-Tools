namespace TTG_Tools
{
    partial class EncryptionVariants
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbNew = new System.Windows.Forms.RadioButton();
            this.rbOld = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbEncFullFile = new System.Windows.Forms.RadioButton();
            this.rbEncTex = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.customKeyText = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(194, 181);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Do it!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNew);
            this.groupBox1.Controls.Add(this.rbOld);
            this.groupBox1.Location = new System.Drawing.Point(29, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(159, 84);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Encryption version";
            // 
            // rbNew
            // 
            this.rbNew.AutoSize = true;
            this.rbNew.Location = new System.Drawing.Point(7, 47);
            this.rbNew.Name = "rbNew";
            this.rbNew.Size = new System.Drawing.Size(115, 21);
            this.rbNew.TabIndex = 5;
            this.rbNew.TabStop = true;
            this.rbNew.Text = "New (ver 7-9)";
            this.rbNew.UseVisualStyleBackColor = true;
            // 
            // rbOld
            // 
            this.rbOld.AutoSize = true;
            this.rbOld.Location = new System.Drawing.Point(7, 21);
            this.rbOld.Name = "rbOld";
            this.rbOld.Size = new System.Drawing.Size(110, 21);
            this.rbOld.TabIndex = 5;
            this.rbOld.TabStop = true;
            this.rbOld.Text = "Old (ver 2-6)";
            this.rbOld.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbEncFullFile);
            this.groupBox2.Controls.Add(this.rbEncTex);
            this.groupBox2.Location = new System.Drawing.Point(194, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 84);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Encrypt font:";
            // 
            // rbEncFullFile
            // 
            this.rbEncFullFile.AutoSize = true;
            this.rbEncFullFile.Location = new System.Drawing.Point(7, 47);
            this.rbEncFullFile.Name = "rbEncFullFile";
            this.rbEncFullFile.Size = new System.Drawing.Size(99, 21);
            this.rbEncFullFile.TabIndex = 1;
            this.rbEncFullFile.TabStop = true;
            this.rbEncFullFile.Text = "Encrypt full";
            this.rbEncFullFile.UseVisualStyleBackColor = true;
            // 
            // rbEncTex
            // 
            this.rbEncTex.AutoSize = true;
            this.rbEncTex.Location = new System.Drawing.Point(7, 21);
            this.rbEncTex.Name = "rbEncTex";
            this.rbEncTex.Size = new System.Drawing.Size(203, 21);
            this.rbEncTex.TabIndex = 0;
            this.rbEncTex.TabStop = true;
            this.rbEncTex.Text = "Encrypt texture header only";
            this.rbEncTex.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(29, 148);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(429, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(29, 109);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(107, 21);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Custom key:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // customKeyText
            // 
            this.customKeyText.Location = new System.Drawing.Point(133, 109);
            this.customKeyText.Name = "customKeyText";
            this.customKeyText.Size = new System.Drawing.Size(320, 22);
            this.customKeyText.TabIndex = 6;
            // 
            // EncryptionVariants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 216);
            this.Controls.Add(this.customKeyText);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EncryptionVariants";
            this.Text = "Encryption methods";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EncryptionVariants_FormClosing);
            this.Load += new System.EventHandler(this.EncryptionVariants_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbNew;
        private System.Windows.Forms.RadioButton rbOld;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbEncFullFile;
        private System.Windows.Forms.RadioButton rbEncTex;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox customKeyText;
    }
}