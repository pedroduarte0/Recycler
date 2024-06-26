﻿using BusinessLogic.FrameworkAbstractions;
using Newtonsoft.Json;

namespace BusinessLogic.FileMonitor.FileDescriptor.FileDescriptorIndexer
{
    /// <summary>
    /// Tests for JsonSerializer are done in 3rd party library tests (example: result = serialize(o), assert o == deserialize(result))
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        private readonly ISystemIOFileWrapper m_systemIOFile;

        public JsonSerializer(ISystemIOFileWrapper systemIOFile)
        {
            m_systemIOFile = systemIOFile;
        }

        public string Serialize(object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public object Deserialize(string path)
        {
            string text = m_systemIOFile.ReadAllText(path);
            return JsonConvert.DeserializeObject(text);
        }
    }
}