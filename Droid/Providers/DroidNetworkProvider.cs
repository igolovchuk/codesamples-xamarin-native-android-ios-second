using System;
using Android.Content;
using Android.Net;
using XamarinNativeDemo.Providers;

namespace XamarinNativeDemo.Droid.Providers
{
    public class DroidNetworkProvider : NetworkProvider
    {
        private ConnectivityManager _connectivityManager;

        public DroidNetworkProvider()
        {
            _connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
        }

        public override bool IsConnected() {
            var connectionInfo = _connectivityManager.ActiveNetworkInfo;
            return connectionInfo != null && connectionInfo.IsConnected;
        }
    }
}
