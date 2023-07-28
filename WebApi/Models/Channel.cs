namespace WebApi.Models;

public class Channel
{
    public int Id { get; set; }
    public string Title { get; set; }

    public virtual Chat Chat {get; set; }
}