namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public interface ISerializer
    {
        string Serialize(object objectToSerialize);
    }
}