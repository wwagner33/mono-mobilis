using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Android.Views;
using Android.Content;
using Mobilis.Lib.Model;

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

        private List<Post> posts;
        private LayoutInflater inflater;

        public PostAdapter(Context context, List<Post> posts) 
        {
            this.posts = posts;
            inflater = LayoutInflater.From(context);
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

        public override View GetView(int position, View convertView, ViewGroup parent)
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
            //holder.postDate = postAtPosition. TODO Post date.

            return convertView;
        }
    }
}
