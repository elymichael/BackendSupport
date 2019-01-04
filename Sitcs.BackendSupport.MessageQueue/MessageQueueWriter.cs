// **************************************************************************
// <copyright file="MessageQueueWriter.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.MessageQueue
{
    using System;
    using System.IO;
    using System.Messaging;
    using Sitcs.BackendSupport.Repository;

    /// <summary>
    /// Message Queue Writer Class.
    /// </summary>
    public class MessageQueueWriter : MessageQueueProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueWriter" /> class.
        /// </summary>
        /// <param name="queuePath">Message queue path.</param>
        /// <param name="isTransactional">Indicates if queue is transactional.</param>
        /// <param name="serverName">Server Name.</param>
        public MessageQueueWriter(string queuePath, bool isTransactional, string serverName)
            : base(queuePath, isTransactional, serverName)
        {
        }

        /// <summary>
        /// Gets FileName without extension to Persist the file to disk.
        /// </summary>
        private string FilenameWithoutExtension
        {
            get
            {
                return this.MessagePath +
                    this.ServerName + "_" +
                    this.QueuePath.Replace("$", string.Empty).Replace("\\", "_") + "_" +
                    Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// Write message content to the message queue processor.
        /// </summary>
        /// <param name="message">message content to write</param>
        /// <returns>return filename</returns>
        public string WriteQueue(object message)
        {
            if (this.PersistMessageToDisk)
            {
                message = this.StoreMessageToDisk(message);
            }

            return ServiceLocator.Resolve<IMsmqRepository>().WriteMessage(
                this.QueuePathName, message, this.IsLocal, this.IsTransactional, this.MessageFormatter);
        }

        /// <summary>
        /// Store the message in disk if the object has set the write message to disk.
        /// </summary>
        /// <param name="message">Message received from the queue body </param>
        /// <returns>Return the filename without path.</returns>
        private string StoreMessageToDisk(object message)
        {
            string filename = Path.Combine(this.MessagePath, this.FilenameWithoutExtension) + ".queued";

            ServiceLocator.Resolve<IFileSystemRepository>().WriteFile(filename, message);

            return Path.GetFileName(filename);
        }
    }
}
