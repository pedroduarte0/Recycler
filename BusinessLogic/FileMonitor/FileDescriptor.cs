namespace BusinessLogic.FileMonitor
{
    public class FileDescriptor
    {
        public ChangeInfoType ChangeInfoType { get; private set; }
        public string FullPath { get; private set; }
        public string Name { get; private set; }

        public FileDescriptor(ChangeInfoType changeInfoType, string fullPath, string name)
        {
            ChangeInfoType = changeInfoType;
            FullPath = fullPath;
            Name = name;
        }
    }
}
