using TinyMessenger;

namespace Mobilis.Lib.Messages
{
    public class BaseViewMessage : GenericTinyMessage<Message>
    {
        public enum MessageTypes
        {
            CONNECTION_ERROR, LOGIN_CONNECTION_OK, COURSE_CONNECTION_OK,CLASS_CONNECTION_OK,DISCUSSION_CONNECTION_OK, NO_NEW_POSTS, FUTURE_POSTS_LOADED,
            FINISHED_PLAYING, UPDATE_SCREEN, PREVIOUS_POSTS_LOADED
        };
        public BaseViewMessage(object sender, Message content) : base(sender, content) { }        
    }
}