using LinkeD365.MockDataGen.Mock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LinkeD365.MockDataGen
{

    public partial class DataSetConfig : Form
    {
        public AllSettings Settings;
        public DataSetConfig(AllSettings allSettings)
        {
            InitializeComponent();
            Settings = allSettings;
        }

        public SortableBindingList<SetItem> SetItems = new SortableBindingList<SetItem>();

        void SetUpGrid()
        {
            grdSet.Columns["MapName"].ReadOnly = true;
            grdSet.Columns["MapName"].DisplayIndex = 1;
            grdSet.Columns["MapName"].HeaderText = "Map Name";
            grdSet.Columns["MapName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            grdSet.Columns["RecordCount"].HeaderText = "No Records";
            grdSet.Columns["RecordCount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            grdSet.Columns["Position"].DisplayIndex = 0;
            grdSet.Columns["Position"].ReadOnly = true;
            grdSet.Columns["Position"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            if (grdSet.Rows.Count > 0) grdSet.Sort(grdSet.Columns["Position"], ListSortDirection.Ascending);
            grdSet.AutoResizeColumns();
        }
        private void DataSetConfig_Load(object sender, EventArgs e)
        {
            listMaps.Items.Clear();
            listMaps.Items.AddRange(Settings.Settings.Select(rw => rw.Name).ToArray());
            grdSet.DataSource = SetItems;
            SetUpGrid();

            cboExisting.Items.AddRange(Settings.Sets.Select(set => set.SetName).ToArray());
        }

        private void btnMoveToSet_Click(object sender, EventArgs e)
        {
            if (listMaps.SelectedItem != null)
            {
                SetItems.Add(new SetItem(listMaps.SelectedItem.ToString(), 100, SetItems.Count == 0 ? 1 : SetItems.Max(si => si.Position) + 1));
                listMaps.Items.Remove(listMaps.SelectedItem);

            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (grdSet.SelectedRows.Count >= 1)
                foreach (DataGridViewRow row in grdSet.SelectedRows)
                {
                    SetItem removeItem = row.DataBoundItem as SetItem;
                    listMaps.Items.Add(removeItem.MapName);
                    SetItems.Remove(removeItem);
                }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text) || SetItems.Count == 0)
            {
                MessageBox.Show(
                    "Please enter a name or select an existing dataset to update as well as one or more maps to include",
                    "Details required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
                return;
            }

            Set newSet = new Set { SetName = txtName.Text, SetItems = SetItems.ToList() };
            if (Settings.Sets.Any(set => set.SetName == txtName.Text))
                Settings.Sets[Settings.Sets.IndexOf(newSet)] = newSet;
            else
                Settings.Sets.Add(newSet);
        }

        private void cboExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set set = Settings.Sets.First(s => s.SetName == cboExisting.SelectedItem.ToString());

            SetItems.Clear();
            set.SetItems.ForEach(si => SetItems.Add(new SetItem(si.MapName, si.RecordCount, si.Position)));

            listMaps.Items.Clear();
            listMaps.Items.AddRange(
                    Settings.Settings.Where(st => !set.SetItems.Any(si => si.MapName == st.Name)).Select(st => st.Name).ToArray());


            txtName.Text = set.SetName;
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (grdSet.SelectedRows.Count >= 1)
            {
                var setitem = (SetItem)grdSet.SelectedRows[0].DataBoundItem;
                if (setitem.Position == 1) return;

                setitem.Position--;

                SetItems.First(si => si.Position == setitem.Position && si.MapName != setitem.MapName).Position++;
                grdSet.Refresh();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (grdSet.SelectedRows.Count >= 1)
            {
                var setitem = (SetItem)grdSet.SelectedRows[0].DataBoundItem;
                if (setitem.Position == SetItems.Max(si => si.Position)) return;

                setitem.Position++;

                SetItems.First(si => si.Position == setitem.Position && si.MapName != setitem.MapName).Position--;
                grdSet.Refresh();
            }
        }
    }


}
