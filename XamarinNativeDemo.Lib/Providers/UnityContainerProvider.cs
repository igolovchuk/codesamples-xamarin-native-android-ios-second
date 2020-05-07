using System;
using System.Net.Http;
using XamarinNativeDemo.Interfaces;
using Unity;
using Unity.Lifetime;

namespace XamarinNativeDemo.Providers
{
    public class UnityContainerProvider 
    {
        /// <summary>
        /// Initializes static members of the <see cref="UnityContainerProvider"/> class.
        /// </summary>
        static UnityContainerProvider()
        {
            Container = new UnityContainer();
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public static IUnityContainer Container { get; set; }

        /// <summary>
        /// Registers the application types.
        /// </summary>
        public virtual void RegisterApplicationTypes()
        {
            Container 
                .RegisterInstance(new HttpClient(), new ContainerControlledLifetimeManager())
                .RegisterType<IJsonProvider, JsonProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IStatisticDataProvider, StatisticDataProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<ICacheDataProvider, CacheDataProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<ICarDataProvider, CarDataProvider>(new ContainerControlledLifetimeManager());
            //.RegisterType<MethodResultCache>(
            //    new ContainerControlledLifetimeManager(),
            //    new InjectionConstructor(Constants.CACHE_CONFIG_NAME))
            //.RegisterType<INetDocumentsAdapter, NetDocumentsAdapter>(
            //    new ContainerControlledLifetimeManager(),
            //    new InterceptionBehavior<PolicyInjectionBehavior>(),
            //    new Interceptor<TransparentProxyInterceptor>())
            //.RegisterInstance<IHostSettings>(Common.Properties.Settings.Default)
            //.RegisterType<IHostSettingsProvider, HostSettingsProvider>(
            //    new InjectionProperty(Common.Constants.HOST_SETTINGS_PROPERTY_NAME, Properties.Settings.Default.Hosts))
            //.RegisterType<IConfigurationProvider, RegistryConfigurationProvider>(Common.Constants.NDMAIL_REGISTRY_MAPPING,
            //                                                                     new InjectionConstructor(Common.Constants.NDMAIL_SETTINGS_REGISTRY_PATH,
            //                                                                                              new ResolvedParameter<IExceptionLogger>()))
            //.RegisterType<IConfigurationProvider, RegistryConfigurationProvider>(Common.Constants.ECHOING_CLIENT_REGISTRY_MAPPING,
            //                                                                     new InjectionConstructor(Common.Constants.ECHOING_CLIENT_SETTINGS_REGISTRY_PATH,
            //                                                                                              new ResolvedParameter<IExceptionLogger>()))
            //.RegisterType<ILocalDbContext, LocalDbContext>(new ContainerControlledLifetimeManager(), new InjectionConstructor(Properties.Settings.Default.LocalDatabaseName))
        }
    }
}
