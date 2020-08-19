using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace LinkeD365.MockDataGen
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
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
            if (saveMap.ShowDialog() != DialogResult.OK) return;

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

            if (mySettings.Settings.Any(mr => mr.Name == setting.Name)) mySettings.Settings[mySettings.Settings.IndexOf(setting)] = setting;
            else mySettings.Settings.Add(setting);

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
                    if (selectedMock is NormalDistribution)
                    {
                        ((NormalDistribution)selectedMock).Decimals = decAttr.Precision.GetValueOrDefault();
                        return;
                    }
                    var decMock = (Number)selectedMock;
                    decMock.Decimals = decAttr.Precision.GetValueOrDefault();
                    decMock.Max = decAttr.MaxValue.GetValueOrDefault();
                    decMock.Min = decAttr.MinValue.GetValueOrDefault();
                    break;

                case DoubleAttributeMetadata doubleAttr:
                    if (selectedMock is NormalDistribution)
                    {
                        ((NormalDistribution)selectedMock).Decimals = doubleAttr.Precision.GetValueOrDefault();
                        return;
                    }
                    var dblMock = (Number)selectedMock; //Fixed numbers
                    dblMock.Decimals = doubleAttr.Precision.GetValueOrDefault();
                    dblMock.Max = (decimal)doubleAttr.MaxValue.GetValueOrDefault();
                    dblMock.Min = (decimal)doubleAttr.MinValue.GetValueOrDefault();
                    break;

                case IntegerAttributeMetadata intAttr:
                    if (selectedMock is NormalDistribution)
                    {
                        ((NormalDistribution)selectedMock).Decimals = 0;
                        return;
                    }
                    var intMock = (Number)selectedMock;
                    intMock.Decimals = 0;
                    intMock.Max = intAttr.MaxValue.GetValueOrDefault();
                    intMock.Min = intAttr.MinValue.GetValueOrDefault();
                    break;

                case MoneyAttributeMetadata moneyAttr:
                    if (selectedMock is NormalDistribution)
                    {
                        ((NormalDistribution)selectedMock).Decimals = moneyAttr.Precision.GetValueOrDefault();
                        return;
                    }
                    var moneyMock = (Number)selectedMock;
                    moneyMock.Decimals = moneyAttr.Precision.GetValueOrDefault();
                    moneyMock.Max = (decimal)moneyAttr.MaxValue.GetValueOrDefault();
                    moneyMock.Min = (decimal)moneyAttr.MinValue.GetValueOrDefault();
                    break;

                case BigIntAttributeMetadata bigIntAttr:
                    if (selectedMock is NormalDistribution)
                    {
                        ((NormalDistribution)selectedMock).Decimals = 0;
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
            string parentName = string.Empty;
            switch (selectedMock)
            {
                case FixedTeam ft:
                case RandomTeam rt:
                    parentName = "team";
                    break;

                case FixedUser fu:
                case RandomUser ru:
                    parentName = "systemuser";
                    break;

                case RandomLookup rl:
                case FixedLookup fl:
                    parentName = ((LookupAttributeMetadata)attribute).Targets[0];
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
                    var entityMeta = Service.GetEntityMetadata(parentName);
                    w.ReportProgress(10, "Getting " + entityMeta.DisplayCollectionName.UserLocalizedLabel == null ? entityMeta.LogicalName : entityMeta.DisplayCollectionName.UserLocalizedLabel.Label);
                    List<Lookup> lookups = new List<Lookup>();
                    var qe = new QueryExpression(entityMeta.LogicalName);
                    int count = 100;
                    int pageNo = 1;
                    qe.ColumnSet.AddColumn(entityMeta.PrimaryNameAttribute);
                    qe.Orders.Add(new OrderExpression(entityMeta.PrimaryNameAttribute, OrderType.Ascending));
                    qe.PageInfo = new PagingInfo() { Count = count, PageNumber = pageNo };
                    qe.PageInfo.PagingCookie = null;

                    while (true)
                    {
                        var responses = Service.RetrieveMultiple(qe);
                        if (responses.Entities != null)
                        {
                            lookups.AddRange(from item in responses.Entities
                                             select new Lookup()
                                             {
                                                 guid = item.Id,
                                                 Name = item.Attributes[entityMeta.PrimaryNameAttribute].ToString()
                                             });
                        }
                        if (responses.MoreRecords)
                        {
                            qe.PageInfo.PageNumber++;
                            qe.PageInfo.PagingCookie = responses.PagingCookie;
                        }
                        else break;
                    }
                    e.Result = lookups;
                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    if (selectedMock is RandomLookup)
                    {
                        ((RandomLookup)selectedMock).AllValues = e.Result as List<Lookup>;
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

            var pickListOptions = attribute is PicklistAttributeMetadata
                ? ((PicklistAttributeMetadata)attribute).OptionSet.Options
                : ((StatusAttributeMetadata)attribute).OptionSet.Options;
            List<PickList> pickLists = new List<PickList>();
            pickLists.AddRange(from pickListOption in pickListOptions
                               select new PickList()
                               {
                                   choiceNo = pickListOption.Value.GetValueOrDefault(),
                                   Name = pickListOption.Label.UserLocalizedLabel.Label
                               });
            if (selectedMock is RandomPickList) ((RandomPickList)selectedMock).AllValues = pickLists;
            else ((FixedPickList)selectedMock).AllValues = pickLists;
        }
    }
}