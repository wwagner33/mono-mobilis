using System.Collections.ObjectModel;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Util;
using System.Collections.Generic;
using Mobilis.Lib.Messages;

namespace Mobilis.Lib.ViewModel
{
    public class PostsViewModel
    {
        public int selectedPosition = -1;
        public ObservableCollection<Post> posts { get; private set;}
        private DiscussionDao discussionDao;
        private PostDao postDao;
        private UserDao userDao;
        public Discussion selectedDiscussion { get; private set;}
        private PostService postService;
        private TTSManager manager;
        public bool contextualSelection {get;private set;}

        public PostsViewModel(IAsyncPlayer player)
        {
            contextualSelection = false;
            postDao = new PostDao();
            userDao = new UserDao();
            postService = new PostService();
            discussionDao = new DiscussionDao();
            posts = new ObservableCollection<Post>(postDao.getPostsFromDiscussion(ContextUtil.Instance.Discussion));
            selectedDiscussion = discussionDao.getDiscussion(ContextUtil.Instance.Discussion);
            ContextUtil.Instance.postsBefore = selectedDiscussion.previousPosts;
            ContextUtil.Instance.postsAfter = selectedDiscussion.nextPosts;
            manager = new TTSManager(player);
        }

        public void loadFuturePosts()
        {
            int oldNext = ContextUtil.Instance.postsAfter;
            int oldPrevious = ContextUtil.Instance.postsBefore;

            string date;
            if (posts.Count == 0)
            {
                date = Constants.OLD_POST_DATE;
            }
            else
            {
                date = HttpUtils.postDateToServerFormat(posts[posts.Count - 1].updatedAt);
            }
            postService.getPosts(Constants.NewPostURL(date), userDao.getToken(), r =>
            {
                List<Post> newPosts = new List<Post>(r.Value);
                if (newPosts.Count == 0)
                {
                    ServiceLocator.Dispatcher.invoke(() =>
                    {
                        System.Diagnostics.Debug.WriteLine("NO NEW POSTS");
                        ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.NO_NEW_POSTS)));
                    });
                }
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    foreach (Post post in newPosts)
                    {
                        posts.Add(post);
                    }
                });

                List<Post> postsFromDatabse = postDao.getPostsFromDiscussion(selectedDiscussion._id);
                postsFromDatabse.AddRange(newPosts);
                while (postsFromDatabse.Count > 20)
                {
                    postsFromDatabse.RemoveAt(0);
                }
                ContextUtil.Instance.postsBefore = oldPrevious + newPosts.Count;
                postDao.insertPost(postsFromDatabse);
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.FUTURE_POSTS_LOADED)));
                });
            });
        }

        public void loadPreviousPosts() 
        {
            int oldNext = ContextUtil.Instance.postsAfter;
            int oldPrevious = ContextUtil.Instance.postsBefore;

            string date = HttpUtils.postDateToServerFormat(posts[0].updatedAt);
            postService.getPosts(Constants.HistoryPostURL(date), userDao.getToken(), r =>
            {
                List<Post> oldPosts = new List<Post>(r.Value);
                int newPostCount = oldPosts.Count;
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    foreach (Post post in oldPosts)
                    {
                        posts.Insert(0, post);
                    }
                });
                List<Post> postsFromDatabse = postDao.getPostsFromDiscussion(selectedDiscussion._id);
                oldPosts.AddRange(postsFromDatabse);
                while (oldPosts.Count > 20)
                {
                    oldPosts.RemoveAt(oldPosts.Count - 1);
                }
                postDao.insertPost(oldPosts);
                ContextUtil.Instance.postsAfter = oldNext + newPostCount;
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.PREVIOUS_POSTS_LOADED)));
                });
            });
        }

        public void saveSelection() 
        {
            selectedDiscussion.nextPosts = ContextUtil.Instance.postsAfter;
            selectedDiscussion.previousPosts = ContextUtil.Instance.postsBefore;
            discussionDao.updateDiscussion(selectedDiscussion);
        }

        public void playSelectedPost()
        {
            manager.start(posts[selectedPosition], finishedPlaying);
        }

        public void playNextInSelection()
        {
            if (selectedPosition != posts.Count - 1)
            {
                togglePostMarked(selectedPosition + 1);
                ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.UPDATE_SCREEN)));
                manager.releaseResources();
                manager.start(posts[selectedPosition], finishedPlaying);
            }
        }

        public void playPreviousInSelection() 
        {
            if (selectedPosition != 0)
            {
                togglePostMarked(selectedPosition - 1);
                ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.UPDATE_SCREEN)));
                manager.releaseResources();
                manager.start(posts[selectedPosition], finishedPlaying);
            }
        }

        public void finishedPlaying() 
        {
            if (selectedPosition != posts.Count - 1)
            {
                togglePostMarked(selectedPosition + 1);
                manager.start(posts[selectedPosition], finishedPlaying);
            }
            else
            {
                removeSelection();
                ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.FINISHED_PLAYING)));
            }
        }

        public void releaseResources() 
        {
            manager.releaseResources();
        }

        public void togglePostMarked(int position) 
        {
            ContextUtil.Instance.Post = posts[position]._id;
            if (selectedPosition == position)
            {
                posts[position].marked = false;
                selectedPosition = -1;
                contextualSelection = false;
            }
            else 
            {
                posts[position].marked = true;
                if (selectedPosition!=-1)
                posts[selectedPosition].marked = false;
                selectedPosition = position;
                contextualSelection = true;        
            }
        }
        public void removeSelection() // TODO Mnadar mensagem para atualizar a VIEW
        {
            if (selectedPosition != -1) 
            {
                posts[selectedPosition].marked = false;
                contextualSelection = false;
                ServiceLocator.Messenger.Publish(new PostViewMessage(this, new Message(PostViewMessage.UPDATE_SCREEN)));
            }       
        }
    }
}