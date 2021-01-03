using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
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

        public void GetInitMockData(int recordCount)
        {
            var intialCollection = new EntityCollection { EntityName = entityName };
            WorkAsync(
                new WorkAsyncInfo
                {
                    Message = "Getting Mockaroo Data...",
                    Work =
                        (w, e) =>
                        {
                            var client = new MockClient(txtMockKey.Text);
                            var returnData = client.GetData(mockClass, recordCount);

                            w.ReportProgress(50, "Got Data, Generating Sample");

                            intialCollection = CreateEntityCollection(returnData);
                            w.ReportProgress(50, "Populating grid");

                            e.Result = intialCollection.Entities;
                        },
                    ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                    PostWorkCallBack =
                        e =>
                        {
                            if (e.Error != null)
                            {
                                LogError(e.Error.ToString());
                                MessageBox.Show(
                                    e.Error.Message.ToString(),
                                    "Error generating data",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                //ShowErrorNotification(
                                //    e.Error.ToString(),
                                //    new Uri("https://www.linked365.blog/"));
                                intialCollection.Entities.Clear();
                            }
                            else
                            {
                                gridSample.DataSource = e.Result;
                                collection.Entities.Clear();
                                collection.Entities.AddRange(e.Result as DataCollection<Entity>);
                                if (tabSample.Parent != tabGrpMain)
                                    tabGrpMain.TabPages.Add(tabSample);

                                tabGrpMain.SelectedTab = tabSample;
                                tabSample.Enabled = true;
                                updateEntities.Clear();

                                if (recordCount != numRecordCount.Value)
                                {
                                    btnCreateBatch.Visible = true;
                                    btnCreateBatch.Text = "Create " + numRecordCount.Value + " records";
                                    btnCreateData.Text = "Create " + recordCount + " records";
                                }
                                else
                                {
                                    btnCreateBatch.Visible = false;
                                    btnCreateData.Text = "Create Records";
                                }
                            }
                        }
                });
        }

        private EntityCollection CreateEntityCollection(dynamic returnData)
        {

            var entityCollection = new EntityCollection { EntityName = entityName };
            foreach (var data in returnData)
            {
                Entity newRecord = new Entity(entityName);
                IDictionary<string, object> propertyValues = data;

                foreach (var map in selectedMaps)
                    switch (map.SelectedMock)
                    {
                        case FakeEmailMock fakeEmail:
                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? null
                                : propertyValues[map.Attribute.LogicalName].ToString() +
                                    ".FAKE";

                            break;
                        case FixedDateTime fixedDateTime:
                            newRecord[map.Attribute.LogicalName] = fixedDateTime.FixedValue;
                            break;

                        case FixedDate fixedDate:
                            newRecord[map.Attribute.LogicalName] = fixedDate.FixedValue;
                            break;

                        case FixedLookup fixedLookup:
                            newRecord[map.Attribute.LogicalName] = new EntityReference(
                                map.ParentTable,
                                ((Lookup)fixedLookup.FixedValue).guid);
                            break;

                        case FixedNumber fixedNumber:
                            newRecord[map.Attribute.LogicalName] = fixedNumber.FixedValue;
                            break;
                        case FixedStatus fixedStatus:
                            var choiceNo = ((PickList)fixedStatus.FixedValue).choiceNo;
                            newRecord[map.Attribute.LogicalName] = new OptionSetValue(
                                choiceNo);
                            var statusReasonMeta = Service.GetAttribute(
                                entityName,
                                map.Attribute.LogicalName) as StatusAttributeMetadata;
                            var stateValue =
                        new OptionSetValue(
                                ((StatusOptionMetadata)statusReasonMeta.OptionSet.Options
                                    .First(o => o.Value == choiceNo)).State.Value);
                            newRecord["statecode"] = stateValue;
                            break;
                        case FixedPickList fixedPickList:
                            newRecord[map.Attribute.LogicalName] = new OptionSetValue(
                                ((PickList)fixedPickList.FixedValue).choiceNo);

                            break;

                        case FixedTime fixedTime:
                            newRecord[map.Attribute.LogicalName] = fixedTime.FixedValue;
                            break;
                        case RandomStatus randomStatus:

                            //  newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(map.Attribute.LogicalName) ? null : new OptionSetValue(randomPickList.AllValues.First(pl => pl.Name == propertyValues[map.Attribute.LogicalName].ToString()).choiceNo);

                            var choiceStatusNo = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? null
                                : new OptionSetValue(
                                    randomStatus.AllValues
                                        .First(
                                            pl => pl.Name ==
                                                        propertyValues[
                                                            map.Attribute.LogicalName].ToString(
                                                            ))
                                        .choiceNo);
                            newRecord[map.Attribute.LogicalName] = choiceStatusNo;
                            if (choiceStatusNo != null)
                            {
                                var statusReasonRdmMeta =
                            Service.GetAttribute(entityName, map.Attribute.LogicalName) as StatusAttributeMetadata;
                                var stateRdmValue =
                            new OptionSetValue(
                                    ((StatusOptionMetadata)statusReasonRdmMeta.OptionSet.Options
                                        .First(o => o.Value == choiceStatusNo.Value)).State.Value);
                                newRecord["statecode"] = stateRdmValue;
                            }

                            break;
                        case RandomPickList randomPickList:
                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? null
                                : new OptionSetValue(
                                    randomPickList.AllValues
                                        .First(
                                            pl => pl.Name ==
                                                        propertyValues[
                                                            map.Attribute.LogicalName].ToString(
                                                            ))
                                        .choiceNo);
                            break;

                        case RandomLookup randomLookup:
                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? null
                                : new EntityReference(
                                    map.ParentTable,
                                    randomLookup.AllValues
                                        .First(
                                            lup => lup.Name ==
                                                        propertyValues[
                                                            map.Attribute.LogicalName].ToString(
                                                            ))
                                        .guid);
                            break;

                        case StringMock stringMock:

                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? null
                                : ((string)propertyValues[map.Attribute.LogicalName]).Truncate(
                                    map.AttributeLength.GetValueOrDefault());
                            break;

                        case Date dateMock:

                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? (DateTime?)null
                                : DateTime.ParseExact(
                                    propertyValues[map.Attribute.LogicalName].ToString(),
                                    "yyyy-MM-dd",
                                    null);
                            break;

                        case Time timeMock:
                            newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                    map.Attribute.LogicalName)
                                ? (DateTime?)null
                                : DateTime.Today
                                    .Add(
                                        TimeSpan.Parse(
                                                propertyValues[map.Attribute.LogicalName].ToString(
                                                    )));

                            break;

                        default:

                            switch (map.SelectedMock.Name)
                            {
                                case (DataTypes.Boolean):
                                case (DataTypes.BinomialDistribution):
                                    newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                            map.Attribute.LogicalName)
                                        ? false
                                        : (bool)propertyValues[map.Attribute.LogicalName];
                                    break;

                                default:
                                    if (map.SelectedMock.Fixed)
                                        newRecord[map.Attribute.LogicalName] = map.SelectedMock.FixedValue;
                                    else
                                        newRecord[map.Attribute.LogicalName] = !propertyValues.ContainsKey(
                                                map.Attribute.LogicalName)
                                            ? null
                                            : propertyValues[map.Attribute.LogicalName];
                                    break;
                            }
                            break;
                    }

                entityCollection.Entities.Add(newRecord);
            }
            return entityCollection;
        }

        private void CreateAllData(int totalRecordCount)
        {
            int initialCount = totalRecordCount;
            int recordCount = totalRecordCount > 500 ? 500 : totalRecordCount;
            int noRuns = (int)Math.Ceiling((double)totalRecordCount / (double)recordCount);
            int percDone = 0;
            var entityCollection = new EntityCollection { EntityName = entityName };


            WorkAsync(
                new WorkAsyncInfo
                {
                    Message = "Getting Mockaroo Data...",
                    Work =
                        (w, e) =>
                        {
                            var client = new MockClient(txtMockKey.Text);
                            string errors = string.Empty;

                            if (collection.Entities.Count > 0)
                            {
                                recordCount = collection.Entities.Count;
                                w.ReportProgress(percDone, "Creating Sample Data");
                                errors += CreateData(collection, w);
                                totalRecordCount -= recordCount;
                                SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(
                                  (int)Math.Round((double)(initialCount - totalRecordCount) * 100 / initialCount),
                                    totalRecordCount + " Records remaining"));
                            }
                            while (totalRecordCount > 0)
                            {
                                recordCount = totalRecordCount > 1000 ? 1000 : totalRecordCount;
                                List<ExpandoObject> returnData = client.GetData(mockClass, recordCount);
                                w.ReportProgress(percDone, "Got Data, Generating Entity Collection");
                                foreach (List<ExpandoObject> subList in SplitList(returnData))
                                {
                                    entityCollection = CreateEntityCollection(subList);
                                    w.ReportProgress(percDone, "Retrieved Mockaroo Data, Creating in Dataverse");
                                    percDone += 100 / noRuns;
                                    errors += CreateData(entityCollection, w);

                                    totalRecordCount -= subList.Count();
                                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(
                                      (int)Math.Round((double)(initialCount - totalRecordCount) * 100 / initialCount),
                                        totalRecordCount + " Records remaining"));
                                    w.ReportProgress(percDone, "Created Data");
                                }



                            }

                            e.Result = errors;
                        },
                    ProgressChanged = e => SetWorkingMessage(e.UserState.ToString()),
                    PostWorkCallBack =
                        e =>
                        {
                            if (e.Error != null)
                            {
                                LogError(e.Error.ToString());
                                MessageBox.Show(
                                    e.Error.Message.ToString() + Environment.NewLine + "Some Data may have been created, please confirm before re-running",
                                    "Error generating data",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
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
                                gridSample.DataSource = collection.Entities;
                            }
                        }
                });
        }

        private List<List<ExpandoObject>> SplitList(List<ExpandoObject> returnData)
        {
            var list = new List<List<ExpandoObject>>();

            for (int i = 0; i < returnData.Count; i += 500)
                list.Add(returnData.GetRange(i, Math.Min(500, returnData.Count - i)));

            return list;
        }

        private string CreateData(EntityCollection entityCollection, BackgroundWorker worker)
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
                errors = responseWithResults.Responses.Where(responseItem => responseItem.Fault != null)
                                                .Aggregate(errors, (accumulator, responseItem) => accumulator += "\r\n" +
                                                    responseItem.RequestIndex + " | " + responseItem.Fault);

                // else collection.Entities[responseItem.RequestIndex].Id = ((CreateResponse) responseItem.Response).id;
                //DisplayFault(requestWithResults.Requests[responseItem.RequestIndex],
                //    responseItem.RequestIndex, responseItem.Fault);
            }

            foreach (var updateEntity in updateEntities)
                errors += SendInactiveRequest(updateEntity, worker);

            return errors;
            //  e.Result = errors;
        }
    }
}
