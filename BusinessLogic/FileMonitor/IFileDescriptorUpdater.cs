namespace BusinessLogic.FileMonitor
{
    public interface IFileDescriptorUpdater
    {
        void Enqueue(ChangeInfo changeInfo);
    }
}
