// **************************************************************************
// <copyright file="INetNamedPipeRepository.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Message Handler delegate
    /// </summary>
    /// <param name="settingName">Setting name</param>
    /// <param name="settingValue">Setting value</param>
    public delegate void MessageHandler(string settingName, object settingValue);

    /// <summary>
    /// Net Named Pipe repository interface.
    /// </summary>
    [ServiceContract]
    public interface INetNamedPipeRepository
    {
        /// <summary>
        /// Gets or sets Message Received
        /// </summary>
        MessageHandler MessageReceived { get; set; }

        /// <summary>
        /// Gets or sets the service host.
        /// </summary>
        ServiceHost Host { get; set; }

        /// <summary>
        /// Start the Manager host
        /// </summary>
        /// <param name="url">Service url</param>
        /// <param name="name">Pipe name</param>
        void RegisterService(string url, string name);

        /// <summary>
        /// Send broadcast to the listeners.
        /// </summary>
        /// <param name="settingName">Setting name</param>
        /// <param name="settingValue">Setting value</param>
        [OperationContract]
        void SendBroadcast(string settingName, object settingValue);

        /// <summary>
        /// Send Broadcast message to the different listener
        /// </summary>
        /// <param name="settingName">Setting Name</param>
        /// <param name="settingValue">Setting Value</param>
        /// <param name="endPoint">End Point</param>
        void SendBroadcastToListener(string settingName, object settingValue, EndpointAddress endPoint);
    }
}
