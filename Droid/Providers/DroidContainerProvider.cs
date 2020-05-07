using System;
using XamarinNativeDemo.Enums;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Providers;
using Unity;
using Unity.Injection;
using Unity.Lifetime;


namespace XamarinNativeDemo.Droid.Providers
{
    public class DroidContainerProvider : UnityContainerProvider
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        private static Lazy<DroidContainerProvider> _instanse = new Lazy<DroidContainerProvider>(() => new DroidContainerProvider());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DroidContainerProvider Instance => _instanse.Value;

        public override void RegisterApplicationTypes()
        {
            base.RegisterApplicationTypes();

            Container
                .RegisterType<IFileSystemProvider, FileSystemProvider>(new InjectionConstructor(OperationSystem.Android))
                .RegisterType<INetworkProvider, DroidNetworkProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IDeviceInfoProvider, DeviceInfoProvider>(new ContainerControlledLifetimeManager());
        }
    }
}
