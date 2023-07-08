using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BusinessLogicTests")]

namespace BusinessLogic
{
    public interface IFileChangeMonitor : IDisposable
    {
        void AddFolderForMonitoring(string path);

        void RemoveFolderForMonitoring(string path);

        void PersistFoldersList();
    }
}