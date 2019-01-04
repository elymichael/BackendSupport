// **************************************************************************
// <copyright file="BroadcastManager.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.InterCommunication
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sitcs.BackendSupport.Repository;

    /// <summary>
    /// Manager class.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]

    public class BroadcastManager
    {
        /// <summary>
        /// Registered Message Handlers;
        /// </summary>
        private MessageHandler registeredService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BroadcastManager"/> class.
        /// </summary>
        public BroadcastManager()
        {
            this.registeredService = null;
        }

        /// <summary>
        /// Message Handler delegate
        /// </summary>
        /// <param name="settingName">Setting name</param>
        /// <param name="settingValue">Setting value</param>
        public delegate void MessageHandler(string settingName, object settingValue);

        /// <summary>
        /// Gets the service host.
        /// </summary>
        public ServiceHost Host
        {
            get
            {
                return ServiceLocator.Resolve<INetNamedPipeRepository>().Host;
            }
        }

        /// <summary>
        /// Gets or sets Message Received
        /// </summary>
        public MessageHandler MessageReceived
        {
            get
            {
                return this.registeredService;
            }

            set
            {
                this.registeredService += value;
            }
        }

        /// <summary>
        /// Send Broadcast message to the different listener
        /// </summary>
        /// <param name="broadcastData">Dictionary containing the setting name, 
        /// setting value and endpoint.</param>
        public void SendBroadcastToListener(Dictionary<string, dynamic> broadcastData)
        {
            ServiceLocator.Resolve<INetNamedPipeRepository>()
                .SendBroadcastToListener(
                broadcastData["settingName"],
                broadcastData["settingValue"],
                broadcastData["endPoint"]);
        }

        /// <summary>
        /// Start the Manager host
        /// </summary>
        /// <param name="url">Service url</param>
        /// <param name="name">Pipe name</param>
        public void RegisterService(string url, string name)
        {
            ServiceLocator.Resolve<INetNamedPipeRepository>()
                .RegisterService(url, name);
        }
    }
}
