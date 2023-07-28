namespace WebApi.Models;

public class Channel : Chat
{
    public int Id { get; set; }
    public string Title { get; set; }

    public virtual Server Server { get; set; }
}