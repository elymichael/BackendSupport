// **************************************************************************
// <copyright file="IMsmqRepository.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************
namespace Sitcs.BackendSupport.Repository
{
    using System;
    using System.Messaging;

    /// <summary>
    /// Delegate method to read the message.
    /// </summary>
    /// <param name="messagecontent">message content</param>    
    public delegate void Execute(string messagecontent);

    /// <summary>
    /// Message queue interface.
    /// </summary>
    public interface IMsmqRepository
    {
        /// <summary>
        /// Event to get the message from the repository to the business logic.
        /// </summary>
        event EventHandler<EngineEventArgs> MessageQueuesProcessed;

        /// <summary>
        /// Gets or sets a value indicating whether if the message queue is transactional or not.
        /// </summary>
        bool IsTransactional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an array of queues to 
        /// process them asynchronously in a transaction queue.
        /// </summary>
        MessageQueue[] Queues { get; set; }

        /// <summary>
        /// Close message queue.
        /// </summary>        
        void Close();

        /// <summary>
        /// Create a message queue using queue path name
        /// </summary>
        /// <param name="pathName">Message queue path</param>
        /// <param name="isTransactional">Indicates if it is transactional or not</param>
        void Create(string pathName, bool isTransactional);

        /// <summary>
        /// Verifies whether message queue path exists.
        /// </summary>
        /// <param name="pathName">message queue path</param>
        /// <returns>message queue exists or not</returns>
        bool Exists(string pathName);

        /// <summary>
        /// Get Total Messages
        /// </summary>        
        /// <returns>Total Messages</returns>
        int GetTotalMessages();

        /// <summary>
        /// Load Message Queue array.
        /// </summary>
        /// <param name="pathName">message queue path</param>
        /// <param name="totalQueue">Parallel total queue to load</param>
        /// <param name="messageFormatter">Message formatter</param>
        void LoadQueue(string pathName, int totalQueue, IMessageFormatter messageFormatter);

        /// <summary>
        /// Start message queue.
        /// </summary>        
        void Start();

        /// <summary>
        /// Write message to Message Queue.
        /// </summary>
        /// <param name="pathName">Path name</param>
        /// <param name="message">Message to write</param>
        /// <param name="isLocal">Indicates whether is local</param>
        /// <param name="isTransactional">Indicates whether is transactional</param>
        /// <param name="messageFormatter">Message Formatter</param>
        /// <returns>return id of the message</returns>
        string WriteMessage(
            string pathName,
            object message,
            bool isLocal,
            bool isTransactional,
            IMessageFormatter messageFormatter);
    }
}
