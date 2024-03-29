﻿using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk.Metadata;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace LinkeD365.MockDataGen
{
    public class SimpleRow : INotifyPropertyChanged
    {
        [Browsable(false)]
        public string LogicalName;

        protected BaseMock selectedMock;
        public AttributeTypeCode? AttributeTypeCode;

        public string ParentTable
        {
            get
            {
                if (selectedMock.EntityName != string.Empty)
                    return selectedMock.EntityName;

                //if (AttributeTypeCode == Microsoft.Xrm.Sdk.Metadata.AttributeTypeCode.Lookup)
                //    return ((LookupAttributeMetadata)Attribute).Targets[0];
                //else if (selectedMock is RandomTeam || selectedMock is FixedTeam)
                //    return "team";
                //else if (selectedMock is RandomUser || selectedMock is FixedUser)
                //    return "systemuser";

                return string.Empty;
            }
        }

        [Browsable(false)]
        public int? Length = null;

        [Browsable(false)]
        public BaseMock SelectedMock
        {
            get => selectedMock;
            set
            {
                if (value != selectedMock) { selectedMock = value; NotifyPropertyChanged(); }
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class MapRow : SimpleRow, INotifyPropertyChanged
    {
        private bool selected;
        private string mockType;
        private MockOption mockOptions;

        public MapRow(AttributeMetadata attributeMetadata)
        {
            Attribute = attributeMetadata;
        }

        private AttributeMetadata attribute;

        [Browsable(false)]
        public AttributeMetadata Attribute
        {
            get => attribute;
            set
            {
                attribute = value;
                LogicalName = value.LogicalName;
                AttributeTypeCode = value.AttributeType;
                if (value is StringAttributeMetadata)
                    Length = ((StringAttributeMetadata)value).MaxLength.GetValueOrDefault();
            }
        }

        [DisplayName(" \n ")]
        public bool Selected
        {
            get => selected;
            set
            {
                if (value != selected) { selected = value; NotifyPropertyChanged(); }
            }
        }

        [DisplayName("Attribute")]
        public string AttributeName => Attribute?.DisplayName?.UserLocalizedLabel?.Label ?? Attribute?.LogicalName;

        [DisplayName("Type")]
        public string AttributeType => Attribute.AttributeTypeName.Value;

        [DisplayName("Length")]
        public int? AttributeLength
        {
            get
            {
                if (Attribute is StringAttributeMetadata)
                {
                    var stringAtt = Attribute as StringAttributeMetadata;
                    return stringAtt.MaxLength.GetValueOrDefault();
                }
                return null;
            }
        }

        public string MockType
        {
            get => mockType;
            set
            {
                if (value != mockType) { mockType = value; NotifyPropertyChanged(); }
            }
        }

        [Browsable(false)]
        [XmlIgnore]
        public MockOption MockOptions
        {
            get => mockOptions;
            set
            {
                if (value != mockOptions) { mockOptions = value; NotifyPropertyChanged(); }
            }
        }

        public string AdditionalProperties
        {
            get
            {
                if (selectedMock == null)
                    return string.Empty;

                return selectedMock.Properties;
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        public string BlankPercentage
        {
            get
            {
                if (selectedMock == null)
                    return string.Empty;

                return selectedMock.PercentBlank.ToString();
            }
            set
            {
                if (selectedMock == null)
                    return;

                var intValue = 0;
                if (!int.TryParse(value, out intValue))
                    selectedMock.PercentBlank = 0;

                if (intValue > 100)
                    intValue = 100;

                selectedMock.PercentBlank = intValue;
                NotifyPropertyChanged();
            }
        }
    }
}