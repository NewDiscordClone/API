using System.ComponentModel;

namespace Application.Models
{
    public class GroupChat : PrivateChat
    {
        [DefaultValue(1)]
        public int OwnerId { get; set; }
    }
}