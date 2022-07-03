namespace TTG_Tools
{
    partial class MainMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
            this.OpenAutopackerForm = new System.Windows.Forms.Button();
            this.About = new System.Windows.Forms.Button();
            this.RunFontEditor = new System.Windows.Forms.Button();
            this.buttonTextCollector = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OpenAutopackerForm
            // 
            this.OpenAutopackerForm.Location = new System.Drawing.Point(16, 13);
            this.OpenAutopackerForm.Margin = new System.Windows.Forms.Padding(4);
            this.OpenAutopackerForm.Name = "OpenAutopackerForm";
            this.OpenAutopackerForm.Size = new System.Drawing.Size(149, 28);
            this.OpenAutopackerForm.TabIndex = 0;
            this.OpenAutopackerForm.Text = "Auto(De)Packer";
            this.OpenAutopackerForm.UseVisualStyleBackColor = true;
            this.OpenAutopackerForm.Click += new System.EventHandler(this.OpenAutopacker_Form_Click);
            // 
            // About
            // 
            this.About.Location = new System.Drawing.Point(212, 104);
            this.About.Margin = new System.Windows.Forms.Padding(4);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(149, 28);
            this.About.TabIndex = 1;
            this.About.Text = "About";
            this.About.UseVisualStyleBackColor = true;
            this.About.Click += new System.EventHandler(this.About_Click);
            // 
            // RunFontEditor
            // 
            this.RunFontEditor.Location = new System.Drawing.Point(212, 13);
            this.RunFontEditor.Margin = new System.Windows.Forms.Padding(4);
            this.RunFontEditor.Name = "RunFontEditor";
            this.RunFontEditor.Size = new System.Drawing.Size(148, 28);
            this.RunFontEditor.TabIndex = 2;
            this.RunFontEditor.Text = "Font Editor";
            this.RunFontEditor.UseVisualStyleBackColor = true;
            this.RunFontEditor.Click += new System.EventHandler(this.RunFontEditor_Click);
            // 
            // buttonTextCollector
            // 
            this.buttonTextCollector.Location = new System.Drawing.Point(16, 244);
            this.buttonTextCollector.Margin = new System.Windows.Forms.Padding(4);
            this.buttonTextCollector.Name = "buttonTextCollector";
            this.buttonTextCollector.Size = new System.Drawing.Size(148, 28);
            this.buttonTextCollector.TabIndex = 6;
            this.buttonTextCollector.Text = "Text Collector";
            this.toolTip1.SetToolTip(this.buttonTextCollector, "Collect original and translated text(s)");
            this.buttonTextCollector.UseVisualStyleBackColor = true;
            this.buttonTextCollector.Click += new System.EventHandler(this.buttonTextCollector_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "TTG Tools";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(212, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 28);
            this.button1.TabIndex = 7;
            this.button1.Text = "Text Editor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(17, 298);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 28);
            this.button2.TabIndex = 8;
            this.button2.Text = "Перевести титры";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(17, 104);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(148, 28);
            this.buttonSettings.TabIndex = 9;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(228, 244);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(148, 28);
            this.button3.TabIndex = 10;
            this.button3.Text = "notabenoid";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(228, 278);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(148, 48);
            this.button4.TabIndex = 11;
            this.button4.Text = "Перенести текстуры";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(17, 60);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(148, 28);
            this.button5.TabIndex = 12;
            this.button5.Text = "Archive packer";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 142);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonTextCollector);
            this.Controls.Add(this.RunFontEditor);
            this.Controls.Add(this.About);
            this.Controls.Add(this.OpenAutopackerForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainMenu";
            this.Text = "TTG Tools by Den Em";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.Resize += new System.EventHandler(this.MainMenu_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenAutopackerForm;
        private System.Windows.Forms.Button About;
        private System.Windows.Forms.Button RunFontEditor;
        private System.Windows.Forms.Button buttonTextCollector;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}