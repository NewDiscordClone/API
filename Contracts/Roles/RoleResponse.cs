namespace Sparkle.Contracts.Roles
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string ServerId { get; set; }
        public int Priority { get; set; }
    }
}
