namespace Sparkle.Application.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Roles of user 
        /// </summary>
        public virtual List<Role> Roles { get; set; } = new();

        /// <summary>
        /// Id of user in the system
        /// </summary>
        public Guid UserId { get; set; }

        public UserProfile()
        {
            Id = Guid.NewGuid();
        }
    }
}
