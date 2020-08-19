using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Label = System.Windows.Forms.Label;

namespace LinkeD365.MockDataGen
{
    internal partial class ConfigEdit : Form
    {
        public MapRow mapRow;
        public IOrganizationService Service;

        public ConfigEdit(MapRow row, IOrganizationService service)
        {
            mapRow = row;
            InitializeComponent();
            Service = service;
        }

        private void ConfigEdit_Load(object sender, EventArgs e)
        {
            switch (mapRow.SelectedMock.Name)
            {
                case DataTypes.Paragraphs:
                    this.Text = "Enter range of paragraphs";
                    table.Controls.Add(new Label() { Text = "Min" }, 0, 0);
                    var minPara = new NumericUpDown() { Minimum = 1, Maximum = 10 };
                    minPara.Value = ((Paragraph)mapRow.SelectedMock).Min;
                    minPara.ValueChanged += Number_ValueChanged;
                    minPara.Tag = "ParaMin";
                    table.Controls.Add(minPara, 1, 0);
                    var maxPara = new NumericUpDown() { Minimum = 1, Maximum = 10 };
                    maxPara.Value = ((Paragraph)mapRow.SelectedMock).Max;
                    maxPara.ValueChanged += Number_ValueChanged;
                    maxPara.Tag = "ParaMax";
                    table.Controls.Add(maxPara, 1, 1);
                    table.Controls.Add(new Label() { Text = "Max" }, 0, 1);

                    break;

                case DataTypes.Custom.RandomLookup:
                case DataTypes.Custom.FixedLookup:
                case DataTypes.Custom.FixedTeam:
                case DataTypes.Custom.FixedUser:
                case DataTypes.Custom.RandomTeam:
                case DataTypes.Custom.RandomUser:
                    //table.Controls.Add(new Label() { Text = "Only" });
                    LookupAttributeMetadata lookup = (LookupAttributeMetadata)mapRow.Attribute;
                    string entity = lookup.Targets[0];
                    var entityMeta = Service.GetEntityMetadata(entity);

                    var selection = new ListView() { Tag = "RandomSelection" };

                    selection.MultiSelect = mapRow.SelectedMock is RandomLookup;
                    this.Text = selection.MultiSelect ? "Choose one or more values to select from" : "Choose the constant value";
                    selection.View = View.Details;

                    selection.Columns.Add(entityMeta.DisplayCollectionName.UserLocalizedLabel.Label.ToString());

                    selection.Items.AddRange(GetLookupItems(mapRow.SelectedMock));
                    selection.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    selection.Dock = DockStyle.Fill;
                    table.Controls.Add(selection, 0, 0);
                    table.SetColumnSpan(selection, 2);
                    selection.MinimumSize = new Size(100, 300);
                    break;

                case DataTypes.Custom.RandomPicklist:
                case DataTypes.Custom.FixedPicklist:

                    //var pickListOptions = mapRow.Attribute is PicklistAttributeMetadata ? ((PicklistAttributeMetadata)mapRow.Attribute).OptionSet.Options : ((StatusAttributeMetadata)mapRow.Attribute).OptionSet.Options;
                    var selectionPick = new ListView() { Tag = "RandomSelection" };

                    selectionPick.MultiSelect = mapRow.SelectedMock.Name == DataTypes.Custom.RandomPicklist;
                    this.Text = selectionPick.MultiSelect ? "Choose one or more values to select from" : "Choose the constant value";
                    selectionPick.View = View.Details;

                    selectionPick.Columns.Add(mapRow.Attribute.DisplayName.UserLocalizedLabel.Label);

                    var pickListOptions = mapRow.SelectedMock is RandomPickList ? ((RandomPickList)mapRow.SelectedMock).AllValues : ((FixedPickList)mapRow.SelectedMock).AllValues;

                    selectionPick.Items.AddRange((from item in pickListOptions
                                                  select new ListViewItem(item.Name) { Tag = item.choiceNo }).ToArray());

                    selectionPick.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    selectionPick.Dock = DockStyle.Fill;
                    table.Controls.Add(selectionPick, 0, 0);
                    table.SetColumnSpan(selectionPick, 2);
                    selectionPick.MinimumSize = new Size(100, 300);
                    break;

                case DataTypes.Number:
                    SetUpNumber();

                    break;

                case DataTypes.BinomialDistribution:
                    table.Controls.Add(new Label() { Text = "Success Probability" }, 0, 0);
                    var probability = new NumericUpDown() { Minimum = 0, Maximum = 1, DecimalPlaces = 3, Tag = "biProbability" };
                    var biMock = (BinomialDistribution)mapRow.SelectedMock;
                    probability.Value = (decimal)biMock.Probability;
                    probability.ValueChanged += Number_ValueChanged;
                    table.Controls.Add(probability, 1, 0);
                    table.RowCount++;
                    break;

                case DataTypes.NormalDistribution:
                    SetupNormalDist();

                    break;

                case DataTypes.Custom.DateTime:
                case DataTypes.Custom.Date:
                case DataTypes.Time:
                    this.Text = "Enter date range";
                    var minDateTime = new DateTimePicker() { Tag = "minDateTime" };

                    var maxDateTime = new DateTimePicker() { Tag = "maxDateTime" };

                    switch (mapRow.SelectedMock)
                    {
                        case Time timeMock:
                            maxDateTime.Format = DateTimePickerFormat.Time;
                            maxDateTime.Value = DateTime.Now.Date + timeMock.Max;
                            minDateTime.Format = DateTimePickerFormat.Time;
                            minDateTime.Value = DateTime.Now.Date + timeMock.Min;
                            break;

                        case DT dateTimeMock:
                            maxDateTime.Format = DateTimePickerFormat.Custom;
                            maxDateTime.Value = dateTimeMock.Max;
                            minDateTime.Format = DateTimePickerFormat.Custom;
                            minDateTime.Value = dateTimeMock.Min;

                            minDateTime.CustomFormat = "dd/MM/yyyy HH:mm:ss";
                            maxDateTime.CustomFormat = "dd/MM/yyyy HH:mm:ss";
                            break;

                        case Date dateMock:
                            maxDateTime.Format = DateTimePickerFormat.Long;
                            maxDateTime.Value = dateMock.Max;
                            minDateTime.Format = DateTimePickerFormat.Long;
                            minDateTime.Value = dateMock.Min;
                            break;
                    }

                    minDateTime.ValueChanged += DateTime_ValueChanged;
                    maxDateTime.ValueChanged += DateTime_ValueChanged;

                    table.Controls.Add(new Label() { Text = "Min", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

                    table.Controls.Add(minDateTime, 1, 0);

                    table.Controls.Add(new Label() { Text = "Max", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);

                    table.Controls.Add(maxDateTime, 1, 1);
                    table.RowCount++;
                    break;

                case DataTypes.Custom.FixedNumber:

                    SetupFixedNumber();
                    break;

                case DataTypes.Custom.FixedDate:
                case DataTypes.Custom.FixedTime:
                case DataTypes.Custom.FixedDateTime:
                    SetupFixedDT();
                    break;

                case DataTypes.CustomList:
                    this.Text = "Enter list, one per line";
                    var textBox = new TextBox() { Tag = "textBox", Anchor = AnchorStyles.Left | AnchorStyles.Right };
                    textBox.Text = string.Join("\n", ((CustomList)mapRow.SelectedMock).Values);
                    textBox.Multiline = true;
                    textBox.Height = 200;
                    textBox.Width = 150;
                    table.Controls.Add(textBox);
                    table.SetColumnSpan(textBox, 2);
                    table.SetRowSpan(textBox, 2);
                    table.RowCount++;
                    break;

                default:
                    break;
            }

            var btn = new Button() { Text = "Ok" };
            btn.DialogResult = DialogResult.OK;
            btn.Click += btnOk_Click;
            btn.Anchor = AnchorStyles.Bottom;
            table.RowCount++;
            table.Controls.Add(btn, 0, table.RowCount - 1);
            table.SetColumnSpan(btn, 2);
        }

        private void SetupFixedDT()
        {
            this.Text = "Enter value";
            var fixedDate = new DateTimePicker() { Tag = "fixedDate" };
            var dateAttr = mapRow.Attribute as DateTimeAttributeMetadata;
            switch (mapRow.SelectedMock)
            {
                case FixedTime timeMock:
                    TimeSpan timeVal = new TimeSpan(12, 0, 0);
                    if (TimeSpan.TryParse(timeMock.FixedValue.ToString(), out timeVal)) fixedDate.Value = DateTime.Now.Date + timeVal;
                    fixedDate.Format = DateTimePickerFormat.Time;
                    break;

                case FixedDateTime dateTimeMock:
                    DateTime dateTimeVal = DateTime.Now;
                    if (DateTime.TryParse(dateTimeMock.FixedValue.ToString(), out dateTimeVal)) fixedDate.Value = dateTimeVal;
                    fixedDate.Format = DateTimePickerFormat.Custom;

                    fixedDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
                    break;

                case FixedDate dateMock:
                    DateTime dateVal = DateTime.Now;
                    if (DateTime.TryParse(dateMock.FixedValue.ToString(), out dateVal)) fixedDate.Value = dateVal;
                    fixedDate.Format = DateTimePickerFormat.Long;

                    break;
            }

            fixedDate.ValueChanged += DateTime_ValueChanged;

            table.Controls.Add(new Label() { Text = "Value", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

            table.Controls.Add(fixedDate, 1, 0);

            table.RowCount++;
        }

        private void SetupFixedNumber()
        {
            this.Text = "Enter numeric value";
            var fixedNumber = new NumericUpDown() { Tag = "fixedNumber" };
            switch (mapRow.Attribute)
            {
                case MoneyAttributeMetadata moneyAttr:
                    fixedNumber.Minimum = (decimal)moneyAttr.MinValue;
                    fixedNumber.Maximum = (decimal)moneyAttr.MaxValue;

                    fixedNumber.DecimalPlaces = moneyAttr.Precision.GetValueOrDefault();

                    break;

                case DecimalAttributeMetadata decAttr:
                    fixedNumber.Minimum = (decimal)decAttr.MinValue;
                    fixedNumber.Maximum = (decimal)decAttr.MaxValue;
                    fixedNumber.DecimalPlaces = decAttr.Precision.GetValueOrDefault();

                    break;

                case DoubleAttributeMetadata doubleAttr:
                    fixedNumber.Minimum = (decimal)doubleAttr.MinValue;
                    fixedNumber.Maximum = (decimal)doubleAttr.MaxValue;
                    fixedNumber.DecimalPlaces = doubleAttr.Precision.GetValueOrDefault();

                    break;

                case IntegerAttributeMetadata intAttr:
                    fixedNumber.Minimum = (decimal)intAttr.MinValue;
                    fixedNumber.Maximum = (decimal)intAttr.MaxValue;
                    fixedNumber.DecimalPlaces = 0;

                    break;

                case BigIntAttributeMetadata bigIntAttr:
                    fixedNumber.Minimum = (decimal)bigIntAttr.MinValue;
                    fixedNumber.Maximum = (decimal)bigIntAttr.MaxValue;
                    fixedNumber.DecimalPlaces = 0;
                    break;
            }
            decimal decValue = 0;
            if (Decimal.TryParse(((FixedNumber)mapRow.SelectedMock).FixedValue.ToString(), out decValue)) fixedNumber.Value = decValue;
            else fixedNumber.Value = 0;

            fixedNumber.ValueChanged += Number_ValueChanged;

            table.Controls.Add(new Label() { Text = "Value", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

            table.Controls.Add(fixedNumber, 1, 0);
            table.RowCount++;
        }

        private void SetupNormalDist()
        {
            var meanNumber = new NumericUpDown() { Tag = "meanNumber" };
            var standardNumber = new NumericUpDown() { Tag = "standardNumber" };
            var dpNormal = new NumericUpDown() { Tag = "dpNormal", DecimalPlaces = 0 };
            var mockNormal = (NormalDistribution)mapRow.SelectedMock;
            switch (mapRow.Attribute)
            {
                case MoneyAttributeMetadata moneyAttr:
                    meanNumber.Minimum = (decimal)moneyAttr.MinValue;
                    meanNumber.Maximum = (decimal)moneyAttr.MaxValue;

                    meanNumber.DecimalPlaces = moneyAttr.Precision.GetValueOrDefault();

                    standardNumber.Minimum = (decimal)moneyAttr.MinValue;
                    standardNumber.Maximum = (decimal)moneyAttr.MaxValue;
                    standardNumber.DecimalPlaces = moneyAttr.Precision.GetValueOrDefault();

                    dpNormal.Maximum = moneyAttr.Precision.GetValueOrDefault();
                    break;

                case DecimalAttributeMetadata decAttr:
                    meanNumber.Minimum = (decimal)decAttr.MinValue;
                    meanNumber.Maximum = (decimal)decAttr.MaxValue;
                    meanNumber.DecimalPlaces = decAttr.Precision.GetValueOrDefault();

                    standardNumber.Minimum = (decimal)decAttr.MinValue;
                    standardNumber.Maximum = (decimal)decAttr.MaxValue;
                    standardNumber.DecimalPlaces = decAttr.Precision.GetValueOrDefault();

                    dpNormal.Maximum = decAttr.Precision.GetValueOrDefault();
                    break;

                case DoubleAttributeMetadata doubleAttr:
                    meanNumber.Minimum = (decimal)doubleAttr.MinValue;
                    meanNumber.Maximum = (decimal)doubleAttr.MaxValue;
                    meanNumber.DecimalPlaces = doubleAttr.Precision.GetValueOrDefault();

                    standardNumber.Minimum = (decimal)doubleAttr.MinValue;
                    standardNumber.Maximum = (decimal)doubleAttr.MaxValue;
                    standardNumber.DecimalPlaces = doubleAttr.Precision.GetValueOrDefault();

                    dpNormal.Maximum = doubleAttr.Precision.GetValueOrDefault();
                    break;

                case IntegerAttributeMetadata intAttr:
                    meanNumber.Minimum = (decimal)intAttr.MinValue;
                    meanNumber.Maximum = (decimal)intAttr.MaxValue;
                    meanNumber.DecimalPlaces = 0;

                    standardNumber.Minimum = (decimal)intAttr.MinValue;
                    standardNumber.Maximum = (decimal)intAttr.MaxValue;
                    standardNumber.DecimalPlaces = 0;

                    dpNormal.Maximum = 0;
                    break;

                case BigIntAttributeMetadata bigIntAttr:
                    meanNumber.Minimum = (decimal)bigIntAttr.MinValue;
                    meanNumber.Maximum = (decimal)bigIntAttr.MaxValue;
                    meanNumber.DecimalPlaces = 0;

                    standardNumber.Minimum = (decimal)bigIntAttr.MinValue;
                    standardNumber.Maximum = (decimal)bigIntAttr.MaxValue;
                    standardNumber.DecimalPlaces = 0;

                    dpNormal.Maximum = 0;
                    break;
            }
            dpNormal.Minimum = 0;
            meanNumber.Value = (decimal)mockNormal.Mean;
            standardNumber.Value = (decimal)mockNormal.StandardDev;

            dpNormal.Value = mockNormal.Decimals;

            meanNumber.ValueChanged += Number_ValueChanged;
            standardNumber.ValueChanged += Number_ValueChanged;
            dpNormal.ValueChanged += Number_ValueChanged;

            table.Controls.Add(new Label() { Text = "Mean", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

            table.Controls.Add(meanNumber, 1, 0);
            table.RowCount++;

            table.Controls.Add(new Label() { Text = "Standard Deviation", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);

            table.Controls.Add(standardNumber, 1, 1);
            table.RowCount++;
            table.Controls.Add(new Label() { Text = "Dec. Places", Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);

            table.Controls.Add(dpNormal, 1, 2);
        }

        private void SetUpNumber()
        {
            var mockNumber = (Number)mapRow.SelectedMock;
            var minNumber = new NumericUpDown() { Tag = "minNumber" };

            var maxNumber = new NumericUpDown() { Tag = "maxNumber" };

            var dp = new NumericUpDown() { Tag = "dp", Value = mockNumber.Decimals };

            switch (mapRow.Attribute)
            {
                case MoneyAttributeMetadata moneyAttr:
                    minNumber.Minimum = (decimal)moneyAttr.MinValue;
                    minNumber.Maximum = (decimal)moneyAttr.MaxValue;

                    minNumber.DecimalPlaces = moneyAttr.Precision.GetValueOrDefault();

                    maxNumber.Minimum = (decimal)moneyAttr.MinValue;
                    maxNumber.Maximum = (decimal)moneyAttr.MaxValue;
                    maxNumber.DecimalPlaces = moneyAttr.Precision.GetValueOrDefault();

                    dp.Maximum = moneyAttr.Precision.GetValueOrDefault();
                    break;

                case DecimalAttributeMetadata decAttr:
                    minNumber.Minimum = (decimal)decAttr.MinValue;
                    minNumber.Maximum = (decimal)decAttr.MaxValue;
                    minNumber.DecimalPlaces = decAttr.Precision.GetValueOrDefault();

                    maxNumber.Minimum = (decimal)decAttr.MinValue;
                    maxNumber.Maximum = (decimal)decAttr.MaxValue;
                    maxNumber.DecimalPlaces = decAttr.Precision.GetValueOrDefault();

                    dp.Maximum = decAttr.Precision.GetValueOrDefault();
                    break;

                case DoubleAttributeMetadata doubleAttr:
                    minNumber.Minimum = (decimal)doubleAttr.MinValue;
                    minNumber.Maximum = (decimal)doubleAttr.MaxValue;
                    minNumber.DecimalPlaces = doubleAttr.Precision.GetValueOrDefault();

                    maxNumber.Minimum = (decimal)doubleAttr.MinValue;
                    maxNumber.Maximum = (decimal)doubleAttr.MaxValue;
                    maxNumber.DecimalPlaces = doubleAttr.Precision.GetValueOrDefault();

                    dp.Maximum = doubleAttr.Precision.GetValueOrDefault();
                    break;

                case IntegerAttributeMetadata intAttr:
                    minNumber.Minimum = (decimal)intAttr.MinValue;
                    minNumber.Maximum = (decimal)intAttr.MaxValue;
                    minNumber.DecimalPlaces = 0;

                    maxNumber.Minimum = (decimal)intAttr.MinValue;
                    maxNumber.Maximum = (decimal)intAttr.MaxValue;
                    maxNumber.DecimalPlaces = 0;

                    dp.Maximum = 0;
                    break;

                case BigIntAttributeMetadata bigIntAttr:
                    minNumber.Minimum = (decimal)bigIntAttr.MinValue;
                    minNumber.Maximum = (decimal)bigIntAttr.MaxValue;
                    minNumber.DecimalPlaces = 0;

                    maxNumber.Minimum = (decimal)bigIntAttr.MinValue;
                    maxNumber.Maximum = (decimal)bigIntAttr.MaxValue;
                    maxNumber.DecimalPlaces = 0;

                    dp.Maximum = 0;
                    break;
            }

            dp.Minimum = 0;
            minNumber.Value = mockNumber.Min;
            maxNumber.Value = mockNumber.Max;

            dp.DecimalPlaces = 0;

            maxNumber.ValueChanged += Number_ValueChanged;
            minNumber.ValueChanged += Number_ValueChanged;
            dp.ValueChanged += Number_ValueChanged;

            table.Controls.Add(new Label() { Text = "Min", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);

            table.Controls.Add(minNumber, 1, 0);
            table.RowCount++;

            table.Controls.Add(new Label() { Text = "Max", Anchor = AnchorStyles.Left | AnchorStyles.Bottom, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);

            table.Controls.Add(maxNumber, 1, 1);
            table.RowCount++;
            table.Controls.Add(new Label() { Text = "Dec. Places", Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.MiddleLeft }, 0, 2);

            table.Controls.Add(dp, 1, 2);
        }

        private ListViewItem[] GetLookupItems(BaseMock lookupMock)
        {
            List<Lookup> lookups = lookupMock is RandomLookup ? ((RandomLookup)lookupMock).AllValues : ((FixedLookup)lookupMock).AllValues;
            var listviewitems = new List<ListViewItem>();
            listviewitems.AddRange(from lookup in lookups select new ListViewItem(lookup.Name) { Tag = lookup.guid });

            return listviewitems.ToArray();
        }

        private void DateTime_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dateTime = sender as DateTimePicker;
            switch (mapRow.SelectedMock)
            {
                case Time timeMock:
                    if (dateTime.Tag.ToString() == "minDateTime") timeMock.Min = dateTime.Value.TimeOfDay;
                    else timeMock.Max = dateTime.Value.TimeOfDay;
                    break;

                case DT dateTimeMock:

                    if (dateTime.Tag.ToString() == "minDateTime") dateTimeMock.Min = dateTime.Value;
                    else dateTimeMock.Max = dateTime.Value;
                    break;

                case Date dateMock:
                    if (dateTime.Tag.ToString() == "minDateTime") dateMock.Min = dateTime.Value;
                    else dateMock.Max = dateTime.Value;
                    break;

                case FixedDate fixedDateMock:
                    fixedDateMock.FixedValue = dateTime.Value.Date;
                    break;

                case FixedTime fixedTimeMock:
                    fixedTimeMock.FixedValue = dateTime.Value.TimeOfDay;
                    break;

                case FixedDateTime fixedDateTimeMock:
                    fixedDateTimeMock.FixedValue = dateTime.Value;
                    break;
            }
        }

        private void Number_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown number = sender as NumericUpDown;
            switch (number.Tag)
            {
                case "ParaMin":
                    ((Paragraph)mapRow.SelectedMock).Min = (int)number.Value;
                    break;

                case "ParaMax":
                    ((Paragraph)mapRow.SelectedMock).Max = (int)number.Value;
                    break;

                case "minNumber":
                    ((Number)mapRow.SelectedMock).Min = number.Value;
                    break;

                case "maxNumber":
                    ((Number)mapRow.SelectedMock).Max = number.Value;
                    break;

                case "dp":
                    ((Number)mapRow.SelectedMock).Decimals = (int)number.Value;
                    ((NumericUpDown)table.Controls.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, "minNumber"))).DecimalPlaces = (int)number.Value;
                    ((NumericUpDown)table.Controls.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, "maxNumber"))).DecimalPlaces = (int)number.Value;
                    break;

                case "biProbability":
                    ((BinomialDistribution)mapRow.SelectedMock).Probability = (double)number.Value;

                    break;

                case "meanNumber":
                    ((NormalDistribution)mapRow.SelectedMock).Mean = (double)number.Value;
                    break;

                case "standardNumber":
                    ((NormalDistribution)mapRow.SelectedMock).StandardDev = (double)number.Value;
                    break;

                case "dpNormal":
                    ((NormalDistribution)mapRow.SelectedMock).Decimals = (int)number.Value;
                    ((NumericUpDown)table.Controls.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, "meanNumber"))).DecimalPlaces = (int)number.Value;
                    ((NumericUpDown)table.Controls.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, "standardNumber"))).DecimalPlaces = (int)number.Value;
                    break;

                case "fixedNumber":
                    ((FixedNumber)mapRow.SelectedMock).FixedValue = number.Value.ToString();
                    break;

                default:
                    break;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            switch (mapRow.SelectedMock.Name)
            {
                case DataTypes.Custom.RandomLookup:
                case DataTypes.Custom.FixedLookup:
                case DataTypes.Custom.FixedTeam:
                case DataTypes.Custom.FixedUser:
                case DataTypes.Custom.RandomTeam:
                case DataTypes.Custom.RandomUser:
                    var selectLookup = (ListView)table.Controls.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, "RandomSelection"));
                    if (mapRow.SelectedMock is RandomLookup) ((RandomLookup)mapRow.SelectedMock).Values = (selectLookup.SelectedItems.Cast<ListViewItem>().Select(listViewItem => new Lookup() { guid = (Guid)listViewItem.Tag, Name = listViewItem.Text })).ToList();
                    else ((FixedLookup)mapRow.SelectedMock).FixedValue = selectLookup.SelectedItems.Cast<ListViewItem>().Select(lvi => new Lookup() { guid = (Guid)lvi.Tag, Name = lvi.Text }).First();

                    break;

                case DataTypes.Custom.RandomPicklist:
                case DataTypes.Custom.FixedPicklist:
                    var selectPickList = (ListView)table.Controls.Cast<Control>().FirstOrDefault(control => String.Equals(control.Tag, "RandomSelection"));
                    if (mapRow.SelectedMock.Name == DataTypes.Custom.RandomPicklist) ((RandomPickList)mapRow.SelectedMock).Values = (selectPickList.SelectedItems.Cast<ListViewItem>().Select(listViewItem => new PickList() { choiceNo = (int)listViewItem.Tag, Name = listViewItem.Text })).ToList();
                    else ((FixedPickList)mapRow.SelectedMock).FixedValue = selectPickList.SelectedItems.Cast<ListViewItem>().Select(lvi => new PickList() { choiceNo = (int)lvi.Tag, Name = lvi.Text }).First();
                    break;

                case DataTypes.CustomList:
                    var customList = (TextBox)table.Controls.Cast<Control>().FirstOrDefault();

                    if (customList.TextLength > 0) ((CustomList)mapRow.SelectedMock).Values = customList.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    break;

                default:
                    break;
            }
        }
    }
}