namespace LinkeD365.MockDataGen
{
    partial class MockDataGenCtl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MockDataGenCtl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtMockKey = new System.Windows.Forms.ToolStripTextBox();
            this.ddConfig = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnDepTables = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDepCol = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDepImpSeqNo = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBypassPluginExec = new System.Windows.Forms.ToolStripMenuItem();
            this.cboSelectSaved = new System.Windows.Forms.ToolStripComboBox();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cboRunDataSet = new System.Windows.Forms.ToolStripComboBox();
            this.btnPlaySet = new System.Windows.Forms.ToolStripButton();
            this.btnCreateDataSet = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ddExport = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnExportMaps = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportSets = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.gridMap = new System.Windows.Forms.DataGridView();
            this.splitConfig = new System.Windows.Forms.SplitContainer();
            this.grpEntity = new System.Windows.Forms.GroupBox();
            this.lblPrimary = new System.Windows.Forms.Label();
            this.cboEntities = new System.Windows.Forms.ComboBox();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.numRecordCount = new System.Windows.Forms.NumericUpDown();
            this.btnMock = new System.Windows.Forms.Button();
            this.grpAttributes = new System.Windows.Forms.GroupBox();
            this.tabGrpMain = new System.Windows.Forms.TabControl();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.tabSample = new System.Windows.Forms.TabPage();
            this.splitResults = new System.Windows.Forms.SplitContainer();
            this.batchSizeLabel = new System.Windows.Forms.Label();
            this.batchSize = new System.Windows.Forms.NumericUpDown();
            this.btnCreateAllData = new System.Windows.Forms.Button();
            this.btnCreate100Data = new System.Windows.Forms.Button();
            this.gridSample = new xrmtb.XrmToolBox.Controls.CRMGridView();
            this.tabGrpHidden = new System.Windows.Forms.TabControl();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitConfig)).BeginInit();
            this.splitConfig.Panel1.SuspendLayout();
            this.splitConfig.Panel2.SuspendLayout();
            this.splitConfig.SuspendLayout();
            this.grpEntity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecordCount)).BeginInit();
            this.grpAttributes.SuspendLayout();
            this.tabGrpMain.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.tabSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitResults)).BeginInit();
            this.splitResults.Panel1.SuspendLayout();
            this.splitResults.Panel2.SuspendLayout();
            this.splitResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.batchSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSample)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.txtMockKey,
            this.ddConfig,
            this.cboSelectSaved,
            this.btnSave,
            this.toolStripSeparator1,
            this.cboRunDataSet,
            this.btnPlaySet,
            this.btnCreateDataSet,
            this.toolStripSeparator2,
            this.ddExport,
            this.btnImport});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1024, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(110, 28);
            this.tsbClose.Text = "Close this tool";
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // txtMockKey
            // 
            this.txtMockKey.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMockKey.Name = "txtMockKey";
            this.txtMockKey.Size = new System.Drawing.Size(120, 31);
            this.txtMockKey.Text = "Mockaroo API Key";
            // 
            // ddConfig
            // 
            this.ddConfig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ddConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnDepTables,
            this.btnDepCol,
            this.btnDepImpSeqNo,
            this.btnBypassPluginExec});
            this.ddConfig.Image = global::LinkeD365.MockDataGen.Properties.Resources.Settings_WF;
            this.ddConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddConfig.Name = "ddConfig";
            this.ddConfig.Size = new System.Drawing.Size(37, 28);
            this.ddConfig.Text = "toolStripDropDownButton1";
            // 
            // btnDepTables
            // 
            this.btnDepTables.CheckOnClick = true;
            this.btnDepTables.Name = "btnDepTables";
            this.btnDepTables.Size = new System.Drawing.Size(298, 22);
            this.btnDepTables.Text = "Exclude \" (Deprecated)\" Tables";
            this.btnDepTables.CheckStateChanged += new System.EventHandler(this.btnDepTables_CheckStateChanged);
            // 
            // btnDepCol
            // 
            this.btnDepCol.CheckOnClick = true;
            this.btnDepCol.Name = "btnDepCol";
            this.btnDepCol.Size = new System.Drawing.Size(298, 22);
            this.btnDepCol.Text = "Exclude \" (Deprecated)\" Columns";
            this.btnDepCol.CheckStateChanged += new System.EventHandler(this.btnDepCol_CheckStateChanged);
            // 
            // btnDepImpSeqNo
            // 
            this.btnDepImpSeqNo.CheckOnClick = true;
            this.btnDepImpSeqNo.Name = "btnDepImpSeqNo";
            this.btnDepImpSeqNo.Size = new System.Drawing.Size(298, 22);
            this.btnDepImpSeqNo.Text = "Exclude Import Sequent Number Columns";
            this.btnDepImpSeqNo.CheckStateChanged += new System.EventHandler(this.btnDepImpSeqNo_CheckStateChanged);
            // 
            // btnBypassPluginExec
            // 
            this.btnBypassPluginExec.CheckOnClick = true;
            this.btnBypassPluginExec.Name = "btnBypassPluginExec";
            this.btnBypassPluginExec.Size = new System.Drawing.Size(298, 22);
            this.btnBypassPluginExec.Text = "Bypass Plugin Execution on Create";
            this.btnBypassPluginExec.CheckStateChanged += new System.EventHandler(this.btnBypassPluginExec_CheckStateChanged);
            // 
            // cboSelectSaved
            // 
            this.cboSelectSaved.Name = "cboSelectSaved";
            this.cboSelectSaved.Size = new System.Drawing.Size(121, 31);
            this.cboSelectSaved.Text = "Select Saved Map";
            this.cboSelectSaved.SelectedIndexChanged += new System.EventHandler(this.cboSelectSaved_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(129, 28);
            this.btnSave.Text = "Save Current Map";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // cboRunDataSet
            // 
            this.cboRunDataSet.Name = "cboRunDataSet";
            this.cboRunDataSet.Size = new System.Drawing.Size(121, 31);
            this.cboRunDataSet.Text = "Run Data Set";
            // 
            // btnPlaySet
            // 
            this.btnPlaySet.Image = ((System.Drawing.Image)(resources.GetObject("btnPlaySet.Image")));
            this.btnPlaySet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPlaySet.Name = "btnPlaySet";
            this.btnPlaySet.Size = new System.Drawing.Size(76, 28);
            this.btnPlaySet.Text = "Play Set";
            this.btnPlaySet.Click += new System.EventHandler(this.btnPlaySet_Click);
            // 
            // btnCreateDataSet
            // 
            this.btnCreateDataSet.Image = global::LinkeD365.MockDataGen.Properties.Resources.Data_Settings;
            this.btnCreateDataSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateDataSet.Name = "btnCreateDataSet";
            this.btnCreateDataSet.Size = new System.Drawing.Size(115, 28);
            this.btnCreateDataSet.Text = "Create Data Set";
            this.btnCreateDataSet.ToolTipText = "Create Data Set";
            this.btnCreateDataSet.Click += new System.EventHandler(this.BtnCreateDataSet_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // ddExport
            // 
            this.ddExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExportMaps,
            this.btnExportSets});
            this.ddExport.Image = global::LinkeD365.MockDataGen.Properties.Resources.baseline_save_alt_black_18dp;
            this.ddExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddExport.Name = "ddExport";
            this.ddExport.Size = new System.Drawing.Size(78, 28);
            this.ddExport.Text = "Export";
            // 
            // btnExportMaps
            // 
            this.btnExportMaps.Name = "btnExportMaps";
            this.btnExportMaps.Size = new System.Drawing.Size(111, 22);
            this.btnExportMaps.Text = "Map(s)";
            this.btnExportMaps.Click += new System.EventHandler(this.btnExportMaps_Click);
            // 
            // btnExportSets
            // 
            this.btnExportSets.Name = "btnExportSets";
            this.btnExportSets.Size = new System.Drawing.Size(111, 22);
            this.btnExportSets.Text = "Set(s)";
            this.btnExportSets.Click += new System.EventHandler(this.btnExportSets_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = global::LinkeD365.MockDataGen.Properties.Resources.baseline_publish_black_18dp;
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(71, 28);
            this.btnImport.Text = "Import";
            this.btnImport.ToolTipText = "Import Maps or Sets file";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // gridMap
            // 
            this.gridMap.AllowUserToAddRows = false;
            this.gridMap.AllowUserToDeleteRows = false;
            this.gridMap.AllowUserToOrderColumns = true;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridMap.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            this.gridMap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridMap.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridMap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gridMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridMap.DefaultCellStyle = dataGridViewCellStyle13;
            this.gridMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMap.Location = new System.Drawing.Point(3, 16);
            this.gridMap.MultiSelect = false;
            this.gridMap.Name = "gridMap";
            this.gridMap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMap.ShowEditingIcon = false;
            this.gridMap.Size = new System.Drawing.Size(1004, 334);
            this.gridMap.TabIndex = 5;
            this.gridMap.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMap_CellClick);
            this.gridMap.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridMap_CellEnter);
            this.gridMap.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridMap_CellPainting);
            this.gridMap.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridMap_CurrentCellDirtyStateChanged);
            this.gridMap.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridMap_DataError);
            this.gridMap.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.gridMap_EditingControlShowing);
            this.gridMap.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.gridMap_RowStateChanged);
            this.gridMap.Sorted += new System.EventHandler(this.gridMap_Sorted);
            // 
            // splitConfig
            // 
            this.splitConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitConfig.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitConfig.Location = new System.Drawing.Point(3, 3);
            this.splitConfig.Name = "splitConfig";
            this.splitConfig.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitConfig.Panel1
            // 
            this.splitConfig.Panel1.Controls.Add(this.grpEntity);
            // 
            // splitConfig.Panel2
            // 
            this.splitConfig.Panel2.Controls.Add(this.grpAttributes);
            this.splitConfig.Size = new System.Drawing.Size(1010, 430);
            this.splitConfig.SplitterDistance = 73;
            this.splitConfig.TabIndex = 6;
            // 
            // grpEntity
            // 
            this.grpEntity.Controls.Add(this.lblPrimary);
            this.grpEntity.Controls.Add(this.cboEntities);
            this.grpEntity.Controls.Add(this.lblRecordCount);
            this.grpEntity.Controls.Add(this.numRecordCount);
            this.grpEntity.Controls.Add(this.btnMock);
            this.grpEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEntity.Location = new System.Drawing.Point(0, 0);
            this.grpEntity.Name = "grpEntity";
            this.grpEntity.Size = new System.Drawing.Size(1010, 73);
            this.grpEntity.TabIndex = 6;
            this.grpEntity.TabStop = false;
            this.grpEntity.Text = "Select Table";
            // 
            // lblPrimary
            // 
            this.lblPrimary.AutoSize = true;
            this.lblPrimary.Location = new System.Drawing.Point(6, 47);
            this.lblPrimary.Name = "lblPrimary";
            this.lblPrimary.Size = new System.Drawing.Size(113, 13);
            this.lblPrimary.TabIndex = 10;
            this.lblPrimary.Text = "Primary Name Column:";
            // 
            // cboEntities
            // 
            this.cboEntities.FormattingEnabled = true;
            this.cboEntities.Location = new System.Drawing.Point(6, 19);
            this.cboEntities.Name = "cboEntities";
            this.cboEntities.Size = new System.Drawing.Size(339, 21);
            this.cboEntities.TabIndex = 9;
            this.cboEntities.SelectedValueChanged += new System.EventHandler(this.cboEntities_SelectedValueChanged);
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordCount.AutoSize = true;
            this.lblRecordCount.Location = new System.Drawing.Point(637, 22);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(102, 13);
            this.lblRecordCount.TabIndex = 8;
            this.lblRecordCount.Text = "Number of Records:";
            // 
            // numRecordCount
            // 
            this.numRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRecordCount.Location = new System.Drawing.Point(745, 20);
            this.numRecordCount.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.numRecordCount.Name = "numRecordCount";
            this.numRecordCount.Size = new System.Drawing.Size(120, 20);
            this.numRecordCount.TabIndex = 7;
            this.numRecordCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // btnMock
            // 
            this.btnMock.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMock.Location = new System.Drawing.Point(871, 16);
            this.btnMock.Name = "btnMock";
            this.btnMock.Size = new System.Drawing.Size(136, 54);
            this.btnMock.TabIndex = 6;
            this.btnMock.Text = "Get Mockaroo Data";
            this.btnMock.UseVisualStyleBackColor = true;
            this.btnMock.Click += new System.EventHandler(this.btnMockData_Click);
            // 
            // grpAttributes
            // 
            this.grpAttributes.Controls.Add(this.gridMap);
            this.grpAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAttributes.Location = new System.Drawing.Point(0, 0);
            this.grpAttributes.Name = "grpAttributes";
            this.grpAttributes.Size = new System.Drawing.Size(1010, 353);
            this.grpAttributes.TabIndex = 6;
            this.grpAttributes.TabStop = false;
            this.grpAttributes.Text = "Configure Map";
            // 
            // tabGrpMain
            // 
            this.tabGrpMain.Controls.Add(this.tabConfig);
            this.tabGrpMain.Controls.Add(this.tabSample);
            this.tabGrpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGrpMain.Location = new System.Drawing.Point(0, 31);
            this.tabGrpMain.Name = "tabGrpMain";
            this.tabGrpMain.SelectedIndex = 0;
            this.tabGrpMain.Size = new System.Drawing.Size(1024, 462);
            this.tabGrpMain.TabIndex = 7;
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.splitConfig);
            this.tabConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(1016, 436);
            this.tabConfig.TabIndex = 0;
            this.tabConfig.Text = "Config";
            this.tabConfig.UseVisualStyleBackColor = true;
            // 
            // tabSample
            // 
            this.tabSample.Controls.Add(this.splitResults);
            this.tabSample.Location = new System.Drawing.Point(4, 22);
            this.tabSample.Name = "tabSample";
            this.tabSample.Padding = new System.Windows.Forms.Padding(3);
            this.tabSample.Size = new System.Drawing.Size(1016, 436);
            this.tabSample.TabIndex = 1;
            this.tabSample.Text = "Results";
            this.tabSample.UseVisualStyleBackColor = true;
            // 
            // splitResults
            // 
            this.splitResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitResults.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitResults.Location = new System.Drawing.Point(3, 3);
            this.splitResults.Name = "splitResults";
            this.splitResults.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitResults.Panel1
            // 
            this.splitResults.Panel1.Controls.Add(this.batchSizeLabel);
            this.splitResults.Panel1.Controls.Add(this.batchSize);
            this.splitResults.Panel1.Controls.Add(this.btnCreateAllData);
            this.splitResults.Panel1.Controls.Add(this.btnCreate100Data);
            // 
            // splitResults.Panel2
            // 
            this.splitResults.Panel2.Controls.Add(this.gridSample);
            this.splitResults.Size = new System.Drawing.Size(1010, 430);
            this.splitResults.SplitterDistance = 61;
            this.splitResults.TabIndex = 5;
            // 
            // batchSizeLabel
            // 
            this.batchSizeLabel.AutoSize = true;
            this.batchSizeLabel.Location = new System.Drawing.Point(3, 11);
            this.batchSizeLabel.Name = "batchSizeLabel";
            this.batchSizeLabel.Size = new System.Drawing.Size(58, 13);
            this.batchSizeLabel.TabIndex = 3;
            this.batchSizeLabel.Text = "Batch Size";
            // 
            // batchSize
            // 
            this.batchSize.Location = new System.Drawing.Point(6, 26);
            this.batchSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.batchSize.Name = "batchSize";
            this.batchSize.Size = new System.Drawing.Size(63, 20);
            this.batchSize.TabIndex = 2;
            this.batchSize.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // btnCreateAllData
            // 
            this.btnCreateAllData.AutoSize = true;
            this.btnCreateAllData.Location = new System.Drawing.Point(90, 31);
            this.btnCreateAllData.Name = "btnCreateAllData";
            this.btnCreateAllData.Size = new System.Drawing.Size(121, 23);
            this.btnCreateAllData.TabIndex = 1;
            this.btnCreateAllData.Text = "Create X Rows";
            this.btnCreateAllData.UseVisualStyleBackColor = true;
            this.btnCreateAllData.Click += new System.EventHandler(this.btnCreateAllData_Click);
            // 
            // btnCreate100Data
            // 
            this.btnCreate100Data.AutoSize = true;
            this.btnCreate100Data.Location = new System.Drawing.Point(89, 5);
            this.btnCreate100Data.Name = "btnCreate100Data";
            this.btnCreate100Data.Size = new System.Drawing.Size(122, 23);
            this.btnCreate100Data.TabIndex = 0;
            this.btnCreate100Data.Text = "Create 100 Rows";
            this.btnCreate100Data.UseVisualStyleBackColor = true;
            this.btnCreate100Data.Click += new System.EventHandler(this.btnCreate100Data_Click);
            // 
            // gridSample
            // 
            this.gridSample.AllowUserToOrderColumns = true;
            this.gridSample.AllowUserToResizeRows = false;
            this.gridSample.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSample.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.gridSample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSample.ColumnOrder = "";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSample.DefaultCellStyle = dataGridViewCellStyle15;
            this.gridSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSample.FilterColumns = "";
            this.gridSample.Location = new System.Drawing.Point(0, 0);
            this.gridSample.Name = "gridSample";
            this.gridSample.OrganizationService = null;
            this.gridSample.ShowFriendlyNames = true;
            this.gridSample.ShowIdColumn = false;
            this.gridSample.Size = new System.Drawing.Size(1010, 365);
            this.gridSample.TabIndex = 4;
            // 
            // tabGrpHidden
            // 
            this.tabGrpHidden.Location = new System.Drawing.Point(96, 55);
            this.tabGrpHidden.Name = "tabGrpHidden";
            this.tabGrpHidden.SelectedIndex = 0;
            this.tabGrpHidden.Size = new System.Drawing.Size(200, 100);
            this.tabGrpHidden.TabIndex = 7;
            // 
            // MockDataGenCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabGrpMain);
            this.Controls.Add(this.tabGrpHidden);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MockDataGenCtl";
            this.PluginIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PluginIcon")));
            this.Size = new System.Drawing.Size(1024, 493);
            this.TabIcon = global::LinkeD365.MockDataGen.Properties.Resources.smallIcon_80;
            this.OnCloseTool += new System.EventHandler(this.MockDataGen_OnCloseTool);
            this.Load += new System.EventHandler(this.MockDataGen_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMap)).EndInit();
            this.splitConfig.Panel1.ResumeLayout(false);
            this.splitConfig.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitConfig)).EndInit();
            this.splitConfig.ResumeLayout(false);
            this.grpEntity.ResumeLayout(false);
            this.grpEntity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecordCount)).EndInit();
            this.grpAttributes.ResumeLayout(false);
            this.tabGrpMain.ResumeLayout(false);
            this.tabConfig.ResumeLayout(false);
            this.tabSample.ResumeLayout(false);
            this.splitResults.Panel1.ResumeLayout(false);
            this.splitResults.Panel1.PerformLayout();
            this.splitResults.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitResults)).EndInit();
            this.splitResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.batchSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSample)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.DataGridView gridMap;
        private System.Windows.Forms.SplitContainer splitConfig;
        private System.Windows.Forms.TabControl tabGrpMain;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.TabPage tabSample;
        private xrmtb.XrmToolBox.Controls.CRMGridView gridSample;
        private System.Windows.Forms.GroupBox grpEntity;
        private System.Windows.Forms.Button btnMock;
        private System.Windows.Forms.GroupBox grpAttributes;
        private System.Windows.Forms.TabControl tabGrpHidden;
        private System.Windows.Forms.SplitContainer splitResults;
        private System.Windows.Forms.Button btnCreate100Data;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.NumericUpDown numRecordCount;
        private System.Windows.Forms.ToolStripTextBox txtMockKey;
        private System.Windows.Forms.ToolStripComboBox cboSelectSaved;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ComboBox cboEntities;
        private System.Windows.Forms.Label lblPrimary;
        private System.Windows.Forms.Button btnCreateAllData;
        private System.Windows.Forms.ToolStripButton btnCreateDataSet;
        private System.Windows.Forms.ToolStripComboBox cboRunDataSet;
        private System.Windows.Forms.ToolStripButton btnPlaySet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton ddConfig;
        private System.Windows.Forms.ToolStripMenuItem btnDepTables;
        private System.Windows.Forms.ToolStripMenuItem btnDepCol;
        private System.Windows.Forms.ToolStripMenuItem btnDepImpSeqNo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton ddExport;
        private System.Windows.Forms.ToolStripMenuItem btnExportMaps;
        private System.Windows.Forms.ToolStripMenuItem btnExportSets;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.Label batchSizeLabel;
        private System.Windows.Forms.NumericUpDown batchSize;
        private System.Windows.Forms.ToolStripMenuItem btnBypassPluginExec;
    }
}
