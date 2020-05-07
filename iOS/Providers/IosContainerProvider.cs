using System;
using XamarinNativeDemo.Enums;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Providers;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace XamarinNativeDemo.iOS.Providers
{
    public class IosContainerProvider : UnityContainerProvider
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        private static Lazy<IosContainerProvider> _instanse = new Lazy<IosContainerProvider>(() => new IosContainerProvider());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static IosContainerProvider Instance => _instanse.Value;

        public override void RegisterApplicationTypes()
        {
            base.RegisterApplicationTypes();
            Container.RegisterType<IFileSystemProvider, FileSystemProvider>()
                     .RegisterType<INetworkProvider, IosNetworkProvider>(new ContainerControlledLifetimeManager())
                     .RegisterType<IDeviceInfoProvider, DeviceInfoProvider>(new ContainerControlledLifetimeManager());
        }
    }
}
