using System.Runtime.Serialization;

namespace SFDCInjector.PlatformEvents.DataCenter
{
    [DataContract]
    public class DataCenterNameEventFields : IPlatformEventFields
    {
        
        [DataMember(Name="Name__c")]
        public string Name { get; set; }

        [DataMember(Name="Data_Center_Id__c")]
        public string DataCenterId { get; set; }
    }
}