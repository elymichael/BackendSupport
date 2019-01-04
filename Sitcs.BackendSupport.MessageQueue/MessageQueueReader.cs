// **************************************************************************
// <copyright file="MessageQueueReader.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.MessageQueue
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Messaging;
    using System.Threading;
    using Sitcs.BackendSupport.Repository;

    /// <summary>
    /// Message queue processor
    /// </summary>    
    public class MessageQueueReader : MessageQueueProcessor
    {
        /// <summary>
        /// Variable to manage the Transmit Queue Information.
        /// </summary>
        private Process transmitQueue;

        /// <summary>
        /// Store the total or queues pending to process.
        /// </summary>        
        private int counter;

        /// <summary>
        /// Store the total or queues pending to process.
        /// </summary>        
        private int total;

        /// <summary>
        /// Set the total of message queues to instantiate.
        /// </summary>
        private int totalMessageQueues;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueReader" /> class.
        /// </summary>
        /// <param name="queuePath">Message queue path.</param>
        /// <param name="isTransactional">Is Transactional</param>
        /// <param name="serverName">Server Name</param>
        /// <param name="totalQueue">Total Queue</param>
        public MessageQueueReader(
            string queuePath,
            bool isTransactional,
            string serverName,
            int totalQueue = 1)
            : base(queuePath, isTransactional, serverName)
        {
            this.LastPeekTime = DateTime.Now;
            this.totalMessageQueues = totalQueue;

            var msmqRepository = ServiceLocator.Resolve<IMsmqRepository>();
            msmqRepository.IsTransactional = this.IsTransactional;
            msmqRepository.MessageQueuesProcessed += this.MessageQueuesProcessed;
        }

        /// <summary>
        /// Delegate method to receive a process.
        /// </summary>
        /// <param name="envelope">envelope object</param>
        /// <returns>Boolean flag</returns>
        public delegate bool Process(object envelope);

        /// <summary>
        /// Event to report the number or message queues processed.
        /// </summary>
        public event EventHandler<MessageQueueusEventArgs> MessageQueuesReaderProcessed;

        /// <summary>
        /// Gets a value indicating whether the processor is open.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Last peek time.
        /// </summary>
        public DateTime LastPeekTime { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the service is closing.
        /// </summary>
        private bool IsClosing { get; set; }

        /// <summary>
        /// Start the message queue reader.
        /// </summary>
        /// <param name="callbackQueue">Sub-Process received by parameter</param>
        public void Start(Process callbackQueue)
        {
            this.transmitQueue = callbackQueue;

            this.Start();
        }

        /// <summary>
        /// Close the message queue processor.
        /// </summary>
        public void Stop()
        {
            this.IsClosing = true;

            ServiceLocator.Resolve<IMsmqRepository>().Close();

            this.IsClosing = this.IsOpen = false;
        }

        /// <summary>
        /// Reload Message Queues.
        /// </summary>
        public void Reload()
        {
            int messages = ServiceLocator.Resolve<IMsmqRepository>().GetTotalMessages();
            if (messages > 0)
            {
                this.Stop();
                this.Start();
            }
        }

        /// <summary>
        /// Report message queue reader processed per each peek.
        /// </summary>
        /// <param name="eventReader">Event for message queues processed</param>
        protected void OnMessageQueuesReaderProcessed(MessageQueueusEventArgs eventReader)
        {
            EventHandler<MessageQueueusEventArgs> handler = this.MessageQueuesReaderProcessed;
            if (handler != null)
            {
                handler(this, eventReader);
            }
        }

        /// <summary>
        /// Received Message Queue content from repository
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="eventArg">Event argument</param>
        private void MessageQueuesProcessed(object sender, EngineEventArgs eventArg)
        {
            this.Execute(eventArg.Value);
        }

        /// <summary>
        /// Start the message queue reader.
        /// </summary>
        private void Start()
        {
            if (!this.IsOpen)
            {
                this.LastPeekTime = DateTime.Now;

                var msmqRepository = ServiceLocator.Resolve<IMsmqRepository>();
                msmqRepository.LoadQueue(this.QueuePath, this.totalMessageQueues, this.MessageFormatter);
                msmqRepository.Start();

                this.IsOpen = true;
            }
        }

        /// <summary>
        /// Method to manage the message processing.
        /// </summary>
        /// <param name="messagecontent">Messaging message queue</param>
        private void Execute(object messagecontent)
        {
            string filename = string.Empty;

            Interlocked.Increment(ref this.total);
            Interlocked.Increment(ref this.counter);

            this.OnMessageQueuesReaderProcessed(new MessageQueueusEventArgs(this.total, messagecontent));

            var fileSystemRepository = ServiceLocator.Resolve<IFileSystemRepository>();
            if (this.PersistMessageToDisk)
            {
                filename = fileSystemRepository.GetPathCombine(this.MessagePath, messagecontent.ToString());
                messagecontent = fileSystemRepository.ReadToEnd(filename);

                fileSystemRepository.Move(filename, string.Join(".", filename, "Read"));
            }

            this.transmitQueue(messagecontent);

            if (this.PersistMessageToDisk)
            {
                fileSystemRepository.Delete(string.Join(".", filename, "Read"));
            }

            Interlocked.Decrement(ref this.counter);
        }
    }
}
