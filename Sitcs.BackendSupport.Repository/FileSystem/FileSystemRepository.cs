// **************************************************************************
// <copyright file="FileSystemRepository.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// File system class repository.
    /// </summary>
    public class FileSystemRepository : IFileSystemRepository
    {
        /// <summary>
        /// Write a file to Disk with a file stream instance.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="overWrite">Indicates whether the file can be overwrite or not.</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of file stream instance.</returns>
        public FileStream CreateFileStreamFromFile(string fileName, bool overWrite, Encoding encoding)
        {
            FileMode fileMode = !overWrite ? FileMode.CreateNew : FileMode.Create;
            return new FileStream(fileName, fileMode, FileAccess.Write, FileShare.Write);
        }

        /// <summary>
        /// Creates a stream reader instance.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of stream reader class.</returns>
        public StreamReader CreateStreamReaderFromFile(string fileName, Encoding encoding)
        {
            return new StreamReader(fileName, encoding);
        }

        /// <summary>
        /// Creates a stream writer instance.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="append">Boolean specifying whether the content should be appended to the file</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of stream writer class.</returns>
        public StreamWriter CreateStreamWriterFromFile(string fileName, bool append, Encoding encoding)
        {
            return new StreamWriter(fileName, false, encoding);
        }

        /// <summary>
        /// Creates a stream writer instance from file stream.
        /// </summary>
        /// <param name="fileName">File's path.</param>
        /// <param name="overWrite">Indicates whether the file can be overwrite or not.</param>
        /// <param name="encoding">File's encoding.</param>
        /// <returns>A new instance of stream writer class.</returns>
        public StreamWriter CreateStreamWriterFromFileStream(string fileName, bool overWrite, Encoding encoding)
        {
            return new StreamWriter(this.CreateFileStreamFromFile(fileName, overWrite, encoding));
        }

        /// <summary>
        /// Delete File Name
        /// </summary>
        /// <param name="fileName">File Name</param>
        public void Delete(string fileName)
        {
            File.Delete(fileName);
        }

        /// <summary>
        /// Verifies if a directory exits.
        /// </summary>
        /// <param name="path">Directory path to verify.</param>
        /// <returns>Whether the directory exists or not.</returns>
        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Verifies if a file exists.
        /// </summary>
        /// <param name="fileName">File name to verify.</param>
        /// <returns>Whether the file exists or not.</returns>
        public bool FileExists(string fileName)
        {
            return File.Exists(fileName);
        }

        /// <summary>
        /// Verifies if a file is in use by another application.
        /// </summary>
        /// <param name="fileName">File name to verify.</param>
        /// <returns>Whether the file is being used or not.</returns>
        public bool FileInUse(string fileName)
        {
            try
            {
                // if file cannot be opened exclusively, means that is in use by another application
                using (FileStream fileStream = File.Open(
                    fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a file name path directory.
        /// </summary>
        /// <param name="fileName">File name to get its directory path.</param>
        /// <returns>File name directory.</returns>
        public string GetFileDirectory(string fileName)
        {
            return Path.GetDirectoryName(fileName);
        }

        /// <summary>
        /// Get file length;
        /// </summary>
        /// <param name="fileName">File name path.</param>
        /// <returns>File's length.</returns>
        public long GetFileLength(string fileName)
        {
            return new FileInfo(fileName).Length;
        }

        /// <summary>
        /// Get the complete filename of the queue.
        /// </summary>
        /// <param name="path">Message Queue physical path</param>
        /// <param name="fileName">FileName stored in disk</param>
        /// <returns>Full Path</returns>
        public string GetPathCombine(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// Move File.
        /// </summary>
        /// <param name="sourceFile">Source File Name</param>
        /// <param name="destinationFile">Destination File Name</param>
        public void Move(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A byte array containing the contents of the file.</returns>
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <summary>
        /// Reads all text from a file.
        /// </summary>
        /// <param name="fileName">File name to read all text from.</param>
        /// <returns>All file text content.</returns>
        public string ReadFileAllText(string fileName)
        {
            return File.ReadAllText(fileName);
        }

        /// <summary>
        /// Read content of whole file.
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <returns>Content of file</returns>
        public string ReadToEnd(string fileName)
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Write a file to Disk.
        /// </summary>
        /// <param name="fileName">File Name</param>
        /// <param name="data">Data to write</param>
        public void WriteFile(string fileName, object data)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(data);
            }
        }

        /// <summary>
        /// Write a file to Disk.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <param name="append">Append data to an existing file.</param>
        /// <param name="encoding">The character encoding to use</param>
        /// <param name="data">Data to write</param>
        public void WriteFile(string fileName, bool append, Encoding encoding, object data)
        {
            using (StreamWriter writer = new StreamWriter(fileName, append, encoding))
            {
                writer.Write(data);
            }
        }
    }
}
