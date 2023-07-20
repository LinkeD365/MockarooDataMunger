using LinkeD365.MockDataGen.Mock;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace LinkeD365.MockDataGen
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

    public class EntityDisplay
    {
        public string LogicalName { get; set; }
        public string DisplayName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EntityDisplay display &&
                   LogicalName == display.LogicalName;
        }

        public override int GetHashCode()
        {
            return 1374632327 + EqualityComparer<string>.Default.GetHashCode(LogicalName);
        }

        public override string ToString()
        {
            return DisplayName + " ( " + LogicalName + " )";
        }
    }

    public partial class MockDataGenCtl : PluginControlBase
    {
        private void AddSavedMaps()
        {
            cboSelectSaved.Items.Clear();
            cboSelectSaved.Items.AddRange(mySettings.Settings.Select(set => set.Name).ToArray());
            cboRunDataSet.Items.Clear();
            cboRunDataSet.Items.AddRange(mySettings.Sets?.Select(set => set.SetName).ToArray());
        }

        private void LoadEntities()
        {
            if (Service == null)
                return;
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities",
                Work = (w, e) =>
                {
                    MetadataFilterExpression EntityFilter = new MetadataFilterExpression(LogicalOperator.And);

                    EntityFilter.Conditions.Add(new MetadataConditionExpression("IsPrivate", MetadataConditionOperator.NotEquals, true));
                    var eqe = new EntityQueryExpression();
                    eqe.Properties = new MetadataPropertiesExpression("LogicalName", "DisplayName");
                    eqe.Criteria = EntityFilter;
                    // query.Criteria.AddCondition("donotemail",
                    // ConditionOperator.Equal, query_donotemail);

                    var req = new RetrieveMetadataChangesRequest
                    {
                        Query = eqe,
                        ClientVersionStamp = null
                    };
                    e.Result = Service.Execute(req) as RetrieveMetadataChangesResponse;
                },
                PostWorkCallBack = e =>
                {
                    cboEntities.Items.Clear();
                    var metaresponse = ((RetrieveMetadataChangesResponse)e.Result).EntityMetadata;

                    cboEntities.Items.AddRange(metaresponse.Where(ent => mySettings.ExcludeConfig.DeprecatedTables ? (!ent.DisplayName.LocalizedLabels.Any()
                                                                                                                        || !ent.DisplayName.LocalizedLabels.Any(lbl => lbl.Label.ToLower().Contains("deprecated"))) : true)
                                                                .Select(ent => new EntityDisplay { LogicalName = ent.LogicalName, DisplayName = ent.DisplayName.UserLocalizedLabel == null ? ent.LogicalName : ent.DisplayName.UserLocalizedLabel.Label }).OrderBy(ent => ent.DisplayName).ToArray());
                }
            });
        }

        private void ShowSaveMap()
        {
            var saveMap = new SaveMap(mySettings);
            if (saveMap.ShowDialog() != DialogResult.OK)
                return;

            mySettings.MockKey = txtMockKey.Text;
            var setting = new Setting();
            setting.Name = saveMap.settingName;
            setting.EntityDisplay = cboEntities.SelectedItem as EntityDisplay;
            setting.MapRows = selectedMaps.Select(mr => new SimpleMapRow
            {
                AttributeName = mr.AttributeName,
                BlackPercentage = mr.BlankPercentage,
                AttributeTypeCode = mr.AttributeTypeCode,
                LogicalName = mr.LogicalName,

                SelectedMock = mr.SelectedMock == null ? null :
                 mr.SelectedMock is FromSet ? mr.SelectedMock.GetField().Where(fs => fs.Key != "values").Select(dic => new KVP
                 {
                     Key = dic.Key,
                     Value = dic.Value is string[]? string.Join("||", (string[])dic.Value) : dic.Value
                 }).ToList()
                 : mr.SelectedMock.GetField().Select(dic => new KVP
                 {
                     Key = dic.Key,
                     Value = dic.Value is string[]? string.Join("||", (string[])dic.Value) : dic.Value
                 }).ToList()
            }).ToList();

            if (mySettings.Settings.Any(mr => mr.Name == setting.Name))
                mySettings.Settings[mySettings.Settings.IndexOf(setting)] = setting;
            else
                mySettings.Settings.Add(setting);

            SettingsManager.Instance.Save(typeof(AllSettings), mySettings);
        }

        private void SetUpNumberDefaults(AttributeMetadata attribute, BaseMock selectedMock)
        {
            if (selectedMock is BinomialDistribution || selectedMock is FixedNumber)
                return;

            switch (attribute)
            {
                case DecimalAttributeMetadata decAttr:
                    if (selectedMock is NormalDistribution normalDistribution1)
                    {
                        normalDistribution1.Decimals = decAttr.Precision.GetValueOrDefault();
                        return;
                    }
                    var decMock = (Number)selectedMock;
                    decMock.Decimals = decAttr.Precision.GetValueOrDefault();
                    decMock.Max = decAttr.MaxValue.GetValueOrDefault();
                    decMock.Min = decAttr.MinValue.GetValueOrDefault();
                    break;

                case DoubleAttributeMetadata doubleAttr:
                    if (selectedMock is NormalDistribution distribution1)
                    {
                        distribution1.Decimals = doubleAttr.Precision.GetValueOrDefault();
                        return;
                    }
                    var dblMock = (Number)selectedMock; //Fixed numbers
                    dblMock.Decimals = doubleAttr.Precision.GetValueOrDefault();
                    dblMock.Max = (decimal)doubleAttr.MaxValue.GetValueOrDefault();
                    dblMock.Min = (decimal)doubleAttr.MinValue.GetValueOrDefault();
                    break;

                case IntegerAttributeMetadata intAttr:
                    if (selectedMock is NormalDistribution mock)
                    {
                        mock.Decimals = 0;
                        return;
                    }
                    var intMock = (Number)selectedMock;
                    intMock.Decimals = 0;
                    intMock.Max = intAttr.MaxValue.GetValueOrDefault();
                    intMock.Min = intAttr.MinValue.GetValueOrDefault();
                    break;

                case MoneyAttributeMetadata moneyAttr:
                    if (selectedMock is NormalDistribution normalDistribution)
                    {
                        normalDistribution.Decimals = moneyAttr.Precision.GetValueOrDefault();
                        return;
                    }
                    var moneyMock = (Number)selectedMock;
                    moneyMock.Decimals = moneyAttr.Precision.GetValueOrDefault();
                    moneyMock.Max = (decimal)moneyAttr.MaxValue.GetValueOrDefault();
                    moneyMock.Min = (decimal)moneyAttr.MinValue.GetValueOrDefault();
                    break;

                case BigIntAttributeMetadata bigIntAttr:
                    if (selectedMock is NormalDistribution distribution)
                    {
                        distribution.Decimals = 0;
                        return;
                    }
                    var bigIntMock = (Number)selectedMock;
                    bigIntMock.Decimals = 0;
                    bigIntMock.Max = bigIntAttr.MaxValue.GetValueOrDefault();
                    bigIntMock.Min = bigIntAttr.MinValue.GetValueOrDefault();
                    break;

                default:
                    break;
            }
        }

        private void Populate(AttributeMetadata attribute, BaseMock selectedMock, bool fromLoad = false)
        {
            PopulateLookup(attribute, selectedMock, fromLoad);
            PopulatePickList(attribute, selectedMock, fromLoad);
            PopulateSet(attribute, selectedMock);
            SetUpNumberDefaults(attribute, selectedMock);
        }

        private void PopulateSet(AttributeMetadata attribute, BaseMock selectedMock)
        {
            if (!(selectedMock is FromSet)) return;
            if (string.IsNullOrEmpty(selectedMock.EntityName)) selectedMock.EntityName = ((LookupAttributeMetadata)attribute).Targets[0];
            // ((FromSet)selectedMock).Values = GetLinkedRecords(selectedMock);
        }

        private void PopulateLookup(AttributeMetadata attribute, BaseMock selectedMock, bool fromLoad)
        {
            if (!(selectedMock is RandomLookup || selectedMock is FixedLookup)) return;
            if (string.IsNullOrEmpty(selectedMock.EntityName)) selectedMock.EntityName = ((LookupAttributeMetadata)attribute).Targets[0];

            //var lookupAttr = attribute as LookupAttributeMetadata;
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting linked records",
                Work = (w, e) =>
                {
                    w.ReportProgress(10, "Getting " + attribute.EntityLogicalName);

                    List<Lookup> lookups = GetLinkedRecords(selectedMock);

                    e.Result = lookups;
                },
                ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                PostWorkCallBack = e =>
                {
                    if (selectedMock is RandomLookup lookup)
                    {
                        if (!fromLoad) lookup.Values.Clear();
                        lookup.AllValues = e.Result as List<Lookup>;
                    }
                    else
                        ((FixedLookup)selectedMock).AllValues = e.Result as List<Lookup>;
                }
            });

            // var rndMock = selectedMock as RandomLookup;
        }

        private void PopulatePickList(AttributeMetadata attribute, BaseMock selectedMock, bool fromLoad)
        {
            if (!(selectedMock is RandomPickList || selectedMock is FixedPickList))
                return;

            var pickListOptions = attribute is PicklistAttributeMetadata metadata
                ? metadata.OptionSet.Options
                : ((StatusAttributeMetadata)attribute).OptionSet.Options;
            List<PickList> pickLists = new List<PickList>();
            pickLists.AddRange(from pickListOption in pickListOptions
                               select new PickList
                               {
                                   choiceNo = pickListOption.Value.GetValueOrDefault(),
                                   Name = pickListOption.Label.UserLocalizedLabel.Label
                               });
            if (selectedMock is RandomPickList)
            {
                if (!fromLoad) ((RandomPickList)selectedMock).Values.Clear();
                ((RandomPickList)selectedMock).AllValues = pickLists;
            }
            else
                ((FixedPickList)selectedMock).AllValues = pickLists;
        }

        private void CreateInactiveRequest(Entity entity)
        {
            var updateEntity = updateEntities.FirstOrDefault(ue => ue.Status.Value == ((OptionSetValue)entity["statuscode"]).Value);
            if (updateEntity == null)
            {
                updateEntity = new UpdateEntity
                {
                    Status = (OptionSetValue)entity["statuscode"],
                    State = (OptionSetValue)entity["statecode"]
                };
                updateEntities.Add(updateEntity);
            }

            entity.Attributes.Remove("statecode");
            entity.Attributes.Remove("statuscode");
            updateEntity.Entities.Add(entity);
        }

        protected List<UpdateEntity> updateEntities = new List<UpdateEntity>();

        public class UpdateEntity
        {
            public List<Entity> Entities = new List<Entity>();

            public OptionSetValue Status { get; set; }

            public OptionSetValue State { get; set; }
        }

        private string SendInactiveRequest(UpdateEntity updateEntity, BackgroundWorker wrker, SetItem setItem, string entityName)
        {
            wrker.ReportProgress(-1, "Creating Inactive Records");
            string errors = string.Empty;
            var requestWithResults = new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            foreach (var entity in updateEntity.Entities)
                requestWithResults.Requests.Add(new CreateRequest { Target = entity });

            var responseWithResults =
                (ExecuteMultipleResponse)Service.Execute(requestWithResults);

            ai.WriteEvent("Data Mocked Count (Inactive)", requestWithResults.Requests.Count);

            errors = responseWithResults.Responses.Where(responseItem => responseItem.Fault != null)
                                               .Aggregate(errors, (accumulator, responseItem) => accumulator += "\r\n" +
                                                   responseItem.RequestIndex + " | " + responseItem.Fault);

            if (setItem != null)
            {
                setItem.AddedValues
                    .AddRange(
                        from item in responseWithResults.Responses
                            .Where(ri => ri.Fault == null)
                            .Select(cr => ((CreateResponse)cr.Response))
                        select new Lookup { guid = item.id, Name = item.id.ToString() });
                setItem.entityName = entityName;
            }

            foreach (var responseItem in responseWithResults.Responses.Where(ri => ri.Fault == null))
                updateEntity.Entities[responseItem.RequestIndex].Id = ((CreateResponse)responseItem.Response).id;

            wrker.ReportProgress(-1, "Updating Inactive Records");

            requestWithResults = new ExecuteMultipleRequest
            {
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            if (updateEntity.Entities[0].LogicalName == "incident" && updateEntity.State.Value == 1)
            {
                wrker.ReportProgress(-1, "Closing Cases");
                // need to call close incident
                foreach (var entity in updateEntity.Entities.Where(ent => ent.Id != Guid.Empty))
                {
                    var incidentRes = new Entity("incidentresolution");
                    incidentRes["subject"] = "Resolved Imported Incident";
                    incidentRes["incidentid"] = new EntityReference("incident", entity.Id);
                    var resolve = new CloseIncidentRequest();
                    resolve.IncidentResolution = incidentRes;
                    resolve.Status = updateEntity.Status;
                    requestWithResults.Requests.Add(resolve);
                }
                responseWithResults =
                    (ExecuteMultipleResponse)Service.Execute(requestWithResults);
                ai.WriteEvent("Data Mocked Count (Inactive Updates)", requestWithResults.Requests.Count);
                foreach (var responseItem in responseWithResults.Responses)

                    // An error has occurred.
                    if (responseItem.Fault != null)
                        errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault.ToString();

                // else updateEntity.Entities[responseItem.RequestIndex].Id
                // = ((updatere) responseItem.Response).id;
            }
            else
            {
                wrker.ReportProgress(-1, "Updating Inactive Records");
                foreach (var entity in updateEntity.Entities.Where(ent => ent.Id != Guid.Empty))
                {
                    entity["statecode"] = updateEntity.State;
                    entity["statuscode"] = updateEntity.Status;
                    requestWithResults.Requests.Add(new UpdateRequest { Target = entity });
                }

                responseWithResults =
                    (ExecuteMultipleResponse)Service.Execute(requestWithResults);
                ai.WriteEvent("Data Mocked Count (Inactive Updates)", requestWithResults.Requests.Count);
                foreach (var responseItem in responseWithResults.Responses)

                    // An error has occurred.
                    if (responseItem.Fault != null)
                        errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault.ToString();

                // else updateEntity.Entities[responseItem.RequestIndex].Id
                // = ((updatere) responseItem.Response).id;
            }

            return errors;
        }
    }
}