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
            {
                return value;
            }

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
        }

        private void LoadEntities()
        {
            if (Service == null)
            {
                return;
            }
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Entities",
                Work = (w, e) =>
                {
                    var eqe = new EntityQueryExpression();
                    eqe.Properties = new MetadataPropertiesExpression("LogicalName", "DisplayName");
                    var req = new RetrieveMetadataChangesRequest()
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

                    cboEntities.Items.AddRange(metaresponse.Select(ent => new EntityDisplay { LogicalName = ent.LogicalName, DisplayName = ent.DisplayName.UserLocalizedLabel == null ? ent.LogicalName : ent.DisplayName.UserLocalizedLabel.Label }).OrderBy(ent => ent.DisplayName).ToArray());
                }
            });
        }

        private void ShowSaveMap()
        {
            var saveMap = new SaveMap(mySettings);
            if (saveMap.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            mySettings.MockKey = txtMockKey.Text;
            var setting = new Setting();
            setting.Name = saveMap.settingName;
            setting.EntityDisplay = cboEntities.SelectedItem as EntityDisplay;
            setting.MapRows = selectedMaps.Select(mr => new SimpleMapRow()
            {
                AttributeName = mr.AttributeName,
                BlackPercentage = mr.BlankPercentage,

                SelectedMock = mr.SelectedMock == null ? null : mr.SelectedMock.GetField().Select(dic => new KVP()
                {
                    Key = dic.Key,
                    Value = dic.Value is string[]? string.Join("||", (string[])dic.Value) : dic.Value
                }).ToList()
            }).ToList();

            if (mySettings.Settings.Any(mr => mr.Name == setting.Name))
            {
                mySettings.Settings[mySettings.Settings.IndexOf(setting)] = setting;
            }
            else
            {
                mySettings.Settings.Add(setting);
            }

            SettingsManager.Instance.Save(typeof(AllSettings), mySettings);
        }

        private void SetUpNumberDefaults(AttributeMetadata attribute, BaseMock selectedMock)
        {
            if (selectedMock is BinomialDistribution || selectedMock is FixedNumber)
            {
                return;
            }

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

        private void PopulateLookup(AttributeMetadata attribute, BaseMock selectedMock)
        {
            //    if (!(selectedMock is RandomLookup || selectedMock is FixedLookup))
            //    {
            //        return;
            //    }
            //var parentName = string.Empty;
            switch (selectedMock)
            {
                case FixedContact fc:
                    fc.EntityName = "contact";
                    //   parentName = "contact";
                    break;
                case FixedTeam ft:
                    ft.EntityName = "team";
                    break;
                case RandomTeam rt:
                    // parentName = "team";
                    rt.EntityName = "team";
                    break;

                case FixedUser fu:
                    fu.EntityName = "systemuser";
                    break;
                case RandomUser ru:
                    ru.EntityName = "systemuser";
                    //parentName = "systemuser";
                    break;

                case RandomLookup rl:
                    rl.EntityName = ((LookupAttributeMetadata)attribute).Targets[0];
                    break;
                case FixedLookup fl:
                    fl.EntityName = ((LookupAttributeMetadata)attribute).Targets[0];
                    break;

                default:
                    return;
            }
            //var lookupAttr = attribute as LookupAttributeMetadata;
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting linked records",
                Work = (w, e) =>
                {
                    var entityMeta = Service.GetEntityMetadata(selectedMock.EntityName);
                    w.ReportProgress(10, "Getting " + entityMeta.DisplayCollectionName.UserLocalizedLabel == null ? entityMeta.LogicalName : entityMeta.DisplayCollectionName.UserLocalizedLabel.Label);
                    var lookups = new List<Lookup>();
                    var qe = new QueryExpression(entityMeta.LogicalName);
                    var count = 100;
                    var pageNo = 1;
                    qe.ColumnSet.AddColumn(entityMeta.PrimaryNameAttribute);
                    qe.Orders.Add(new OrderExpression(entityMeta.PrimaryNameAttribute, OrderType.Ascending));
                    qe.PageInfo = new PagingInfo { Count = count, PageNumber = pageNo, PagingCookie = null };

                    while (true)
                    {
                        var responses = Service.RetrieveMultiple(qe);
                        if (responses.Entities != null)
                        {
                            lookups.AddRange(from item in responses.Entities
                                             select new Lookup()
                                             {
                                                 guid = item.Id,
                                                 Name = item.Attributes.Contains(entityMeta.PrimaryNameAttribute) ? (item.Attributes[entityMeta.PrimaryNameAttribute] ?? string.Empty).ToString() : string.Empty
                                             });
                        }
                        if (responses.MoreRecords)
                        {
                            qe.PageInfo.PageNumber++;
                            qe.PageInfo.PagingCookie = responses.PagingCookie;
                        }
                        else
                        {
                            break;
                        }
                    }
                    e.Result = lookups;
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    if (selectedMock is RandomLookup lookup)
                    {
                        lookup.AllValues = e.Result as List<Lookup>;
                    }
                    else
                    {
                        ((FixedLookup)selectedMock).AllValues = e.Result as List<Lookup>;
                    }
                }
            });
            //  var rndMock = selectedMock as RandomLookup;
        }

        private void PopulatePickList(AttributeMetadata attribute, BaseMock selectedMock)
        {
            if (!(selectedMock is RandomPickList || selectedMock is FixedPickList))
            {
                return;
            }

            var pickListOptions = attribute is PicklistAttributeMetadata metadata
                ? metadata.OptionSet.Options
                : ((StatusAttributeMetadata)attribute).OptionSet.Options;
            List<PickList> pickLists = new List<PickList>();
            pickLists.AddRange(from pickListOption in pickListOptions
                               select new PickList()
                               {
                                   choiceNo = pickListOption.Value.GetValueOrDefault(),
                                   Name = pickListOption.Label.UserLocalizedLabel.Label
                               });
            if (selectedMock is RandomPickList)
            {
                ((RandomPickList)selectedMock).AllValues = pickLists;
            }
            else
            {
                ((FixedPickList)selectedMock).AllValues = pickLists;
            }
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


        private string SendInactiveRequest(UpdateEntity updateEntity, BackgroundWorker wrker)
        {
            wrker.ReportProgress(-1, "Creating Inactive Records");
            string errors = string.Empty;
            var requestWithResults = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            foreach (var entity in updateEntity.Entities)
            {
                requestWithResults.Requests.Add(new CreateRequest() { Target = entity });
            }

            var responseWithResults =
                (ExecuteMultipleResponse)Service.Execute(requestWithResults);

            ai.WriteEvent("Data Mocked Count (Inactive)", requestWithResults.Requests.Count);
            foreach (var responseItem in responseWithResults.Responses)
            {

                // An error has occurred.
                if (responseItem.Fault != null)
                {
                    errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault.ToString();
                }
                else
                {
                    updateEntity.Entities[responseItem.RequestIndex].Id = ((CreateResponse)responseItem.Response).id;
                }
            }
            wrker.ReportProgress(-1, "Updating Inactive Records");

            requestWithResults = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
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
                {

                    // An error has occurred.
                    if (responseItem.Fault != null)
                    {
                        errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault.ToString();
                    }
                    //  else updateEntity.Entities[responseItem.RequestIndex].Id = ((updatere) responseItem.Response).id;

                }
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
                {

                    // An error has occurred.
                    if (responseItem.Fault != null)
                    {
                        errors += "\r\n" + responseItem.RequestIndex + " | " + responseItem.Fault.ToString();
                    }
                    //  else updateEntity.Entities[responseItem.RequestIndex].Id = ((updatere) responseItem.Response).id;

                }
            }

            return errors;
        }
    }
}