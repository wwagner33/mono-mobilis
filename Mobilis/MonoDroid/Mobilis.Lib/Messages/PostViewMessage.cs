using TinyMessenger;
namespace Mobilis.Lib.Messages
{
        public class PostViewMessage : GenericTinyMessage<Message>
        {
            public const int NO_NEW_POSTS = 1;
            public const int FUTURE_POSTS_LOADED = 2;
            public const int FINISHED_PLAYING = 3;
            public const int UPDATE_SCREEN = 4;
            public const int PREVIOUS_POSTS_LOADED = 5;
            public PostViewMessage(object sender, Message content) : base(sender, content) { }
        }
}