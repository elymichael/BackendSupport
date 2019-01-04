// **************************************************************************
// <copyright file="MessageQueueProcessor.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.MessageQueue
{
    using System;
    using System.Messaging;
    using Sitcs.BackendSupport.Repository;

    /// <summary>
    /// Message Queue Processor Class, it contains common variables and functions.
    /// </summary>
    public abstract class MessageQueueProcessor
    {
        /// <summary>
        /// Store Message Formatter variable to assign the formatter to the message queue.
        /// </summary>
        private IMessageFormatter messageFormatter;

        /// <summary>
        /// Store Message in Disk.
        /// </summary>
        private bool persistMessageToDisk;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueProcessor"/> class
        /// </summary>
        /// <param name="queuePath">Private Queue path in the following format.</param>
        /// <param name="isTransactional">Indicates if Is Transactional</param>
        /// <param name="serverName">Server Name</param>
        public MessageQueueProcessor(string queuePath, bool isTransactional, string serverName) : base()
        {
            this.ServerName = serverName;
            this.QueuePath = queuePath;
            this.IsTransactional = isTransactional;
            this.MessagePath = string.Join(string.Empty, Environment.CurrentDirectory, @"\");
            this.Initialize();
        }

        /// <summary>
        /// Gets or sets a value indicating whether Store Message In Disk.
        /// </summary>
        public bool PersistMessageToDisk
        {
            get
            {
                return this.persistMessageToDisk;
            }

            set
            {
                this.persistMessageToDisk = value;
            }
        }

        /// <summary>
        /// Gets or sets Message Path to persist the files.
        /// </summary>
        public string MessagePath { get; set; }

        /// <summary>
        /// Gets or sets the Message Formatter 
        /// </summary>
        public IMessageFormatter MessageFormatter
        {
            get { return this.messageFormatter; }
            set { this.messageFormatter = value; }
        }

        /// <summary>
        /// Gets Server name
        /// </summary>
        protected string ServerName { get; private set; }

        /// <summary>
        /// Gets Queue Path.
        /// </summary>
        protected string QueuePath { get; private set; }

        /// <summary>
        /// Gets Store Queue Path for reference.
        /// </summary>
        protected string QueuePathName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the message queue works with Transaction or no
        /// </summary>
        protected bool IsTransactional { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is local computer or remote.
        /// </summary>
        protected bool IsLocal { get; private set; }

        /// <summary>
        /// Initialize default variables related to the object Message Queue.
        /// </summary>        
        private void Initialize()
        {
            this.persistMessageToDisk = true;
            this.messageFormatter = new BinaryMessageFormatter();
            this.QueuePathName = string.Format("{0}\\{1}", this.ServerName, this.QueuePath);
            this.IsLocal = true;
            if (this.ServerName.Length > 1 &&
                !this.ServerName.ToLower().Equals(Environment.MachineName.ToLower()))
            {
                this.QueuePathName = string.Format(
                    "FormatName:Direct=OS:{0}\\{1}",
                    this.ServerName,
                    this.QueuePath);
                this.IsLocal = false;
            }

            var msmqRepository = ServiceLocator.Resolve<IMsmqRepository>();
            if (this.IsLocal && !msmqRepository.Exists(this.QueuePathName))
            {
                msmqRepository.Create(this.QueuePathName, this.IsTransactional);
            }
        }
    }
}