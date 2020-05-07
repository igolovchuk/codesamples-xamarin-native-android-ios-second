using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using XamarinNativeDemo.Models;

namespace XamarinNativeDemo.Droid.Adapters
{
    public class PickerFragmentAdapter : BaseAdapter<NameValueItem>
    {
        protected List<NameValueItem> _items;
        protected Activity _context;

        public PickerFragmentAdapter(Activity context, List<NameValueItem> items) : base()
        {
            _context = context;
            _items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override NameValueItem this[int position]
        {
            get { return _items[position]; }
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Name;

            return view;
        }

        public void Update(List<NameValueItem> items)
        {
            _items = items;
            NotifyDataSetChanged();
        }
    }
}
