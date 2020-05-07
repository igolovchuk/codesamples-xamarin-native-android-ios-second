using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using XamarinNativeDemo.Droid.Providers;

namespace XamarinNativeDemo.Droid.Extensions
{
    public static class TableRowExtensions
    {
        private const string SelectorTag = "selector_tag";
        private static ColorStateList _primaryColor;

        public static void SetLoadingState(this TableRow tableRow){
            var selector = tableRow.FindViewWithTag(SelectorTag) as TextView;

            if(_primaryColor == null){
                _primaryColor = selector.TextColors;  
            }
             
            selector.Enabled = false;
            selector.SetTextColor(_primaryColor);
            selector.Text = LocalizationProvider.Translate(Resource.String.message_loading);
        }

        public static void SetActiveState(this TableRow tableRow, bool containsData)
        {
            var selector = tableRow.FindViewWithTag(SelectorTag) as TextView;
             selector.Enabled = true;

             var title = containsData ? Resource.String.select : Resource.String.message_loading_error;
             selector.Text = LocalizationProvider.Translate(title);
        }

        public static bool IsEnabled(this TableRow tableRow){
            var selector = tableRow.FindViewWithTag(SelectorTag) as TextView;
            return selector.Enabled;
        }

        public static void SetData(this TableRow tableRow, bool enabled, int textRes)
        {
            var selector = tableRow.FindViewWithTag(SelectorTag) as TextView;
            selector.Enabled = enabled;

            if(_primaryColor != null){
                selector.SetTextColor(_primaryColor);
            }

            selector.Text = LocalizationProvider.Translate(textRes);
        }

        public static void SetTitle(this TableRow tableRow, string title)
        {
            var selector = tableRow.FindViewWithTag(SelectorTag) as TextView;
          
            selector.Text = title;
            selector.Enabled = true;
            selector.SetTextColor(Color.Rgb(46, 167, 243));// light_blue
        }
    }
}
