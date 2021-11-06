using System.Collections.Generic;

namespace BusinessLogic
{
    public interface IStorage
    {
        /// <summary>
        /// Persists a collection of strings into the destination location.
        /// </summary>
        /// <param name="strings"></param>
        void SaveStrings(ICollection<string> strings, string destination);

        /// <summary>
        /// Persists a string into the destination location.
        /// </summary>
        /// <param name="singleToSave"></param>
        /// <param name="filePath"></param>
        void SaveString(string singleToSave, string destination);

        /// <summary>
        /// Loads a string from the source location.
        /// </summary>
        /// <param name="source">The location where to load.</param>
        /// <returns>The obtained string.</returns>
        string LoadString(string source);
    }
}
