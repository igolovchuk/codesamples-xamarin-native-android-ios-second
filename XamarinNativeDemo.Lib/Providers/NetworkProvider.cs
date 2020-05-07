using XamarinNativeDemo.Interfaces;

namespace XamarinNativeDemo.Providers
{
    public abstract class NetworkProvider : INetworkProvider
    {
        public abstract bool IsConnected();
    }
}
