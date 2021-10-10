namespace BusinessLogic.FileMonitor
{
    // TODO: Replace by FileDescriptor ?
    public class ChangeInfo
    {
        public ChangeInfoType ChangeInfoType { get; private set; }
        public string FullPath { get; private set; }
        public string Name { get; private set; }

        public ChangeInfo(ChangeInfoType changeInfoType, string fullPath, string name)
        {
            ChangeInfoType = changeInfoType;
            FullPath = fullPath;
            Name = name;
        }
    }

    public enum ChangeInfoType
    {
        Created,
        Changed,
        Deleted,
    }
}
