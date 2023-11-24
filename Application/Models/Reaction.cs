using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models;

/// <summary>
/// Represents emoji reaction under the message
/// </summary>
public record Reaction
{
    /// <summary>
    /// Emoji code
    /// </summary>
    [RegularExpression(":\\w+:")]
    [DefaultValue(":smile:")]
    public string Emoji { get; set; }

    /// <summary>
    /// Reaction author ID
    /// </summary>
    public Guid AuthorProfile { get; set; }
}