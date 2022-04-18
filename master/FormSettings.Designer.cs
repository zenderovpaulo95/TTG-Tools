namespace TTG_Tools
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.numericUpDownASCII = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelAdditionalChar = new System.Windows.Forms.Label();
            this.textBoxAdditionalChar = new System.Windows.Forms.TextBox();
            this.checkBoxSortStrings = new System.Windows.Forms.CheckBox();
            this.checkBoxExportRealID = new System.Windows.Forms.CheckBox();
            this.checkBoxImportingOfNames = new System.Windows.Forms.CheckBox();
            this.textBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.textBoxInputFolder = new System.Windows.Forms.TextBox();
            this.buttonOutputFolder = new System.Windows.Forms.Button();
            this.buttonInputFolder = new System.Windows.Forms.Button();
            this.checkBoxDDS_after_import = new System.Windows.Forms.CheckBox();
            this.checkBoxD3DTX_after_import = new System.Windows.Forms.CheckBox();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonExitSettingsForm = new System.Windows.Forms.Button();
            this.buttonApplyAndExitSettings = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbNonNormalUnicode = new System.Windows.Forms.RadioButton();
            this.rbNormalUnicode = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.rbNonNormalUnicode2 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASCII)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownASCII
            // 
            this.numericUpDownASCII.Location = new System.Drawing.Point(75, 9);
            this.numericUpDownASCII.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.numericUpDownASCII.Maximum = new decimal(new int[] {
            1258,
            0,
            0,
            0});
            this.numericUpDownASCII.Name = "numericUpDownASCII";
            this.numericUpDownASCII.ReadOnly = true;
            this.numericUpDownASCII.Size = new System.Drawing.Size(85, 22);
            this.numericUpDownASCII.TabIndex = 7;
            this.numericUpDownASCII.Value = new decimal(new int[] {
            1251,
            0,
            0,
            0});
            this.numericUpDownASCII.ValueChanged += new System.EventHandler(this.numericUpDownASCII_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "ASCII";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelAdditionalChar);
            this.groupBox1.Controls.Add(this.textBoxAdditionalChar);
            this.groupBox1.Controls.Add(this.checkBoxSortStrings);
            this.groupBox1.Controls.Add(this.checkBoxExportRealID);
            this.groupBox1.Controls.Add(this.checkBoxImportingOfNames);
            this.groupBox1.Controls.Add(this.textBoxOutputFolder);
            this.groupBox1.Controls.Add(this.textBoxInputFolder);
            this.groupBox1.Controls.Add(this.buttonOutputFolder);
            this.groupBox1.Controls.Add(this.buttonInputFolder);
            this.groupBox1.Controls.Add(this.checkBoxDDS_after_import);
            this.groupBox1.Controls.Add(this.checkBoxD3DTX_after_import);
            this.groupBox1.Location = new System.Drawing.Point(17, 41);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Size = new System.Drawing.Size(573, 221);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto(De)Packer";
            // 
            // labelAdditionalChar
            // 
            this.labelAdditionalChar.AutoSize = true;
            this.labelAdditionalChar.Location = new System.Drawing.Point(69, 98);
            this.labelAdditionalChar.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelAdditionalChar.Name = "labelAdditionalChar";
            this.labelAdditionalChar.Size = new System.Drawing.Size(108, 17);
            this.labelAdditionalChar.TabIndex = 6;
            this.labelAdditionalChar.Text = "Additional Char:";
            // 
            // textBoxAdditionalChar
            // 
            this.textBoxAdditionalChar.Location = new System.Drawing.Point(202, 95);
            this.textBoxAdditionalChar.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBoxAdditionalChar.Name = "textBoxAdditionalChar";
            this.textBoxAdditionalChar.Size = new System.Drawing.Size(359, 22);
            this.textBoxAdditionalChar.TabIndex = 5;
            this.textBoxAdditionalChar.Text = "ГЁЙЦУКЕНШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮгёйцукеншщзхъфывапролджэячсмитьбю";
            // 
            // checkBoxSortStrings
            // 
            this.checkBoxSortStrings.AutoSize = true;
            this.checkBoxSortStrings.Location = new System.Drawing.Point(371, 161);
            this.checkBoxSortStrings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxSortStrings.Name = "checkBoxSortStrings";
            this.checkBoxSortStrings.Size = new System.Drawing.Size(102, 21);
            this.checkBoxSortStrings.TabIndex = 4;
            this.checkBoxSortStrings.Text = "Sort strings";
            this.checkBoxSortStrings.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportRealID
            // 
            this.checkBoxExportRealID.AutoSize = true;
            this.checkBoxExportRealID.Location = new System.Drawing.Point(16, 126);
            this.checkBoxExportRealID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxExportRealID.Name = "checkBoxExportRealID";
            this.checkBoxExportRealID.Size = new System.Drawing.Size(296, 21);
            this.checkBoxExportRealID.TabIndex = 3;
            this.checkBoxExportRealID.Text = "Use a real ID of text in LANDB, LANGDB...";
            this.checkBoxExportRealID.UseVisualStyleBackColor = true;
            // 
            // checkBoxImportingOfNames
            // 
            this.checkBoxImportingOfNames.AutoSize = true;
            this.checkBoxImportingOfNames.Location = new System.Drawing.Point(371, 126);
            this.checkBoxImportingOfNames.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxImportingOfNames.Name = "checkBoxImportingOfNames";
            this.checkBoxImportingOfNames.Size = new System.Drawing.Size(151, 21);
            this.checkBoxImportingOfNames.TabIndex = 3;
            this.checkBoxImportingOfNames.Text = "Import actor names";
            this.checkBoxImportingOfNames.UseVisualStyleBackColor = true;
            // 
            // textBoxOutputFolder
            // 
            this.textBoxOutputFolder.Location = new System.Drawing.Point(11, 62);
            this.textBoxOutputFolder.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBoxOutputFolder.Name = "textBoxOutputFolder";
            this.textBoxOutputFolder.ReadOnly = true;
            this.textBoxOutputFolder.Size = new System.Drawing.Size(421, 22);
            this.textBoxOutputFolder.TabIndex = 2;
            // 
            // textBoxInputFolder
            // 
            this.textBoxInputFolder.Location = new System.Drawing.Point(11, 26);
            this.textBoxInputFolder.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBoxInputFolder.Name = "textBoxInputFolder";
            this.textBoxInputFolder.ReadOnly = true;
            this.textBoxInputFolder.Size = new System.Drawing.Size(421, 22);
            this.textBoxInputFolder.TabIndex = 2;
            // 
            // buttonOutputFolder
            // 
            this.buttonOutputFolder.Location = new System.Drawing.Point(442, 59);
            this.buttonOutputFolder.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonOutputFolder.Name = "buttonOutputFolder";
            this.buttonOutputFolder.Size = new System.Drawing.Size(120, 28);
            this.buttonOutputFolder.TabIndex = 1;
            this.buttonOutputFolder.Text = "Output Folder";
            this.buttonOutputFolder.UseVisualStyleBackColor = true;
            this.buttonOutputFolder.Click += new System.EventHandler(this.buttonOutputFolder_Click);
            // 
            // buttonInputFolder
            // 
            this.buttonInputFolder.Location = new System.Drawing.Point(442, 23);
            this.buttonInputFolder.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonInputFolder.Name = "buttonInputFolder";
            this.buttonInputFolder.Size = new System.Drawing.Size(120, 28);
            this.buttonInputFolder.TabIndex = 1;
            this.buttonInputFolder.Text = "Input Folder";
            this.buttonInputFolder.UseVisualStyleBackColor = true;
            this.buttonInputFolder.Click += new System.EventHandler(this.buttonInputFolder_Click);
            // 
            // checkBoxDDS_after_import
            // 
            this.checkBoxDDS_after_import.AutoSize = true;
            this.checkBoxDDS_after_import.Location = new System.Drawing.Point(16, 195);
            this.checkBoxDDS_after_import.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkBoxDDS_after_import.Name = "checkBoxDDS_after_import";
            this.checkBoxDDS_after_import.Size = new System.Drawing.Size(180, 21);
            this.checkBoxDDS_after_import.TabIndex = 0;
            this.checkBoxDDS_after_import.Text = "Delete DDS after import";
            this.checkBoxDDS_after_import.UseVisualStyleBackColor = true;
            // 
            // checkBoxD3DTX_after_import
            // 
            this.checkBoxD3DTX_after_import.AutoSize = true;
            this.checkBoxD3DTX_after_import.Location = new System.Drawing.Point(16, 161);
            this.checkBoxD3DTX_after_import.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkBoxD3DTX_after_import.Name = "checkBoxD3DTX_after_import";
            this.checkBoxD3DTX_after_import.Size = new System.Drawing.Size(197, 21);
            this.checkBoxD3DTX_after_import.TabIndex = 0;
            this.checkBoxD3DTX_after_import.Text = "Delete D3DTX after import";
            this.checkBoxD3DTX_after_import.UseVisualStyleBackColor = true;
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(222, 407);
            this.buttonSaveSettings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(181, 28);
            this.buttonSaveSettings.TabIndex = 9;
            this.buttonSaveSettings.Text = "Apply";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonExitSettingsForm
            // 
            this.buttonExitSettingsForm.Location = new System.Drawing.Point(430, 407);
            this.buttonExitSettingsForm.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonExitSettingsForm.Name = "buttonExitSettingsForm";
            this.buttonExitSettingsForm.Size = new System.Drawing.Size(117, 28);
            this.buttonExitSettingsForm.TabIndex = 10;
            this.buttonExitSettingsForm.Text = "Exit";
            this.buttonExitSettingsForm.UseVisualStyleBackColor = true;
            this.buttonExitSettingsForm.Click += new System.EventHandler(this.buttonCloseSettingsForm_Click);
            // 
            // buttonApplyAndExitSettings
            // 
            this.buttonApplyAndExitSettings.Location = new System.Drawing.Point(19, 407);
            this.buttonApplyAndExitSettings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonApplyAndExitSettings.Name = "buttonApplyAndExitSettings";
            this.buttonApplyAndExitSettings.Size = new System.Drawing.Size(181, 28);
            this.buttonApplyAndExitSettings.TabIndex = 11;
            this.buttonApplyAndExitSettings.Text = "Apply and Exit";
            this.buttonApplyAndExitSettings.UseVisualStyleBackColor = true;
            this.buttonApplyAndExitSettings.Click += new System.EventHandler(this.buttonOkSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbNonNormalUnicode2);
            this.groupBox2.Controls.Add(this.rbNonNormalUnicode);
            this.groupBox2.Controls.Add(this.rbNormalUnicode);
            this.groupBox2.Location = new System.Drawing.Point(24, 282);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(561, 118);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coding for new games (From \"Tales From the Borderlands\" game)";
            // 
            // rbNonNormalUnicode
            // 
            this.rbNonNormalUnicode.AutoSize = true;
            this.rbNonNormalUnicode.Location = new System.Drawing.Point(7, 86);
            this.rbNonNormalUnicode.Name = "rbNonNormalUnicode";
            this.rbNonNormalUnicode.Size = new System.Drawing.Size(416, 21);
            this.rbNonNormalUnicode.TabIndex = 1;
            this.rbNonNormalUnicode.TabStop = true;
            this.rbNonNormalUnicode.Text = "NOT normal unicode (uses additional char. Very old method.)";
            this.toolTip2.SetToolTip(this.rbNonNormalUnicode, resources.GetString("rbNonNormalUnicode.ToolTip"));
            this.rbNonNormalUnicode.UseVisualStyleBackColor = true;
            // 
            // rbNormalUnicode
            // 
            this.rbNormalUnicode.AutoSize = true;
            this.rbNormalUnicode.Location = new System.Drawing.Point(7, 21);
            this.rbNormalUnicode.Name = "rbNormalUnicode";
            this.rbNormalUnicode.Size = new System.Drawing.Size(130, 21);
            this.rbNormalUnicode.TabIndex = 0;
            this.rbNormalUnicode.TabStop = true;
            this.rbNormalUnicode.Text = "Normal Unicode";
            this.toolTip1.SetToolTip(this.rbNormalUnicode, "Recommend to use in new games (From\r\nMinecraft: Story Mode). This option could he" +
        "lp\r\nto export/import text files from a new game and\r\nremake fonts with support o" +
        "f your symbols.");
            this.rbNormalUnicode.UseVisualStyleBackColor = true;
            // 
            // rbNonNormalUnicode2
            // 
            this.rbNonNormalUnicode2.AutoSize = true;
            this.rbNonNormalUnicode2.Location = new System.Drawing.Point(7, 43);
            this.rbNonNormalUnicode2.Name = "rbNonNormalUnicode2";
            this.rbNonNormalUnicode2.Size = new System.Drawing.Size(440, 38);
            this.rbNonNormalUnicode2.TabIndex = 2;
            this.rbNonNormalUnicode2.TabStop = true;
            this.rbNonNormalUnicode2.Text = "NOT normal unicode (convert your language into non-latin chars.\r\nRecommend for ne" +
    "w version TftB.)";
            this.rbNonNormalUnicode2.UseVisualStyleBackColor = true;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 440);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonApplyAndExitSettings);
            this.Controls.Add(this.buttonExitSettingsForm);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numericUpDownASCII);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASCII)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownASCII;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonInputFolder;
        private System.Windows.Forms.CheckBox checkBoxDDS_after_import;
        private System.Windows.Forms.CheckBox checkBoxD3DTX_after_import;
        private System.Windows.Forms.Button buttonOutputFolder;
        private System.Windows.Forms.TextBox textBoxOutputFolder;
        private System.Windows.Forms.TextBox textBoxInputFolder;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Button buttonExitSettingsForm;
        private System.Windows.Forms.Button buttonApplyAndExitSettings;
        private System.Windows.Forms.CheckBox checkBoxImportingOfNames;
        private System.Windows.Forms.CheckBox checkBoxSortStrings;
        private System.Windows.Forms.Label labelAdditionalChar;
        private System.Windows.Forms.TextBox textBoxAdditionalChar;
        private System.Windows.Forms.CheckBox checkBoxExportRealID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbNonNormalUnicode;
        private System.Windows.Forms.RadioButton rbNormalUnicode;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.RadioButton rbNonNormalUnicode2;
    }
}