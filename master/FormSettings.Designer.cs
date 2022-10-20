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
            this.clearMessagesCB = new System.Windows.Forms.CheckBox();
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
            this.rbNonNormalUnicode2 = new System.Windows.Forms.RadioButton();
            this.rbNormalUnicode = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.specificCB = new System.Windows.Forms.CheckBox();
            this.specificEncComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASCII)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // numericUpDownASCII
            // 
            this.numericUpDownASCII.Location = new System.Drawing.Point(56, 7);
            this.numericUpDownASCII.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numericUpDownASCII.Maximum = new decimal(new int[] {
            1258,
            0,
            0,
            0});
            this.numericUpDownASCII.Name = "numericUpDownASCII";
            this.numericUpDownASCII.ReadOnly = true;
            this.numericUpDownASCII.Size = new System.Drawing.Size(64, 20);
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
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "ASCII";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clearMessagesCB);
            this.groupBox1.Controls.Add(this.checkBoxSortStrings);
            this.groupBox1.Controls.Add(this.checkBoxExportRealID);
            this.groupBox1.Controls.Add(this.checkBoxImportingOfNames);
            this.groupBox1.Controls.Add(this.textBoxOutputFolder);
            this.groupBox1.Controls.Add(this.textBoxInputFolder);
            this.groupBox1.Controls.Add(this.buttonOutputFolder);
            this.groupBox1.Controls.Add(this.buttonInputFolder);
            this.groupBox1.Controls.Add(this.checkBoxDDS_after_import);
            this.groupBox1.Controls.Add(this.checkBoxD3DTX_after_import);
            this.groupBox1.Location = new System.Drawing.Point(13, 33);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(452, 158);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto(De)Packer";
            // 
            // clearMessagesCB
            // 
            this.clearMessagesCB.AutoSize = true;
            this.clearMessagesCB.Location = new System.Drawing.Point(246, 131);
            this.clearMessagesCB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.clearMessagesCB.Name = "clearMessagesCB";
            this.clearMessagesCB.Size = new System.Drawing.Size(193, 17);
            this.clearMessagesCB.TabIndex = 7;
            this.clearMessagesCB.Text = "Clear messages in Auto (De)Packer";
            this.clearMessagesCB.UseVisualStyleBackColor = true;
            // 
            // checkBoxSortStrings
            // 
            this.checkBoxSortStrings.AutoSize = true;
            this.checkBoxSortStrings.Location = new System.Drawing.Point(246, 104);
            this.checkBoxSortStrings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxSortStrings.Name = "checkBoxSortStrings";
            this.checkBoxSortStrings.Size = new System.Drawing.Size(78, 17);
            this.checkBoxSortStrings.TabIndex = 4;
            this.checkBoxSortStrings.Text = "Sort strings";
            this.checkBoxSortStrings.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportRealID
            // 
            this.checkBoxExportRealID.AutoSize = true;
            this.checkBoxExportRealID.Location = new System.Drawing.Point(12, 76);
            this.checkBoxExportRealID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxExportRealID.Name = "checkBoxExportRealID";
            this.checkBoxExportRealID.Size = new System.Drawing.Size(229, 17);
            this.checkBoxExportRealID.TabIndex = 3;
            this.checkBoxExportRealID.Text = "Use a real ID of text in LANDB, LANGDB...";
            this.checkBoxExportRealID.UseVisualStyleBackColor = true;
            // 
            // checkBoxImportingOfNames
            // 
            this.checkBoxImportingOfNames.AutoSize = true;
            this.checkBoxImportingOfNames.Location = new System.Drawing.Point(246, 76);
            this.checkBoxImportingOfNames.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxImportingOfNames.Name = "checkBoxImportingOfNames";
            this.checkBoxImportingOfNames.Size = new System.Drawing.Size(116, 17);
            this.checkBoxImportingOfNames.TabIndex = 3;
            this.checkBoxImportingOfNames.Text = "Import actor names";
            this.checkBoxImportingOfNames.UseVisualStyleBackColor = true;
            // 
            // textBoxOutputFolder
            // 
            this.textBoxOutputFolder.Location = new System.Drawing.Point(8, 50);
            this.textBoxOutputFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxOutputFolder.Name = "textBoxOutputFolder";
            this.textBoxOutputFolder.ReadOnly = true;
            this.textBoxOutputFolder.Size = new System.Drawing.Size(317, 20);
            this.textBoxOutputFolder.TabIndex = 2;
            // 
            // textBoxInputFolder
            // 
            this.textBoxInputFolder.Location = new System.Drawing.Point(8, 21);
            this.textBoxInputFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBoxInputFolder.Name = "textBoxInputFolder";
            this.textBoxInputFolder.ReadOnly = true;
            this.textBoxInputFolder.Size = new System.Drawing.Size(317, 20);
            this.textBoxInputFolder.TabIndex = 2;
            // 
            // buttonOutputFolder
            // 
            this.buttonOutputFolder.Location = new System.Drawing.Point(332, 48);
            this.buttonOutputFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonOutputFolder.Name = "buttonOutputFolder";
            this.buttonOutputFolder.Size = new System.Drawing.Size(90, 23);
            this.buttonOutputFolder.TabIndex = 1;
            this.buttonOutputFolder.Text = "Output Folder";
            this.buttonOutputFolder.UseVisualStyleBackColor = true;
            this.buttonOutputFolder.Click += new System.EventHandler(this.buttonOutputFolder_Click);
            // 
            // buttonInputFolder
            // 
            this.buttonInputFolder.Location = new System.Drawing.Point(332, 19);
            this.buttonInputFolder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonInputFolder.Name = "buttonInputFolder";
            this.buttonInputFolder.Size = new System.Drawing.Size(90, 23);
            this.buttonInputFolder.TabIndex = 1;
            this.buttonInputFolder.Text = "Input Folder";
            this.buttonInputFolder.UseVisualStyleBackColor = true;
            this.buttonInputFolder.Click += new System.EventHandler(this.buttonInputFolder_Click);
            // 
            // checkBoxDDS_after_import
            // 
            this.checkBoxDDS_after_import.AutoSize = true;
            this.checkBoxDDS_after_import.Location = new System.Drawing.Point(12, 132);
            this.checkBoxDDS_after_import.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxDDS_after_import.Name = "checkBoxDDS_after_import";
            this.checkBoxDDS_after_import.Size = new System.Drawing.Size(138, 17);
            this.checkBoxDDS_after_import.TabIndex = 0;
            this.checkBoxDDS_after_import.Text = "Delete DDS after import";
            this.checkBoxDDS_after_import.UseVisualStyleBackColor = true;
            // 
            // checkBoxD3DTX_after_import
            // 
            this.checkBoxD3DTX_after_import.AutoSize = true;
            this.checkBoxD3DTX_after_import.Location = new System.Drawing.Point(12, 104);
            this.checkBoxD3DTX_after_import.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBoxD3DTX_after_import.Name = "checkBoxD3DTX_after_import";
            this.checkBoxD3DTX_after_import.Size = new System.Drawing.Size(151, 17);
            this.checkBoxD3DTX_after_import.TabIndex = 0;
            this.checkBoxD3DTX_after_import.Text = "Delete D3DTX after import";
            this.checkBoxD3DTX_after_import.UseVisualStyleBackColor = true;
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(187, 291);
            this.buttonSaveSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(136, 23);
            this.buttonSaveSettings.TabIndex = 9;
            this.buttonSaveSettings.Text = "Apply";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonExitSettingsForm
            // 
            this.buttonExitSettingsForm.Location = new System.Drawing.Point(343, 291);
            this.buttonExitSettingsForm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonExitSettingsForm.Name = "buttonExitSettingsForm";
            this.buttonExitSettingsForm.Size = new System.Drawing.Size(88, 23);
            this.buttonExitSettingsForm.TabIndex = 10;
            this.buttonExitSettingsForm.Text = "Exit";
            this.buttonExitSettingsForm.UseVisualStyleBackColor = true;
            this.buttonExitSettingsForm.Click += new System.EventHandler(this.buttonCloseSettingsForm_Click);
            // 
            // buttonApplyAndExitSettings
            // 
            this.buttonApplyAndExitSettings.Location = new System.Drawing.Point(34, 291);
            this.buttonApplyAndExitSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonApplyAndExitSettings.Name = "buttonApplyAndExitSettings";
            this.buttonApplyAndExitSettings.Size = new System.Drawing.Size(136, 23);
            this.buttonApplyAndExitSettings.TabIndex = 11;
            this.buttonApplyAndExitSettings.Text = "Apply and Exit";
            this.buttonApplyAndExitSettings.UseVisualStyleBackColor = true;
            this.buttonApplyAndExitSettings.Click += new System.EventHandler(this.buttonOkSettings_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbNonNormalUnicode2);
            this.groupBox2.Controls.Add(this.rbNormalUnicode);
            this.groupBox2.Location = new System.Drawing.Point(56, 206);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(354, 73);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coding for new games (From \"Tales From the Borderlands\" game)";
            // 
            // rbNonNormalUnicode2
            // 
            this.rbNonNormalUnicode2.AutoSize = true;
            this.rbNonNormalUnicode2.Location = new System.Drawing.Point(5, 35);
            this.rbNonNormalUnicode2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbNonNormalUnicode2.Name = "rbNonNormalUnicode2";
            this.rbNonNormalUnicode2.Size = new System.Drawing.Size(330, 30);
            this.rbNonNormalUnicode2.TabIndex = 2;
            this.rbNonNormalUnicode2.TabStop = true;
            this.rbNonNormalUnicode2.Text = "NOT normal unicode (convert your language into non-latin chars.\r\nRecommend for ne" +
    "w version TftB.)";
            this.rbNonNormalUnicode2.UseVisualStyleBackColor = true;
            // 
            // rbNormalUnicode
            // 
            this.rbNormalUnicode.AutoSize = true;
            this.rbNormalUnicode.Location = new System.Drawing.Point(5, 17);
            this.rbNormalUnicode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbNormalUnicode.Name = "rbNormalUnicode";
            this.rbNormalUnicode.Size = new System.Drawing.Size(101, 17);
            this.rbNormalUnicode.TabIndex = 0;
            this.rbNormalUnicode.TabStop = true;
            this.rbNormalUnicode.Text = "Normal Unicode";
            this.toolTip1.SetToolTip(this.rbNormalUnicode, "Recommend to use in new games (From\r\nMinecraft: Story Mode). This option could he" +
        "lp\r\nto export/import text files from a new game and\r\nremake fonts with support o" +
        "f your symbols.");
            this.rbNormalUnicode.UseVisualStyleBackColor = true;
            // 
            // specificCB
            // 
            this.specificCB.AutoSize = true;
            this.specificCB.Location = new System.Drawing.Point(142, 9);
            this.specificCB.Name = "specificCB";
            this.specificCB.Size = new System.Drawing.Size(146, 17);
            this.specificCB.TabIndex = 14;
            this.specificCB.Text = "Specific ASCII encodings";
            this.specificCB.UseVisualStyleBackColor = true;
            // 
            // specificEncComboBox
            // 
            this.specificEncComboBox.FormattingEnabled = true;
            this.specificEncComboBox.Location = new System.Drawing.Point(294, 7);
            this.specificEncComboBox.Name = "specificEncComboBox";
            this.specificEncComboBox.Size = new System.Drawing.Size(121, 21);
            this.specificEncComboBox.TabIndex = 15;
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 329);
            this.Controls.Add(this.specificEncComboBox);
            this.Controls.Add(this.specificCB);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonApplyAndExitSettings);
            this.Controls.Add(this.buttonExitSettingsForm);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.numericUpDownASCII);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
        private System.Windows.Forms.CheckBox checkBoxExportRealID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbNormalUnicode;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.ToolTip toolTip3;
        private System.Windows.Forms.RadioButton rbNonNormalUnicode2;
        private System.Windows.Forms.CheckBox clearMessagesCB;
        private System.Windows.Forms.CheckBox specificCB;
        private System.Windows.Forms.ComboBox specificEncComboBox;
    }
}