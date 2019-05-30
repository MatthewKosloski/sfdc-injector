using System.Runtime.Serialization;

namespace SFDCInjector.ResponseBodies
{
    [DataContract]
    public class InjectEventResponseBody 
    {

        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="success")]
        public bool Success { get; set; }

        [DataMember(Name="errors")]
        public string[] Errors { get; set; }

    }
}