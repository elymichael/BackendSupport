// **************************************************************************
// <copyright file="MessageQueueReaderTests.cs" company="Sitcs EIRL">
//     Copyright ©Sitcs 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.MessageQueue.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sitcs.BackendSupport.MessageQueue;
    using Sitcs.BackendSupport.Repository;       
    using Unity;

    /// <summary>
    /// Message queue testing class.
    /// </summary>
    [TestClass]
    public class MessageQueueReaderTests
    {
        /// <summary>
        /// Does the necessary setup for each test prior to the test running.
        /// </summary>
        [TestInitialize]
        public void TestSetup()
        {
            // initialize the test helper so that the service locator container is reset
            TestHelper.Initialize();
        }

        /// <summary>
        /// Test class message queue reader, methods stop and reload.
        /// </summary>
        [TestMethod]
        public void MessageQueueMessageQueueReaderMethodReadMessagesStopAndReload()
        {
            var mockMsmqRepository = new Moq.Mock<IMsmqRepository>();
            mockMsmqRepository.Setup(x => x.Start()).Raises(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(string.Empty));
            mockMsmqRepository.Setup(x => x.Close());
            mockMsmqRepository.Setup(x => x.GetTotalMessages()).Returns(1);

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<IMsmqRepository>(mockMsmqRepository.Object);
            TestHelper.LoadMockFileSystemRepository();

            string queuePath = "private$\\Testing3";

            MessageQueueReader messageQueueReader = new MessageQueueReader(queuePath, true, ".");
            messageQueueReader.MessageQueuesReaderProcessed += this.MessageQueuesProcessed;
            messageQueueReader.Start(this.TransmitQueueWithTimeout);
            messageQueueReader.Stop();
            messageQueueReader.Reload();

            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test class message queue reader.
        /// </summary>
        [TestMethod]
        public void MessageQueueMessageQueueReaderMethodReadMessagesRemote()
        {
            var mockMsmqRepository = new Moq.Mock<IMsmqRepository>();
            mockMsmqRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockMsmqRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<bool>()));
            mockMsmqRepository.Setup(x => x.WriteMessage(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<System.Messaging.IMessageFormatter>()));

            mockMsmqRepository.Setup(x => x.Start()).Raises(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(string.Empty));
            mockMsmqRepository.Setup(x => x.Close());
            mockMsmqRepository.Setup(x => x.GetTotalMessages()).Returns(It.IsAny<int>());
            mockMsmqRepository.Raise(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(It.IsAny<string>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<IMsmqRepository>(mockMsmqRepository.Object);
            TestHelper.LoadMockFileSystemRepository();

            string serverName = "sedidevlab02";
            string queuePath = "private$\\testing";

            MessageQueueReader messageQueueReader =
                new MessageQueueReader(queuePath, false, serverName);

            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test class message queue reader.
        /// </summary>
        [TestMethod]
        public void MessageQueueMessageQueueReaderMethodReadMessagesNoTransactional()
        {
            var mockMsmqRepository = new Moq.Mock<IMsmqRepository>();
            mockMsmqRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            mockMsmqRepository.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<bool>()));
            mockMsmqRepository.Setup(x => x.WriteMessage(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<System.Messaging.IMessageFormatter>()));

            mockMsmqRepository.Setup(x => x.Start()).Raises(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(string.Empty));
            mockMsmqRepository.Setup(x => x.Close());
            mockMsmqRepository.Setup(x => x.GetTotalMessages()).Returns(It.IsAny<int>());
            mockMsmqRepository.Raise(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(It.IsAny<string>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<IMsmqRepository>(mockMsmqRepository.Object);
            TestHelper.LoadMockFileSystemRepository();

            string serverName = Environment.MachineName;
            string queuePath = "private$\\notrans";

            MessageQueueReader messageQueueReader = new MessageQueueReader(queuePath, false, serverName);
            messageQueueReader.PersistMessageToDisk = false;
            messageQueueReader.MessageQueuesReaderProcessed += this.MessageQueuesProcessed;
            messageQueueReader.Start(this.TransmitQueue);

            Assert.IsTrue(true);
        }

        /// <summary>
        /// Event raised by the message queue reader class
        /// </summary>
        /// <param name="sender">sender information</param>
        /// <param name="e">Event result</param>
        private void MessageQueuesProcessed(object sender, MessageQueueusEventArgs e)
        {
            string information =
                string.Format("Processed: {0}, Last Message: {1}", e.ValueArg, e.ValueMessage);
        }

        /// <summary>
        /// Example of retrieving the message content from the Message Queue Reader and 
        /// Process the message.
        /// </summary>
        /// <param name="message">Message content</param>
        /// <returns>Boolean flag</returns>
        private bool TransmitQueue(object message)
        {
            string messageContent = message.ToString();
            //// Your Code Here -- TODO!!
            return true;
        }

        /// <summary>
        /// Example of retrieving the message content from the Message Queue Reader and 
        /// Process the message.
        /// </summary>
        /// <param name="message">Message content</param>
        /// <returns>Boolean flag</returns>
        private bool TransmitQueueWithTimeout(object message)
        {
            string messageContent = message.ToString();
            System.Threading.Thread.Sleep(400);
            return true;
        }
    }
}
