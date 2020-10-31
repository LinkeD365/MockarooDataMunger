using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace LinkeD365.MockDataGen
{
    public partial class MockDataGenCtl : PluginControlBase
    {
        private void BuildGrid(string logicalName)
        {
            gridMap.DataSource = null;
            gridMap.AutoGenerateColumns = false;
            if (tabSample.Parent == tabGrpMain)
            {
                tabGrpHidden.TabPages.Add(tabSample);
            }

            List<string> mapsNotFound = new List<string>();
            string primaryFieldLabel = "Primary Name Field: ";
            var saveMap = cboSelectSaved.SelectedItem == null ? null : mySettings.Settings.First(stng => stng.Name == cboSelectSaved.SelectedItem.ToString());
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Retrieving Attributes...",
                Work = (w, e) =>
                {
                    var entityMeta = Service.GetEntityMetadata(logicalName);// entitiesDD.SelectedEntity.LogicalName);
                    var primaryField = entityMeta.Attributes.First(at => at.LogicalName == entityMeta.PrimaryNameAttribute) as StringAttributeMetadata;
                    primaryFieldLabel += primaryField.DisplayName.UserLocalizedLabel == null ? primaryField.LogicalName : primaryField.DisplayName.UserLocalizedLabel.Label + " ( " + primaryField.LogicalName + " ) ";
                    if (!string.IsNullOrEmpty(primaryField.FormulaDefinition))
                    {
                        primaryFieldLabel += " Formula: " + primaryField.FormulaDefinition;
                    }

                    SortableBindingList<MapRow> attributes = new SortableBindingList<MapRow>();
                    w.ReportProgress(50, "Got Attributes");
                    foreach (var field in entityMeta.Attributes.Where(fld => !notPermitted.Any(np => np == fld.AttributeType)).Where(fld => fld.IsValidForCreate == true))
                    {
                        var mapRow = new MapRow(field);
                        mapRow.PropertyChanged += MapRow_PropertyChanged;

                        attributes.Add(mapRow);
                    }

                    w.ReportProgress(100, "Loading Grid");
                    e.Result = attributes;
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    gridMap.DataSource = e.Result;
                    SetUpColumns();
                    //ColourPrimaryNameField(e.Result as SortableBindingList<MapRow>);
                    lblPrimary.Text = primaryFieldLabel;
                    LogInfo(DateTime.UtcNow + " |  Start with SavedMaps");
                    if (saveMap != null)
                    {
                        var attributes = gridMap.DataSource as SortableBindingList<MapRow>;
                        foreach (var map in saveMap.MapRows)
                        {
                            if (attributes.Any(mr => mr.AttributeName == map.AttributeName))
                            {
                                // #5 If no mocks in the save, don't bother doing anything
                                if (map.SelectedMock.Count > 0)
                                {
                                    var mapRow = attributes.First(mr => mr.AttributeName == map.AttributeName);
                                    AddOptions(mapRow);
                                    var mockType = map.SelectedMock.First(kvp => kvp.Key == "MockName").Value.ToString();
                                    var mockOption = mapRow.MockOptions.Mocks.FirstOrDefault(m => m.Name == mockType);
                                    if (mockOption != null)
                                    {
                                        selectedMaps.Add(mapRow);
                                        mapRow.PropertyChanged -= MapRow_PropertyChanged;
                                        LogInfo(DateTime.UtcNow + " |  Pre Select Change");
                                        mapRow.Selected = true;
                                        LogInfo(DateTime.UtcNow + " |  Pre mocktype change");

                                        mapRow.MockType = mockType;

                                        mapRow.SelectedMock = mockOption.Clone();
                                        LogInfo(DateTime.UtcNow + " |  Pre KVPPopulate");
                                        PopulateLookup(mapRow.Attribute, mapRow.SelectedMock);
                                        PopulatePickList(mapRow.Attribute, mapRow.SelectedMock);
                                        ;
                                        mapRow.SelectedMock.PopulateFromKVP(map.SelectedMock);
                                        LogInfo(DateTime.UtcNow + " | post KVPPopulate");

                                        gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First().Cells["Config"].ReadOnly
                                            = mapRow.AdditionalProperties == string.Empty;
                                        gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First().Cells[percBlank].ReadOnly
                                            = mapRow.SelectedMock.Fixed;

                                        SetUpNumberDefaults(mapRow.Attribute, mapRow.SelectedMock);
                                        mapRow.PropertyChanged += MapRow_PropertyChanged;
                                    }
                                }
                            }
                            else
                            {
                                mapsNotFound.Add(map.AttributeName);
                            }
                        }
                    }

                    if (mapsNotFound.Count > 0)
                    {
                        MessageBox.Show("These attributes could not be found\r\n" + string.Join("\r\n", mapsNotFound.ToArray()) + "\r\nPlease ensure mapping still valid", "Not all maps found", MessageBoxButtons.OK);
                    }
                    gridMap.AutoResizeColumns();
                    gridMap.Sort(gridMap.Columns[1], ListSortDirection.Ascending);
                    LogInfo(DateTime.UtcNow + " |  Ended with SavedMaps");
                }
            });
        }

        private void ColourPrimaryNameField(SortableBindingList<MapRow> mapRows)
        {
            //var primaryRow = mapRows.First(mr => mr.Attribute.IsPrimaryName.GetValueOrDefault());

            gridMap.Rows.Cast<DataGridViewRow>().Where(mr => ((MapRow)mr.DataBoundItem).Attribute.IsPrimaryName.GetValueOrDefault()).First().DefaultCellStyle = new DataGridViewCellStyle() { BackColor = Color.Yellow };
        }

        private void SetUpColumns()
        {
            if (gridMap.Columns.Count > 0)
            {
                return;
            }

            var selectedCol = new DataGridViewCheckBoxColumn()
            { DataPropertyName = "Selected", Name = " \n " };
            gridMap.Columns.Add(selectedCol);

            var fieldName = new DataGridViewTextBoxColumn() { DataPropertyName = "AttributeName", Name = "Attribute" };
            gridMap.Columns.Add(fieldName);

            var dataType = new DataGridViewTextBoxColumn() { DataPropertyName = "AttributeType", Name = "Type" };
            gridMap.Columns.Add(dataType);

            var length = new DataGridViewTextBoxColumn() { DataPropertyName = "AttributeLength", Name = "Length" };
            gridMap.Columns.Add(length);

            var combo = new DataGridViewComboBoxColumn();
            combo.ReadOnly = true;
            combo.Name = "MockType";
            combo.DataPropertyName = "MockType";
            gridMap.Columns.Add(combo);

            var options = new DataGridViewTextBoxColumn() { DataPropertyName = "AdditionalProperties", Name = "Options" };
            options.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            gridMap.Columns.Add(options);

            var btnConfig = new DataGridViewButtonColumn() { HeaderText = " ", Name = "Config" };
            btnConfig.Text = "c";
            btnConfig.ReadOnly = true;
            gridMap.Columns.Add(btnConfig);

            var intBlank = new DataGridViewTextBoxColumn() { DataPropertyName = "BlankPercentage", Name = percBlank };
            intBlank.ReadOnly = true;
            gridMap.Columns.Add(intBlank);
        }


        private void gridMap_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
            var datagridview = sender as DataGridView;

            // Check to make sure the cell clicked is the cell containing the combobox
            if (!datagridview.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly && datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
            {
                datagridview.BeginEdit(true);
                ((ComboBox)datagridview.EditingControl).DroppedDown = true;
            }
        }

        private void gridMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
            {
                return;
            }

            if (!gridMap.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly && gridMap.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                try
                {
                    var selectedRow = gridMap.Rows[e.RowIndex].DataBoundItem as MapRow;
                    ConfigEdit configEdit = new ConfigEdit(selectedRow, Service);
                    configEdit.Location = MousePosition;

                    configEdit.ShowDialog();

                    if (configEdit.DialogResult == DialogResult.OK)
                    {
                        selectedRow = configEdit.mapRow;

                        gridMap.Refresh();
                        gridMap.AutoResizeColumns();
                        gridMap.AutoResizeRows();
                    }
                }
                catch (Exception exc)
                {
                    LogError("Error in Config Editor" + exc.Message);
                    throw new Exception("Error in Config Editor", exc.InnerException);
                }
            }
        }

        private void gridMap_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(intCol_KeyPress);
            if (gridMap.CurrentCell.ColumnIndex == gridMap.Columns[percBlank].Index)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += intCol_KeyPress;
                }
            }
        }

        private void intCol_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void gridMap_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (gridMap.CurrentCell.ColumnIndex != gridMap.Columns[percBlank].Index)
            {
                gridMap.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void gridMap_Sorted(object sender, EventArgs e)
        {
            gridMap.Columns["MockType"].ReadOnly = true;
            gridMap.Columns[percBlank].ReadOnly = true;
            gridMap.Columns["Config"].ReadOnly = true;

            foreach (var dgvr in gridMap.Rows.Cast<DataGridViewRow>().Where(mr => ((MapRow)mr.DataBoundItem).Selected))
            {
                var mapRow = dgvr.DataBoundItem as MapRow;
                var cboBox = dgvr.Cells["MockType"] as DataGridViewComboBoxCell;
                cboBox.DataSource = null;
                if (mapRow.MockOptions != null) // #5 If coming from save, dont fill if no mocks
                {
                    cboBox.DataSource = mapRow.MockOptions.Mocks.Select(m => m.Name).ToList();
                    cboBox.ReadOnly = false;

                    dgvr.Cells["Config"].ReadOnly = mapRow.AdditionalProperties == string.Empty;
                    dgvr.Cells[percBlank].ReadOnly = mapRow.SelectedMock == null || mapRow.SelectedMock.Fixed;
                }
            }
        }

        private void MapRow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var mapRow = sender as MapRow;
            //var row = gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First();
            switch (e.PropertyName)
            {
                case "Selected":
                    //gridMap.Rows[row.Index].Cells["MockType"].ReadOnly = !mapRow.Selected;
                    if (mapRow.Selected)
                    {
                        selectedMaps.Add(mapRow);
                        AddOptions(mapRow);
                    }
                    else
                    {
                        selectedMaps.Remove(mapRow);
                        mapRow.MockType = string.Empty;
                        mapRow.MockOptions = null;
                        mapRow.SelectedMock = null;
                        mapRow.BlankPercentage = string.Empty;
                        ((DataGridViewComboBoxCell)gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First().Cells["MockType"]).ReadOnly = true;
                    }

                    break;

                case "MockType":
                    if (!string.IsNullOrEmpty(mapRow.MockType))
                    {
                        mapRow.SelectedMock = mapRow.MockOptions.Mocks.FirstOrDefault(m => m.Name == mapRow.MockType).Clone();
                    }

                    break;

                case "SelectedMock":
                    if (mapRow.SelectedMock == null)
                    {
                        return;
                    }

                    gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First().Cells["Config"].ReadOnly = mapRow.AdditionalProperties == string.Empty;
                    gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First().Cells[percBlank].ReadOnly = mapRow.SelectedMock.Fixed;

                    //  gridMap.Rows[row.Index].Cells["Config"].ReadOnly = mapRow.AdditionalProperties == string.Empty;
                    //gridMap.Rows[row.Index].Cells[percBlank].ReadOnly = mapRow.SelectedMock.Fixed;

                    PopulateLookup(mapRow.Attribute, mapRow.SelectedMock);
                    PopulatePickList(mapRow.Attribute, mapRow.SelectedMock);
                    SetUpNumberDefaults(mapRow.Attribute, mapRow.SelectedMock);
                    break;

                default:
                    break;
            }
            gridMap.AutoResizeColumns();
            gridMap.AutoResizeRows();
        }

        private void AddOptions(MapRow mapRow)
        {
            mapRow.MockOptions = MockOptions.First(mo => mo.AttributeTypeCode == mapRow.Attribute.AttributeType);

            var cboBox = (DataGridViewComboBoxCell)gridMap.Rows.Cast<DataGridViewRow>().Where(mr => mr.DataBoundItem == mapRow).First().Cells["MockType"];
            //var cboBox =  (DataGridViewComboBoxCell)gridMap.Rows[ row.Index].Cells["MockType"];
            cboBox.DataSource = null;
            cboBox.DataSource = mapRow.MockOptions.Mocks.Select(m => m.Name).ToList();
            cboBox.ReadOnly = false;
        }
    }


}