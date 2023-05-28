using LinkeD365.MockDataGen.Properties;
using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Deployment;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Args;
using XrmToolBox.Extensibility.Interfaces;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace LinkeD365.MockDataGen
{
    public partial class MockDataGenCtl : PluginControlBase, IGitHubPlugin, IPayPalPlugin
    {
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

        public string DonationDescription => "Mockaroo Munger Fans";

        public string EmailAccount => "carl.cookson@gmail.com";

        // private string entityName;

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
            // LoadEntities(); Loads or creates the settings
            // for the plugin
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
            btnDepTables.Checked = mySettings.ExcludeConfig.DeprecatedTables;
            btnDepImpSeqNo.Checked = mySettings.ExcludeConfig.ImportSeqNo;
            btnDepCol.Checked = mySettings.ExcludeConfig.DeprecatedColumns;
            btnBypassPluginExec.Checked = mySettings.ExcludeConfig.BypassPluginExecution;

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
        /// This event occurs when the connection has been
        /// updated in XrmToolBox
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
            var maps = selectedMaps.Where(mr => mr.SelectedMock != null && mr.SelectedMock.Mockaroo).ToList<SimpleRow>();
            if (maps.Count() == 0)
            {
                MessageBox.Show(
                    "Only fixed actions have been selected, please select more than one random or Mockaroo sourced field",
                    "Need non-fixed fields",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            var entityName = ((EntityDisplay)cboEntities.SelectedItem).LogicalName;

            WorkAsync(
            new WorkAsyncInfo
            {
                Message = "Creating Data...",
                Work = (w, e) =>
                {
                    var response = LoadMockData((int)numRecordCount.Value, maps, w);
                    var entityCollection = new EntityCollection()
                    {
                        EntityName = entityName
                    };

                    foreach (List<ExpandoObject> subList in SplitList(response))
                        entityCollection.Entities.AddRange(CreateEntityList(subList, entityCollection.EntityName, maps));

                    e.Result = entityCollection;
                },
                ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        LogError(e.Error.ToString());
                        MessageBox.Show(e.Error.Message.ToString(), "Error generating data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        collection.Entities.Clear();
                    }
                    else
                    {
                        collection = e.Result as EntityCollection;
                        gridSample.DataSource = collection.Entities.Take(100);
                        ShowResults();

                        if (collection.Entities.Count > 100)
                        {
                            btnCreateAllData.Visible = true;
                            btnCreate100Data.Visible = true;

                            btnCreateAllData.Text = "Create " + numRecordCount.Value + " records";
                        }
                        else
                        {
                            btnCreate100Data.Visible = false;
                        }
                    }
                }
            });
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

        private void CreateData(int? countLimit = null)
        {
            var recordCount = countLimit ?? collection.Entities.Count;
            var batchCount = (int)Math.Ceiling(recordCount / batchSize.Value);
            var dataToImport = collection.Entities.Take(recordCount).ToList();
            if (countLimit.HasValue && countLimit < collection.Entities.Count)
            {
                // Take out the ones we have done, keep the rest for later
                var remainingEntities = collection.Entities.Skip(countLimit.Value).ToList();
                collection.Entities.Clear();
                collection.Entities.AddRange(remainingEntities);
            }
            
            WorkAsync(
            new WorkAsyncInfo
            {
                Message = "Creating Data...",
                Work = (w, e) =>
                {
                    for (var i = 0; i < batchCount; i++)
                    {
                        var batchedCollection = new EntityCollection
                        {
                            EntityName = collection.EntityName,
                            TotalRecordCount = collection.TotalRecordCount
                        };
                        batchedCollection.Entities.AddRange(dataToImport.Skip(i * (int)batchSize.Value).Take((int)batchSize.Value).ToList());
                        w.ReportProgress(-1, $"Running batch {i + 1} of {batchCount}");

                        var requestWithResults = new ExecuteMultipleRequest
                        {
                            Settings =
                                new ExecuteMultipleSettings
                                {
                                    ContinueOnError = false,
                                    ReturnResponses = true,
                                },
                            Requests = new OrganizationRequestCollection()
                        };

                        foreach (var entity in batchedCollection.Entities)
                        {
                            if (entity.Attributes.Contains("statecode") &&
                                ((OptionSetValue)entity["statecode"]).Value >= 1) // inactive
                                CreateInactiveRequest(entity);
                            else
                            {
                                var createRequest = new CreateRequest { Target = entity };
                                if (btnBypassPluginExec.Checked)
                                    createRequest.Parameters.Add("BypassCustomPluginExecution", true);

                                requestWithResults.Requests.Add(createRequest);
                            }
                        }

                        var errors = string.Empty;
                        if (requestWithResults.Requests.Count > 0)
                        {
                            var responseWithResults =
                            (ExecuteMultipleResponse)Service.Execute(requestWithResults);

                            ai.WriteEvent("Data Mocked Count", requestWithResults.Requests.Count);
                            foreach (var responseItem in responseWithResults.Responses)

                                // An error has occurred.
                                if (responseItem.Fault != null)
                                    errors += "\r\n" +
                                        responseItem.RequestIndex +
                                        " | " +
                                        responseItem.Fault;
                        }

                        foreach (var updateEntity in updateEntities)
                            errors += SendInactiveRequest(updateEntity, w, null, string.Empty);

                        e.Result = errors;
                    }
                },
                ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                PostWorkCallBack = e =>
                {
                    if (e.Error != null)
                    {
                        LogError(e.Error.ToString());
                        MessageBox.Show(e.Error.Message.ToString(), "Error generating data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        var errs = e.Result as string;
                        if (errs == String.Empty)
                        {
                            MessageBox.Show("All data was created successfully. Check your environment to ensure data quality", "No Errors", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (countLimit.HasValue && countLimit < collection.Entities.Count)
                            {
                                gridSample.DataSource = collection.Entities;
                                btnCreateAllData.Text = "Create remaining " + collection.Entities.Count + " records";
                            }
                            else
                            {
                                gridSample.DataSource = null;
                                HideResults();
                                tabGrpMain.SelectedTab = tabConfig;
                            }
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
                }
            });
        }

        private void btnCreate100Data_Click(object sender, EventArgs args)
        {
            CreateData(100);
        }

        private void btnCreateAllData_Click(object sender, EventArgs args)
        {
            CreateData(null);
        }

        private void HideResults()
        {
            if (tabSample.Parent != tabGrpHidden)
                tabGrpHidden.TabPages.Add(tabSample);
        }

        private void cboSelectSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSelectSaved.SelectedItem == null)
                return;

            var selectedMap = mySettings.Settings
                .First(stng => stng.Name == cboSelectSaved.SelectedItem.ToString());
            if (selectedMap is null)
                return;

            // var entDisplay
            // = new EntityDisplay { LogicalName =
            // selectedMap.EntityName };
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

        private void BtnCreateDataSet_Click(object sender, EventArgs e)
        {
            if (mySettings.Settings.Count == 0)
                MessageBox.Show(
                    "Please create Mockaroo Maps prior to creating a dataset",
                    "Create Maps first",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            DataSetConfig dsConfig = new DataSetConfig(mySettings);
            if (dsConfig.ShowDialog() != DialogResult.OK)
                return;

            mySettings = dsConfig.Settings;

            SettingsManager.Instance.Save(typeof(AllSettings), mySettings);
            AddSavedMaps();
        }

        private void btnPlaySet_Click(object sender, EventArgs e)
        {
            PlaySet();
        }

        private void btnDepTables_CheckStateChanged(object sender, EventArgs e)
        {
            mySettings.ExcludeConfig.DeprecatedTables = btnDepTables.Checked;
        }

        private void btnDepCol_CheckStateChanged(object sender, EventArgs e)
        {
            mySettings.ExcludeConfig.DeprecatedColumns = btnDepCol.Checked;
        }

        private void btnDepImpSeqNo_CheckStateChanged(object sender, EventArgs e)
        {
            mySettings.ExcludeConfig.ImportSeqNo = btnDepImpSeqNo.Checked;
        }

        private void btnBypassPluginExec_CheckStateChanged(object sender, EventArgs e)
        {
            mySettings.ExcludeConfig.BypassPluginExecution = btnBypassPluginExec.Checked;
        }

        private void btnExportMaps_Click(object sender, EventArgs e)
        {
            Export export = new Export();
            export.grpSelector.Text = "Select Map";
            export.Text = "Select Map";
            export.chkListSelect.Items.AddRange(mySettings.Settings.Select(st => st.Name).ToArray());

            if (export.ShowDialog() != DialogResult.OK) return;

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "XML Files | *.xml";
            fileDialog.FileName = export.chkListSelect.CheckedItems.Count == 1 ? export.chkListSelect.CheckedItems[0].ToString() + ".xml" : "My Maps.xml";
            if (fileDialog.ShowDialog() != DialogResult.OK) return;

            ExportMaps exportMaps = new ExportMaps(mySettings.Settings.Where(st => export.chkListSelect.CheckedItems.OfType<string>().Contains(st.Name)).ToList());
            XmlSerializer writer = new XmlSerializer(typeof(ExportMaps));

            FileStream file = File.Create(fileDialog.FileName);

            writer.Serialize(file, exportMaps);
            file.Close();

            MessageBox.Show("Maps Exported", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportSets_Click(object sender, EventArgs e)
        {
            Export export = new Export();
            export.grpSelector.Text = "Select Sets";
            export.Text = "Select Sets";
            export.chkListSelect.Items.AddRange(mySettings.Sets.Select(st => st.SetName).ToArray());

            if (export.ShowDialog() != DialogResult.OK) return;
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "XML Files | *.xml";

            fileDialog.FileName = export.chkListSelect.CheckedItems.Count == 1 ? export.chkListSelect.CheckedItems[0].ToString() + ".xml" : "My Sets.xml";
            if (fileDialog.ShowDialog() != DialogResult.OK) return;

            ExportMaps exportMaps = new ExportMaps();
            exportMaps.Sets = mySettings.Sets.Where(set => export.chkListSelect.CheckedItems.OfType<string>().Contains(set.SetName)).ToList();
            exportMaps.Maps = mySettings.Settings.Where(mp => exportMaps.Sets.SelectMany(st => st.SetItems.Select(set => set.MapName)).Contains(mp.Name)).ToList();
            // exportMaps.Maps
            // = mySettings.Settings.Where(st
            // => mySettings.Sets.Select(set
            // => set.SetItems.Select(si=>si.MapName)).Contains(st.Name)).ToList();
            XmlSerializer writer = new XmlSerializer(typeof(ExportMaps));

            FileStream file = File.Create(fileDialog.FileName);

            writer.Serialize(file, exportMaps);
            file.Close();

            MessageBox.Show("Sets Exported", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "XML Files | *.xml";
            fileDialog.Title = "Select exported settings file";

            if (fileDialog.ShowDialog() != DialogResult.OK) return;

            XmlSerializer reader =
                new XmlSerializer(typeof(ExportMaps));
            try
            {
                StreamReader file = new StreamReader(fileDialog.FileName);

                var importMap = (ExportMaps)reader.Deserialize(file);
                bool changedMaps = false;
                bool changedSets = false;
                foreach (Setting map in importMap.Maps)
                {
                    if (mySettings.Settings.Contains(map))
                    {
                        foreach (var si in importMap.Sets.SelectMany(set => set.SetItems.Where(setItem => setItem.MapName == map.Name)))
                        {
                            si.MapName = map.Name + "_Import";
                        }
                        map.Name = map.Name + "_Import";
                        changedMaps = true;
                    }
                    mySettings.Settings.Add(map);
                }

                foreach (Set set in importMap.Sets)
                {
                    if (mySettings.Sets.Contains(set))
                    {
                        set.SetName = set.SetName + "_Import";
                        changedSets = true;
                    }
                    mySettings.Sets.Add(set);
                }

                SettingsManager.Instance.Save(typeof(AllSettings), mySettings);
                AddSavedMaps();
                MessageBox.Show($@"Your configuration has been updated with the sets/maps in the import file
{(changedSets || changedMaps ? "One or more maps or sets have been updated with an _import prefix." : "")}
Please confirm your configuration before using", "Import Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error in importing your configuration, please confirm file is correct and try again", "Error on Import", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}