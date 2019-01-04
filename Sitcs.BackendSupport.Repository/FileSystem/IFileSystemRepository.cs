// **************************************************************************
// <copyright file="IFileSystemRepository.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// File system repository.
    /// </summary>
    public interface IFileSystemRepository
    {
        /// <summary>
        /// Write a file to Disk with a file stream instance.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="overWrite">Indicates whether the file can be overwrite or not.</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of file stream instance.</returns>
        FileStream CreateFileStreamFromFile(string fileName, bool overWrite, Encoding encoding);

        /// <summary>
        /// Creates a stream reader instance.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of stream reader class.</returns>
        StreamReader CreateStreamReaderFromFile(string fileName, Encoding encoding);

        /// <summary>
        /// Creates a stream writer instance.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="append">Boolean specifying whether the content should be appended to the file</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of stream writer class.</returns>
        StreamWriter CreateStreamWriterFromFile(string fileName, bool append, Encoding encoding);

        /// <summary>
        /// Creates a stream writer instance from file stream.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="overWrite">Indicates whether the file can be overwrite or not.</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of stream writer class.</returns>
        StreamWriter CreateStreamWriterFromFileStream(string fileName, bool overWrite, Encoding encoding);

        /// <summary>
        /// Delete File Name
        /// </summary>
        /// <param name="fileName">File Name</param>
        void Delete(string fileName);

        /// <summary>
        /// Verifies if a directory exists.
        /// </summary>
        /// <param name="path">Directory path to verify.</param>
        /// <returns>Whether the directory exists or not.</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Verifies if a file exists.
        /// </summary>
        /// <param name="fileName">File name to verify.</param>
        /// <returns>Whether the file exists or not.</returns>
        bool FileExists(string fileName);

        /// <summary>
        /// Verifies if a file is in use by another application.
        /// </summary>
        /// <param name="fileName">File name to verify.</param>
        /// <returns>Whether the file is being used or not.</returns>
        bool FileInUse(string fileName);

        /// <summary>
        /// Gets a file name path directory.
        /// </summary>
        /// <param name="fileName">File name to get its directory path.</param>
        /// <returns>File name directory.</returns>
        string GetFileDirectory(string fileName);

        /// <summary>
        /// Get file length;
        /// </summary>
        /// <param name="fileName">File name path.</param>
        /// <returns>File's length.</returns>
        long GetFileLength(string fileName);

        /// <summary>
        /// Get the complete filename of the queue.
        /// </summary>
        /// <param name="path">Message Queue physical path</param>
        /// <param name="fileName">FileName stored in disk</param>
        /// <returns>Full Path</returns>
        string GetPathCombine(string path, string fileName);

        /// <summary>
        /// Move File.
        /// </summary>
        /// <param name="sourceFile">Source File Name</param>
        /// <param name="destinationFile">Destination File Name</param>
        void Move(string sourceFile, string destinationFile);

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A byte array containing the contents of the file.</returns>
        byte[] ReadAllBytes(string path);

        /// <summary>
        /// Reads all text from a file.
        /// </summary>
        /// <param name="fileName">File name to read all text from.</param>
        /// <returns>All file text content.</returns>
        string ReadFileAllText(string fileName);

        /// <summary>
        /// Read content of whole file.
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>Content of file</returns>
        string ReadToEnd(string fileName);

        /// <summary>
        /// Write a file to Disk.
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="data">Data to write</param>
        void WriteFile(string fileName, object data);

        /// <summary>
        /// Write a file to Disk.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <param name="append">Append data to an existing file.</param>
        /// <param name="encoding">The character encoding to use</param>
        /// <param name="data">Data to write</param>
        void WriteFile(string fileName, bool append, Encoding encoding, object data);
    }
}
