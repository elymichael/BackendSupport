// **************************************************************************
// <copyright file="ServiceLocator.cs" company="Sitcs EIRL">
//     Copyright ©SitcsRD 2018. All rights reserved.
// </copyright>
// <author>Ely Michael Núñez</author>
// **************************************************************************

namespace Sitcs.BackendSupport.Repository
{
    using System;
    using Unity;
    using Unity.Lifetime;

    /// <summary>
    /// ServiceLocator class, which is uses for IOC and dependency injection.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// Gets or sets the Unity Container
        /// </summary>
        public static IUnityContainer Container { get; set; }

        /// <summary>
        /// Method used to resolve an instance of a registered type.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <returns>Returns a new instance of the instance type for registered types. 
        /// Returns the same instance for registered instances.</returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        /// <summary>
        /// Method used to resolve an instance of a registered type.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instanceName">The name of the instance.</param>
        /// <returns>Returns the same instance for registered instances.</returns>
        public static T Resolve<T>(string instanceName)
        {
            return Container.Resolve<T>(instanceName);
        }

        /// <summary>
        /// Method used to resolve an instance of a registered type.
        /// </summary>
        /// <param name="instanceType">The type of the instance.</param>
        /// <returns>Returns a new instance of the instance type for registered types. 
        /// Returns the same instance for registered instances.</returns>
        public static object Resolve(Type instanceType)
        {
            return Container.Resolve(instanceType);
        }

        /// <summary>
        /// Method used to register an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">An instance of the type.</param>
        public static void RegisterInstance<T>(T instance)
        {
            Container.RegisterInstance<T>(instance);
        }

        /// <summary>
        /// Method used to register an instance of a given type.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instanceName">The name of the instance.</param>
        /// <param name="instance">An instance of the type.</param>
        public static void RegisterInstance<T>(string instanceName, T instance)
        {
            Container.RegisterInstance<T>(instanceName, instance);
        }

        /// <summary>
        /// Method used to register a type.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <typeparam name="U">The type of the target.</typeparam>
        public static void RegisterType<T, U>() where U : T
        {
            Container.RegisterType<T, U>();
        }

        /// <summary>
        /// Method used to register a type.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <typeparam name="U">The type of the target.</typeparam>
        /// <param name="lifetimeManager">The lifetime Manger.</param>
        public static void RegisterType<T, U>(LifetimeManager lifetimeManager) where U : T
        {
            Container.RegisterType<T, U>(lifetimeManager);
        }
    }
}
