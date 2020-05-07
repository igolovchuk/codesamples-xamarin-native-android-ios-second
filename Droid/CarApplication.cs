
using System;

using Android.App;
using Android.Runtime;
using XamarinNativeDemo.Droid.Providers;
using XamarinNativeDemo.Providers;

namespace XamarinNativeDemo.Droid
{
    [Application]
    public class CarApplication : Application
    {
        public CarApplication(IntPtr handle, JniHandleOwnership transer)
                  : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            DroidContainerProvider.Instance.RegisterApplicationTypes();
        }
    }
}
