using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.FrameworkAbstractions
{
    /// <summary>
    /// Abstraction for System.IO.Directory.
    /// </summary>
    public interface IDirectoryWrapper
    {
        // Tests if the given path refers to an existing DirectoryInfo on disk.
        bool Exists([NotNullWhen(true)] string? path);

        DirectoryInfo CreateDirectory(string path);
    }
}
