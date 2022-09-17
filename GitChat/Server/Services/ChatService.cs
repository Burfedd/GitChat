using GitChat.Server.Models;
using GitChat.Server.Repository;

namespace GitChat.Server.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Group> repository;

        public ChatService(IRepository<Group> repository)
        {
            this.repository = repository;
        }

        public async Task AddGroupChat(string groupname)
        {
            var groupchat = new Group();
            groupchat.GroupName = groupname;

            repository.Add(groupchat);
            await repository.SaveChangesAsync();
        }

        public async Task RemoveGroupChat(int id)
        {
            var group = await repository.GetByIdAsync(id);
            if(group == null)
            {
                throw new NullReferenceException();
            }

            repository.Delete(group);
            await repository.SaveChangesAsync();
        }

        public Group FindChat(string groupname)
        {
            var group = repository.Find(x => x.GroupName == groupname).FirstOrDefault();

            return group;
        }
    }
}
