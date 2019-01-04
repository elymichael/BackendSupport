// **************************************************************************
// <copyright file="TestHelper.cs" company="Sitcs EIRL">
//     Copyright ©Sitcs 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Messaging;
    using System.Net.Mail;
    using System.ServiceModel;
    using System.Text;
    using System.Xml;
    using Moq;
    using Unity;

    /// <summary>
    /// TestHelper class, which will be used help building Unit Tests.
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// Does the initial class setup. Is use primarily to Reset the ServiceLocator Container.
        /// </summary>
        public static void Initialize()
        {
            ServiceLocator.Container = new UnityContainer();
        }

        /// <summary>
        /// Helper class used to create a SQL Parameter.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="type">Parameter type</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Returns a SQLParameter</returns>
        public static SqlParameter CreateSqlParameter(string name, object type, string value)
        {
            SqlParameter parameter = new SqlParameter(name, type);
            parameter.Value = value;

            return parameter;
        }        

        /// <summary>
        /// Helper method to load the default mocks for the FileSystem repository.
        /// </summary>
        /// <param name="fileExist">Indicates if file exists.</param>
        /// <param name="directoryExists">indicates if directory exists.</param>
        public static void LoadMockFileSystemRepository(
            bool fileExist = false,
            bool directoryExists = true)
        {
            var mockFileSystemRepository = new Moq.Mock<IFileSystemRepository>();
            mockFileSystemRepository.Setup(x => x.Delete(
                It.IsAny<string>()));

            mockFileSystemRepository.Setup(x => x.DirectoryExists(
                It.IsAny<string>()))
                .Returns(directoryExists);

            mockFileSystemRepository.Setup(x => x.FileExists(
                It.IsAny<string>()))
                .Returns(fileExist);

            mockFileSystemRepository.Setup(x => x.GetFileDirectory(
                It.IsAny<string>()))
                .Returns(string.Empty);

            mockFileSystemRepository.Setup(x => x.GetFileLength(
                It.IsAny<string>()))
                .Returns(5);

            mockFileSystemRepository.Setup(x => x.GetPathCombine(
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns(string.Empty);

            mockFileSystemRepository.Setup(x => x.Move(
                It.IsAny<string>(),
                It.IsAny<string>()));

            mockFileSystemRepository.Setup(x => x.ReadToEnd(
                It.IsAny<string>()))
                .Returns(string.Empty);

            mockFileSystemRepository.Setup(x => x.ReadAllBytes(
                It.IsAny<string>()))
                .Returns(new byte[] { 0x50, 0x4b, 0x03, 0x04 });

            mockFileSystemRepository.Setup(x => x.WriteFile(
                It.IsAny<string>(),
                It.IsAny<object>()));

            ServiceLocator.Container.RegisterInstance<IFileSystemRepository>(mockFileSystemRepository.Object);
        }        

        /// <summary>
        /// Helper method to load the default mocks for the MSMQ repository.
        /// </summary>
        public static void LoadMockMsmqRepository()
        {
            var mockMsmqRepository = new Moq.Mock<IMsmqRepository>();
            mockMsmqRepository.Setup(x => x.Close());

            mockMsmqRepository.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<bool>()));

            mockMsmqRepository.Setup(x => x.Exists(
                It.IsAny<string>()))
                .Returns(true);

            mockMsmqRepository.Setup(x => x.GetTotalMessages())
                .Returns(1);

            mockMsmqRepository.Setup(x => x.LoadQueue(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<IMessageFormatter>()));

            mockMsmqRepository.Setup(x => x.Start());

            mockMsmqRepository.Setup(x => x.WriteMessage(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<System.Messaging.IMessageFormatter>()));

            ServiceLocator.Container.RegisterInstance<IMsmqRepository>(mockMsmqRepository.Object);
        }

        /// <summary>
        /// Helper method to load the default mocks for the MSMQ repository.
        /// </summary>
        public static void LoadMockMsmqRepositoryServiceManager()
        {
            var mockMsmqRepository = new Moq.Mock<IMsmqRepository>();
            mockMsmqRepository.Setup(x => x.Exists(
                It.IsAny<string>()))
                .Returns(It.IsAny<bool>);

            mockMsmqRepository.Setup(x => x.Create(
                It.IsAny<string>(),
                It.IsAny<bool>()));

            mockMsmqRepository.Setup(x => x.WriteMessage(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<System.Messaging.IMessageFormatter>()));

            mockMsmqRepository.Setup(x => x.Start()).Raises(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(new object[] { "Name", "Value" }));

            mockMsmqRepository.Setup(x => x.Close());

            mockMsmqRepository.Setup(x => x.GetTotalMessages()).Returns(It.IsAny<int>());

            mockMsmqRepository.Raise(
                x => x.MessageQueuesProcessed += null,
                new EngineEventArgs(It.IsAny<string>()));

            ServiceLocator.Container.RegisterInstance<IMsmqRepository>(mockMsmqRepository.Object);
        }        
    }
}
