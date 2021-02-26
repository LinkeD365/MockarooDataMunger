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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtMockKey = new System.Windows.Forms.ToolStripTextBox();
            this.cboSelectSaved = new System.Windows.Forms.ToolStripComboBox();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.cboRunDataSet = new System.Windows.Forms.ToolStripComboBox();
            this.btnPlaySet = new System.Windows.Forms.ToolStripButton();
            this.btnCreateDataSet = new System.Windows.Forms.ToolStripButton();
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
            this.btnCreateBatch = new System.Windows.Forms.Button();
            this.btnCreateData = new System.Windows.Forms.Button();
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
            this.cboSelectSaved,
            this.btnSave,
            this.cboRunDataSet,
            this.btnPlaySet,
            this.btnCreateDataSet});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(860, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(110, 28);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
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
            // gridMap
            // 
            this.gridMap.AllowUserToAddRows = false;
            this.gridMap.AllowUserToDeleteRows = false;
            this.gridMap.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gridMap.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridMap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridMap.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridMap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridMap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridMap.DefaultCellStyle = dataGridViewCellStyle3;
            this.gridMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMap.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMap.Location = new System.Drawing.Point(3, 16);
            this.gridMap.MultiSelect = false;
            this.gridMap.Name = "gridMap";
            this.gridMap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMap.ShowEditingIcon = false;
            this.gridMap.Size = new System.Drawing.Size(840, 334);
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
            this.splitConfig.Size = new System.Drawing.Size(846, 430);
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
            this.grpEntity.Size = new System.Drawing.Size(846, 73);
            this.grpEntity.TabIndex = 6;
            this.grpEntity.TabStop = false;
            this.grpEntity.Text = "Select Entity";
            // 
            // lblPrimary
            // 
            this.lblPrimary.AutoSize = true;
            this.lblPrimary.Location = new System.Drawing.Point(6, 47);
            this.lblPrimary.Name = "lblPrimary";
            this.lblPrimary.Size = new System.Drawing.Size(100, 13);
            this.lblPrimary.TabIndex = 10;
            this.lblPrimary.Text = "Primary Name Field:";
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
            this.lblRecordCount.Location = new System.Drawing.Point(473, 22);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(102, 13);
            this.lblRecordCount.TabIndex = 8;
            this.lblRecordCount.Text = "Number of Records:";
            // 
            // numRecordCount
            // 
            this.numRecordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numRecordCount.Location = new System.Drawing.Point(581, 20);
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
            this.btnMock.Location = new System.Drawing.Point(707, 16);
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
            this.grpAttributes.Size = new System.Drawing.Size(846, 353);
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
            this.tabGrpMain.Size = new System.Drawing.Size(860, 462);
            this.tabGrpMain.TabIndex = 7;
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.splitConfig);
            this.tabConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(852, 436);
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
            this.tabSample.Size = new System.Drawing.Size(852, 436);
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
            this.splitResults.Panel1.Controls.Add(this.btnCreateBatch);
            this.splitResults.Panel1.Controls.Add(this.btnCreateData);
            // 
            // splitResults.Panel2
            // 
            this.splitResults.Panel2.Controls.Add(this.gridSample);
            this.splitResults.Size = new System.Drawing.Size(846, 430);
            this.splitResults.SplitterDistance = 48;
            this.splitResults.TabIndex = 5;
            // 
            // btnCreateBatch
            // 
            this.btnCreateBatch.AutoSize = true;
            this.btnCreateBatch.Location = new System.Drawing.Point(144, 12);
            this.btnCreateBatch.Name = "btnCreateBatch";
            this.btnCreateBatch.Size = new System.Drawing.Size(145, 23);
            this.btnCreateBatch.TabIndex = 1;
            this.btnCreateBatch.Text = "Create All Remaining Rows";
            this.btnCreateBatch.UseVisualStyleBackColor = true;
            this.btnCreateBatch.Click += new System.EventHandler(this.BtnCreateBatch_Click);
            // 
            // btnCreateData
            // 
            this.btnCreateData.AutoSize = true;
            this.btnCreateData.Location = new System.Drawing.Point(30, 12);
            this.btnCreateData.Name = "btnCreateData";
            this.btnCreateData.Size = new System.Drawing.Size(75, 23);
            this.btnCreateData.TabIndex = 0;
            this.btnCreateData.Text = "Create Data";
            this.btnCreateData.UseVisualStyleBackColor = true;
            this.btnCreateData.Click += new System.EventHandler(this.btnCreateData_Click);
            // 
            // gridSample
            // 
            this.gridSample.AllowUserToOrderColumns = true;
            this.gridSample.AllowUserToResizeRows = false;
            this.gridSample.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSample.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridSample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSample.ColumnOrder = "";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridSample.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridSample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSample.FilterColumns = "";
            this.gridSample.Location = new System.Drawing.Point(0, 0);
            this.gridSample.Name = "gridSample";
            this.gridSample.OrganizationService = null;
            this.gridSample.ShowFriendlyNames = true;
            this.gridSample.ShowIdColumn = false;
            this.gridSample.Size = new System.Drawing.Size(846, 378);
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
            this.Size = new System.Drawing.Size(860, 493);
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
        private System.Windows.Forms.Button btnCreateData;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.NumericUpDown numRecordCount;
        private System.Windows.Forms.ToolStripTextBox txtMockKey;
        private System.Windows.Forms.ToolStripComboBox cboSelectSaved;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ComboBox cboEntities;
        private System.Windows.Forms.Label lblPrimary;
        private System.Windows.Forms.Button btnCreateBatch;
        private System.Windows.Forms.ToolStripButton btnCreateDataSet;
        private System.Windows.Forms.ToolStripComboBox cboRunDataSet;
        private System.Windows.Forms.ToolStripButton btnPlaySet;
    }
}
