using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk.Metadata;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace LinkeD365.MockDataGen
{
    public class MapRow : INotifyPropertyChanged
    {
        private bool selected;
        private string mockType;
        private MockOption mockOptions;
        private BaseMock selectedMock;

        public MapRow(AttributeMetadata attributeMetadata)
        {
            Attribute = attributeMetadata;
        }

        [Browsable(false)]
        public AttributeMetadata Attribute { get; set; }

        [DisplayName(" \n ")]
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (value != selected) { selected = value; NotifyPropertyChanged(); }
            }
        }

        [DisplayName("Attribute")]
        public string AttributeName { get => Attribute?.DisplayName?.UserLocalizedLabel?.Label ?? Attribute?.LogicalName; }

        [DisplayName("Type")]
        public string AttributeType { get => Attribute.AttributeTypeName.Value; }

        [DisplayName("Length")]
        public int? AttributeLength
        {
            get
            {
                if (Attribute is StringAttributeMetadata)
                {
                    var stringAtt = Attribute as StringAttributeMetadata;
                    return stringAtt.DatabaseLength.GetValueOrDefault();
                }
                return null;
            }
        }

        public string ParentTable
        {
            get
            {
                if (selectedMock.EntityName != string.Empty)
                {
                    return selectedMock.EntityName;
                }

                if (Attribute is LookupAttributeMetadata)
                {
                    return ((LookupAttributeMetadata)Attribute).Targets[0];
                }
                else if (selectedMock is RandomTeam || selectedMock is FixedTeam)
                {
                    return "team";
                }
                else if (selectedMock is RandomUser || selectedMock is FixedUser)
                {
                    return "systemuser";
                }

                return string.Empty;
            }
        }

        public string MockType
        {
            get { return mockType; }
            set
            {
                if (value != mockType) { mockType = value; NotifyPropertyChanged(); }
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public MockOption MockOptions
        {
            get { return mockOptions; }
            set
            {
                if (value != mockOptions) { mockOptions = value; NotifyPropertyChanged(); }
            }
        }

        [Browsable(false)]
        public BaseMock SelectedMock
        {
            get { return selectedMock; }
            set
            {
                if (value != selectedMock) { selectedMock = value; NotifyPropertyChanged(); }
            }
        }

        public string AdditionalProperties
        {
            get
            {
                if (selectedMock == null)
                {
                    return string.Empty;
                }

                return selectedMock.Properties;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string BlankPercentage
        {
            get
            {
                if (selectedMock == null)
                {
                    return string.Empty;
                }

                return selectedMock.PercentBlank.ToString();
            }
            set
            {
                if (selectedMock == null)
                {
                    return;
                }

                var intValue = 0;
                if (!int.TryParse(value, out intValue))
                {
                    selectedMock.PercentBlank = 0;
                }

                if (intValue > 100)
                {
                    intValue = 100;
                }

                selectedMock.PercentBlank = intValue;
                NotifyPropertyChanged();
            }
        }
    }
}