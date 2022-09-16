using GitChat.Server.Models;

namespace GitChat.Server.Services
{
    public class ChatService : IChatService
    {
        public void AddGroupChat(string groupname)
        {
            var groupchat = new Group();
            groupchat.GroupName = groupname;

            //TODO: Functionality to add group to DB
        }

        public void RemoveGroupChat(int id)
        {
            //TODO: Get chat from DB and remove it
        }

        public Group FindChat(string groupname)
        {
            throw new NotImplementedException();
        }
    }
}
