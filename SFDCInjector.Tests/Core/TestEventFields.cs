using SFDCInjector.PlatformEvents;
using System.Runtime.Serialization;

namespace SFDCInjector.Tests.Core
{

    /// <summary>
    /// The Fields for TestEvent, an event used in tests.
    /// </summary>
    [DataContract]
    public class TestEventFields : IPlatformEventFields
    {
        [DataMember(Name="TestField__c")]
        public string TestField { get; set; }
    }
}