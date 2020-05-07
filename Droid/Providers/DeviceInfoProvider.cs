using System;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Models;

namespace XamarinNativeDemo.Droid.Providers
{
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        public DeviceInfo GetDeviceInfo()
        {
            return new DeviceInfo
            {
                Id = $"{Android.OS.Build.Id}{Android.OS.Build.Serial}",
                Model = $"{Android.OS.Build.Manufacturer} - {Android.OS.Build.Product} - {Android.OS.Build.Model}",
                Version = $"{Android.OS.Build.VERSION.BaseOs} - {Android.OS.Build.VERSION.SdkInt} - {Android.OS.Build.VERSION.Sdk} - {Android.OS.Build.VERSION.Codename}"
            };
        }
    }
}
