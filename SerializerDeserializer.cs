using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace SFDCInjector
{
    public class SerializerDeserializer
    {
        public static T DeserializeJsonToType<T>(string json) where T : class, new()
        {
            T obj = new T();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            obj = serializer.ReadObject(stream) as T;
            stream.Close();
            return obj;
        }

        // Serializes an instance of Type T into a JSON string.
        public static string SerializeTypeToJson<T>(T obj)
        {
            MemoryStream stream = new MemoryStream();  
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));  
            serializer.WriteObject(stream, obj);  
            byte[] json = stream.ToArray();  
            stream.Close();  
            return Encoding.UTF8.GetString(json, 0, json.Length);  
        }
    }
}