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

        public string LoadString(string source)
        {
            return m_systemIoFileWrapper.ReadAllText(source);
        }

        public void SaveStrings(ICollection<string> strings, string destination)
        {
            var sb = new StringBuilder();
            foreach (string item in strings)
            {
                sb.AppendLine(item);
            }

            m_systemIoFileWrapper.WriteAllText(destination, sb.ToString());
        }

        public void SaveString(string singleToSave, string destination)
        {
            m_systemIoFileWrapper.WriteAllText(destination, singleToSave);
        }
    }
}
