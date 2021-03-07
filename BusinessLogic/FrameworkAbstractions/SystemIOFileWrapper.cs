﻿using System.IO;

namespace BusinessLogic.FrameworkAbstractions
{
    public class SystemIOFileWrapper : ISystemIOFileWrapper
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
