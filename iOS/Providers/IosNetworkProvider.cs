using System;
using XamarinNativeDemo.iOS.Adapters;
using XamarinNativeDemo.Providers;

namespace XamarinNativeDemo.iOS.Providers
{
    public class IosNetworkProvider : NetworkProvider
    {
        public override bool IsConnected()
        {
            return Reachability.IsHostReachable(Constants.ApiAutoBaseUrl);
        }
    }
}
