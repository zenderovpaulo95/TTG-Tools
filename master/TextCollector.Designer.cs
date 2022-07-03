namespace TTG_Tools
{
    partial class TextCollector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextCollector));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenOrig = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenTranslated = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOpenOutput = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Clear = new System.Windows.Forms.Button();
            this.buttonDoItWithOne = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDoItWithAll = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.buttonOpenOrigFolder = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonOpenOutputFolder = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.buttonOpenTranslFolder = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(96, 23);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(287, 22);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original:";
            // 
            // buttonOpenOrig
            // 
            this.buttonOpenOrig.Location = new System.Drawing.Point(392, 20);
            this.buttonOpenOrig.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenOrig.Name = "buttonOpenOrig";
            this.buttonOpenOrig.Size = new System.Drawing.Size(63, 28);
            this.buttonOpenOrig.TabIndex = 2;
            this.buttonOpenOrig.Text = "Set";
            this.buttonOpenOrig.UseVisualStyleBackColor = true;
            this.buttonOpenOrig.Click += new System.EventHandler(this.buttonOpenOrig_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(96, 55);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(287, 22);
            this.textBox2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Translated:";
            // 
            // buttonOpenTranslated
            // 
            this.buttonOpenTranslated.Location = new System.Drawing.Point(392, 52);
            this.buttonOpenTranslated.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenTranslated.Name = "buttonOpenTranslated";
            this.buttonOpenTranslated.Size = new System.Drawing.Size(63, 28);
            this.buttonOpenTranslated.TabIndex = 2;
            this.buttonOpenTranslated.Text = "Set";
            this.buttonOpenTranslated.UseVisualStyleBackColor = true;
            this.buttonOpenTranslated.Click += new System.EventHandler(this.buttonOpenTranslated_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(96, 87);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(287, 22);
            this.textBox3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Output:";
            // 
            // buttonOpenOutput
            // 
            this.buttonOpenOutput.Location = new System.Drawing.Point(392, 84);
            this.buttonOpenOutput.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenOutput.Name = "buttonOpenOutput";
            this.buttonOpenOutput.Size = new System.Drawing.Size(63, 28);
            this.buttonOpenOutput.TabIndex = 2;
            this.buttonOpenOutput.Text = "Set";
            this.buttonOpenOutput.UseVisualStyleBackColor = true;
            this.buttonOpenOutput.Click += new System.EventHandler(this.buttonOpenOutput_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Clear);
            this.groupBox1.Controls.Add(this.buttonDoItWithOne);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.buttonOpenOutput);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.buttonOpenTranslated);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.buttonOpenOrig);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(467, 160);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "One file";
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(392, 121);
            this.button_Clear.Margin = new System.Windows.Forms.Padding(4);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(63, 28);
            this.button_Clear.TabIndex = 5;
            this.button_Clear.Text = "Clear!";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // buttonDoItWithOne
            // 
            this.buttonDoItWithOne.Location = new System.Drawing.Point(96, 121);
            this.buttonDoItWithOne.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDoItWithOne.Name = "buttonDoItWithOne";
            this.buttonDoItWithOne.Size = new System.Drawing.Size(288, 28);
            this.buttonDoItWithOne.TabIndex = 3;
            this.buttonDoItWithOne.Text = "Do it!";
            this.buttonDoItWithOne.UseVisualStyleBackColor = true;
            this.buttonDoItWithOne.Click += new System.EventHandler(this.buttonDoItWithOne_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonDoItWithAll);
            this.groupBox2.Controls.Add(this.textBox6);
            this.groupBox2.Controls.Add(this.buttonOpenOrigFolder);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.buttonOpenOutputFolder);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.buttonOpenTranslFolder);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(16, 183);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(467, 161);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "All in folder";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // buttonDoItWithAll
            // 
            this.buttonDoItWithAll.Location = new System.Drawing.Point(96, 121);
            this.buttonDoItWithAll.Margin = new System.Windows.Forms.Padding(4);
            this.buttonDoItWithAll.Name = "buttonDoItWithAll";
            this.buttonDoItWithAll.Size = new System.Drawing.Size(288, 28);
            this.buttonDoItWithAll.TabIndex = 3;
            this.buttonDoItWithAll.Text = "Do it!";
            this.buttonDoItWithAll.UseVisualStyleBackColor = true;
            this.buttonDoItWithAll.Click += new System.EventHandler(this.buttonDoItWithAll_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(96, 23);
            this.textBox6.Margin = new System.Windows.Forms.Padding(4);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(287, 22);
            this.textBox6.TabIndex = 0;
            // 
            // buttonOpenOrigFolder
            // 
            this.buttonOpenOrigFolder.Location = new System.Drawing.Point(392, 20);
            this.buttonOpenOrigFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenOrigFolder.Name = "buttonOpenOrigFolder";
            this.buttonOpenOrigFolder.Size = new System.Drawing.Size(63, 28);
            this.buttonOpenOrigFolder.TabIndex = 2;
            this.buttonOpenOrigFolder.Text = "Set";
            this.buttonOpenOrigFolder.UseVisualStyleBackColor = true;
            this.buttonOpenOrigFolder.Click += new System.EventHandler(this.buttonOpenOrigFolder_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 17);
            this.label6.TabIndex = 1;
            this.label6.Text = "Original:";
            // 
            // buttonOpenOutputFolder
            // 
            this.buttonOpenOutputFolder.Location = new System.Drawing.Point(392, 84);
            this.buttonOpenOutputFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenOutputFolder.Name = "buttonOpenOutputFolder";
            this.buttonOpenOutputFolder.Size = new System.Drawing.Size(63, 28);
            this.buttonOpenOutputFolder.TabIndex = 2;
            this.buttonOpenOutputFolder.Text = "Set";
            this.buttonOpenOutputFolder.UseVisualStyleBackColor = true;
            this.buttonOpenOutputFolder.Click += new System.EventHandler(this.buttonOpenOutputFolder_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(96, 87);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(287, 22);
            this.textBox4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 91);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Output:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(96, 55);
            this.textBox5.Margin = new System.Windows.Forms.Padding(4);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(287, 22);
            this.textBox5.TabIndex = 0;
            // 
            // buttonOpenTranslFolder
            // 
            this.buttonOpenTranslFolder.Location = new System.Drawing.Point(392, 52);
            this.buttonOpenTranslFolder.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenTranslFolder.Name = "buttonOpenTranslFolder";
            this.buttonOpenTranslFolder.Size = new System.Drawing.Size(63, 28);
            this.buttonOpenTranslFolder.TabIndex = 2;
            this.buttonOpenTranslFolder.Text = "Set";
            this.buttonOpenTranslFolder.UseVisualStyleBackColor = true;
            this.buttonOpenTranslFolder.Click += new System.EventHandler(this.buttonOpenTranslFolder_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 59);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Translated:";
            // 
            // TextCollector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 355);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TextCollector";
            this.Text = "Text Collector";
            this.Load += new System.EventHandler(this.TextCollector_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenOrig;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOpenTranslated;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOpenOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonDoItWithOne;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonDoItWithAll;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button buttonOpenOrigFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonOpenOutputFolder;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button buttonOpenTranslFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_Clear;
    }
}