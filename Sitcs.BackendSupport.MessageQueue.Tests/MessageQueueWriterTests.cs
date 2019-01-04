// **************************************************************************
// <copyright file="MessageQueueWriterTests.cs" company="Sitcs EIRL">
//     Copyright ©Sitcs 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.MessageQueue.Tests
{
    using System;
    using System.Messaging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sitcs.BackendSupport.MessageQueue;
    using Sitcs.BackendSupport.Repository;      
    using Unity;

    /// <summary>
    /// Message queue testing class.
    /// </summary>
    [TestClass]
    public class MessageQueueWriterTests
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
        /// Test class message queue writer.
        /// </summary>
        [TestMethod]
        public void MessageQueueMessageQueueWriterMethodWriteMessages()
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

            var mockFileSystemRepository = new Moq.Mock<IFileSystemRepository>();
            mockFileSystemRepository.Setup(x => x.WriteFile(It.IsAny<string>(), It.IsAny<object>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<IMsmqRepository>(mockMsmqRepository.Object);
            ServiceLocator.Container.RegisterInstance<IFileSystemRepository>(mockFileSystemRepository.Object);

            string serverName = Environment.MachineName;
            MessageQueueWriter messageQueueWriter = new MessageQueueWriter(
                "private$\\Testing", true, serverName);
            messageQueueWriter.MessageFormatter = new BinaryMessageFormatter();

            int i = 0;
            do
            {
                messageQueueWriter.WriteQueue("message " + i.ToString());
                i++;
            }
            while (i < 5);

            messageQueueWriter = new MessageQueueWriter("private$\\Testing", false, serverName);
            messageQueueWriter.WriteQueue("message ");

            Assert.IsTrue(i > 0);
        }
    }
}
