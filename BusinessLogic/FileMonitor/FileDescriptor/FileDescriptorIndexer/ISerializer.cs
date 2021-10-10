namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public interface ISerializer<T>
    {
        string Serialize(T objectToSerialize);

        T Deserialize(string path);
    }
}