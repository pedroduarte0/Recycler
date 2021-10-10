using System.Collections.Generic;
using System.Text;
using BusinessLogic.FrameworkAbstractions;

namespace BusinessLogic
{
    public class FileStorage : IStorage
    {
        private readonly ISystemIOFileWrapper m_systemIoFileWrapper;

        public FileStorage(ISystemIOFileWrapper wrapper)
        {
            m_systemIoFileWrapper = wrapper;
        }

        public void Save(ICollection<string> strings, string filePath)
        {
            var sb = new StringBuilder();
            foreach (string item in strings)
            {
                sb.AppendLine(item);
            }

            m_systemIoFileWrapper.WriteAllText(filePath, sb.ToString());
        }

        public void Save(string singleToSave, string filePath)
        {
            m_systemIoFileWrapper.WriteAllText(filePath, singleToSave);
        }
    }
}
