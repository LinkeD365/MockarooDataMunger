using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Windows.Forms;
using xrmtb.XrmToolBox.Controls;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Args;
using XrmToolBox.Extensibility.Interfaces;

namespace LinkeD365.MockDataGen
{
    public partial class MockDataGenCtl : PluginControlBase, IStatusBarMessenger
    {
        public event EventHandler<StatusBarMessageEventArgs> SendMessageToStatusBar;

        private void ShowResults()
        {
            if (tabSample.Parent != tabGrpMain)
                tabGrpMain.TabPages.Add(tabSample);
            tabGrpMain.SelectedTab = tabSample;
            tabSample.Enabled = true;
        }

        private EntityCollection CreateEntityCollection(dynamic returnData, string entityName, List<SimpleRow> selectedMaps)
        {
            EntityMetadata entity = Service.GetEntityMetadata(entityName);
            var entityCollection = new EntityCollection { EntityName = entityName };
            foreach (var data in returnData)
            {
                Entity newRecord = new Entity(entityName);
                IDictionary<string, object> propertyValues = data;

                foreach (var map in selectedMaps)
                    switch (map.SelectedMock)
                    {
                        case FakeEmailMock fakeEmail:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : propertyValues[map.LogicalName].ToString() +
                                    ".FAKE";

                            break;

                        case FixedDateTime fixedDateTime:
                            newRecord[map.LogicalName] = fixedDateTime.FixedValue;
                            break;

                        case FixedDate fixedDate:
                            newRecord[map.LogicalName] = fixedDate.FixedValue;
                            break;

                        case FixedLookup fixedLookup:
                            newRecord[map.LogicalName] = new EntityReference(
                                map.ParentTable,
                                ((Lookup)fixedLookup.FixedValue).guid);
                            break;

                        case FixedNumber fixedNumber:
                            newRecord[map.LogicalName] = fixedNumber.FixedValue;
                            break;

                        case FixedStatus fixedStatus:
                            var choiceNo = ((PickList)fixedStatus.FixedValue).choiceNo;
                            newRecord[map.LogicalName] = new OptionSetValue(
                                choiceNo);
                            var statusReasonMeta = Service.GetAttribute(
                                entityName,
                                map.LogicalName) as StatusAttributeMetadata;
                            var stateValue =
                        new OptionSetValue(
                                ((StatusOptionMetadata)statusReasonMeta.OptionSet.Options
                                    .First(o => o.Value == choiceNo)).State.Value);
                            newRecord["statecode"] = stateValue;
                            break;

                        case FixedPickList fixedPickList:
                            newRecord[map.LogicalName] = new OptionSetValue(
                                ((PickList)fixedPickList.FixedValue).choiceNo);

                            break;

                        case FixedTime fixedTime:
                            newRecord[map.LogicalName] = fixedTime.FixedValue;
                            break;

                        case RandomStatus randomStatus:
                            var choiceStatusNo = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new OptionSetValue(
                                    randomStatus.AllValues
                                        .First(
                                            pl => pl.Name ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .choiceNo);
                            newRecord[map.LogicalName] = choiceStatusNo;
                            if (choiceStatusNo != null)
                            {
                                var statusReasonRdmMeta =
                            Service.GetAttribute(entityName, map.LogicalName) as StatusAttributeMetadata;
                                var stateRdmValue =
                            new OptionSetValue(
                                    ((StatusOptionMetadata)statusReasonRdmMeta.OptionSet.Options
                                        .First(o => o.Value == choiceStatusNo.Value)).State.Value);
                                newRecord["statecode"] = stateRdmValue;
                            }

                            break;

                        case RandomPickList randomPickList:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new OptionSetValue(
                                    randomPickList.AllValues
                                        .First(
                                            pl => pl.Name ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .choiceNo);
                            break;

                        case RandomLookup randomLookup:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new EntityReference(
                                    map.ParentTable,
                                    randomLookup.AllValues
                                        .First(
                                            lup => lup.Name.Trim() ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .guid);
                            break;

                        case FromSet fromSet: //todo
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new EntityReference(
                                    map.ParentTable,
                                    fromSet.Values
                                        .First(
                                            lup => lup.Name ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .guid);
                            break;

                        case StringMock stringMock:
                            if (!propertyValues.ContainsKey(map.LogicalName)) newRecord[map.LogicalName] = null;
                            else if (map.Length != null)
                                newRecord[map.LogicalName] = ((string)propertyValues[map.LogicalName]).Truncate(map.Length.GetValueOrDefault());
                            else
                            {
                                StringAttributeMetadata attMeta = (StringAttributeMetadata)entity.Attributes
                                    .FirstOrDefault(at => at.LogicalName == map.LogicalName);
                                newRecord[map.LogicalName] = ((string)propertyValues[map.LogicalName]).Truncate(
                                    attMeta.DatabaseLength.GetValueOrDefault());
                            }
                            break;

                        case Date dateMock:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? (DateTime?)null
                                : DateTime.ParseExact(
                                    propertyValues[map.LogicalName].ToString(),
                                    "yyyy-MM-dd",
                                    null);
                            break;

                        case Time timeMock:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? (DateTime?)null
                                : DateTime.Today
                                    .Add(
                                        TimeSpan.Parse(
                                                propertyValues[map.LogicalName].ToString(
                                                    )));

                            break;

                        case CustomList custList:
                            newRecord[map.LogicalName] = propertyValues[map.LogicalName].ToString();
                            break;

                        case Number number:
                            switch (map.AttributeTypeCode)
                            {
                                case AttributeTypeCode.Integer:
                                    newRecord[map.LogicalName] = int.TryParse(propertyValues[map.LogicalName].ToString(), out int intRes)
                                ? intRes : Int32.TryParse(propertyValues[map.LogicalName].ToString(), out Int32 int32Res)
                                ? int32Res : Int64.TryParse(propertyValues[map.LogicalName].ToString(), out Int64 int64Res)
                                ? int64Res : propertyValues[map.LogicalName];
                                    break;

                                case AttributeTypeCode.Decimal:
                                    newRecord[map.LogicalName] = decimal.TryParse(propertyValues[map.LogicalName].ToString(), out decimal decRes) ? decRes : propertyValues[map.LogicalName];
                                    break;

                                case AttributeTypeCode.Double:
                                    newRecord[map.LogicalName] = double.TryParse(propertyValues[map.LogicalName].ToString(), out double dblRes) ? dblRes : propertyValues[map.LogicalName];
                                    break;

                                default:
                                    newRecord[map.LogicalName] = propertyValues[map.LogicalName];
                                    break;
                            }

                            break;

                        default:
                            switch (map.SelectedMock.Name)
                            {
                                case (DataTypes.Boolean):
                                case (DataTypes.BinomialDistribution):
                                    newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                            map.LogicalName)
                                        ? false
                                        : (bool)propertyValues[map.LogicalName];
                                    break;

                                default:
                                    if (map.SelectedMock.Fixed)
                                        newRecord[map.LogicalName] = map.SelectedMock.FixedValue;
                                    else
                                        newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                                map.LogicalName)
                                            ? null
                                            : propertyValues[map.LogicalName];
                                    break;
                            }
                            break;
                    }

                entityCollection.Entities.Add(newRecord);
            }
            return entityCollection;
        }

        private List<Entity> CreateEntityList(dynamic returnData, string entityName, List<SimpleRow> selectedMaps)
        {
            var entity = Service.GetEntityMetadata(entityName);
            var resp = new List<Entity>();
            foreach (var data in returnData)
            {
                Entity newRecord = new Entity(entityName);
                IDictionary<string, object> propertyValues = data;

                foreach (var map in selectedMaps)
                    switch (map.SelectedMock)
                    {
                        case FakeEmailMock fakeEmail:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : propertyValues[map.LogicalName].ToString() +
                                    ".FAKE";

                            break;

                        case CarModelYearMock carYearMock:
                            newRecord[map.LogicalName] = ((Int64)propertyValues[map.LogicalName]).ToString();
                            break;

                        case FixedDateTime fixedDateTime:
                            newRecord[map.LogicalName] = fixedDateTime.FixedValue;
                            break;

                        case FixedDate fixedDate:
                            newRecord[map.LogicalName] = fixedDate.FixedValue;
                            break;

                        case FixedLookup fixedLookup:
                            newRecord[map.LogicalName] = new EntityReference(
                                map.ParentTable,
                                ((Lookup)fixedLookup.FixedValue).guid);
                            break;

                        case FixedNumber fixedNumber:
                            newRecord[map.LogicalName] = fixedNumber.FixedValue;
                            break;

                        case FixedStatus fixedStatus:
                            var choiceNo = ((PickList)fixedStatus.FixedValue).choiceNo;
                            newRecord[map.LogicalName] = new OptionSetValue(
                                choiceNo);
                            var statusReasonMeta = Service.GetAttribute(
                                entityName,
                                map.LogicalName) as StatusAttributeMetadata;
                            var stateValue =
                        new OptionSetValue(
                                ((StatusOptionMetadata)statusReasonMeta.OptionSet.Options
                                    .First(o => o.Value == choiceNo)).State.Value);
                            newRecord["statecode"] = stateValue;
                            break;

                        case FixedPickList fixedPickList:
                            newRecord[map.LogicalName] = new OptionSetValue(
                                ((PickList)fixedPickList.FixedValue).choiceNo);

                            break;

                        case FixedTime fixedTime:
                            newRecord[map.LogicalName] = fixedTime.FixedValue;
                            break;

                        case RandomStatus randomStatus:
                            var choiceStatusNo = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new OptionSetValue(
                                    randomStatus.AllValues
                                        .First(
                                            pl => pl.Name ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .choiceNo);
                            newRecord[map.LogicalName] = choiceStatusNo;
                            if (choiceStatusNo != null)
                            {
                                var statusReasonRdmMeta =
                            Service.GetAttribute(entityName, map.LogicalName) as StatusAttributeMetadata;
                                var stateRdmValue =
                            new OptionSetValue(
                                    ((StatusOptionMetadata)statusReasonRdmMeta.OptionSet.Options
                                        .First(o => o.Value == choiceStatusNo.Value)).State.Value);
                                newRecord["statecode"] = stateRdmValue;
                            }

                            break;

                        case RandomPickList randomPickList:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new OptionSetValue(
                                    randomPickList.AllValues
                                        .First(
                                            pl => pl.Name ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .choiceNo);
                            break;

                        case RandomLookup randomLookup:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new EntityReference(
                                    map.ParentTable,
                                    randomLookup.AllValues
                                        .First(
                                            lup => lup.Name.Trim() ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .guid);
                            break;

                        case FromSet fromSet: //todo
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? null
                                : new EntityReference(
                                    map.ParentTable,
                                    fromSet.Values
                                        .First(
                                            lup => lup.Name ==
                                                        propertyValues[
                                                            map.LogicalName].ToString(
                                                            ))
                                        .guid);
                            break;

                        case StringMock stringMock:
                            if (!propertyValues.ContainsKey(map.LogicalName)) newRecord[map.LogicalName] = null;
                            else if (map.Length != null)
                                newRecord[map.LogicalName] = ((string)propertyValues[map.LogicalName]).Truncate(map.Length.GetValueOrDefault());
                            else
                            {
                                StringAttributeMetadata attMeta = (StringAttributeMetadata)entity.Attributes
                                    .FirstOrDefault(at => at.LogicalName == map.LogicalName);
                                newRecord[map.LogicalName] = ((string)propertyValues[map.LogicalName]).Truncate(
                                    attMeta.DatabaseLength.GetValueOrDefault());
                            }
                            break;

                        case Date dateMock:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? (DateTime?)null
                                : DateTime.ParseExact(
                                    propertyValues[map.LogicalName].ToString(),
                                    "yyyy-MM-dd",
                                    null);
                            break;

                        case Time timeMock:
                            newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                    map.LogicalName)
                                ? (DateTime?)null
                                : DateTime.Today
                                    .Add(
                                        TimeSpan.Parse(
                                                propertyValues[map.LogicalName].ToString(
                                                    )));

                            break;

                        case CustomList custList:
                            newRecord[map.LogicalName] = propertyValues[map.LogicalName].ToString();
                            break;

                        case Number number:
                            if (!propertyValues.ContainsKey(map.LogicalName)) newRecord[map.LogicalName] = null;
                            else
                            {
                                switch (map.AttributeTypeCode)
                                {
                                    case AttributeTypeCode.Integer:
                                        newRecord[map.LogicalName] = int.TryParse(propertyValues[map.LogicalName].ToString(), out int intRes)
                                    ? intRes : Int32.TryParse(propertyValues[map.LogicalName].ToString(), out Int32 int32Res)
                                    ? int32Res : Int64.TryParse(propertyValues[map.LogicalName].ToString(), out Int64 int64Res)
                                    ? int64Res : propertyValues[map.LogicalName];
                                        break;

                                    case AttributeTypeCode.Decimal:
                                        newRecord[map.LogicalName] = decimal.TryParse(propertyValues[map.LogicalName].ToString(), out decimal decRes) ? decRes : propertyValues[map.LogicalName];
                                        break;
                                    
                                    case AttributeTypeCode.Money:
                                        decimal decResMoney; 
                                        if (decimal.TryParse(propertyValues[map.LogicalName].ToString(), out decResMoney)) 
                                        { newRecord[map.LogicalName] = new Money(decResMoney); } 
                                        else { newRecord[map.LogicalName] = null; }                                        
                                        break;
                                    
                                    case AttributeTypeCode.Double:
                                        newRecord[map.LogicalName] = double.TryParse(propertyValues[map.LogicalName].ToString(), out double dblRes) ? dblRes : propertyValues[map.LogicalName];
                                        break;
                                    
                                    default:
                                        newRecord[map.LogicalName] = propertyValues[map.LogicalName];
                                        break;
                                }
                            }
                            break;

                        default:
                            switch (map.SelectedMock.Name)
                            {
                                case (DataTypes.Boolean):
                                case (DataTypes.BinomialDistribution):
                                    newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                            map.LogicalName)
                                        ? (bool?)null
                                        : (bool)propertyValues[map.LogicalName];
                                    break;

                                default:
                                    if (map.SelectedMock.Fixed)
                                        newRecord[map.LogicalName] = map.SelectedMock.FixedValue;
                                    else
                                        newRecord[map.LogicalName] = !propertyValues.ContainsKey(
                                                map.LogicalName)
                                            ? null
                                            : propertyValues[map.LogicalName];
                                    break;
                            }
                            break;
                    }

                resp.Add(newRecord);
            }
            return resp;
        }

        public string CreateAllData(int totalRecordCount, List<SimpleRow> maps, string entityName, BackgroundWorker wrker)
        {
            return CreateAllData(totalRecordCount, maps, entityName, wrker, null);
        }

        public List<ExpandoObject> LoadMockData(int recordCount, List<SimpleRow> maps, BackgroundWorker worker)
        {
            worker.ReportProgress(0, "Getting Mockaroo Data");
            var response = new List<ExpandoObject>();

            var pageSize = recordCount > 1000 ? 1000 : recordCount;
            var pageRequests = (int)Math.Ceiling(recordCount / (double)pageSize);
            var client = new MockClient(txtMockKey.Text);
            var mockClass = new ExpandoObject();
            foreach (var map in maps)
                ((IDictionary<string, object>)mockClass)[map.LogicalName] = map.SelectedMock;

            for (var i = 0; i < pageRequests; i++)
            {
                worker.ReportProgress((int)(i / pageRequests * 100), $"Getting Mockaroo Data - Batch {i + 1}");
                response.AddRange(client.GetData(mockClass, pageSize));
            }

            return response.Take(recordCount).ToList();
        }

        public string CreateAllData(int totalRecordCount, List<SimpleRow> maps, string entityName, BackgroundWorker wrker, SetItem setItem)
        {
            int initialCount = totalRecordCount;
            int recordCount = totalRecordCount > 500 ? 500 : totalRecordCount;
            int noRuns = (int)Math.Ceiling((double)totalRecordCount / (double)recordCount);
            int percDone = 0;
            var entityCollection = new EntityCollection { EntityName = entityName };
            var mockClass = new ExpandoObject();
            foreach (var map in maps)
                ((IDictionary<string, object>)mockClass)[map.LogicalName] = map.SelectedMock;

            wrker.ReportProgress(0, "Getting Mockaroo Data for " + entityName);
            var client = new MockClient(txtMockKey.Text);
            string errors = string.Empty;

            if (collection != null && collection.Entities.Count > 0)
            {
                recordCount = collection.Entities.Count;
                wrker.ReportProgress(percDone, "Creating Sample Data");
                errors += CreateData(collection, wrker, setItem, entityName);
                totalRecordCount -= recordCount;
                SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(
                  (int)Math.Round((double)(initialCount - totalRecordCount) * 100 / initialCount),
                    $"{totalRecordCount} {entityName} Records remaining"));
            }
            while (totalRecordCount > 0)
            {
                recordCount = totalRecordCount > 1000 ? 1000 : totalRecordCount;
                List<ExpandoObject> returnData = client.GetData(mockClass, recordCount);
                wrker.ReportProgress(percDone, "Got Data, Generating Entity Collection");
                foreach (List<ExpandoObject> subList in SplitList(returnData))
                {
                    entityCollection = CreateEntityCollection(subList, entityName, maps);
                    wrker.ReportProgress(percDone, "Retrieved Mockaroo Data, Creating in Dataverse");
                    percDone += 100 / noRuns;
                    errors += CreateData(entityCollection, wrker, setItem, entityName);

                    totalRecordCount -= subList.Count();
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(
                      (int)Math.Round((double)(initialCount - totalRecordCount) * 100 / initialCount),
                        $"{totalRecordCount} {entityName} Records remaining"));
                    wrker.ReportProgress(percDone, "Created Data");
                }
            }

            return errors;
        }

        private List<List<ExpandoObject>> SplitList(List<ExpandoObject> returnData)
        {
            var list = new List<List<ExpandoObject>>();

            for (int i = 0; i < returnData.Count; i += 500)
                list.Add(returnData.GetRange(i, Math.Min(500, returnData.Count - i)));

            return list;
        }

        private string CreateData(EntityCollection entityCollection, BackgroundWorker worker, SetItem setItem, string entityName)
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
            foreach (var entity in entityCollection.Entities)
                if (entity.Attributes.Contains("statecode") && ((OptionSetValue)entity["statecode"]).Value >= 1) // inactive
                    CreateInactiveRequest(entity);
                else requestWithResults.Requests.Add(new CreateRequest { Target = entity });
            string errors = string.Empty;
            if (requestWithResults.Requests.Count > 0)
            {
                var responseWithResults =
                     (ExecuteMultipleResponse)Service.Execute(requestWithResults);

                ai.WriteEvent("Data Mocked Count", requestWithResults.Requests.Count);
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
                //setItem.AddedValues.AddRange(responseWithResults.Responses.Select(ri => new Lookup(){guid = ((CreateResponse) ri.Response).)

                // else collection.Entities[responseItem.RequestIndex].Id = ((CreateResponse) responseItem.Response).id;
                //DisplayFault(requestWithResults.Requests[responseItem.RequestIndex],
                //    responseItem.RequestIndex, responseItem.Fault);
            }

            foreach (var updateEntity in updateEntities)
                errors += SendInactiveRequest(updateEntity, worker, setItem, entityName);

            return errors;
            // e.Result = errors;
        }

