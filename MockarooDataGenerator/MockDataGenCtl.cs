using LinkeD365.MockDataGen.Mock;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using XrmToolBox.Extensibility;

namespace LinkeD365.MockDataGen
{
    public partial class MockDataGenCtl : PluginControlBase
    {
        #region Private Fields

        private const string percBlank = "Percentage Blank";
        private AllSettings mySettings;
        private List<MapRow> selectedMaps = new List<MapRow>();
        private EntityCollection collection;

        internal AttributeTypeCode[] notPermitted = new[] {AttributeTypeCode.Virtual
            ,AttributeTypeCode.Uniqueidentifier , AttributeTypeCode.PartyList
            , AttributeTypeCode.EntityName , AttributeTypeCode.CalendarRules,
            AttributeTypeCode.State,AttributeTypeCode.ManagedProperty};

        private AppInsights ai;
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";

        private const string aiKey = "cc383234-dfdb-429a-a970-d17847361df3";

        #endregion Private Fields

        #region Public Constructor stuff

        public MockDataGenCtl()
        {
            InitializeComponent();

            ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly());
            ai.WriteEvent("Control Loaded");
        }

        private void MockDataGen_Load(object sender, EventArgs e)
        {
            // LoadEntities();
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new AllSettings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
                txtMockKey.Text = mySettings.MockKey;
                AddSavedMaps();
            }
        }

        /// <summary>
        /// Closing tool, check save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MockDataGen_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            if (selectedMaps.Count == 0) return;
            if (MessageBox.Show("Do you want to save current configuration?", "Save Config?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
            ShowSaveMap();
            //SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
            LoadEntities();
            cboEntities.SelectedItem = null;
        }

        #endregion Public Constructor stuff

        #region FormEvents

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void gridMap_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
            {
                return;
            }
        }

        private void gridMap_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void btnMockData_Click(object sender, EventArgs args)
        {
            if (!selectedMaps.Any(mr => mr.SelectedMock != null)) return;

            if (string.IsNullOrEmpty(txtMockKey.Text) || txtMockKey.Text == "Mockaroo API Key")
            {
                MessageBox.Show("Please get an API key from www.mockaroo.com and enter it within the toolbar", "Need Mockaroo Key",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "https://www.mockaroo.com");
                return;
            }
            string entityName = ((EntityDisplay)cboEntities.SelectedItem).LogicalName;
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting Mockaroo Data...",
                Work = (w, e) =>
                {
                    var maps = selectedMaps.Where(mr => mr.SelectedMock != null && mr.SelectedMock.Mockaroo);
                    dynamic mockClass = new ExpandoObject();

                    foreach (var map in maps)
                    {
                        ((IDictionary<string, object>)mockClass)[map.Attribute.LogicalName] = map.SelectedMock;
                    }

                    var client = new MockClient(txtMockKey.Text);
                    var returnData = client.GetData(mockClass, (int)numRecordCount.Value);

                    w.ReportProgress(50, "Got Data, Loading Grid");

                    collection = new EntityCollection() { EntityName = entityName };

                    foreach (var data in returnData)
                    {
                        Entity newRecord = new Entity(entityName);
                        IDictionary<string, object> propertyValues = data;

                        foreach (var map in selectedMaps)
                        {
                            var mock = map.SelectedMock;
                            // if (!propertyValues.ContainsKey(map.Attribute.LogicalName)) continue;
                            switch (map.SelectedMock)
                            {
                                case FixedDateTime fixedDateTime:
                                    newRecord[map.Attribute.LogicalName] = fixedDateTime.FixedValue;
                                    break;

                                case FixedDate fixedDate:
                                    newRecord[map.Attribute.LogicalName] = fixedDate.FixedValue;
                                    break;

                                case FixedLookup fixedLookup:
                                    newRecord[map.Attribute.LogicalName] = new EntityReference(map.ParentTable, ((Lookup)fixedLookup.FixedValue).guid);
                                    break;

                                case FixedNumber fixedNumber:
                                    newRecord[map.Attribute.LogicalName] = fixedNumber.FixedValue;
                                    break;

                                case FixedPickList fixedPickList:
                                    newRecord[map.Attribute.LogicalName] = new OptionSetValue(((PickList)fixedPickList.FixedValue).choiceNo);
                                    break;

                                case FixedTime fixedTime:
                                    newRecord[map.Attribute.LogicalName] = fixedTime.FixedValue;
                                    break;

                                case RandomPickList randomPickList:
                                    newRecord[map.Attribute.LogicalName] = new OptionSetValue(randomPickList.AllValues.First(pl => pl.Name == propertyValues[map.Attribute.LogicalName].ToString()).choiceNo);
                                    break;

                                case RandomLookup randomLookup:
                                    newRecord[map.Attribute.LogicalName] = new EntityReference(map.ParentTable, randomLookup.AllValues.First(lup => lup.Name == propertyValues[map.Attribute.LogicalName].ToString()).guid);
                                    break;

                                case StringMock stringMock:

                                    newRecord[map.Attribute.LogicalName] = ((string)propertyValues[map.Attribute.LogicalName]).Truncate(map.AttributeLength.GetValueOrDefault());
                                    break;

                                default:

                                    switch (map.SelectedMock.Name)
                                    {
                                        case (DataTypes.Boolean):
                                        case (DataTypes.BinomialDistribution):
                                            newRecord[map.Attribute.LogicalName] = (bool)propertyValues[map.Attribute.LogicalName];
                                            break;

                                        default:
                                            if (map.SelectedMock.Fixed) newRecord[map.Attribute.LogicalName] = map.SelectedMock.FixedValue;
                                            else
                                            {
                                                newRecord[map.Attribute.LogicalName] = propertyValues[map.Attribute.LogicalName];
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }

                        collection.Entities.Add(newRecord);
                    }

                    e.Result = collection.Entities;
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    gridSample.DataSource = e.Result;
                    if (tabSample.Parent != tabGrpMain) tabGrpMain.TabPages.Add(tabSample);
                    tabGrpMain.SelectedTab = tabSample;
                    tabSample.Enabled = true;
                }
            });

            return;

            // Service.Create

            // List<MapRow> selectedRows = gridMap.Rows.Cast<DataGridViewRow>().Where(row => (MapRow) row.DataBoundItem  Select(dgvr => ((MapRow)dgvr.DataBoundItem);
        }

        private void entitiesDD_SelectedItemChanged(object sender, EventArgs e)
        {
            //BuildGrid();
        }

        /// <summary>
        /// save current map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            ShowSaveMap();

            AddSavedMaps();
        }

        private void btnCreateData_Click(object sender, EventArgs args)
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Creating Data...",
                Work = (w, e) =>
                {
                    var requestWithResults = new ExecuteMultipleRequest()
                    {
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = false,
                            ReturnResponses = true
                        },
                        Requests = new OrganizationRequestCollection()
                    };
                    foreach (var entity in collection.Entities)
                    {
                        var createRequest = new CreateRequest { Target = entity };
                        requestWithResults.Requests.Add(createRequest);
                    }

                    var responseWithResults =
                               (ExecuteMultipleResponse)Service.Execute(requestWithResults);
                    string errors = string.Empty;
                    ai.WriteEvent("Data Mocked Count", collection.Entities.Count);
                    foreach (var responseItem in responseWithResults.Responses)
                    {
                        // DisplayResponse(requestWithResults.Requests[responseItem.RequestIndex], responseItem.Response);

                        // An error has occurred.
                        if (responseItem.Fault != null) errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault.ToString();
                        else collection.Entities.RemoveAt(responseItem.RequestIndex);
                        //DisplayFault(requestWithResults.Requests[responseItem.RequestIndex],
                        //    responseItem.RequestIndex, responseItem.Fault);
                    }
                    e.Result = errors;
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    string errs = e.Result as string;
                    if (errs == string.Empty)
                    {
                        MessageBox.Show("All data was created successfully. Check your environment to ensure data quality", "No Errors", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        gridSample.DataSource = null;
                        if (tabSample.Parent != tabGrpHidden) tabGrpHidden.TabPages.Add(tabSample);
                        tabGrpMain.SelectedTab = tabConfig;
                    }
                    else
                    {
                        MessageBox.Show("The following shows the rows that caused errors" + errs, "Rows created with errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridSample.DataSource = null;
                        gridSample.DataSource = collection.Entities;
                    }
                }
            });
        }

        private void cboSelectSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectSaved.SelectedItem == null) return;
            var selectedMap = mySettings.Settings.First(stng => stng.Name == cboSelectSaved.SelectedItem.ToString());
            if (selectedMap is null) return;
            // var entDisplay = new EntityDisplay { LogicalName = selectedMap.EntityName };
            if (cboEntities.Items.Contains(selectedMap.EntityDisplay))
            {
                cboEntities.SelectedItem = null;
                cboEntities.SelectedItem = selectedMap.EntityDisplay;
            }
            //if (entitiesDD.AllEntities.Any(ent => ent.LogicalName == selectedMap.EntityName))
            //{
            //    entitiesDD.com
            //}
        }

        private void cboEntities_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedMaps = new List<MapRow>();
            if (cboEntities.SelectedItem == null)
            {
                gridMap.DataSource = null;
                return;
            }

            BuildGrid(((EntityDisplay)cboEntities.SelectedItem).LogicalName);
            cboSelectSaved.SelectedItem = null;
        }

        #endregion FormEvents
    }
}