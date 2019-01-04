// **************************************************************************
// <copyright file="BroadCastManagerTests.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.InterCommunication.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Sitcs.BackendSupport.InterCommunication;
    using Sitcs.BackendSupport.Repository;   
    using Unity;

    /// <summary>
    /// Unit Test Settings Manager
    /// </summary>
    [TestClass]
    public class BroadCastManagerTests
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
        /// Register service host method
        /// </summary>
        [TestMethod]
        public void BroadcastManagerMethodStartHost()
        {
            // creating net pipe repository
            var mockNetNamedPipeRepository = new Mock<INetNamedPipeRepository>();
            mockNetNamedPipeRepository.Setup(x => x.RegisterService(It.IsAny<string>(), It.IsAny<string>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<INetNamedPipeRepository>(
                mockNetNamedPipeRepository.Object);

            var netNamedPipeRepository = ServiceLocator.Resolve<INetNamedPipeRepository>();
            netNamedPipeRepository.MessageReceived += this.MessageReceivedHandler;

            BroadcastManager settingsManager = new BroadcastManager();
            settingsManager.RegisterService(
                "net.pipe://localhost",
                "Host");

            Assert.IsNotNull(mockNetNamedPipeRepository.Object);
        }

        /// <summary>
        /// Send broadcast message method.
        /// </summary>
        [TestMethod]
        public void BroadcastManagerMethodSend()
        {
            // creating net pipe repository
            var mockNetNamedPipeRepository = new Mock<INetNamedPipeRepository>();
            mockNetNamedPipeRepository.Setup(x => x.RegisterService(It.IsAny<string>(), It.IsAny<string>()));
            mockNetNamedPipeRepository.Setup(x => x.SendBroadcastToListener(
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<EndpointAddress>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<INetNamedPipeRepository>(
                mockNetNamedPipeRepository.Object);

            var netNamedPipeRepository = ServiceLocator.Resolve<INetNamedPipeRepository>();
            netNamedPipeRepository.MessageReceived += this.MessageReceivedHandler;

            BroadcastManager settingsManager = new BroadcastManager();
            settingsManager.RegisterService("net.pipe://localhost", "Host3");

            EndpointAddress endpoint = new EndpointAddress("net.pipe://localhost/Host3");

            settingsManager.SendBroadcastToListener(
                new Dictionary<string, dynamic>()
                        {
                            { "settingName", "HostServer" },
                            { "settingValue", "message HostServer" },
                            { "endPoint", endpoint }
                        });

            Assert.IsTrue(true);
        }

        /// <summary>
        /// Send broadcast message method.
        /// </summary>
        [TestMethod]
        public void CachedBroadcastManagerMethodSendFail()
        {
            // creating net pipe repository
            var mockNetNamedPipeRepository = new Mock<INetNamedPipeRepository>();
            mockNetNamedPipeRepository.Setup(x => x.RegisterService(It.IsAny<string>(), It.IsAny<string>()));
            mockNetNamedPipeRepository.Setup(x => x.SendBroadcastToListener(
                It.IsAny<string>(), It.IsAny<object>(), It.IsAny<EndpointAddress>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<INetNamedPipeRepository>(
                mockNetNamedPipeRepository.Object);

            var netNamedPipeRepository = ServiceLocator.Resolve<INetNamedPipeRepository>();
            netNamedPipeRepository.MessageReceived += this.MessageReceivedHandler;

            BroadcastManager settingsManager = new BroadcastManager();
            settingsManager.RegisterService("net.pipe://localhost", "Host3");

            EndpointAddress endpoint = new EndpointAddress("net.pipe://localhost/Host3");
            settingsManager.SendBroadcastToListener(
                new Dictionary<string, dynamic>()
                        {
                            { "settingName", "HostServer" },
                            { "settingValue", "message HostServer" },
                            { "endPoint", endpoint }
                        });

            Assert.IsTrue(true);
        }

        /// <summary>
        /// Test property get for host.
        /// </summary>
        [TestMethod]
        public void CachedSettingManagerCachedSettingsManagerPropertyGetForHost()
        {
            var mockNetNamedPipeRepository = new Moq.Mock<INetNamedPipeRepository>();
            mockNetNamedPipeRepository.Setup(x => x.RegisterService(It.IsAny<string>(), It.IsAny<string>()));

            // add mock repositories to the container
            ServiceLocator.Container.RegisterInstance<INetNamedPipeRepository>(
                mockNetNamedPipeRepository.Object);

            BroadcastManager settingsManager = new BroadcastManager();
            ServiceHost host = settingsManager.Host;

            Assert.IsNull(host);
        }

        /// <summary>
        /// Manage message received handler.
        /// </summary>
        /// <param name="settingName">Setting Name</param>
        /// <param name="settingValue">Setting Value</param>
        private void MessageReceivedHandler(string settingName, object settingValue)
        {
            string var = settingName;
        }
    }
}
