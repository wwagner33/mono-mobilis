using System;
namespace Mobilis.Lib.Messages
{
    public class Message
    {
        public Mobilis.Lib.Messages.BaseViewMessage.MessageTypes message;
        public string error = null;

        public Message(Mobilis.Lib.Messages.BaseViewMessage.MessageTypes message) 
        {
            this.message = message;
        }
    }
}