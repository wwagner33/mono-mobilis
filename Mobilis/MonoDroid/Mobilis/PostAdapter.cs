using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Views;
using Android.Content;
using Mobilis.Lib.Model;
using System.Collections.ObjectModel;
using Android.Graphics;
using Mobilis.Lib.Util;

namespace Mobilis
{
    public class PostAdapter : BaseAdapter
    {
          class ViewHolder : Java.Lang.Object
        {
              public TextView userNick { get; set;}
              public TextView postDate { get; set;}
              public TextView postContent { get; set;}
              public ImageView avatar { get; set;}
        }

        private ObservableCollection<Post> posts;
        private LayoutInflater inflater;
        private Context context;

        public PostAdapter(Context context, ObservableCollection<Post> posts) 
        {
            this.posts = posts;
            this.context = context;
            inflater = LayoutInflater.From(context);
            posts.CollectionChanged += (o,e) => this.NotifyDataSetChanged();
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
            get {return posts.Count; }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, ViewGroup parent)
        {
            ViewHolder holder;

            if (convertView == null)
            {
                convertView = inflater.Inflate(Resource.Layout.PostItem, parent, false);
                holder = new ViewHolder();

                
                holder.avatar = (ImageView)convertView
                    .FindViewById(Resource.Id.user_photo);
                holder.userNick = (TextView)convertView
                        .FindViewById(Resource.Id.user_nick);
                holder.postDate = (TextView)convertView
                        .FindViewById(Resource.Id.post_date);
                holder.postContent = (TextView)convertView
                        .FindViewById(Resource.Id.post_content);
                
                convertView.SetTag(Resource.String.view_holder_tag, holder);
            }

            else 
            {
                holder = (ViewHolder)convertView.GetTag(Resource.String.view_holder_tag);
            }

            Post postAtPosition = posts[position];
            holder.postContent.Text = postAtPosition.content;
            holder.userNick.Text = postAtPosition.userName;
            holder.postDate.Text = HttpUtils.postDateToShowFormat(postAtPosition.updatedAt);
            Color backgroundColor = postAtPosition.marked ? context.Resources.GetColor(Resource.Color.post_selected) : context.Resources.GetColor(Resource.Color.post_idle);
            convertView.SetBackgroundColor(backgroundColor);
            return convertView;
        }
    }
}
