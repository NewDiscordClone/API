namespace WebApi.Models;

public class Channel : Chat
{
    public string Title { get; set; }

    public virtual Server Server { get; set; }
}