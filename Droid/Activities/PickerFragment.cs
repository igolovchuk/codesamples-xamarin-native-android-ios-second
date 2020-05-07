
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using XamarinNativeDemo.Droid.Adapters;
using XamarinNativeDemo.Droid.Providers;
using XamarinNativeDemo.Models;

namespace XamarinNativeDemo.Droid.Activities
{
    public class PickerFragment : BaseDialogFragment
    {
        private List<NameValueItem> _collection;
        private List<NameValueItem> _filteredColection;
        private InputTypes _inputType;


        public PickerFragment(string title, List<NameValueItem> collection, string updateProperty, InputTypes inputType = InputTypes.ClassText)
            : base(title, updateProperty, Resource.Layout.PickerLayout)
        {
            _collection = collection;
            _filteredColection = collection;
            _inputType = inputType;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var pickerListView = _baseView.FindViewById<ListView>(Resource.Id.pickerList);
            var pickerSearchView = _baseView.FindViewById<SearchView>(Resource.Id.pickerSearch);

            pickerListView.Adapter = new PickerFragmentAdapter(Activity, _collection);
            pickerListView.ItemClick += PickerListView_ItemClick;
          
            pickerSearchView.SetQueryHint(LocalizationProvider.Translate(Resource.String.search_text));
            pickerSearchView.QueryTextChange += (s, e) =>
            {
                string text = e.NewText.ToLower();
                _filteredColection = _collection.Where(ac => ac.Name.ToLower().Contains(text)).ToList();
                (pickerListView.Adapter as PickerFragmentAdapter).Update(_filteredColection);
            };
           
            pickerSearchView.SetInputType(_inputType);
            pickerSearchView.OnActionViewExpanded();
            pickerSearchView.ClearFocus();

           // pickerView.SetBackgroundResource(Resource.Drawable.layout_border);

            return _baseView;
        }

        private void PickerListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var selectedModel = _filteredColection[e.Position];
            InvokeResultReceived(selectedModel);  
            Close();
        }
    }
}
