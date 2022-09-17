using System.ComponentModel.DataAnnotations;

namespace GitChat.Server.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string GroupName { get; set; } = string.Empty;

        public bool IsPrivate { get; set; } = false;
    }
}
