using System.ComponentModel;

namespace Sparkle.Application.Models;

public class Channel : Chat
{

    [DefaultValue("Test Channel")]
    public string Title { get; set; }

    [DefaultValue("5f95a3c3d0ddad0017ea9291")]
    public string ServerId { get; set; }
}