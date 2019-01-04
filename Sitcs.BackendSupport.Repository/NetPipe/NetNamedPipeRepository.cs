// **************************************************************************
// <copyright file="NetNamedPipeRepository.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Net Named Pipe repository.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NetNamedPipeRepository : INetNamedPipeRepository
    {
        /// <summary>
        /// Registered Message Handlers;
        /// </summary>
        private MessageHandler registeredService;

        /// <summary>
        ///  Initializes a new instance of the <see cref="NetNamedPipeRepository"/> class.
        /// </summary>
        public NetNamedPipeRepository()
        {
            this.Host = null;
            this.registeredService = null;
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
        /// Gets or sets the service host.
        /// </summary>
        public ServiceHost Host { get; set; }

        /// <summary>
        /// Start the Manager host
        /// </summary>
        /// <param name="url">Service url</param>
        /// <param name="name">Pipe name</param>
        public void RegisterService(string url, string name)
        {
            Uri serviceURI = new Uri(url);
            this.Host = new ServiceHost(this, serviceURI);

            this.Host.AddServiceEndpoint(typeof(INetNamedPipeRepository), new NetNamedPipeBinding(), name);
            this.Host.Open();
        }

        /// <summary>
        /// Send Broadcast to the different listener.
        /// </summary>
        /// <param name="settingName">Setting Name</param>
        /// <param name="settingValue">Setting Value</param>
        public void SendBroadcast(string settingName, object settingValue)
        {
            if (this.registeredService != null)
            {
                this.registeredService(settingName, settingValue);
            }
        }

        /// <summary>
        /// Send Broadcast message to the different listener
        /// </summary>
        /// <param name="settingName">Setting Name</param>
        /// <param name="settingValue">Setting Value</param>
        /// <param name="endPoint">End Point</param>
        public void SendBroadcastToListener(string settingName, object settingValue, EndpointAddress endPoint)
        {
            INetNamedPipeRepository client =
                ChannelFactory<INetNamedPipeRepository>.CreateChannel(new NetNamedPipeBinding(), endPoint);
            client.SendBroadcast(settingName, settingValue);
        }
    }
}
