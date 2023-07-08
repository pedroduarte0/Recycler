namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common
{
    internal static class DataBaseMethodHelpers
    {
        internal static string GetConnectionString()
        {
            const string dbFilename = "RecyclerDB.sqlite";

            string dbPath = DataBaseMethodHelpers.GetDatabasePath(dbFilename);
            return string.Format("DataSource={0}", dbPath);
        }

        private static string GetDatabasePath(string dbFilename)
        {
            // Note: In a comment of https://stackoverflow.com/questions/867485/c-sharp-getting-the-path-of-appdata
            // it mentions in Linux the appdata folder doesnt exist and tells how to create it.
            string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string dbFolderPath = Path.Combine(localAppDataPath, "Recycler");
            if (!Directory.Exists(dbFolderPath))
            {
                Directory.CreateDirectory(dbFolderPath);
            }

            return Path.Combine(dbFolderPath, dbFilename);
        }
    }
}
