using LinkeD365.MockDataGen.Mock;
using LinkeD365.MockDataGen.Properties;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using xrmtb.XrmToolBox.Controls;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace LinkeD365.MockDataGen
{
    public partial class MockDataGenCtl : PluginControlBase, IGitHubPlugin
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

        public string RepositoryName => "MockarooDataMunger";

        public string UserName => "LinkeD365";

        private Image Cog => (Image)Resources.ResourceManager.GetObject("Settings_WF16");
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
            if (selectedMaps.Count == 0)
            {
                return;
            }

            if (MessageBox.Show("Do you want to save current configuration?", "Save Config?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

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
            if (!selectedMaps.Any(mr => mr.SelectedMock != null))
            {
                return;
            }

            if (string.IsNullOrEmpty(txtMockKey.Text) || txtMockKey.Text == "Mockaroo API Key")
            {
                MessageBox.Show("Please get an API key from www.mockaroo.com and enter it within the toolbar", "Need Mockaroo Key",
                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, "https://www.mockaroo.com");
                return;
            }
            var maps = selectedMaps.Where(mr => mr.SelectedMock != null && mr.SelectedMock.Mockaroo);
            if (maps.Count() == 0)
            {
                MessageBox.Show("Only fixed actions have been selected, please select more than one random or Mockaroo sourced field",
                    "Need non-fixed fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string entityName = ((EntityDisplay)cboEntities.SelectedItem).LogicalName;
            WorkAsync(new WorkAsyncInfo
            {

                Message = "Getting Mockaroo Data...",
                Work = (w, e) =>
                {
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
                            switch (map.SelectedMock)
                            {
                                case FakeEmailMock fakeEmail:
                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : propertyValues[map.Attribute.LogicalName].ToString() + ".FAKE";

                                    break;
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
                                case FixedStatus fixedStatus:
                                    var choiceNo = ((PickList)fixedStatus.FixedValue).choiceNo;
                                    newRecord[map.Attribute.LogicalName] = new OptionSetValue(choiceNo);
                                    var statusReasonMeta = Service.GetAttribute(entityName, map.Attribute.LogicalName) as StatusAttributeMetadata;
                                    var stateValue =
                                        new OptionSetValue(((StatusOptionMetadata)statusReasonMeta.OptionSet.Options.First(o => o.Value == choiceNo)).State.Value);
                                    newRecord["statecode"] = stateValue;
                                    break;
                                case FixedPickList fixedPickList:
                                    newRecord[map.Attribute.LogicalName] = new OptionSetValue(((PickList)fixedPickList.FixedValue).choiceNo);

                                    break;

                                case FixedTime fixedTime:
                                    newRecord[map.Attribute.LogicalName] = fixedTime.FixedValue;
                                    break;
                                case RandomStatus randomStatus:
                                    //  newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : new OptionSetValue(randomPickList.AllValues.First(pl => pl.Name == propertyValues[map.Attribute.LogicalName].ToString()).choiceNo);

                                    var choiceStatusNo = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : new OptionSetValue(randomStatus.AllValues.First(pl => pl.Name == propertyValues[map.Attribute.LogicalName].ToString()).choiceNo);
                                    newRecord[map.Attribute.LogicalName] = choiceStatusNo;
                                    if (choiceStatusNo != null)
                                    {
                                        var statusReasonRdmMeta =
                                            Service.GetAttribute(entityName, map.Attribute.LogicalName) as StatusAttributeMetadata;
                                        var stateRdmValue =
                                            new OptionSetValue(
                                                ((StatusOptionMetadata)statusReasonRdmMeta.OptionSet.Options.First(
                                                    o => o.Value == choiceStatusNo.Value)).State.Value);
                                        newRecord["statecode"] = stateRdmValue;
                                    }

                                    break;
                                case RandomPickList randomPickList:
                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : new OptionSetValue(randomPickList.AllValues.First(pl => pl.Name == propertyValues[map.Attribute.LogicalName].ToString()).choiceNo);
                                    break;

                                case RandomLookup randomLookup:
                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : new EntityReference(map.ParentTable, randomLookup.AllValues.First(lup => lup.Name == propertyValues[map.Attribute.LogicalName].ToString()).guid);
                                    break;

                                case StringMock stringMock:

                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : ((string)propertyValues[map.Attribute.LogicalName]).Truncate(map.AttributeLength.GetValueOrDefault());
                                    break;

                                case Date dateMock:

                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? (DateTime?)null : DateTime.ParseExact(propertyValues[map.Attribute.LogicalName].ToString(), "yyyy-MM-dd", null);
                                    break;

                                case Time timeMock:
                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? (DateTime?)null : DateTime.Today.Add(TimeSpan.Parse(propertyValues[map.Attribute.LogicalName].ToString()));

                                    break;


                                default:

                                    switch (map.SelectedMock.Name)
                                    {
                                        case (DataTypes.Boolean):
                                        case (DataTypes.BinomialDistribution):
                                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? false : (bool)propertyValues[map.Attribute.LogicalName];
                                            break;

                                        default:
                                            if (map.SelectedMock.Fixed)
                                            {
                                                newRecord[map.Attribute.LogicalName] = map.SelectedMock.FixedValue;
                                            }
                                            else
                                            {
                                                newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : propertyValues[map.Attribute.LogicalName];
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
                    if (e.Error == null)
                    {
                        gridSample.DataSource = e.Result;
                        if (tabSample.Parent != tabGrpMain)
                        {
                            tabGrpMain.TabPages.Add(tabSample);
                        }

                        tabGrpMain.SelectedTab = tabSample;
                        tabSample.Enabled = true;
                        updateEntities.Clear();
                    }
                    else
                    {
                        LogError(e.Error.ToString());
                        ShowErrorNotification(e.Error.ToString(), new Uri("https://www.linked365.blog/"));
                    }
                }
            });

            return;

            // Service.Create

            // List<MapRow> selectedRows = gridMap.Rows.Cast<DataGridViewRow>().Where(row => (MapRow) row.DataBoundItem  Select(dgvr => ((MapRow)dgvr.DataBoundItem);
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

                        if (entity.Attributes.Contains("statecode") && ((OptionSetValue)entity["statecode"]).Value >= 1) // inactive
                        {
                            CreateInactiveRequest(entity);
                        }
                        else
                        {
                            var createRequest = new CreateRequest { Target = entity };
                            requestWithResults.Requests.Add(createRequest);
                        }
                    }
                    string errors = string.Empty;
                    if (requestWithResults.Requests.Count > 0)
                    {
                        var responseWithResults =
                            (ExecuteMultipleResponse)Service.Execute(requestWithResults);

                        ai.WriteEvent("Data Mocked Count", requestWithResults.Requests.Count);
                        foreach (var responseItem in responseWithResults.Responses)
                        {
                            // DisplayResponse(requestWithResults.Requests[responseItem.RequestIndex], responseItem.Response);

                            // An error has occurred.
                            if (responseItem.Fault != null)
                            {
                                errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault;
                            }
                            // else collection.Entities[responseItem.RequestIndex].Id = ((CreateResponse) responseItem.Response).id;
                            //DisplayFault(requestWithResults.Requests[responseItem.RequestIndex],
                            //    responseItem.RequestIndex, responseItem.Fault);
                        }


                    }

                    foreach (var updateEntity in updateEntities)
                    {
                        errors += SendInactiveRequest(updateEntity, w);
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
                        if (tabSample.Parent != tabGrpHidden)
                        {
                            tabGrpHidden.TabPages.Add(tabSample);
                        }

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
            if (cboSelectSaved.SelectedItem == null)
            {
                return;
            }

            var selectedMap = mySettings.Settings.First(stng => stng.Name == cboSelectSaved.SelectedItem.ToString());
            if (selectedMap is null)
            {
                return;
            }
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

        private void gridMap_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex > -1)
            {

                e.PaintContent(e.CellBounds);

                e.Graphics.DrawImage(Cog,
                    new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - Cog.Width) / 2, e.CellBounds.Top + (e.CellBounds.Height - Cog.Height) / 2,
                        Cog.Width, Cog.Height));
                e.Handled = true;
            }
        }
    }
}