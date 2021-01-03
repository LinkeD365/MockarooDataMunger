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

        private void gridMap_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 6 && e.RowIndex > -1)
            {
                e.PaintContent(e.CellBounds);

                e.Graphics
                    .DrawImage(
                        Cog,
                        new Rectangle(
                            e.CellBounds.Left + (e.CellBounds.Width - Cog.Width) / 2,
                            e.CellBounds.Top + (e.CellBounds.Height - Cog.Height) / 2,
                            Cog.Width,
                            Cog.Height));
                e.Handled = true;
            }
        }

        #region Private Fields
        private const string percBlank = "Percentage Blank";
        private AllSettings mySettings;
        private List<MapRow> selectedMaps = new List<MapRow>();
        private EntityCollection collection;

        internal AttributeTypeCode[] notPermitted = new[]
        {
            AttributeTypeCode.Virtual,
            AttributeTypeCode.Uniqueidentifier,
            AttributeTypeCode.PartyList,
            AttributeTypeCode.EntityName,
            AttributeTypeCode.CalendarRules,
            AttributeTypeCode.State,
            AttributeTypeCode.ManagedProperty
        };

        private AppInsights ai;
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";

        private const string aiKey = "cc383234-dfdb-429a-a970-d17847361df3";


        public string RepositoryName => "MockarooDataMunger";

        public string UserName => "LinkeD365";

        private Image Cog => (Image)Resources.ResourceManager.GetObject("Settings_WF16");

        private List<MapRow> maps;
        private dynamic mockClass;
        private string entityName;
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
                return;

            if (MessageBox.Show(
                    "Do you want to save current configuration?",
                    "Save Config?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) ==
                DialogResult.No)
                return;

            ShowSaveMap();

            //SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(
            IOrganizationService newService,
            ConnectionDetail detail,
            string actionName,
            object parameter)
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
        private void tsbClose_Click(object sender, EventArgs e) { CloseTool(); }

        private void gridMap_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected)
                return;
        }

        private void gridMap_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void btnMockData_Click(object sender, EventArgs args)
        {
            if (!selectedMaps.Any(mr => mr.SelectedMock != null))
                return;

            if (string.IsNullOrEmpty(txtMockKey.Text) || txtMockKey.Text == "Mockaroo API Key")
            {
                MessageBox.Show(
                    "Please get an API key from www.mockaroo.com and enter it within the toolbar",
                    "Need Mockaroo Key",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    0,
                    "https://www.mockaroo.com");
                return;
            }
            maps = selectedMaps.Where(mr => mr.SelectedMock != null && mr.SelectedMock.Mockaroo).ToList();
            if (maps.Count() == 0)
            {
                MessageBox.Show(
                    "Only fixed actions have been selected, please select more than one random or Mockaroo sourced field",
                    "Need non-fixed fields",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            mockClass = new ExpandoObject();
            foreach (var map in maps)
                ((IDictionary<string, object>)mockClass)[map.Attribute.LogicalName] = map.SelectedMock;
            entityName = ((EntityDisplay)cboEntities.SelectedItem).LogicalName;

            // #11 Added ability to generate more than 1000 records, firstly limit to 100 if more than 1000
            int recordCount = numRecordCount.Value <= 1000 ? (int)numRecordCount.Value : 100;
            collection = new EntityCollection { EntityName = entityName };

            GetInitMockData(recordCount);

            //if (collection.Entities.Count > 0)
            //{
            //    gridSample.DataSource = collection.Entities;
            //    if (tabSample.Parent != tabGrpMain)
            //        tabGrpMain.TabPages.Add(tabSample);

            //    tabGrpMain.SelectedTab = tabSample;
            //    tabSample.Enabled = true;
            //    updateEntities.Clear();

            //    if (recordCount != numRecordCount.Value)
            //    {
            //        btnCreateBatch.Visible = true;
            //        btnCreateBatch.Text = "Create " + numRecordCount.Value + " records";
            //        btnCreateData.Text = "Create " + recordCount + " records";
            //    }
            //    else
            //    {
            //        btnCreateBatch.Visible = false;
            //        btnCreateData.Text = "Create Records";
            //    }
            //}

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
            WorkAsync(
                new WorkAsyncInfo
                {
                    Message = "Creating Data...",
                    Work =
                        (w, e) =>
                        {
                            var requestWithResults = new ExecuteMultipleRequest
                            {
                                Settings =
                                    new ExecuteMultipleSettings
                                    {
                                        ContinueOnError = false,
                                        ReturnResponses = true
                                    },
                                Requests = new OrganizationRequestCollection()
                            };
                            foreach (var entity in collection.Entities)

                                if (entity.Attributes.Contains("statecode") &&
                                    ((OptionSetValue)entity["statecode"]).Value >= 1) // inactive
                                    CreateInactiveRequest(entity);
                                else
                                {
                                    var createRequest = new CreateRequest { Target = entity };
                                    requestWithResults.Requests.Add(createRequest);
                                }
                            string errors = string.Empty;
                            if (requestWithResults.Requests.Count > 0)
                            {
                                var responseWithResults =
                            (ExecuteMultipleResponse)Service.Execute(requestWithResults);

                                ai.WriteEvent("Data Mocked Count", requestWithResults.Requests.Count);
                                foreach (var responseItem in responseWithResults.Responses)

                                    // DisplayResponse(requestWithResults.Requests[responseItem.RequestIndex], responseItem.Response);

                                    // An error has occurred.
                                    if (responseItem.Fault != null)
                                        errors += "\r\n" +
                                            responseItem.RequestIndex +
                                            " | " +
                                            responseItem.Fault;

                                // else collection.Entities[responseItem.RequestIndex].Id = ((CreateResponse) responseItem.Response).id;
                                //DisplayFault(requestWithResults.Requests[responseItem.RequestIndex],
                                //    responseItem.RequestIndex, responseItem.Fault);
                            }

                            foreach (var updateEntity in updateEntities)
                                errors += SendInactiveRequest(updateEntity, w);
                            e.Result = errors;
                        },
                    ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                    PostWorkCallBack =
                        e =>
                        {
                            string errs = e.Result as string;
                            if (errs == string.Empty)
                            {
                                MessageBox.Show(
                                    "All data was created successfully. Check your environment to ensure data quality",
                                    "No Errors",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                gridSample.DataSource = null;
                                if (tabSample.Parent != tabGrpHidden)
                                    tabGrpHidden.TabPages.Add(tabSample);

                                tabGrpMain.SelectedTab = tabConfig;
                            }
                            else
                            {
                                MessageBox.Show(
                                    "The following shows the rows that caused errors" + errs,
                                    "Rows created with errors",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                gridSample.DataSource = null;
                                gridSample.DataSource = collection.Entities;
                            }
                        }
                });
        }

        private void cboSelectSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectSaved.SelectedItem == null)
                return;

            var selectedMap = mySettings.Settings
                .First(stng => stng.Name == cboSelectSaved.SelectedItem.ToString());
            if (selectedMap is null)
                return;

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

        private void BtnCreateBatch_Click(object sender, EventArgs e)
        {
            CreateAllData((int)numRecordCount.Value);
        }
        #endregion FormEvents

    }
}