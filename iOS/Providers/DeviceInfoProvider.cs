using System;
using XamarinNativeDemo.Interfaces;
using XamarinNativeDemo.Models;
using UIKit;

namespace XamarinNativeDemo.iOS.Providers
{
    public class DeviceInfoProvider : IDeviceInfoProvider
    {
        public DeviceInfo GetDeviceInfo()
        {
            var device = UIDevice.CurrentDevice;

            return new DeviceInfo
            {
                Id = device.IdentifierForVendor.ToString(),
                Model = device.Model,
                Version = $"{device.SystemName} - {device.SystemVersion}"
            };
        }
    }
}
