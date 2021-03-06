using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace SFDCInjector.Utils
{
    /// <summary>
    /// Includes helper methods to serialize an object into a JSON 
    /// string and deserialize a JSON string into an object.
    /// </summary>
    public class SerializerDeserializer
    {
        /// <summary>
        /// Deserializes a JSON string into an instance of type T.
        /// </summary>
        public static T DeserializeJsonToType<T>(string json) where T : class, new()
        {
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                T obj = new T();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                obj = serializer.ReadObject(stream) as T;
                return obj;
            }
        }

        /// <summary>
        /// Serializes an instance of Type T into a JSON string.
        /// </summary>
        public static string SerializeTypeToJson<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));  
                serializer.WriteObject(stream, obj);  
                byte[] json = stream.ToArray();  
                return Encoding.UTF8.GetString(json, 0, json.Length); 
            } 
        }
    }
}