using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BusinessLogicTests")]

namespace BusinessLogic
{
    public interface IFileMonitor
    {
        void AddFolderForMonitoring(string path);

        void RemoveFolderForMonitoring(string path);

        void StartMonitoring();
    }
}