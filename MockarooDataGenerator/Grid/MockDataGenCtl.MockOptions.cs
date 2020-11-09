using LinkeD365.MockDataGen.Mock;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;
using XrmToolBox.Extensibility;

namespace LinkeD365.MockDataGen
{
    public partial class MockDataGenCtl : PluginControlBase
    {
        internal List<MockOption> MockOptions { get; } = new List<MockOption>{
                    new MockOption(AttributeTypeCode.String, //STRING
                    new List<BaseMock>{  new StringMock(DataTypes.AppBundleID),
                        new StringMock(DataTypes.CompanyName), new CharSequence(), new Country(), new CustomList(),
                    new StringMock(DataTypes.AppName),new StringMock(DataTypes.AppVersion),new StringMock(DataTypes.Base64ImageUrl),
                    new StringMock(DataTypes.City),new StringMock(DataTypes.Color),new StringMock(DataTypes.Country),new StringMock(DataTypes.CountryCode),
                    new StringMock(DataTypes.CreditCardNumber),new StringMock(DataTypes.CreditCardType),new StringMock(DataTypes.DepartmentCorporate),
                    new StringMock(DataTypes.DepartmentRetail),new StringMock(DataTypes.DomainName),new StringMock(DataTypes.DrugCompany),new StringMock(DataTypes.DrugNameBrand),
                    new StringMock(DataTypes.DrugNameGeneric),new StringMock(DataTypes.DummyImageUrl),new StringMock(DataTypes.EmailAddress),new StringMock(DataTypes.FamilyNameChinese),
                    new StringMock(DataTypes.FdaNdcCode),new StringMock(DataTypes.FileName),new StringMock(DataTypes.FirstName),new StringMock(DataTypes.FirstNameEuropean),
                    new StringMock(DataTypes.FirstNameFemale),new StringMock(DataTypes.FirstNameMale), new StringMock(DataTypes.Frequency),
                        new StringMock(DataTypes.FullName), new StringMock(DataTypes.Gender),new StringMock(DataTypes.GenderAbbrev),new StringMock(DataTypes.GivenNameChinese),
                        new StringMock(DataTypes.Guid),new StringMock(DataTypes.HexColor), new StringMock(DataTypes.IBAN),new StringMock(DataTypes.ICD10DiagnosisCode),
                        new StringMock(DataTypes.ICD10DxDescLong),new StringMock(DataTypes.ICD10DxDescShort),new StringMock(DataTypes.ICD10ProcDescLong), new StringMock(DataTypes.ICD10ProcDescShort),
                        new StringMock(DataTypes.ICD10ProcedureCode),new StringMock(DataTypes.ICD9DiagnosisCode),new StringMock(DataTypes.ICD9DxDescLong),new StringMock(DataTypes.ICD9DxDescShort),
                        new StringMock(DataTypes.ICD9ProcDescLong),new StringMock(DataTypes.ICD9ProcDescShort),new StringMock(DataTypes.ICD9ProcedureCode),new StringMock(DataTypes.IPAddressV4),
                    new StringMock(DataTypes.IPAddressV6),new StringMock(DataTypes.IPAddressV6Cidr),new StringMock(DataTypes.ISBN),new StringMock(DataTypes.JobTitle),
                        new StringMock(DataTypes.Language),new StringMock(DataTypes.LastName),new StringMock(DataTypes.Latitude),new StringMock(DataTypes.LinkedInSkill),
                        new StringMock(DataTypes.Longitude),new StringMock(DataTypes.MacAddress),new StringMock(DataTypes.MimeType),new StringMock(DataTypes.Money),
                        new StringMock(DataTypes.MongoDbObjectId),new StringMock(DataTypes.NaughtyString),new StringMock(DataTypes.Password),new StringMock(DataTypes.Phone),
                        new StringMock(DataTypes.PostalCode),new StringMock(DataTypes.Race),new StringMock(DataTypes.Scenario),new StringMock(DataTypes.Sequence),
                        new StringMock(DataTypes.SHA1),new StringMock(DataTypes.SHA256),new StringMock(DataTypes.ShirtSize),new StringMock(DataTypes.ShortHexColor),
                        new StringMock(DataTypes.SSN),new StringMock(DataTypes.State),new StringMock(DataTypes.StateAbbreviated),//new StringMock(DataTypes.StateAbbreviated),
                        new StringMock(DataTypes.StreetAddress),new StringMock(DataTypes.StreetName),new StringMock(DataTypes.StreetNumber),new StringMock(DataTypes.StreetSuffix),
                        new StringMock(DataTypes.Suffix),new StringMock(DataTypes.Template),new StringMock(DataTypes.Title),new StringMock(DataTypes.TopLevelDomain),
                        new StringMock(DataTypes.University),new StringMock(DataTypes.Url),new StringMock(DataTypes.UserAgent),new StringMock(DataTypes.Username),
                        new StringMock(DataTypes.Words), new FakeEmailMock()
                    }.OrderBy(bm=>bm.Name).ToList()),
                    new MockOption(AttributeTypeCode.Memo, //MEMO
                    new List<BaseMock>{ new StringMock(DataTypes.AppBundleID),
                        new StringMock(DataTypes.CompanyName), new CharSequence(), new Country(), new CustomList(),
                    new StringMock(DataTypes.AppName),new StringMock(DataTypes.AppVersion),new StringMock(DataTypes.Base64ImageUrl),
                    new StringMock(DataTypes.City),new StringMock(DataTypes.Color),new StringMock(DataTypes.Country),new StringMock(DataTypes.CountryCode),
                    new StringMock(DataTypes.CreditCardNumber),new StringMock(DataTypes.CreditCardType),new StringMock(DataTypes.DepartmentCorporate),
                    new StringMock(DataTypes.DepartmentRetail),new StringMock(DataTypes.DomainName),new StringMock(DataTypes.DrugCompany),new StringMock(DataTypes.DrugNameBrand),
                    new StringMock(DataTypes.DrugNameGeneric),new StringMock(DataTypes.DummyImageUrl),new StringMock(DataTypes.EmailAddress),new StringMock(DataTypes.FamilyNameChinese),
                    new StringMock(DataTypes.FdaNdcCode),new StringMock(DataTypes.FileName),new StringMock(DataTypes.FirstName),new StringMock(DataTypes.FirstNameEuropean),
                    new StringMock(DataTypes.FirstNameFemale),new StringMock(DataTypes.FirstNameMale), new StringMock(DataTypes.Frequency),
                        new StringMock(DataTypes.FullName), new StringMock(DataTypes.Gender),new StringMock(DataTypes.GenderAbbrev),new StringMock(DataTypes.GivenNameChinese),
                        new StringMock(DataTypes.Guid),new StringMock(DataTypes.HexColor), new StringMock(DataTypes.IBAN),new StringMock(DataTypes.ICD10DiagnosisCode),
                        new StringMock(DataTypes.ICD10DxDescLong),new StringMock(DataTypes.ICD10DxDescShort),new StringMock(DataTypes.ICD10ProcDescLong), new StringMock(DataTypes.ICD10ProcDescShort),
                        new StringMock(DataTypes.ICD10ProcedureCode),new StringMock(DataTypes.ICD9DiagnosisCode),new StringMock(DataTypes.ICD9DxDescLong),new StringMock(DataTypes.ICD9DxDescShort),
                        new StringMock(DataTypes.ICD9ProcDescLong),new StringMock(DataTypes.ICD9ProcDescShort),new StringMock(DataTypes.ICD9ProcedureCode),new StringMock(DataTypes.IPAddressV4),
                    new StringMock(DataTypes.IPAddressV6),new StringMock(DataTypes.IPAddressV6Cidr),new StringMock(DataTypes.ISBN),new StringMock(DataTypes.JobTitle),
                        new StringMock(DataTypes.Language),new StringMock(DataTypes.LastName),new StringMock(DataTypes.Latitude),new StringMock(DataTypes.LinkedInSkill),
                        new StringMock(DataTypes.Longitude),new StringMock(DataTypes.MacAddress),new StringMock(DataTypes.MimeType),new StringMock(DataTypes.Money),
                        new StringMock(DataTypes.MongoDbObjectId),new StringMock(DataTypes.NaughtyString),new StringMock(DataTypes.Password),new StringMock(DataTypes.Phone),
                        new StringMock(DataTypes.PostalCode),new StringMock(DataTypes.Race),new StringMock(DataTypes.Scenario),new StringMock(DataTypes.Sequence),
                        new StringMock(DataTypes.SHA1),new StringMock(DataTypes.SHA256),new StringMock(DataTypes.ShirtSize),new StringMock(DataTypes.ShortHexColor),
                        new StringMock(DataTypes.SSN),new StringMock(DataTypes.State),new StringMock(DataTypes.StateAbbreviated),
                        new StringMock(DataTypes.StreetAddress),new StringMock(DataTypes.StreetName),new StringMock(DataTypes.StreetNumber),new StringMock(DataTypes.StreetSuffix),
                        new StringMock(DataTypes.Suffix),new StringMock(DataTypes.Template),new StringMock(DataTypes.Title),new StringMock(DataTypes.TopLevelDomain),
                        new StringMock(DataTypes.University),new StringMock(DataTypes.Url),new StringMock(DataTypes.UserAgent),new StringMock(DataTypes.Username),
                        new StringMock(DataTypes.Words), new Paragraph()
                    }.OrderBy(bm=>bm.Name).ToList()),
                    new MockOption(AttributeTypeCode.Boolean, //Boolean
                        new List<BaseMock>{new True(), new False(),// new BinomialDistribution(),
                            new StringMock(DataTypes.Boolean)                    }.OrderBy(bm=>bm.Name).ToList()),
                    new MockOption(AttributeTypeCode.BigInt, //BigInt
                        new List<BaseMock>{new NormalDistribution(),  new FixedNumber()}),

                    new MockOption(AttributeTypeCode.DateTime, //DateTime
                        new List<BaseMock>{new DT(), new Date(), new Time(), new FixedDate(), new FixedDateTime(), new FixedTime()                    }.OrderBy(bm=>bm.Name).ToList()),
                    new MockOption(AttributeTypeCode.Decimal, //Decimal
                        new List<BaseMock>{new NormalDistribution(),  new FixedNumber(), new Number()                    }.OrderBy(bm=>bm.Name).ToList()),
                     new MockOption(AttributeTypeCode.Double, //Double
                        new List<BaseMock>{new NormalDistribution(), new FixedNumber(), new Number()                    }.OrderBy(bm=>bm.Name).ToList()),
                      new MockOption(AttributeTypeCode.Integer, //BigInt
                        new List<BaseMock>{new NormalDistribution(), new FixedNumber(), new Number()                    }.OrderBy(bm=>bm.Name).ToList()),
                       new MockOption(AttributeTypeCode.Lookup, //Lookup
                        new List<BaseMock>{new RandomLookup(), new FixedLookup()                    }.OrderBy(bm=>bm.Name).ToList()),
                       new MockOption(AttributeTypeCode.Money, //Money
                        new List<BaseMock>{new NormalDistribution(),
                         new FixedNumber(), new Number()                    }.OrderBy(bm=>bm.Name).ToList()),
                       new MockOption(AttributeTypeCode.Picklist, //Picklist
                        new List<BaseMock>{new FixedPickList(), new RandomPickList()}),
                      new MockOption(AttributeTypeCode.Status, //Status
                        new List<BaseMock>{new FixedStatus(), new RandomStatus()}),
                      new MockOption(AttributeTypeCode.Owner, // Owner
                          new List<BaseMock>{new FixedTeam(), new FixedUser(),
                              new RandomTeam(), new RandomUser()}.OrderBy(bm=>bm.Name).ToList()),
                      new MockOption(AttributeTypeCode.Customer,
                          new List<BaseMock>{new FixedContact(), new FixedAccount(), new RandomAccount(), new RandomContact()})
        };
    }

    public class MockOption
    {
        public MockOption(AttributeTypeCode attributeTypeCode, List<BaseMock> mocks)
        {
            AttributeTypeCode = attributeTypeCode;
            Mocks = mocks;
        }

        public AttributeTypeCode AttributeTypeCode { get; set; }
        public List<BaseMock> Mocks { get; set; }
        public string Name { get; set; }
    }
}