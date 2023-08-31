namespace Application.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string Id { get; init; }
        private string _message;
        public override string Message => _message;
        public EntityNotFoundException(string id) : base()
        {
            Id = id;
            _message = $"Entity {id} not found";
        }
        public EntityNotFoundException(string message, string id) : base()
        {
            Id = id;
            _message = message;
        }

    }
}
