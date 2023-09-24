using System.ComponentModel;

namespace Sparkle.Application.Models;

/// <summary>
/// Representation of user's profile on server
/// </summary>
public class ServerProfile : UserProfile
{
    /// <summary>
    /// Non-unique user name displays on server
    /// </summary>
    [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// List of roles of user on server
    /// </summary>
    public override List<Role> Roles { get => base.Roles; set => base.Roles = value; }

    public new string? ChatId { get; set; } = null;

    /// <summary>
    /// Id of server where profile is
    /// </summary>
    public string ServerId { get; set; }
}