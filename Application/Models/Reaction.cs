using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models;

/// <summary>
/// Represents emoji reaction under the message
/// </summary>
public class Reaction
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

    public override bool Equals(object? obj)
    {
        if (obj is not Reaction reaction)
            return false;

        return Emoji == reaction.Emoji && AuthorProfile == reaction.AuthorProfile;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Emoji, AuthorProfile);
    }
}