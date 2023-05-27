using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    public class SQLiteFileDescriptorIndexer : IFileDescriptorIndexer
    {
        private readonly SQLiteConnection m_connection;
        private readonly string m_tableName = "FileDescriptors";


        public SQLiteFileDescriptorIndexer()
        {
            const string dbFilename = "RecyclerDB.sqlite";

            string dbPath = GetDatabasePath(dbFilename);
            string connectionString = string.Format("DataSource={0}", dbPath);

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
            
            cmd.CommandText = $"INSERT INTO {m_tableName}(ChangeInfoType, FullPath, Name, Age)" +
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
            throw new System.NotImplementedException();
        }

        public void Remove(FileDescriptor descriptor)
        {
            throw new System.NotImplementedException();
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
