using GitChat.Server.Models;

namespace GitChat.Server.Services
{
    public interface IChatService
    {
        void AddGroupChat(string groupname);

        void RemoveGroupChat(int id);

        Group FindChat(string groupname);
    }
}
