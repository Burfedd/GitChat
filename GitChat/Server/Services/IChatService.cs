using GitChat.Server.Models;

namespace GitChat.Server.Services
{
    public interface IChatService
    {
        Task AddGroupChat(string groupname);

        Task RemoveGroupChat(int id);

        Group FindChat(string groupname);
    }
}
