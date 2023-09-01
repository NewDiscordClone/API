using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

/// <summary>
/// Represents personal chat between 2-10 people 
/// </summary>
public class PrivateChat : Chat
{
    [RegularExpression("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\/api\\/media\\/[a-z0-9]{24}$")]
    [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
    [DataType(DataType.ImageUrl)]
    public string? Image { get; set; }

    [StringLength(32, MinimumLength = 1)]
    [DefaultValue("Title")]
    public string? Title { get; set; }

    /// <summary>
    /// Id of user created this chat
    /// </summary>
    [DefaultValue(1)]
    public int OwnerId { get; set; }
}