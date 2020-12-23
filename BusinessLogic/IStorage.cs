using System.Collections.Generic;

namespace BusinessLogic
{
    public interface IStorage
    {
        /// <summary>
        /// Persists a collection of strings into a single file.
        /// </summary>
        /// <param name="strings"></param>
        void Save(List<string> strings, string filePath);

        /// <summary>
        /// Persists a strings into a file.
        /// </summary>
        /// <param name="singleToSave"></param>
        /// <param name="filePath"></param>
        void Save(string singleToSave, string filePath);
    }
}
