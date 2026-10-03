namespace BusinessLogic
{
    public interface IFileChangeMonitor
    {
        void AddFolderForMonitoring(string path);

        void RemoveFolderForMonitoring(string path);

        void PersistFoldersList();
    }
}