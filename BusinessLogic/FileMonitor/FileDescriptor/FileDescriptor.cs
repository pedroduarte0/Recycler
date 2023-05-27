using System.IO;

namespace BusinessLogic.FileMonitor.FileDescriptor
{
    // https://stackoverflow.com/questions/2646498/best-method-to-store-enum-in-database

    public class FileDescriptor
    {
        public ChangeInfoType ChangeInfoType { get; private set; }
        public string FullPath { get; private set; }
        public string Name { get; private set; }
        public int Age { get; set; }

        public FileDescriptor(ChangeInfoType changeInfoType, string fullPath, string name)
        {
            ChangeInfoType = changeInfoType;
            FullPath = fullPath;
            Name = name;
            Age = 0;
        }
    }
}
