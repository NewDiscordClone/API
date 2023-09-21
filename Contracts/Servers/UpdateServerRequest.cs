using System.ComponentModel;

namespace Sparkle.Contracts.Servers
{
    public record UpdateServerRequest
    {
        /// <summary>
        /// Server's name (Optional)
        /// </summary>
        [DefaultValue("Server 1")]
        public string? Title { get; init; }
        /// <summary>
        /// Server's image url (Optional)
        /// </summary>
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }
    }
}
