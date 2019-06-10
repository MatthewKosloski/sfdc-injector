using System.Runtime.Serialization;
using SFDCInjector.Attributes;

namespace SFDCInjector.PlatformEvents.DataCenter
{
    [DataContract]
    public class DataCenterNameEventFields : IPlatformEventFields
    {
        
        [DataMember(Name="Name__c")]
        [CommandLineArgumentIndex(0)]
        public string Name { get; set; }

        [DataMember(Name="Data_Center_Id__c")]
        [CommandLineArgumentIndex(1)]
        public string DataCenterId { get; set; }
    }
}