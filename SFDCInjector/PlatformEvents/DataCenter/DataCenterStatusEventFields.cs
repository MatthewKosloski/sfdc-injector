using System.Runtime.Serialization;
using SFDCInjector.Attributes;

namespace SFDCInjector.PlatformEvents.DataCenter
{
    [DataContract]
    public class DataCenterStatusEventFields : IPlatformEventFields
    {
        
        [DataMember(Name="Status_Code__c")]
        [CommandLineArgumentIndex(0)]
        public int StatusCode { get; set; }

        [DataMember(Name="Data_Center_Id__c")]
        [CommandLineArgumentIndex(1)]
        public string DataCenterId { get; set; }
    }
}