namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    // TODO: Currently under evaluation. Not used at the moment since it leads to user implementation details leaking.
    public interface ISerializer<T>
    {
        string Serialize(T objectToSerialize);
    }

    public interface ISerializer2
    {
        string Serialize(object objectToSerialize);
    }
}