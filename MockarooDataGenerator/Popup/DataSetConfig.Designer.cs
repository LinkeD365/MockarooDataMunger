
namespace LinkeD365.MockDataGen
{
    partial class DataSetConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataSetConfig));
            this.splitGrids = new System.Windows.Forms.SplitContainer();
            this.splitLeft = new System.Windows.Forms.SplitContainer();
            this.listMaps = new System.Windows.Forms.ListBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnMoveToSet = new System.Windows.Forms.Button();
            this.grdSet = new System.Windows.Forms.DataGridView();
            this.splitFull = new System.Windows.Forms.SplitContainer();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.cboExisting = new System.Windows.Forms.ComboBox();
            this.lblExisting = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).BeginInit();
            this.splitGrids.Panel1.SuspendLayout();
            this.splitGrids.Panel2.SuspendLayout();
            this.splitGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).BeginInit();
            this.splitLeft.Panel1.SuspendLayout();
            this.splitLeft.Panel2.SuspendLayout();
            this.splitLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitFull)).BeginInit();
            this.splitFull.Panel1.SuspendLayout();
            this.splitFull.Panel2.SuspendLayout();
            this.splitFull.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitGrids
            // 
            this.splitGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitGrids.Location = new System.Drawing.Point(0, 0);
            this.splitGrids.Name = "splitGrids";
            // 
            // splitGrids.Panel1
            // 
            this.splitGrids.Panel1.Controls.Add(this.splitLeft);
            // 
            // splitGrids.Panel2
            // 
            this.splitGrids.Panel2.Controls.Add(this.grdSet);
            this.splitGrids.Size = new System.Drawing.Size(544, 378);
            this.splitGrids.SplitterDistance = 266;
            this.splitGrids.TabIndex = 0;
            // 
            // splitLeft
            // 
            this.splitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLeft.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitLeft.Location = new System.Drawing.Point(0, 0);
            this.splitLeft.Name = "splitLeft";
            // 
            // splitLeft.Panel1
            // 
            this.splitLeft.Panel1.Controls.Add(this.listMaps);
            // 
            // splitLeft.Panel2
            // 
            this.splitLeft.Panel2.Controls.Add(this.btnDown);
            this.splitLeft.Panel2.Controls.Add(this.btnUp);
            this.splitLeft.Panel2.Controls.Add(this.btnRemove);
            this.splitLeft.Panel2.Controls.Add(this.btnMoveToSet);
            this.splitLeft.Size = new System.Drawing.Size(266, 378);
            this.splitLeft.SplitterDistance = 203;
            this.splitLeft.TabIndex = 0;
            // 
            // listMaps
            // 
            this.listMaps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMaps.FormattingEnabled = true;
            this.listMaps.Location = new System.Drawing.Point(0, 0);
            this.listMaps.Name = "listMaps";
            this.listMaps.Size = new System.Drawing.Size(203, 378);
            this.listMaps.TabIndex = 2;
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRemove.Location = new System.Drawing.Point(8, 92);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(43, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // btnMoveToSet
            // 
            this.btnMoveToSet.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMoveToSet.Location = new System.Drawing.Point(8, 63);
            this.btnMoveToSet.Name = "btnMoveToSet";
            this.btnMoveToSet.Size = new System.Drawing.Size(43, 23);
            this.btnMoveToSet.TabIndex = 2;
            this.btnMoveToSet.Text = ">";
            this.btnMoveToSet.UseVisualStyleBackColor = true;
            this.btnMoveToSet.Click += new System.EventHandler(this.btnMoveToSet_Click);
            // 
            // grdSet
            // 
            this.grdSet.AllowUserToAddRows = false;
            this.grdSet.AllowUserToDeleteRows = false;
            this.grdSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSet.Location = new System.Drawing.Point(0, 0);
            this.grdSet.MultiSelect = false;
            this.grdSet.Name = "grdSet";
            this.grdSet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSet.Size = new System.Drawing.Size(274, 378);
            this.grdSet.TabIndex = 0;
            // 
            // splitFull
            // 
            this.splitFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFull.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitFull.Location = new System.Drawing.Point(0, 0);
            this.splitFull.Name = "splitFull";
            this.splitFull.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitFull.Panel1
            // 
            this.splitFull.Panel1.Controls.Add(this.splitMain);
            // 
            // splitFull.Panel2
            // 
            this.splitFull.Panel2.Controls.Add(this.btnCancel);
            this.splitFull.Panel2.Controls.Add(this.btnOK);
            this.splitFull.Size = new System.Drawing.Size(544, 461);
            this.splitFull.SplitterDistance = 410;
            this.splitFull.TabIndex = 1;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitMain.Location = new System.Drawing.Point(0, 0);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.cboExisting);
            this.splitMain.Panel1.Controls.Add(this.lblExisting);
            this.splitMain.Panel1.Controls.Add(this.lblName);
            this.splitMain.Panel1.Controls.Add(this.txtName);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.splitGrids);
            this.splitMain.Size = new System.Drawing.Size(544, 410);
            this.splitMain.SplitterDistance = 28;
            this.splitMain.TabIndex = 1;
            // 
            // cboExisting
            // 
            this.cboExisting.FormattingEnabled = true;
            this.cboExisting.Location = new System.Drawing.Point(366, 4);
            this.cboExisting.Name = "cboExisting";
            this.cboExisting.Size = new System.Drawing.Size(167, 21);
            this.cboExisting.TabIndex = 3;
            this.cboExisting.SelectedIndexChanged += new System.EventHandler(this.cboExisting_SelectedIndexChanged);
            // 
            // lblExisting
            // 
            this.lblExisting.AutoSize = true;
            this.lblExisting.Location = new System.Drawing.Point(276, 8);
            this.lblExisting.Name = "lblExisting";
            this.lblExisting.Size = new System.Drawing.Size(84, 13);
            this.lblExisting.TabIndex = 2;
            this.lblExisting.Text = "Update Existing:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(65, 5);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(205, 20);
            this.txtName.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(456, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(364, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUp.Location = new System.Drawing.Point(8, 176);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(43, 23);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDown.Location = new System.Drawing.Point(8, 205);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(43, 23);
            this.btnDown.TabIndex = 5;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // DataSetConfig
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(544, 461);
            this.Controls.Add(this.splitFull);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(560, 500);
            this.Name = "DataSetConfig";
            this.Text = "DataSet";
            this.Load += new System.EventHandler(this.DataSetConfig_Load);
            this.splitGrids.Panel1.ResumeLayout(false);
            this.splitGrids.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitGrids)).EndInit();
            this.splitGrids.ResumeLayout(false);
            this.splitLeft.Panel1.ResumeLayout(false);
            this.splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).EndInit();
            this.splitLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSet)).EndInit();
            this.splitFull.Panel1.ResumeLayout(false);
            this.splitFull.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFull)).EndInit();
            this.splitFull.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitGrids;
        private System.Windows.Forms.SplitContainer splitFull;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.SplitContainer splitLeft;
        private System.Windows.Forms.ListBox listMaps;
        private System.Windows.Forms.Button btnMoveToSet;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ComboBox cboExisting;
        private System.Windows.Forms.Label lblExisting;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.DataGridView grdSet;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
    }
}