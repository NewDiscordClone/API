using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public enum UserStatus
{
    Online,
    Idle,
    DoNotDisturb,
    Offline
}
public class User : IdentityUser<int>
{
    [DefaultValue(1)]
    public override int Id { get; set; }
    [DefaultValue("username")]
    public override string UserName { get; set; }

    [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
    public string? DisplayName { get; set; }
    [DataType(DataType.ImageUrl)]
    [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
    public string? Avatar { get; set; }
    
    [DefaultValue(UserStatus.Online)]
    public UserStatus Status { get; init; } = UserStatus.Online;
    [DefaultValue("I'm Good")]
    public string? TextStatus { get; init; }
}