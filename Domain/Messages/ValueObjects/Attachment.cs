using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Domain.Messages.ValueObjects;

/// <summary>
/// Represents url attachment
/// </summary>
public class Attachment
{
    /// <summary>
    /// <see langword="True"/> if url are in the message text
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
    /// Flag that indicates that content in attachment is a spoiler
    /// </summary>
    [Required]
    [DefaultValue(false)]
    public bool IsSpoiler { get; set; }
}