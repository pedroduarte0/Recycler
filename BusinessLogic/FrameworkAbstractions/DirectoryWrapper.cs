namespace BusinessLogic.FrameworkAbstractions
{
    public class DirectoryWrapper : IDirectoryWrapper
    {
        public bool Exists(string? path)
        {
            return Directory.Exists(path);
        }

        public DirectoryInfo CreateDirectory(string path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}
