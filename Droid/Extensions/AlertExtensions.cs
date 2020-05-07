using System;
using Android.App;
using Android.Graphics;
using Android.Widget;
using XamarinNativeDemo.Droid.Providers;

namespace XamarinNativeDemo.Droid.Extensions
{
    public static class AlertExtensions
    {
        public static void ShowError(this Activity scope, string message)
        {
            ShowOneButtonAlert(scope, LocalizationProvider.Translate(Resource.String.title_error), message);
        }

        public static void ShowInfo(this Activity scope, string message)
        {
            ShowOneButtonAlert(scope, LocalizationProvider.Translate(Resource.String.title_info), message);
        }

        public static void ShowWarning(this Activity scope, string message)
        {
            ShowOneButtonAlert(scope, LocalizationProvider.Translate(Resource.String.title_delete), message);
        }

        public static void ShowDialog(this Activity scope, string title, string message)
        {
            ShowOneButtonAlert(scope, title, message);
        }

        public static void ShowDialog(this Activity scope, string title, string message, Action action, int actionTitle)
        {
            ShowOneButtonAlert(scope, title, message, action, actionTitle);
        }

        private static void ShowOneButtonAlert(Activity scope, string type, string message, Action action = null, int? actionTitle = null)
        {
            var custom_title = new TextView(scope);
            custom_title.Text = type;
            custom_title.SetBackgroundResource(Resource.Color.main);
            custom_title.SetPadding(10, 10, 10, 10);
            custom_title.Gravity = Android.Views.GravityFlags.Center;
            custom_title.SetTextColor(Color.White);
            custom_title.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            custom_title.SetTypeface(custom_title.Typeface, TypefaceStyle.Bold);

            var buttonTitle = actionTitle ?? Resource.String.ok;

            var builder = new AlertDialog.Builder(scope)
             .SetNeutralButton(LocalizationProvider.Translate(buttonTitle), (s, ev) => action?.Invoke())
             .SetMessage(message)
             .SetCustomTitle(custom_title)
             .SetCancelable(false)
             .Show();

            builder.Window.SetBackgroundDrawableResource(Resource.Drawable.solid_shape);
            //builder.Window.SetBackgroundDrawableResource(Resource.Color.windowBackground);

            var button = builder.FindViewById<Button>(Android.Resource.Id.Button3);
            button.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            button.SetTextColor(Color.Rgb(46, 167, 243));// light_blue
            button.LayoutParameters = new LinearLayout.LayoutParams(Android.Views.ViewGroup.LayoutParams.MatchParent, Android.Views.ViewGroup.LayoutParams.WrapContent);
            button.SetBackgroundResource(Resource.Drawable.single_border);
            button.Gravity = Android.Views.GravityFlags.CenterHorizontal;
        }

        public static void ShowOkCancelAlert(this Activity scope, string type, string message, Action okHandler)
        {           
            var custom_title = new TextView(scope);
            custom_title.Text = type;
            custom_title.SetBackgroundResource(Resource.Color.main);
            custom_title.SetPadding(10, 10, 10, 10);
            custom_title.Gravity = Android.Views.GravityFlags.Center;
            custom_title.SetTextColor(Color.White);
            custom_title.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            custom_title.SetTypeface(custom_title.Typeface, TypefaceStyle.Bold);

            var builder = new AlertDialog.Builder(scope)
              .SetPositiveButton(Resource.String.ok, (s, e) => scope.RunOnUiThread(okHandler))
              .SetNegativeButton(Resource.String.cancel, (s, e) => { })
              .SetMessage(message)
              .SetCustomTitle(custom_title)
              .Show();

            builder.Window.SetBackgroundDrawableResource(Resource.Drawable.solid_shape);
            //builder.Window.SetBackgroundDrawableResource(Resource.Color.windowBackground);

            var messageText = builder.FindViewById<TextView>(Android.Resource.Id.Message);
            messageText.Gravity = Android.Views.GravityFlags.Center;

            var btnwidth = (int)(scope.Resources.DisplayMetrics.WidthPixels / 2 - 40 * scope.Resources.DisplayMetrics.Density);
            var buttonCancel = builder.FindViewById<Button>(Android.Resource.Id.Button2);
            buttonCancel.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            buttonCancel.LayoutParameters = new LinearLayout.LayoutParams(btnwidth, Android.Views.ViewGroup.LayoutParams.WrapContent);
           // buttonCancel.SetBackgroundResource(Resource.Drawable.right_button_border);
            buttonCancel.Gravity = Android.Views.GravityFlags.Center;

            var buttonOk = builder.FindViewById<Button>(Android.Resource.Id.Button1);
            buttonOk.SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
            buttonOk.LayoutParameters = new LinearLayout.LayoutParams(btnwidth, Android.Views.ViewGroup.LayoutParams.WrapContent);
            buttonOk.Gravity = Android.Views.GravityFlags.Center;
        }

        public static ProgressDialog ShowProgress(this Activity scope){
            var mDialog = new ProgressDialog(scope, Resource.Style.dialogStyle);
                mDialog.SetMessage(LocalizationProvider.Translate(Resource.String.message_loading));
                mDialog.SetCancelable(false);
                mDialog.Show();
           
            return mDialog;
        }
    }
}
