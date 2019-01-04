// **************************************************************************
// <copyright file="EngineEventArgs.cs" company="Sitcs EIRL">
//     Copyright ©Sitcs 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System;

    /// <summary>
    /// Special EventArgs class to hold info about items being processed.
    /// </summary>
    public class EngineEventArgs : EventArgs
    {
        /// <summary>
        /// Set the value argument.
        /// </summary>
        private object valueArg;

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineEventArgs" /> class.
        /// </summary>        
        /// <param name="value">String Message</param>
        public EngineEventArgs(object value)
        {
            this.valueArg = value;
        }

        /// <summary>
        /// Gets value message.
        /// </summary>
        public object Value
        {
            get
            {
                return this.valueArg;
            }
        }
    }
}
