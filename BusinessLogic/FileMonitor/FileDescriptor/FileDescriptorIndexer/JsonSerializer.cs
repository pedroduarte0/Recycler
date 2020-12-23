using Newtonsoft.Json;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    // TODO: Currently under evaluation. Not used at the moment since it leads to user implementation details leaking.
    public class JsonSerializer<T> : ISerializer<T>
    {
        public string Serialize(T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }
    }

    /// <summary>
    /// Tests for JsonSerializer are done in 3rd party library tests.
    /// </summary>
    public class JsonSerializer : ISerializer2
    {
        public string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }
    }
}