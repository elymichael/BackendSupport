// **************************************************************************
// <copyright file="MsmqRepository.cs" company="Sitcs EIRL">
//     Copyright ©Sitcs 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************
namespace Sitcs.BackendSupport.Repository
{
    using System;
    using System.Linq;
    using System.Messaging;

    /// <summary>
    /// Message queue repository
    /// </summary>
    public class MsmqRepository : IMsmqRepository
    {
        /// <summary>
        /// Event to get the message from the repository to the business logic.
        /// </summary>
        public event EventHandler<EngineEventArgs> MessageQueuesProcessed;

        /// <summary>
        /// Gets or sets a value indicating whether the message queue is transactional or not.
        /// </summary>
        public bool IsTransactional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an array of queues to process them asynchronously in a transaction queue.
        /// </summary>
        public MessageQueue[] Queues { get; set; }

        /// <summary>
        /// Close Message Queue.
        /// </summary>        
        public void Close()
        {
            foreach (var queue in this.Queues)
            {
                queue.PeekCompleted -= this.PeekCompleted;
                queue.Close();
            }
        }

        /// <summary>
        /// Create a message queue using queue path name
        /// </summary>
        /// <param name="pathName">Message queue path</param>
        /// <param name="isTransactional">Indicates if it is transactional or not</param>
        public void Create(string pathName, bool isTransactional)
        {
            MessageQueue.Create(pathName, isTransactional);
        }

        /// <summary>
        /// Verifies whether message queue path exists.
        /// </summary>
        /// <param name="pathName">message queue path</param>
        /// <returns>message queue exists or not</returns>
        public bool Exists(string pathName)
        {
            return MessageQueue.Exists(pathName);
        }

        /// <summary>
        /// Get Total Messages
        /// </summary>        
        /// <returns>Total Messages</returns>
        public int GetTotalMessages()
        {
            return this.Queues[0].GetAllMessages().Count();
        }

        /// <summary>
        /// Load Message Queue array.
        /// </summary>
        /// <param name="pathName">message queue path</param>
        /// <param name="totalQueue">Parallel total queue to load</param>
        /// <param name="messageFormatter">Message formatter</param>
        public void LoadQueue(string pathName, int totalQueue, IMessageFormatter messageFormatter)
        {
            this.Queues = Enumerable.Range(0, totalQueue)
           .Select(i =>
           {
               var queue = new MessageQueue(pathName, QueueAccessMode.Receive)
               {
                   Formatter = messageFormatter
               };

               queue.MessageReadPropertyFilter.SetAll();
               return queue;
           })
           .ToArray();
        }

        /// <summary>
        /// Start Message Queue
        /// </summary>        
        public void Start()
        {
            foreach (var queue in this.Queues)
            {
                queue.PeekCompleted += this.PeekCompleted;
                queue.BeginPeek();
            }
        }

        /// <summary>
        /// Write message to Message Queue.
        /// </summary>
        /// <param name="pathName">Path name</param>
        /// <param name="message">Message to write</param>
        /// <param name="isLocal">Indicates whether is local</param>
        /// <param name="isTransactional">Indicates whether is transactional</param>
        /// <param name="messageFormatter">Message Formatter</param>
        /// <returns>return id of the message</returns>
        public string WriteMessage(
            string pathName,
            object message,
            bool isLocal,
            bool isTransactional,
            IMessageFormatter messageFormatter)
        {
            MessageQueue messageQueue = new MessageQueue(pathName, QueueAccessMode.Send);
            if (isLocal)
            {
                messageQueue.Label = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss");
            }

            messageQueue.Formatter = messageFormatter;

            if (isTransactional)
            {
                this.WriteTransactionalQueue(messageQueue, message);
            }
            else
            {
                messageQueue.Send(message);
            }

            string id = messageQueue.Id.ToString();
            messageQueue.Close();

            return id;
        }

        /// <summary>
        /// Peek completed event
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="eventArg">Event argument</param>
        private void PeekCompleted(object sender, System.Messaging.PeekCompletedEventArgs eventArg)
        {
            object message = new object();

            ((MessageQueue)sender).EndPeek(eventArg.AsyncResult);

            if (this.IsTransactional)
            {
                message = this.ReceiveMessageTransactional((MessageQueue)sender);
            }
            else
            {
                message = this.ReceiveMessage((MessageQueue)sender);
            }

            this.MessageQueuesProcessed(this, new EngineEventArgs(new object[] { message }));
        }

        /// <summary>
        /// Receive transaction message.
        /// </summary>
        /// <param name="queue">Message queue</param>
        /// <returns>Message Content</returns>
        private object ReceiveMessage(MessageQueue queue)
        {
            Message message = queue.Receive();

            return message.Body;
        }

        /// <summary>
        /// Receive transactional message
        /// </summary>
        /// <param name="queue">Message Queue</param>        
        /// <returns>Message Content</returns>
        private object ReceiveMessageTransactional(MessageQueue queue)
        {
            object messagecontent = new object();
            var transaction = new MessageQueueTransaction();
            transaction.Begin();
            try
            {
                Message message = queue.Receive(transaction);
                messagecontent = message.Body;

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Abort();
            }

            return messagecontent;
        }

        /// <summary>
        /// Write message content to the message queue transactional processor.
        /// </summary>
        /// <param name="queue">message queue object</param>
        /// <param name="message">message content</param>
        private void WriteTransactionalQueue(MessageQueue queue, object message)
        {
            var transaction = new MessageQueueTransaction();

            transaction.Begin();

            queue.Send(message, transaction);

            transaction.Commit();
        }
    }
}
