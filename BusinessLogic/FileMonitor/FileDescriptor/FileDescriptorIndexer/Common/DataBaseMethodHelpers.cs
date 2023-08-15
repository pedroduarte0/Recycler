using BusinessLogic.FrameworkAbstractions;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common
{
    public class DataBaseMethodHelpers : IDataBaseMethodHelpers
    {
        private readonly IDirectoryWrapper m_directoryWrapper;

        public DataBaseMethodHelpers(IDirectoryWrapper directoryWrapper)
        {
            m_directoryWrapper = directoryWrapper;
        }

        public string GetConnectionString()
        {
            const string dbFilename = "RecyclerDB.sqlite";

            string dbPath = GetDatabasePath();
            CreatePathIfNotExists(dbPath);

            string pathToDb = Path.Combine(dbPath, dbFilename);
            return string.Format("DataSource={0}", pathToDb);
        }

        private void CreatePathIfNotExists(string path)
        {
            if (!m_directoryWrapper.Exists(path))
            {
                m_directoryWrapper.CreateDirectory(path);
            }
        }

        private static string GetDatabasePath()
        {
            // Note: In a comment of https://stackoverflow.com/questions/867485/c-sharp-getting-the-path-of-appdata
            // it mentions in Linux the appdata folder doesnt exist and tells how to create it.
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return Path.Combine(localAppDataPath, "Recycler");
        }
    }
}
