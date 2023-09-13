using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

/// <summary>
/// Representation of user's profile on server
/// </summary>
public class ServerProfile
{
    /// <summary>
    /// Non-unique user name displays on server
    /// </summary>
    [StringLength(32, MinimumLength = 1)]
    [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
    public string? DisplayName { get; set; }
    /// <summary>
    /// User ID with most popular data
    /// </summary>
    public virtual Guid UserId { get; set; }

    /// <summary>
    /// List of user's roles on server
    /// </summary>
    public virtual List<Role> Roles { get; set; }
}