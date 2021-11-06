using Newtonsoft.Json;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    /// <summary>
    /// Tests for JsonSerializer are done in 3rd party library tests (example: result = serialize(o), assert o == deserialize(result))
    /// </summary>
    public class JsonSerializer<T> : ISerializer<T>
    {
        public string Serialize(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public T Deserialize(string serializedObject)
        {
            return (T)JsonConvert.DeserializeObject(serializedObject);
        }
    }
}