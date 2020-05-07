
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using XamarinNativeDemo.Models;

namespace XamarinNativeDemo.Droid.Activities
{
    public abstract class BaseDialogFragment : DialogFragment
    {
        private string _updateProperty;
        private int _layout;
        private string _title;

        protected View _baseView;
        public event Action<string, NameValueItem> ResultReceived;

        public BaseDialogFragment(string title, string updateProperty, int layout)
        {
            _title = title;
            _updateProperty = updateProperty;
            _layout = layout;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetStyle(DialogFragmentStyle.Normal, Resource.Style.pickerStyle);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _baseView = inflater.Inflate(_layout, container, false);
            var pickerCloseButton = _baseView.FindViewById<ImageView>(Resource.Id.btnCloseDialog);
            var pickerTitle = _baseView.FindViewById<TextView>(Resource.Id.pickerTitle);

            pickerTitle.Text = _title;
            pickerCloseButton.Click += (sender, e) => Close();

            this.Cancelable = false;

            return _baseView;
        }

        protected void InvokeResultReceived(NameValueItem nameValueItem){
            ResultReceived?.Invoke(_updateProperty, nameValueItem);
        }

        protected virtual void Close()
        {
            Dismiss();
            Dispose();
        }
    }
}
