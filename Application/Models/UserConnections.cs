namespace Application.Models
{
    public class UserConnections
    {
        public int UserId { get; set; }
        public HashSet<string> Connections { get; set; }
    }
}