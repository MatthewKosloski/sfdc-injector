using System.Runtime.Serialization;

namespace SFDCInjector
{
    [DataContract]
    public class AccessTokenResponseBody {

        [DataMember(Name="access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name="instance_url")]
        public string InstanceUrl { get; set; }

        [DataMember(Name="id")]
        public string Id { get; set; }

        [DataMember(Name="token_type")]
        public string TokenType { get; set; }

        [DataMember(Name="issued_at")]
        public string IssuedAt { get; set; }

        [DataMember(Name="signature")]
        public string Signature { get; set; }
    }
}