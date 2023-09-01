using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models;

/// <summary>
/// Represents url attachment
/// </summary>
public class Attachment
{
    /// <summary>
    /// <see langword="True"/> if url in message text
    /// </summary>
    [DefaultValue(false)]
    public bool IsInText { get; set; }

    /// <summary>
    /// Attachment url
    /// </summary>
    [Required]
    [DefaultValue("http://test.com/")]
    [DataType(DataType.Url)]
    public string Path { get; set; }

    /// <summary>
    /// Flag that indicates content in attachment as a spoiler
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool IsSpoiler { get; set; }
}