using LinkeD365.MockDataGen.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class Setting
    {
        public string Name { get; set; }

        public string EntityName { get { return EntityDisplay.LogicalName; } }

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
    }

    public class KVP
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}