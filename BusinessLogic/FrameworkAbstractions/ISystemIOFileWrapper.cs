using System.Security;

namespace BusinessLogic.FrameworkAbstractions
{
    public interface ISystemIOFileWrapper
    {
        // Summary:
        //     Opens a text file, reads all lines of the file, and then closes the file.
        //
        // Parameters:
        //   path:
        //     The file to open for reading.
        //
        // Returns:
        //     A string containing all lines of the file.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     path is a zero-length string, contains only white space, or contains one or more
        //     invalid characters as defined by System.IO.Path.InvalidPathChars.
        //
        //   T:System.ArgumentNullException:
        //     path is null.
        //
        //   T:System.IO.PathTooLongException:
        //     The specified path, file name, or both exceed the system-defined maximum length.
        //     For example, on Windows-based platforms, paths must be less than 248 characters,
        //     and file names must be less than 260 characters.
        //
        //   T:System.IO.DirectoryNotFoundException:
        //     The specified path is invalid (for example, it is on an unmapped drive).
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred while opening the file.
        //
        //   T:System.UnauthorizedAccessException:
        //     path specified a file that is read-only.-or- This operation is not supported
        //     on the current platform.-or- path specified a directory.-or- The caller does
        //     not have the required permission.
        //
        //   T:System.IO.FileNotFoundException:
        //     The file specified in path was not found.
        //
        //   T:System.NotSupportedException:
        //     path is in an invalid format.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission.
        [SecuritySafeCritical]
        string ReadAllText(string path);

        // Summary:
        //     Determines whether the specified file exists.
        //
        // Parameters:
        //   path:
        //     The file to check.
        //
        // Returns:
        //     true if the caller has the required permissions and path contains the name of
        //     an existing file; otherwise, false. This method also returns false if path is
        //     null, an invalid path, or a zero-length string. If the caller does not have sufficient
        //     permissions to read the specified file, no exception is thrown and the method
        //     returns false regardless of the existence of path.
        [SecuritySafeCritical]
        bool Exists(string path);

        //
        // Summary:
        //     Creates a new file, writes the specified string to the file, and then closes
        //     the file. If the target file already exists, it is overwritten.
        //
        // Parameters:
        //   path:
        //     The file to write to.
        //
        //   contents:
        //     The string to write to the file.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     path is a zero-length string, contains only white space, or contains one or more
        //     invalid characters as defined by System.IO.Path.InvalidPathChars.
        //
        //   T:System.ArgumentNullException:
        //     path is null or contents is empty.
        //
        //   T:System.IO.PathTooLongException:
        //     The specified path, file name, or both exceed the system-defined maximum length.
        //
        //   T:System.IO.DirectoryNotFoundException:
        //     The specified path is invalid (for example, it is on an unmapped drive).
        //
        //   T:System.IO.IOException:
        //     An I/O error occurred while opening the file.
        //
        //   T:System.UnauthorizedAccessException:
        //     path specified a file that is read-only. -or- This operation is not supported
        //     on the current platform. -or- path specified a directory. -or- The caller does
        //     not have the required permission.
        //
        //   T:System.NotSupportedException:
        //     path is in an invalid format.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission.
        [SecuritySafeCritical]
        void WriteAllText(string path, string contents);
    }
}