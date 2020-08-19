using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NMockaroo;
using NMockaroo.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            {
                sb.AppendLine($"Property '{property.Key}' = '{property.Value}'");
            }

            return sb.ToString();
        }
    }

    public abstract class BaseMock
    {
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
                if (!_field.ContainsKey("Type")) _field.Add("Type", MockType);
                else _field["Type"] = MockType;
                if (!_field.ContainsKey("MockName")) _field.Add("MockName", MockType);
                else _field["MockName"] = Name;

                if (!_field.ContainsKey("PercentBlank")) _field.Add("PercentBlank", PercentBlank);
                else _field["PercentBlank"] = PercentBlank;

                if (!_field.ContainsKey("Formula")) if (!string.IsNullOrEmpty(Formula)) _field.Add("Formula", Formula);
                    else if (!string.IsNullOrEmpty(Formula)) _field["Formula"] = Formula;

                if (!_field.ContainsKey("FixedValue")) _field.Add("FixedValue", FixedValue);
                else _field["FixedValue"] = FixedValue;

                //if (!string.IsNullOrEmpty(Formula)) _field.Add("Formula", Formula);
                return _field;
            }
        }

        protected void AddToBase(string Key, object Value)
        {
            if (baseField.ContainsKey(Key)) baseField[Key] = Value;
            else baseField.Add(Key, Value);
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

        public override string Properties
        {
            get
            {
                return "Min: " + Min.ToString() + "\nMax: " + Max.ToString();
            }
        }
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

        public override string Properties
        {
            get { return "Width: " + Width.ToString() + "\nHeight: " + Height.ToString(); }
        }

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
                if (All) return "All Countries";
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
            Countries = ((string)kvps.First(kvp => kvp.Key == "counties").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
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
                if (Values == null || Values.Count() == 0) return "Selection Style: " + SelectionStyle;
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

        public Date() : base(DataTypes.Custom.Date)
        { }

        public override string Properties
        {
            get
            {
                return "Min: " + Min.ToString("d") + "\nMax: " + Max.ToString("d");
            }
        }

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Min", Min.ToString("d"));
            AddToBase("Max", Max.ToString("d"));
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Min = (DateTime)kvps.First(kvp => kvp.Key == "Min").Value;
            Max = (DateTime)kvps.First(kvp => kvp.Key == "Max").Value;
        }
    }

    public class Time : BaseMock
    {
        public TimeSpan Min { get; set; } = new TimeSpan(00, 0, 0);
        public TimeSpan Max { get; set; } = new TimeSpan(23, 59, 59);

        public Time() : base(DataTypes.Time)
        { }

        public override string Properties
        {
            get
            {
                return "Min: " + Min.ToString() + "\nMax: " + Max.ToString();
            }
        }

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Min", Min.ToString());
            AddToBase("Max", Max.ToString());
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Min = (TimeSpan)kvps.First(kvp => kvp.Key == "Min").Value;
            Max = (TimeSpan)kvps.First(kvp => kvp.Key == "Max").Value;
        }
    }

    public class DT : BaseMock
    {
        public DateTime Min { get; set; } = DateTime.Now.AddYears(-1);
        public DateTime Max { get; set; } = DateTime.Now.AddYears(-1);

        public DT() : base(DataTypes.Custom.DateTime)
        {
        }

        public override string Properties
        {
            get
            {
                return "Min: " + Min.ToString() + "\nMax: " + Max.ToString();
            }
        }

        public override Dictionary<string, object> GetField()
        {
            AddToBase("Min", Min.ToString());
            AddToBase("Max", Max.ToString());
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            Min = (DateTime)kvps.First(kvp => kvp.Key == "Min").Value;
            Max = (DateTime)kvps.First(kvp => kvp.Key == "Max").Value;
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

        public override string Properties { get { return "Value: " + FixedValue; } }

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
                if (FixedValue.ToString() != string.Empty) return "Value: " + ((DateTime)FixedValue).ToString("d");
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

        public override string Properties { get { return "Value: " + FixedValue; } }

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

        public override string Properties { get { return "Value: " + FixedValue; } }

        public override Dictionary<string, object> GetField()
        {
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
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
                if (FixedValue is Lookup) return "Value: " + ((Lookup)FixedValue).Name;
                else return "Value: ";
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
        { }
    }

    public class FixedUser : FixedLookup
    {
        public FixedUser() : base(DataTypes.Custom.FixedUser)
        { }
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

        public OptionSetValue Option
        {
            get
            {
                return new OptionSetValue(choiceNo);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class RandomLookup : BaseMock
    {
        public List<Lookup> Values { get; set; } = new List<Lookup>();

        public List<Lookup> AllValues { get; set; } = new List<Lookup>();

        [DefaultValue("random")]
        public string SelectionStyle { get; set; } = SelectionStyles.Random;

        public bool All { get; set; }

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
                if (Values.Count() == 0) return "Selection Style: " + SelectionStyle + "\n Values: All";
                return "Selection Style: " + SelectionStyle + "\n Values: \"" + string.Join("\",\"", Values.Select(lu => lu.Name)) + "\"";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            if (Values.Count == 0) AddToBase("values", AllValues.Select(lup => lup.ToString()).ToArray());
            else AddToBase("values", Values.Select(lup => lup.ToString()).ToArray());
            AddToBase("selectionstyle", SelectionStyle);
            return baseField;
        }

        public override void PopulateFromKVP(List<KVP> kvps)
        {
            BasePopulateFromKVP(kvps);
            var valueStrings = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            Values = AllValues.Where(val => valueStrings.Contains(val.Name)).ToList();
            // Values = ((string)kvps.First(kvp => kvp.Key == "values").Value).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries).Select(s => new looku;
            SelectionStyle = kvps.First(kvp => kvp.Key == "selectionstyle").Value.ToString();
        }
    }

    public class RandomTeam : RandomLookup
    {
        public RandomTeam() : base(DataTypes.Custom.RandomTeam)
        {
        }
    }

    public class RandomUser : RandomLookup
    {
        public RandomUser() : base(DataTypes.Custom.RandomUser)
        {
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
                if (FixedValue is PickList) return "Value: " + ((PickList)FixedValue).Name;
                else return "Value: ";
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
                if (Values.Count() == 0) return "Selection Style: " + SelectionStyle + "\n Values: All";
                return "Selection Style: " + SelectionStyle + "\n Values: \"" + string.Join("\",\"", Values.Select(lu => lu.Name)) + "\"";
            }
        }

        public override Dictionary<string, object> GetField()
        {
            if (Values.Count == 0) AddToBase("values", AllValues.Select(lup => lup.ToString()).ToArray());
            else AddToBase("values", Values.Select(lup => lup.ToString()).ToArray());
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

        public override string Properties
        {
            get
            {
                return "Format: " + Format;
            }
        }

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

        public override string Properties
        {
            get
            {
                return "Mean: " + Mean.ToString() + "\n Standard Deviation: " + StandardDev.ToString() + "\n Decimals: " + Decimals.ToString();
            }
        }

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

        public override string Properties
        {
            get
            {
                return "Probability: " + Probability.ToString();
            }
        }

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

        public override string Properties
        {
            get
            {
                return "Min: " + Min.ToString() + "\n Max: " + Max.ToString() + "\n Decimals: " + Decimals.ToString();
            }
        }

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
            Max = (decimal)kvps.First(kvp => kvp.Key == "min").Value;
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