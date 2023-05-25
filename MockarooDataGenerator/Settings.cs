using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LinkeD365.MockDataGen
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class AllSettings
    {
        public List<Setting> Settings { get; set; } = new List<Setting>();
        public string LastUsedOrganizationWebappUrl { get; set; }

        public string MockKey { get; set; }

        public List<Set> Sets { get; set; } = new List<Set>();

        public ExcludeConfig ExcludeConfig {get;set;} = new ExcludeConfig();
    }

    public class Setting
    {
        public string Name { get; set; }

        public string EntityName => EntityDisplay.LogicalName;

        public EntityDisplay EntityDisplay { get; set; }

        public List<SimpleMapRow> MapRows { get; set; } = new List<SimpleMapRow>();

        public override bool Equals(object obj)
        {
            return obj is Setting setting &&
                   Name == setting.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class SimpleMapRow
    {
        public string AttributeName { get; set; }

        public List<KVP> SelectedMock { get; set; }

        public string BlackPercentage { get; set; }
        public AttributeTypeCode? AttributeTypeCode { get; set; }
        public string LogicalName { get; set; }
    }

    public class KVP
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class SetItem
    {
        public string MapName { get; set; }
        public int RecordCount { get; set; }
        public int Position { get; set; }
        public SetItem(string mapName, int recordCount, int postion)
        {
            MapName = mapName;
            RecordCount = recordCount;
            Position = postion;
        }

        public SetItem() { }

        [XmlIgnore]
        public List<Lookup> AddedValues = new List<Lookup>();

        [XmlIgnore]
        public string entityName = string.Empty;
    }

    public class Set
    {
        public string SetName { get; set; }
        public List<SetItem> SetItems { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Set set &&
                   SetName == set.SetName;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(SetName);
        }

        public override string ToString()
        {
            return SetName;
        }
    }

    public class ExcludeConfig
    {
        public bool DeprecatedTables { get; set; } = true;
        public bool DeprecatedColumns { get;set; } = true;
        public bool ImportSeqNo { get; set; } = true;
        public bool BypassPluginExecution { get; set; } = false;

    }

    public class ExportMaps
    {
        public ExportMaps() {
            Maps = new List<Setting>();
            Sets = new List<Set>();
        }
        public ExportMaps(List<Setting> maps)
        {
            Maps = maps;
        }
        public List<Setting> Maps { get; set; } //= new List<Setting>();

        public List<Set> Sets { get; set; }
    }
}