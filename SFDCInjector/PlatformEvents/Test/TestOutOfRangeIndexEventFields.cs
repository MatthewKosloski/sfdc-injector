using SFDCInjector.Attributes;
using System.Runtime.Serialization;

namespace SFDCInjector.PlatformEvents.Test
{

    /// <summary>
    /// The Fields for TestOutOfRangeIndexEvent, an event used in tests.
    /// </summary>
    [DataContract]
    public class TestOutOfRangeIndexEventFields: IPlatformEventFields
    {
        [DataMember(Name="String_Test_Field__c")]
        [CommandLineArgumentIndex(0)]
        public string StringTestField { get; set; }

        [DataMember(Name="Int_Test_Field__c")]
        [CommandLineArgumentIndex(1)]
        public int IntTestField { get; set; }

        [DataMember(Name="Double_Test_Field__c")]
        // out of range index
        [CommandLineArgumentIndex(-1)]
        public double DoubleTestField { get; set; }
    }
}