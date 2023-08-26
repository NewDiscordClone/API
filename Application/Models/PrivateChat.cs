using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models;

public class PrivateChat : Chat
{
    public string? Image { get; set; }
    public string? Title { get; set; }
    public int OwnerId { get; set; }
}