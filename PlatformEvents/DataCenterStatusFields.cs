using System.Runtime.Serialization;

namespace SFDCInjector.PlatformEvents
{
    [DataContract]
    public class DataCenterStatusFields : IPlatformEventFields
    {
        
        [DataMember(Name="Status_Code__c")]
        public int StatusCode { get; set; }

        [DataMember(Name="Data_Center_Id__c")]
        public string DataCenterId { get; set; }
    }
}