        private List<Lookup> GetLinkedRecords(BaseMock selectedMock)//, BackgroundWorker d)
        {
            var entityMeta = Service.GetEntityMetadata(selectedMock.EntityName);
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
                    lookups.AddRange(from item in responses.Entities
                                     select new Lookup
                                     {
                                         guid = item.Id,
                                         Name = item.Attributes.Contains(entityMeta.PrimaryNameAttribute) ? (item.Attributes[entityMeta.PrimaryNameAttribute] ?? string.Empty).ToString() : string.Empty
                                     });
                if (responses.MoreRecords)
                {
                    qe.PageInfo.PageNumber++;
                    qe.PageInfo.PagingCookie = responses.PagingCookie;
                }
                else
                    break;
            }
            return lookups;
        }

        private void PlaySet()
        {
            if (collection?.Entities != null) collection.Entities.Clear();
            HideResults();
            if (cboRunDataSet.SelectedItem == null)
                return;

            var selectedSet = mySettings.Sets
                .First(stng => stng.SetName == cboRunDataSet.SelectedItem.ToString());
            if (selectedSet is null)
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
            WorkAsync(
               new WorkAsyncInfo
               {
                   Message = "Getting Mockaroo Data...",
                   Work =
                       (w, e) =>
                       {
                           string errors = string.Empty;
                           foreach (SetItem setItem in selectedSet.SetItems.OrderBy(si => si.Position))
                           {
                               var map = mySettings.Settings.FirstOrDefault(m => m.Name == setItem.MapName);
                               if (map == null)
                               {
                                   MessageBox.Show(
                                       $"The defined map {setItem.MapName} does not exist. Please check the set/map definition",
                                       "No map found",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                                   return;
                               }

                               if (map.MapRows.Any(mr => mr.AttributeTypeCode == null || String.IsNullOrEmpty(mr.LogicalName)))
                               {
                                   MessageBox.Show(
                                       $"The defined map {setItem.MapName} needs updating to the new format. Please resave the map to enable for Sets.",
                                       "Old Map config found",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Error);
                                   return;
                               }
                               List<SimpleRow> mapRows = new List<SimpleRow>();

                               foreach (var simpleMapRow in map.MapRows)
                                   if (simpleMapRow.SelectedMock?.FirstOrDefault(kvp => kvp.Key == "MockName")?.Value != null)
                                   {
                                       var simpleRow = new SimpleRow
                                       {
                                           LogicalName = simpleMapRow.LogicalName,
                                           AttributeTypeCode = simpleMapRow.AttributeTypeCode,
                                           SelectedMock =
                                                  MockOptions.First(mo => mo.AttributeTypeCode == simpleMapRow.AttributeTypeCode)
                                                      .Mocks
                                                      .First(
                                                          m => m.Name ==
                                                                  simpleMapRow.SelectedMock.First(kvp => kvp.Key == "MockName").Value
                                                                      .ToString())
                                       };
                                       simpleRow.SelectedMock.PopulateFromKVP(simpleMapRow.SelectedMock);
                                       if (simpleRow.SelectedMock is RandomLookup)
                                       {
                                           var randomLookup = simpleRow.SelectedMock as RandomLookup;
                                           w.ReportProgress(10, "Getting " + map.EntityDisplay.DisplayName);

                                           if (randomLookup.All)
                                               randomLookup.AllValues = GetLinkedRecords(simpleRow.SelectedMock);
                                       }
                                       else if (simpleRow.SelectedMock is FromSet)
                                       {
                                           var parentSI = selectedSet.SetItems
                                               .FirstOrDefault(si => si.entityName == simpleRow.SelectedMock.EntityName);

                                           if (parentSI == null) ((FromSet)simpleRow.SelectedMock).Values = GetLinkedRecords(
                                                                     simpleRow.SelectedMock);
                                           else ((FromSet)simpleRow.SelectedMock).Values = new List<Lookup>(parentSI.AddedValues);
                                       }
                                       mapRows.Add(simpleRow);
                                   }
                               errors += CreateAllData(setItem.RecordCount, mapRows, map.EntityName, w, setItem);

                               e.Result = errors;
                           }
                       },
                   ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                   PostWorkCallBack =
                        e =>
                        {
                            if (e.Error == null && e.Result == null)

                                //Handled error
                                return;
                            if (e.Error != null)
                            {
                                LogError(e.Error.ToString());
                                MessageBox.Show(
                                    e.Error.Message.ToString() + Environment.NewLine + "Some Data may have been created, please confirm before re-running",
                                                            "Error generating data",
                                                            MessageBoxButtons.OK,
                                                            MessageBoxIcon.Error);
                                SendMessageToStatusBar(this, new StatusBarMessageEventArgs(string.Empty));

                                return;
                            }
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
                                // gridSample.DataSource = collection.Entities;
                            }

                            SendMessageToStatusBar(this, new StatusBarMessageEventArgs(string.Empty));
                        }
               });
        }
    }
}