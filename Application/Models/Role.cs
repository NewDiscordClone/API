namespace WebApi.Models;

public class Role
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Color { get; set; }

    public virtual Server Server { get; set; }
    public virtual List<ServerProfile> ServerProfile { get; set; } = new();
}