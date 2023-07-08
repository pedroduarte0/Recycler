using System.Data.SQLite;
using BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer.Common;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public class SQLiteFileDescriptorIndexer : IFileDescriptorIndexer
    {
        private readonly SQLiteConnection m_connection;
        private readonly string m_tableName = "FileDescriptors";


        public SQLiteFileDescriptorIndexer()
        {
            string connectionString = DataBaseMethodHelpers.GetConnectionString();

            m_connection = new SQLiteConnection(connectionString);
            m_connection.Open();
        }

        ~SQLiteFileDescriptorIndexer()
        {
            if (m_connection != null)
            {
               m_connection.Dispose();
            }
        }

        public void Initialize()
        {
            CreateDatabaseIfDoesNotExist();
        }

        public void Insert(FileDescriptor descriptor)
        {
            using var cmd = new SQLiteCommand(m_connection);
            
            cmd.CommandText = $"INSERT OR REPLACE INTO {m_tableName}(ChangeInfoType, FullPath, Name, Age)" +
                              " VALUES(@changeInfoType, @fullPath, @name, @age)";

            cmd.Parameters.AddWithValue("@changeInfoType", descriptor.ChangeInfoType);
            cmd.Parameters.AddWithValue("@fullPath", descriptor.FullPath);
            cmd.Parameters.AddWithValue("@name", descriptor.Name);
            cmd.Parameters.AddWithValue("@age", descriptor.Age);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public void Persist()
        {
            // Does nothing. Persisting isn't  needed for this implementation.
        }

        public void Remove(FileDescriptor descriptor)
        {
            using var cmd = new SQLiteCommand(m_connection);

            cmd.CommandText = $"DELETE FROM {m_tableName}" +
                              " WHERE FullPath = @fullPath";

            cmd.Parameters.AddWithValue("@fullPath", descriptor.FullPath);
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        public ICollection<FileDescriptor> RetrieveAll()
        {
            throw new System.NotImplementedException();
        }

        private void CreateDatabaseIfDoesNotExist()     //This is not needed: https://github.com/pedroduarte0/Recycler/issues/22
        {
            // Check if database exists by checking whether a table exists. Optionally, could just check if the local db file was there, as an afterthought.
            bool tableExists = false;
            string stm = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{m_tableName}';";
            using var cmdCheck = new SQLiteCommand(stm, m_connection);
            using SQLiteDataReader rdr = cmdCheck.ExecuteReader();
            if (rdr.Read())
            {
                if (rdr.GetValue(0).ToString() == m_tableName)
                {
                    tableExists = true;
                }
            }

            // Initialize database
            if (!tableExists)
            {
                using var cmd = new SQLiteCommand(m_connection);
                cmd.CommandText = @"CREATE TABLE FileDescriptors(ChangeInfoType INTEGER NOT NULL,
	            FullPath  varchar(400) NOT NULL,
                Name  varchar(255) NOT NULL,
                Age   INTEGER NOT NULL,
	            PRIMARY KEY(FullPath))";

                cmd.ExecuteNonQuery();
            }
        }
    }
}
