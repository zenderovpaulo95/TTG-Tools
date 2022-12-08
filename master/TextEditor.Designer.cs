namespace TTG_Tools
{
    partial class TextEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextEditor));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.tabPagesControl = new System.Windows.Forms.TabControl();
            this.mergeTextTabPage = new System.Windows.Forms.TabPage();
            this.replaceTextTabPage = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabPagesControl.SuspendLayout();
            this.mergeTextTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 177);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(709, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(78, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(447, 20);
            this.textBox1.TabIndex = 9;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(78, 96);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(447, 20);
            this.textBox2.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(537, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(33, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(537, 94);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(33, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "First file:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Ready file:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(611, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 68);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Replace";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(18, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(70, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Single file";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(18, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(82, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Several files";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // tabPagesControl
            // 
            this.tabPagesControl.Controls.Add(this.mergeTextTabPage);
            this.tabPagesControl.Controls.Add(this.replaceTextTabPage);
            this.tabPagesControl.Location = new System.Drawing.Point(15, 12);
            this.tabPagesControl.Name = "tabPagesControl";
            this.tabPagesControl.SelectedIndex = 0;
            this.tabPagesControl.Size = new System.Drawing.Size(743, 286);
            this.tabPagesControl.TabIndex = 16;
            // 
            // mergeTextTabPage
            // 
            this.mergeTextTabPage.Controls.Add(this.button6);
            this.mergeTextTabPage.Controls.Add(this.button5);
            this.mergeTextTabPage.Controls.Add(this.button4);
            this.mergeTextTabPage.Controls.Add(this.label3);
            this.mergeTextTabPage.Controls.Add(this.progressBar1);
            this.mergeTextTabPage.Controls.Add(this.textBox3);
            this.mergeTextTabPage.Controls.Add(this.button3);
            this.mergeTextTabPage.Controls.Add(this.button1);
            this.mergeTextTabPage.Controls.Add(this.groupBox1);
            this.mergeTextTabPage.Controls.Add(this.textBox1);
            this.mergeTextTabPage.Controls.Add(this.label2);
            this.mergeTextTabPage.Controls.Add(this.textBox2);
            this.mergeTextTabPage.Controls.Add(this.label1);
            this.mergeTextTabPage.Controls.Add(this.button2);
            this.mergeTextTabPage.Location = new System.Drawing.Point(4, 22);
            this.mergeTextTabPage.Name = "mergeTextTabPage";
            this.mergeTextTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.mergeTextTabPage.Size = new System.Drawing.Size(735, 260);
            this.mergeTextTabPage.TabIndex = 0;
            this.mergeTextTabPage.Text = "Merge texts";
            this.mergeTextTabPage.UseVisualStyleBackColor = true;
            // 
            // replaceTextTabPage
            // 
            this.replaceTextTabPage.Location = new System.Drawing.Point(4, 22);
            this.replaceTextTabPage.Name = "replaceTextTabPage";
            this.replaceTextTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.replaceTextTabPage.Size = new System.Drawing.Size(735, 260);
            this.replaceTextTabPage.TabIndex = 1;
            this.replaceTextTabPage.Text = "Replace texts";
            this.replaceTextTabPage.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Second file:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(78, 59);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(447, 20);
            this.textBox3.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(537, 57);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(33, 23);
            this.button3.TabIndex = 17;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(93, 221);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 19;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(286, 221);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 20;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(495, 221);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 21;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // TextEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 305);
            this.Controls.Add(this.tabPagesControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TextEditor";
            this.Text = "TextEditor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPagesControl.ResumeLayout(false);
            this.mergeTextTabPage.ResumeLayout(false);
            this.mergeTextTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TabControl tabPagesControl;
        private System.Windows.Forms.TabPage mergeTextTabPage;
        private System.Windows.Forms.TabPage replaceTextTabPage;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button3;
    }
}