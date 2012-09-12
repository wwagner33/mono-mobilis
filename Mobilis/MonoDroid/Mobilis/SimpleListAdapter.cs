using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Mobilis
{
    public class SimpleListAdapter<T> : BaseAdapter
    {
        private readonly Context _context;
        public List<T> _content;
        private LayoutInflater inflater;

        public SimpleListAdapter(Context context, List<T> content)
        {
            _context = context;
            _content = content;
            inflater = LayoutInflater.From(_context);
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _content[position];
            System.Diagnostics.Debug.WriteLine("item name = " + item.ToString());
            var view = (convertView ?? inflater.Inflate(Resource.Layout.SimpleListItem, parent, false));
            TextView topicName = view.FindViewById<TextView>(Resource.Id.topic_name);
            topicName.SetText(item.ToString(),TextView.BufferType.Normal);
            return view;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return _content.Count; }
        }
    }
}