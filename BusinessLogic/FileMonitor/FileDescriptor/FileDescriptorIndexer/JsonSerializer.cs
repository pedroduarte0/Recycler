using Newtonsoft.Json;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    /// <summary>
    /// Tests for JsonSerializer are done in 3rd party library tests (example: result = serialize(o), assert o == deserialize(result))
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        public string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }
    }
}