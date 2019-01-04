// **************************************************************************
// <copyright file="MessageQueueusEventArgs.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.MessageQueue
{
    using System;

    /// <summary>
    /// Special EventArgs class to hold info about Message Queues processed.
    /// </summary>
    public class MessageQueueusEventArgs : EventArgs
    {
        /// <summary>
        /// value argument to store the quantity of message processed.
        /// </summary>
        private double valueArg;

        /// <summary>
        /// value message.
        /// </summary>
        private object valueMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageQueueusEventArgs" /> class.
        /// </summary>
        /// <param name="value">Value argument</param>
        /// <param name="message">String Message</param>
        public MessageQueueusEventArgs(int value, object message)
        {
            this.valueArg = value;
            this.valueMessage = message;
        }

        /// <summary>
        /// Gets the Value argument.
        /// </summary>
        public double ValueArg
        {
            get { return this.valueArg; }
        }

        /// <summary>
        /// Gets the Value Message.
        /// </summary>
        public object ValueMessage
        {
            get { return this.valueMessage; }
        }
    }
}
