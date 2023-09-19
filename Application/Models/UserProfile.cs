﻿namespace Sparkle.Application.Models
{
    public class UserProfile
    {
        public string Id { get; set; } = null!;

        /// <summary>
        /// Roles of user 
        /// </summary>
        public virtual List<Role> Roles { get; set; } = null!;

        /// <summary>
        /// Id of user in the system
        /// </summary>
        public Guid UserId { get; set; }
    }
}
