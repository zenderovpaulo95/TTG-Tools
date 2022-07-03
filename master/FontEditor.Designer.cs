﻿namespace TTG_Tools
{
    partial class FontEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FontEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonCopyCoordinates = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewWithTextures = new System.Windows.Forms.DataGridView();
            this.N = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripExport_Import = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDDSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportCoordinatesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripImportFNT = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewWithCoord = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripExp_imp_Coord = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDuplicatesCharsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbNoKerning = new System.Windows.Forms.RadioButton();
            this.rbKerning = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonXstart = new System.Windows.Forms.RadioButton();
            this.radioButtonXend = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ninSwizzleCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWithTextures)).BeginInit();
            this.contextMenuStripExport_Import.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWithCoord)).BeginInit();
            this.contextMenuStripExp_imp_Coord.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.buttonCopyCoordinates);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(457, 33);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox1.Size = new System.Drawing.Size(248, 119);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy coordinates";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(165, 87);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(76, 28);
            this.buttonClear.TabIndex = 16;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(61, 23);
            this.textBox8.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(94, 22);
            this.textBox8.TabIndex = 13;
            this.textBox8.TextChanged += new System.EventHandler(this.textBox8_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "(0)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(164, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 16);
            this.label1.TabIndex = 11;
            this.label1.Text = "(0)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 11;
            this.label3.Text = "From";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(201, 27);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(35, 20);
            this.checkBox1.TabIndex = 15;
            this.checkBox1.Text = "x";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonCopyCoordinates
            // 
            this.buttonCopyCoordinates.Location = new System.Drawing.Point(61, 87);
            this.buttonCopyCoordinates.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonCopyCoordinates.Name = "buttonCopyCoordinates";
            this.buttonCopyCoordinates.Size = new System.Drawing.Size(94, 28);
            this.buttonCopyCoordinates.TabIndex = 14;
            this.buttonCopyCoordinates.Text = "Do it!";
            this.buttonCopyCoordinates.UseVisualStyleBackColor = true;
            this.buttonCopyCoordinates.Click += new System.EventHandler(this.buttonCopyCoordinates_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(201, 59);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(36, 20);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Text = "y";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(61, 55);
            this.textBox9.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(94, 22);
            this.textBox9.TabIndex = 13;
            this.textBox9.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "To";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1339, 28);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.saveAsToolStripMenuItem.Text = "Save As ...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataGridViewWithTextures
            // 
            this.dataGridViewWithTextures.AllowUserToAddRows = false;
            this.dataGridViewWithTextures.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWithTextures.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewWithTextures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWithTextures.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.N,
            this.Height,
            this.Width,
            this.Size});
            this.dataGridViewWithTextures.ContextMenuStrip = this.contextMenuStripExport_Import;
            this.dataGridViewWithTextures.Location = new System.Drawing.Point(16, 33);
            this.dataGridViewWithTextures.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.dataGridViewWithTextures.Name = "dataGridViewWithTextures";
            this.dataGridViewWithTextures.ReadOnly = true;
            this.dataGridViewWithTextures.RowHeadersWidth = 51;
            this.dataGridViewWithTextures.Size = new System.Drawing.Size(433, 119);
            this.dataGridViewWithTextures.TabIndex = 26;
            this.dataGridViewWithTextures.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewWithTextures_CellMouseClick);
            this.dataGridViewWithTextures.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.dataGridViewWithTextures_RowContextMenuStripNeeded);
            // 
            // N
            // 
            this.N.HeaderText = "№";
            this.N.MinimumWidth = 6;
            this.N.Name = "N";
            this.N.ReadOnly = true;
            this.N.Width = 30;
            // 
            // Height
            // 
            this.Height.HeaderText = "Height";
            this.Height.MinimumWidth = 6;
            this.Height.Name = "Height";
            this.Height.ReadOnly = true;
            this.Height.Width = 102;
            // 
            // Width
            // 
            this.Width.HeaderText = "Width";
            this.Width.MinimumWidth = 6;
            this.Width.Name = "Width";
            this.Width.ReadOnly = true;
            this.Width.Width = 102;
            // 
            // Size
            // 
            this.Size.HeaderText = "Size";
            this.Size.MinimumWidth = 6;
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            this.Size.Width = 102;
            // 
            // contextMenuStripExport_Import
            // 
            this.contextMenuStripExport_Import.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripExport_Import.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.importDDSToolStripMenuItem,
            this.exportCoordinatesToolStripMenuItem1,
            this.toolStripImportFNT});
            this.contextMenuStripExport_Import.Name = "contextMenuStripExport_Import";
            this.contextMenuStripExport_Import.Size = new System.Drawing.Size(502, 100);
            this.contextMenuStripExport_Import.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripExport_Import_Opening);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(501, 24);
            this.exportToolStripMenuItem.Text = "Export texture";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // importDDSToolStripMenuItem
            // 
            this.importDDSToolStripMenuItem.Enabled = false;
            this.importDDSToolStripMenuItem.Name = "importDDSToolStripMenuItem";
            this.importDDSToolStripMenuItem.Size = new System.Drawing.Size(501, 24);
            this.importDDSToolStripMenuItem.Text = "Import texture";
            this.importDDSToolStripMenuItem.Click += new System.EventHandler(this.importDDSToolStripMenuItem_Click);
            // 
            // exportCoordinatesToolStripMenuItem1
            // 
            this.exportCoordinatesToolStripMenuItem1.Enabled = false;
            this.exportCoordinatesToolStripMenuItem1.Name = "exportCoordinatesToolStripMenuItem1";
            this.exportCoordinatesToolStripMenuItem1.Size = new System.Drawing.Size(501, 24);
            this.exportCoordinatesToolStripMenuItem1.Text = "Export coordinates";
            this.exportCoordinatesToolStripMenuItem1.Click += new System.EventHandler(this.exportCoordinatesToolStripMenuItem1_Click);
            // 
            // toolStripImportFNT
            // 
            this.toolStripImportFNT.Name = "toolStripImportFNT";
            this.toolStripImportFNT.Size = new System.Drawing.Size(501, 24);
            this.toolStripImportFNT.Text = "Import coordinates from Bitmap Font Generator (*.fnt типа xml)";
            this.toolStripImportFNT.Click += new System.EventHandler(this.toolStripImportFNT_Click);
            // 
            // dataGridViewWithCoord
            // 
            this.dataGridViewWithCoord.AllowUserToAddRows = false;
            this.dataGridViewWithCoord.AllowUserToDeleteRows = false;
            this.dataGridViewWithCoord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWithCoord.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewWithCoord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWithCoord.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13});
            this.dataGridViewWithCoord.Location = new System.Drawing.Point(16, 160);
            this.dataGridViewWithCoord.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.dataGridViewWithCoord.Name = "dataGridViewWithCoord";
            this.dataGridViewWithCoord.RowHeadersWidth = 51;
            this.dataGridViewWithCoord.Size = new System.Drawing.Size(1305, 464);
            this.dataGridViewWithCoord.TabIndex = 27;
            this.dataGridViewWithCoord.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridViewWithCoord_CellBeginEdit);
            this.dataGridViewWithCoord.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewWithCoord_CellEndEdit);
            this.dataGridViewWithCoord.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewWithCoord_CellMouseClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "№";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Char";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 40;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "X start";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "X end";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Y start";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Y end";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 60;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "№ dds";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 65;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Width";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.Width = 119;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Height";
            this.Column9.MinimumWidth = 6;
            this.Column9.Name = "Column9";
            this.Column9.Width = 119;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Channel";
            this.Column10.MinimumWidth = 6;
            this.Column10.Name = "Column10";
            this.Column10.Width = 119;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "X offset";
            this.Column11.MinimumWidth = 6;
            this.Column11.Name = "Column11";
            this.Column11.Width = 119;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Y offset";
            this.Column12.MinimumWidth = 6;
            this.Column12.Name = "Column12";
            this.Column12.Width = 119;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "X advanced";
            this.Column13.MinimumWidth = 6;
            this.Column13.Name = "Column13";
            this.Column13.Width = 119;
            // 
            // contextMenuStripExp_imp_Coord
            // 
            this.contextMenuStripExp_imp_Coord.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripExp_imp_Coord.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportCoordinatesToolStripMenuItem,
            this.importCoordinatesToolStripMenuItem,
            this.removeDuplicatesCharsToolStripMenuItem});
            this.contextMenuStripExp_imp_Coord.Name = "contextMenuStripExp_imp_Coord";
            this.contextMenuStripExp_imp_Coord.Size = new System.Drawing.Size(243, 76);
            // 
            // exportCoordinatesToolStripMenuItem
            // 
            this.exportCoordinatesToolStripMenuItem.Name = "exportCoordinatesToolStripMenuItem";
            this.exportCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(242, 24);
            this.exportCoordinatesToolStripMenuItem.Text = "Export coordinates";
            this.exportCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.exportCoordinatesToolStripMenuItem_Click);
            // 
            // importCoordinatesToolStripMenuItem
            // 
            this.importCoordinatesToolStripMenuItem.Name = "importCoordinatesToolStripMenuItem";
            this.importCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(242, 24);
            this.importCoordinatesToolStripMenuItem.Text = "Import coordinates";
            this.importCoordinatesToolStripMenuItem.Click += new System.EventHandler(this.importCoordinatesToolStripMenuItem_Click);
            // 
            // removeDuplicatesCharsToolStripMenuItem
            // 
            this.removeDuplicatesCharsToolStripMenuItem.Name = "removeDuplicatesCharsToolStripMenuItem";
            this.removeDuplicatesCharsToolStripMenuItem.Size = new System.Drawing.Size(242, 24);
            this.removeDuplicatesCharsToolStripMenuItem.Text = "Remove duplicates chars";
            this.removeDuplicatesCharsToolStripMenuItem.Click += new System.EventHandler(this.removeDuplicatesCharsToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbNoKerning);
            this.groupBox2.Controls.Add(this.rbKerning);
            this.groupBox2.Location = new System.Drawing.Point(947, 34);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Size = new System.Drawing.Size(195, 123);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Import/Export:";
            // 
            // rbNoKerning
            // 
            this.rbNoKerning.AutoSize = true;
            this.rbNoKerning.Location = new System.Drawing.Point(9, 73);
            this.rbNoKerning.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rbNoKerning.Name = "rbNoKerning";
            this.rbNoKerning.Size = new System.Drawing.Size(116, 20);
            this.rbNoKerning.TabIndex = 1;
            this.rbNoKerning.Text = "without Kerning";
            this.rbNoKerning.UseVisualStyleBackColor = true;
            // 
            // rbKerning
            // 
            this.rbKerning.AutoSize = true;
            this.rbKerning.Checked = true;
            this.rbKerning.Location = new System.Drawing.Point(9, 44);
            this.rbKerning.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.rbKerning.Name = "rbKerning";
            this.rbKerning.Size = new System.Drawing.Size(98, 20);
            this.rbKerning.TabIndex = 0;
            this.rbKerning.TabStop = true;
            this.rbKerning.Text = "with Kerning";
            this.rbKerning.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonXstart);
            this.groupBox3.Controls.Add(this.radioButtonXend);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(715, 34);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox3.Size = new System.Drawing.Size(215, 123);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Change char width to:";
            // 
            // radioButtonXstart
            // 
            this.radioButtonXstart.AutoSize = true;
            this.radioButtonXstart.Location = new System.Drawing.Point(89, 58);
            this.radioButtonXstart.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.radioButtonXstart.Name = "radioButtonXstart";
            this.radioButtonXstart.Size = new System.Drawing.Size(64, 20);
            this.radioButtonXstart.TabIndex = 4;
            this.radioButtonXstart.Text = "X start";
            this.radioButtonXstart.UseVisualStyleBackColor = true;
            // 
            // radioButtonXend
            // 
            this.radioButtonXend.AutoSize = true;
            this.radioButtonXend.Checked = true;
            this.radioButtonXend.Location = new System.Drawing.Point(9, 58);
            this.radioButtonXend.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.radioButtonXend.Name = "radioButtonXend";
            this.radioButtonXend.Size = new System.Drawing.Size(62, 20);
            this.radioButtonXend.TabIndex = 3;
            this.radioButtonXend.TabStop = true;
            this.radioButtonXend.Text = "X end";
            this.radioButtonXend.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(95, 27);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "pixels.";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 22);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(76, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 85);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Change";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ninSwizzleCheckBox
            // 
            this.ninSwizzleCheckBox.AutoSize = true;
            this.ninSwizzleCheckBox.Location = new System.Drawing.Point(1150, 58);
            this.ninSwizzleCheckBox.Name = "ninSwizzleCheckBox";
            this.ninSwizzleCheckBox.Size = new System.Drawing.Size(171, 20);
            this.ninSwizzleCheckBox.TabIndex = 30;
            this.ninSwizzleCheckBox.Text = "Swizzle Nintendo Switch";
            this.ninSwizzleCheckBox.UseVisualStyleBackColor = true;
            this.ninSwizzleCheckBox.CheckedChanged += new System.EventHandler(this.ninSwizzleCheckBox_CheckedChanged);
            // 
            // FontEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1339, 639);
            this.Controls.Add(this.ninSwizzleCheckBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.dataGridViewWithCoord);
            this.Controls.Add(this.dataGridViewWithTextures);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.Name = "FontEditor";
            this.Text = "Font Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FontEditor_FormClosing);
            this.Load += new System.EventHandler(this.FontEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWithTextures)).EndInit();
            this.contextMenuStripExport_Import.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWithCoord)).EndInit();
            this.contextMenuStripExp_imp_Coord.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonCopyCoordinates;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewWithTextures;
        private System.Windows.Forms.DataGridView dataGridViewWithCoord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExport_Import;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDDSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripExp_imp_Coord;
        private System.Windows.Forms.ToolStripMenuItem exportCoordinatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCoordinatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportCoordinatesToolStripMenuItem1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbNoKerning;
        private System.Windows.Forms.RadioButton rbKerning;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButtonXstart;
        private System.Windows.Forms.RadioButton radioButtonXend;
        private System.Windows.Forms.ToolStripMenuItem toolStripImportFNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn N;
        private System.Windows.Forms.DataGridViewTextBoxColumn Height;
        private System.Windows.Forms.DataGridViewTextBoxColumn Width;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.ToolStripMenuItem removeDuplicatesCharsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.CheckBox ninSwizzleCheckBox;
    }
}