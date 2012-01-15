namespace ServiceModelContrib.IoC.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting;
    using System.Text;
    using Microsoft.Practices.Unity;

    ///<summary>
    /// Singleton for holding the Application-wide Unity Container.
    ///</summary>
    public static class UnityApplicationContainer
    {
        ///<summary>
        ///</summary>
        public const string UnityRegistryAssembliesAppSettingsKey = "UnityRegistryAssemblies";

        private const char AppSettingsValueSeparator = ',';
        private static readonly object LockObject = new object();
        private static IUnityContainer _container;
        private static List<UnityRegistry> _unityRegistries;

        /// <summary>
        /// Gets the <see cref="IUnityContainer"/> instance. Will scan for UnityRegistry classes and used them to configure the container (only the first time).
        /// </summary>
        public static IUnityContainer Instance
        {
            get
            {
                lock (LockObject)
                {
                    if (_container == null)
                    {
                        _container = CreateAndConfigureContainer();
                    }
                    return _container;
                }
            }
        }

        ///<summary>
        /// Returns a list of the discovered <see cref="UnityRegistry"/> instances.
        ///</summary>
        public static ReadOnlyCollection<UnityRegistry> UnityRegistries
        {
            get
            {
                lock (LockObject)
                {
                    return new ReadOnlyCollection<UnityRegistry>(_unityRegistries);
                }
            }
        }

        ///<summary>
        /// Should only be used for testing
        ///</summary>
        ///<param name="container"></param>
        public static void SetInstanceForTest(IUnityContainer container)
        {
            _container = container;
        }

        private static IUnityContainer CreateAndConfigureContainer()
        {
            LoadAssembliesFromAppSettings();

            var container = new UnityContainer();
            _unityRegistries = GetUnityRegistryTypes();
            foreach (UnityRegistry unityRegistry in _unityRegistries)
            {
                unityRegistry.ConfigureContainer(container);
            }
            return container;
        }

        private static List<UnityRegistry> GetUnityRegistryTypes()
        {
            try
            {
                return (from t in AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                        where typeof (UnityRegistry).IsAssignableFrom(t) && t.IsAbstract == false && t.IsPublic
                        select (UnityRegistry) Activator.CreateInstance(t)).ToList();
            }
            catch (ReflectionTypeLoadException reflectionTypeLoadException)
            {
                var loaderExceptions = new StringBuilder();

                foreach (Exception loaderException in reflectionTypeLoadException.LoaderExceptions)
                {
                    loaderExceptions.AppendFormat("\n============================\nLoaderException:\n{0}",
                                                  loaderException.Message);
                }
                string exceptionMessage =
                    string.Format("ReflectionTypeLoadException: Unable to load one or more of the requested types {0}",
                                  loaderExceptions);

                throw new ServerException(exceptionMessage, reflectionTypeLoadException);
            }
        }

        private static void LoadAssembliesFromAppSettings()
        {
            string unityAssemblies = ConfigurationManager.AppSettings[UnityRegistryAssembliesAppSettingsKey];
            if (string.IsNullOrEmpty(unityAssemblies)) return;

            string[] assemblyNames = unityAssemblies.Split(AppSettingsValueSeparator);
            foreach (string assemblyName in assemblyNames)
            {
                Debug.WriteLine("#####  Loading " + assemblyName);
                Assembly.Load(assemblyName);
            }
        }
    }
}