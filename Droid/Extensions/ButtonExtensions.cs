using System;
using Android.Widget;

namespace XamarinNativeDemo.Droid.Extensions
{
    public static  class ButtonExtensions
    {
        public static void SetSate(this Button button, bool enabled)
        {
            button.Enabled = enabled;
            var background = enabled ? Resource.Color.lightBlue : Resource.Color.light_grey;
            button.SetBackgroundResource(background);
        }
    }
}
