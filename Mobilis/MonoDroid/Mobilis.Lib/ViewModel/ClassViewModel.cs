using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Util;
using Mobilis.Lib.Messages;

namespace Mobilis.Lib.ViewModel
{
    public class ClassViewModel
    {
        public List<Class> classes {get;private set;}
        private ClassDao classDao;
        private UserDao userDao;
        private DiscussionDao discussionDao;
        private DiscussionService discussionService;

        public ClassViewModel() 
        {
            classDao = new ClassDao();
            userDao = new UserDao();
            discussionDao = new DiscussionDao();
            discussionService = new DiscussionService();
            classes = classDao.getClassesFromCourse(ContextUtil.Instance.Course);
        }

        public void logout()
        {
            User user = userDao.getUser();
            user.token = null;
            userDao.addUser(user);
        }

        public bool existDiscussionsAtClass(int classPosition) 
        {
            Class selectedClass = classes[classPosition];
            ContextUtil.Instance.Class = selectedClass._id;
            return discussionDao.existDiscussionsAtClass(selectedClass._id);
        }

        public void requestDiscussions() 
        {
            discussionService.getDiscussions(Constants.DiscussionURL, userDao.getToken(), r =>
            {
                if (r.hasError())
                {
                    ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.CONNECTION_ERROR)));
                }
                else
                {
                    discussionDao.insertDiscussion(r.Value);
                    ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.DISCUSSION_CONNECTION_OK)));
                }
            });
        }
    }
}