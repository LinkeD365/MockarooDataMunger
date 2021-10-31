
namespace LinkeD365.MockDataGen
{
    partial class Export
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Export));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpSelector = new System.Windows.Forms.GroupBox();
            this.chkListSelect = new System.Windows.Forms.CheckedListBox();
            this.grpSelector.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(66, 148);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(158, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpSelector
            // 
            this.grpSelector.Controls.Add(this.chkListSelect);
            this.grpSelector.Location = new System.Drawing.Point(12, 12);
            this.grpSelector.Name = "grpSelector";
            this.grpSelector.Size = new System.Drawing.Size(281, 115);
            this.grpSelector.TabIndex = 2;
            this.grpSelector.TabStop = false;
            this.grpSelector.Text = "Select ";
            // 
            // chkListSelect
            // 
            this.chkListSelect.CheckOnClick = true;
            this.chkListSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkListSelect.FormattingEnabled = true;
            this.chkListSelect.Location = new System.Drawing.Point(3, 16);
            this.chkListSelect.Name = "chkListSelect";
            this.chkListSelect.Size = new System.Drawing.Size(275, 94);
            this.chkListSelect.TabIndex = 0;
            // 
            // Export
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(305, 183);
            this.Controls.Add(this.grpSelector);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Export";
            this.ShowInTaskbar = false;
            this.Text = "Export";
            this.grpSelector.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.GroupBox grpSelector;
        public System.Windows.Forms.CheckedListBox chkListSelect;
    }
}