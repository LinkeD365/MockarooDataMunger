using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LinkeD365.MockDataGen.Mock
{
    internal class MockClass : DynamicObject
    {
        private readonly Dictionary<string, object> _dynamicProperties = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dynamicProperties.Add(binder.Name, value);

            // additional error checking code omitted

            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dynamicProperties.TryGetValue(binder.Name, out result);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var property in _dynamicProperties)
                sb.AppendLine($"Property '{property.Key}' = '{property.Value}'");

            return sb.ToString();
        }
    }

    public abstract class BaseMock
    {
        public string EntityName { get; set; }
        protected Dictionary<string, object> _field { get; set; } = new Dictionary<string, object>();
        public string MockType { get; set; }

        public bool Fixed { get; set; }

        public bool Mockaroo { get; set; } = true;

        public object FixedValue = string.Empty;

        public BaseMock()
        {
        }

        public BaseMock(string mockType)
        {
            MockType = mockType;
            Name = mockType;
        }

        public BaseMock(string mockType, string mockName)
        {
            MockType = mockType;
            Name = mockName;
        }

        public string Name { get; set; }

        [DefaultValue(0)]
        public int PercentBlank { get; set; } = 0;

        public string Formula { get; set; }

        public abstract string Properties { get; }

        public abstract Dictionary<string, object> GetField();

        public void ClearField()
        {
            _field = new Dictionary<string, object>();
        }

        public BaseMock Clone()
        {
            var newMock = (BaseMock)this.MemberwiseClone();
            newMock.ClearField();
            return newMock;
        }

        protected Dictionary<string, object> baseField
        {
            get
            {
                if (!_field.ContainsKey("Type"))
                    _field.Add("Type", MockType);
                else
                    _field["Type"] = MockType;

                if (!_field.ContainsKey("MockName"))
                    _field.Add("MockName", Name);
                else
                    _field["MockName"] = Name;

                if (!_field.ContainsKey("PercentBlank"))
                    _field.Add("PercentBlank", PercentBlank);
                else
                    _field["PercentBlank"] = PercentBlank;

                if (!_field.ContainsKey("Formula"))
                    if (!string.IsNullOrEmpty(Formula))
                        _field.Add("Formula", Formula);
                    else if (!string.IsNullOrEmpty(Formula))
                        _field["Formula"] = Formula;

                if (!_field.ContainsKey("FixedValue"))
                    _field.Add("FixedValue", FixedValue);
                else
                    _field["FixedValue"] = FixedValue;

                //if (!string.IsNullOrEmpty(Formula)) _field.Add("Formula", Formula);
                return _field;
            }
        }

        protected void AddToBase(string Key, object Value)
        {
            if (baseField.ContainsKey(Key))
                baseField[Key] = Value;
            else
                baseField.Add(Key, Value);
        }

        public abstract void PopulateFromKVP(List<KVP> kvps);

        protected void BasePopulateFromKVP(List<KVP> kvps)
        {
            MockType = kvps.First(kvp => kvp.Key == "Type").Value.ToString();
            Name = kvps.First(kvp => kvp.Key == "MockName").Value.ToString();
            PercentBlank = (int)kvps.First(kvp => kvp.Key == "PercentBlank").Value;
            FixedValue = kvps.First(kvp => kvp.Key == "FixedValue").Value;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class StringMock : BaseMock
    {
        public StringMock()
        {
        }

        public StringMock(string datatype) : base(datatype)
        { }

        public override string Properties { get; } = string.Empty;

        public override Dictionary<string, object> GetField()
        {
            //var field = new Dictionary<string, object>();
            //field.Add("Type", MockType);
            //if (PercentBlank != 0) field.Add("PercentBlank", PercentBlank);
            //if (!string.IsNullOrEmpty(Formula)) field.Add("Formula", Formula);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    //[MockarooInfo(Type = DataTypes.FirstName)]
    public class FirstName : StringMock
    {
        public FirstName() : base(DataTypes.FirstName)
        {
        }
    }

    public class AppName : StringMock
    {
        public AppName() : base(DataTypes.AppName)
        { }
    }

    public class FakeEmailMock : StringMock
    {
        public FakeEmailMock() : base(DataTypes.EmailAddress)
        {
            Name = "Fake Email";
        }
    }

    public class CarModelYearMock : StringMock
    {
        public CarModelYearMock() : base(DataTypes.CarModelYear)
        { }
    }

    public class Paragraph : BaseMock
    {
        public Paragraph() : base(DataTypes.Paragraphs)
        { }

        [DefaultValue(1)]
        public int Min { get; set; } = 1;

        [DefaultValue(3)]
        public int Max { get; set; } = 3;

        public override Dictionary<string, object> GetField()
        {
            AddToBase("min", Min);
            AddToBase("max", Max);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Min = (int)kvps.First(kvp => kvp.Key == "min").Value;
            Max = (int)kvps.First(kvp => kvp.Key == "max").Value;
        }

        public override string Properties => "Min: " + Min.ToString() + "\nMax: " + Max.ToString();
    }

    public class Avatar : BaseMock
    {
        public Avatar() : base(DataTypes.Avatar)
        {
        }

        [DefaultValue(60)]
        public int Height { get; set; } = 60;

        [DefaultValue(60)]
        public int Width { get; set; } = 60;

        public override string Properties => "Width: " + Width.ToString() + "\nHeight: " + Height.ToString();

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Height", Height);
            AddToBase("Width", Width);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Height = (int)kvps.First(kvp => kvp.Key == "Height").Value;
            Width = (int)kvps.First(kvp => kvp.Key == "Width").Value;
        }
    }

    public class Country : BaseMock
    {
        public Country() : base(DataTypes.Country)
        {
        }

        public bool All { get; set; } = true;

        /// <summary>
        /// Restricted to this list or all if blank
        /// </summary>
        public string[] Countries { get; set; } = new string[] { };

        public override string Properties
        {
            get
            {
                if (All)
                    return "All Countries";

                return "Countries: " + string.Join("\n", Countries);
            }
        }

        public override Dictionary<string, object> GetField()
        {
            AddToBase("countries", Countries);

            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Countries = ((string)kvps.First(kvp => kvp.Key == "countries").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public class CustomList : BaseMock

    {
        public CustomList() : base(DataTypes.CustomList)
        {
        }

        public string[] Values { get; set; } = new string[] { };

        [DefaultValue("random")]
        public string SelectionStyle { get; set; } = "random";

        public override string Properties
        {
            get
            {
                if (Values == null || Values.Count() == 0)
                    return "Selection Style: " + SelectionStyle;

                return "Selection Style: " + SelectionStyle + "\nCountries: " + string.Join("\n", Values);
            }
        }

        public override Dictionary<string, object> GetField()
        {
            AddToBase("values", Values);
            AddToBase("selectionstyle", SelectionStyle);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Values = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            SelectionStyle = kvps.First(kvp => kvp.Key == "selectionstyle").Value.ToString();
        }
    }

    public class Date : BaseMock
    {
        public DateTime Min { get; set; } = DateTime.Now.AddYears(-1).Date;
        public DateTime Max { get; set; } = DateTime.Now.AddYears(1).Date;

        public Date() : base(DataTypes.DateTime, DataTypes.Custom.Date)
        { }

        public override string Properties => "Min: " + Min.ToString("d") + "\nMax: " + Max.ToString("d");

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Min", Min.ToString("MM/dd/yyyy"));
            AddToBase("Max", Max.ToString("MM/dd/yyyy"));
            AddToBase("format", "%Y-%m-%d");
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            DateTime min;
            if (!DateTime.TryParseExact(kvps.First(kvp => kvp.Key == "Min").Value.ToString(), "MM/dd/yyyy", null, DateTimeStyles.AdjustToUniversal, out min))
                Min = DateTime.Parse(kvps.First(kvp => kvp.Key == "Min").Value.ToString());
            else Min = min;
            DateTime max;
            if (!DateTime.TryParseExact(kvps.First(kvp => kvp.Key == "Max").Value.ToString(), "MM/dd/yyyy", null, DateTimeStyles.AdjustToUniversal, out max))
                Max = DateTime.Parse(kvps.First(kvp => kvp.Key == "Max").Value.ToString());
            else Max = max;
        }
    }

    public class Time : BaseMock
    {
        public TimeSpan Min { get; set; } = new TimeSpan(00, 0, 0);
        public TimeSpan Max { get; set; } = new TimeSpan(23, 59, 59);

        public Time() : base(DataTypes.Time)
        { }

        public override string Properties => "Min: " + Min.ToString() + "\nMax: " + Max.ToString();

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Min", DateTime.Today.Add(Min).ToString("hh:mm tt"));
            AddToBase("Max", DateTime.Today.Add(Max).ToString("hh:mm tt"));
            // AddToBase("format", "%H:%m-%d");
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);

            Min = DateTime.ParseExact(kvps.First(kvp => kvp.Key == "Min").Value.ToString(), "hh:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None).TimeOfDay;
            Max = DateTime.ParseExact(kvps.First(kvp => kvp.Key == "Max").Value.ToString(), "hh:mm tt", CultureInfo.CurrentCulture, DateTimeStyles.None).TimeOfDay;
        }
    }

    public class DT : BaseMock
    {
        public DateTime Min { get; set; } = DateTime.Now.AddYears(-1);
        public DateTime Max { get; set; } = DateTime.Now;

        public DT() : base(DataTypes.Custom.DateTime)
        {
        }

        public override string Properties => "Min: " + Min.ToString() + "\nMax: " + Max.ToString();

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Min", Min.ToString("MM/dd/yyyy hh:mm:ss"));
            AddToBase("Max", Max.ToString("MM/dd/yyyy hh:mm:ss"));
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            DateTime min;
            if (!DateTime.TryParseExact(kvps.First(kvp => kvp.Key == "Min").Value.ToString(), "MM/dd/yyyy hh:mm:ss", null, DateTimeStyles.AdjustToUniversal, out min))
                Min = DateTime.Parse(kvps.First(kvp => kvp.Key == "Min").Value.ToString());
            else Min = min;
            DateTime max;
            if (!DateTime.TryParseExact(kvps.First(kvp => kvp.Key == "Max").Value.ToString(), "MM/dd/yyyy hh:mm:ss", null, DateTimeStyles.AdjustToUniversal, out max))
                Max = DateTime.Parse(kvps.First(kvp => kvp.Key == "Max").Value.ToString());
            else Max = max;
        }
    }

    public class True : BaseMock
    {
        public True() : base("True")
        {
            Fixed = true;
            Mockaroo = false;
            FixedValue = true;
        }

        public override string Properties { get; } = string.Empty;

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class False : BaseMock
    {
        public False() : base("False")
        {
            Fixed = true;
            Mockaroo = false;
            FixedValue = false;
        }

        public override string Properties { get; } = string.Empty;

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class FixedNumber : BaseMock
    {
        public FixedNumber() : base(DataTypes.Custom.FixedNumber)
        {
            Fixed = true;
            Mockaroo = false;
        }

        public override string Properties => "Value: " + FixedValue;

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class FixedDate : BaseMock
    {
        public FixedDate() : base(DataTypes.Custom.FixedDate)
        {
            Fixed = true;
            Mockaroo = false;
        }

        public override string Properties
        {
            get
            {
                if (FixedValue.ToString() != string.Empty)
                    return "Value: " + ((DateTime)FixedValue).ToString("d");

                return "Value:";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class FixedDateTime : BaseMock
    {
        public FixedDateTime() : base(DataTypes.Custom.FixedDateTime)
        {
            Fixed = true;
            Mockaroo = false;
        }

        public override string Properties => "Value: " + FixedValue;

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class FixedTime : BaseMock
    {
        public FixedTime() : base(DataTypes.Custom.FixedTime)
        {
            Fixed = true;
            Mockaroo = false;
        }

        public override string Properties => "Value: " + FixedValue;

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class FixedContact : FixedLookup
    {
        public FixedContact() : base(DataTypes.Custom.FixedContact)
        {
            EntityName = "contact";
        }
    }

    public class FixedAccount : FixedLookup
    {
        public FixedAccount() : base(DataTypes.Custom.FixedAccount)
        {
            EntityName = "account";
        }
    }

    public class FixedLookup : BaseMock
    {

        public FixedLookup() : base(DataTypes.Custom.FixedLookup)
        {
            Fixed = true;
            Mockaroo = false;
        }

        public FixedLookup(string mockType) : base(mockType)
        {
            Fixed = true;
            Mockaroo = false;
        }


        public List<Lookup> AllValues { get; set; } = new List<Lookup>();

        public override string Properties
        {
            get
            {
                if (FixedValue is Lookup)
                    return "Value: " + ((Lookup)FixedValue).Name;
                else
                    return "Value: ";
            }
        }

        public override Dictionary<string, object> GetField()
        { return baseField; }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            var lookup = FixedValue as Lookup;
            FixedValue = AllValues.FirstOrDefault(val => lookup.Name == val.Name);
        }
    }

    public class FixedTeam : FixedLookup
    {
        public FixedTeam() : base(DataTypes.Custom.FixedTeam)
        {
            EntityName = "team";
        }
    }

    public class FixedUser : FixedLookup
    {
        public FixedUser() : base(DataTypes.Custom.FixedUser)
        {
            EntityName = "systemuser";

        }
    }

    public class Lookup
    {
        public Guid guid { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class PickList
    {
        public int choiceNo { get; set; }
        public string Name { get; set; }

        public OptionSetValue Option => new OptionSetValue(choiceNo);

        public override string ToString()
        {
            return Name;
        }
    }

    public class FromSet : BaseMock
    {
        public string SelectionStyle { get; set; } = SelectionStyles.Random;
        public FromSet() : base(DataTypes.CustomList)
        {
            Name = DataTypes.Custom.FromSet;
        }

        public FromSet(string dataType) : base(DataTypes.CustomList)
        {
            Name = dataType;
        }
        public override string Properties { get; } = string.Empty;
        public override Dictionary<string, object> GetField()
        {
            AddToBase("values", Values.Select(lup => lup.ToString()).ToArray());

            AddToBase("selectionstyle", SelectionStyle);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            SelectionStyle = kvps.First(kvp => kvp.Key == "selectionstyle").Value.ToString();

            BasePopulateFromKVP(kvps);
        }

        public List<Lookup> Values { get; set; } = new List<Lookup>();
    }

    public class FromContactSet : FromSet
    {
        public FromContactSet() : base(DataTypes.Custom.FromContact)
        {
            EntityName = "contact";
        }
    }

    public class FromAccountSet : FromSet
    {
        public FromAccountSet() : base(DataTypes.Custom.FromAccount)
        {
            EntityName = "account";
        }
    }


    public class RandomLookup : BaseMock
    {

        public List<Lookup> Values { get; set; } = new List<Lookup>();

        public List<Lookup> AllValues { get; set; } = new List<Lookup>();

        [DefaultValue("random")]
        public string SelectionStyle { get; set; } = SelectionStyles.Random;

        public bool All => !Values.Any();

        public RandomLookup() : base(DataTypes.CustomList)
        {
            Name = DataTypes.Custom.RandomLookup;
        }

        public RandomLookup(string dataType) : base(DataTypes.CustomList)
        {
            Name = dataType;
        }

        public override string Properties
        {
            get
            {
                if (Values.Count() == 0)
                    return "Selection Style: " + SelectionStyle + "\n Values: All";

                return "Selection Style: " + SelectionStyle + "\n Values: \"" + string.Join("\",\"", Values.Select(lu => lu.Name)) + "\"";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            if (Values.Count == 0)
                AddToBase("values", AllValues.Select(lup => lup.ToString()).ToArray());
            else
                AddToBase("values", Values.Select(lup => lup.ToString()).ToArray());

            AddToBase("selectionstyle", SelectionStyle);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            SelectionStyle = kvps.First(kvp => kvp.Key == "selectionstyle").Value.ToString();
            string values = (string)kvps.FirstOrDefault(kvp => kvp.Key == "values")?.Value;
            if (values == null) return;
            var valueStrings = values.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            Values = AllValues.Where(val => valueStrings.Contains(val.Name)).ToList();
            // Values = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => new looku;
        }
    }

    public class RandomAccount : RandomLookup
    {
        public RandomAccount() : base(DataTypes.Custom.RandomAccount)
        {
            EntityName = "account";
        }
    }
    public class RandomContact : RandomLookup
    {
        public RandomContact() : base(DataTypes.Custom.RandomContact)
        {
            EntityName = "contact";
        }
    }
    public class RandomTeam : RandomLookup
    {
        public RandomTeam() : base(DataTypes.Custom.RandomTeam)
        {
            EntityName = "team";
        }
    }

    public class RandomUser : RandomLookup
    {
        public RandomUser() : base(DataTypes.Custom.RandomUser)
        {
            EntityName = "systemuser";
        }
    }

    public class FixedStatus : FixedPickList
    {
        public FixedStatus()
        {
            Name = DataTypes.Custom.FixedStatus;
        }
    }

    public class FixedPickList : BaseMock
    {
        public List<PickList> AllValues { get; set; } = new List<PickList>();

        public FixedPickList() : base(DataTypes.Custom.FixedPicklist)
        {
            Fixed = true;
            Mockaroo = false;
            Name = DataTypes.Custom.FixedPicklist;
        }

        public override string Properties
        {
            get
            {
                if (FixedValue is PickList)
                    return "Value: " + ((PickList)FixedValue).Name;
                else
                    return "Value: ";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            var lookup = FixedValue as PickList;
            FixedValue = AllValues.FirstOrDefault(val => lookup.Name == val.Name);
            // Values = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            // SelectionStyle = kvps.First(kvp => kvp.Key == "selectionstyle").Value.ToString();
        }
    }

    public class Boolean : BaseMock
    { 
        public Boolean() : base(DataTypes.Boolean)
        {
            Name = DataTypes.Boolean;
        }
        public List<PickList> AllValues { get; set; } = new List<PickList>() 
        { new PickList { Name = "True", choiceNo = 1 }, new PickList { Name = "False", choiceNo = 0 } };

        [DefaultValue("random")]
        public string SelectionStyle { get; set; } = SelectionStyles.Random;

        public override string Properties
        {
            get
            {
                return "Selection Style: " + SelectionStyle + "\n Values: All";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            AddToBase("values", AllValues.Select(lup => lup.ToString()).ToArray());
            AddToBase("selectionstyle", SelectionStyle);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
        }
    }

    public class RandomStatus : RandomPickList
    {
        public RandomStatus()
        {
            Name = DataTypes.Custom.RandomStatus;
        }
    }
    public class RandomPickList : BaseMock
    {
        public List<PickList> Values { get; set; } = new List<PickList>();

        public List<PickList> AllValues { get; set; } = new List<PickList>();

        [DefaultValue("random")]
        public string SelectionStyle { get; set; } = SelectionStyles.Random;

        public RandomPickList() : base(DataTypes.CustomList)
        {
            Name = DataTypes.Custom.RandomPicklist;
        }

        public override string Properties
        {
            get
            {
                if (Values.Count() == 0)
                    return "Selection Style: " + SelectionStyle + "\n Values: All";

                return "Selection Style: " + SelectionStyle + "\n Values: \"" + string.Join("\",\"", Values.Select(lu => lu.Name)) + "\"";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            if (Values.Count == 0)
                AddToBase("values", AllValues.Select(lup => lup.ToString()).ToArray());
            else
                AddToBase("values", Values.Select(lup => lup.ToString()).ToArray());

            AddToBase("selectionstyle", SelectionStyle);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            var valueStrings = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            Values = AllValues.Where(val => valueStrings.Contains(val.Name)).ToList();
            //var lookup = FixedValue as PickList;
            //FixedValue = AllValues.FirstOrDefault(val => lookup.Name == val.Name);
            //    Values = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            SelectionStyle = kvps.First(kvp => kvp.Key == "selectionstyle").Value.ToString();
        }
    }

    public class CharSequence : BaseMock
    {
        public string Format { get; set; }

        public CharSequence() : base(DataTypes.CharSequence)
        {
        }

        public override string Properties => "Format: " + Format;

        public override Dictionary<string, object> GetField()
        {
            AddToBase("format", Format);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);

            Format = kvps.First(kvp => kvp.Key == "format").Value.ToString();
        }
    }

    public class NormalDistribution : BaseMock
    {
        public double Mean { get; set; } = 0;
        public double StandardDev { get; set; } = 1;

        public int Decimals { get; set; } = 2;

        public override string Properties => "Mean: " + Mean.ToString() + "\n Standard Deviation: " + StandardDev.ToString() + "\n Decimals: " + Decimals.ToString();

        public override Dictionary<string, object> GetField()
        {
            AddToBase("mean", Mean);
            AddToBase("sd", StandardDev);
            AddToBase("decimals", Decimals);
            return baseField;
        }

        public NormalDistribution() : base(DataTypes.NormalDistribution)
        {
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);

            Mean = (double)kvps.First(kvp => kvp.Key == "mean").Value;
            StandardDev = (double)kvps.First(kvp => kvp.Key == "sd").Value;
            Decimals = (int)kvps.First(kvp => kvp.Key == "mean").Value;
        }
    }

    public class BinomialDistribution : BaseMock
    {
        public double Probability { get; set; } = 0.5;

        public override Dictionary<string, object> GetField()
        {
            AddToBase("probability", Probability);
            return baseField;
        }

        public override string Properties => "Probability: " + Probability.ToString();

        public BinomialDistribution() : base(DataTypes.BinomialDistribution)
        {
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);

            Probability = (double)kvps.First(kvp => kvp.Key == "probability").Value;
        }
    }

    public class Number : BaseMock
    {
        public int Decimals { get; set; } = 2;
        public decimal Min { get; set; } = 1;
        public decimal Max { get; set; } = 100;

        public Number() : base(DataTypes.Number)
        {
        }

        public override string Properties => "Min: " + Min.ToString() + "\n Max: " + Max.ToString() + "\n Decimals: " + Decimals.ToString();

        public override Dictionary<string, object> GetField()
        {
            AddToBase("min", Min);
            AddToBase("max", Max);
            AddToBase("decimals", Decimals);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);

            Min = (decimal)kvps.First(kvp => kvp.Key == "min").Value;
            Max = (decimal)kvps.First(kvp => kvp.Key == "max").Value;
            Decimals = (int)kvps.First(kvp => kvp.Key == "decimals").Value;
        }
    }

    public static class SelectionStyles
    {
        public static readonly string Random = "random";
        public static readonly string Sequential = "sequential";
        public static readonly string Custom = "custom";
        public static readonly string Cartesian = "cartesian";
    }
}