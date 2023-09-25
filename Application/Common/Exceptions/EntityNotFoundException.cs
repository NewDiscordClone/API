namespace Sparkle.Application.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public object Id { get; init; }
        private readonly string _message;
        public override string Message => _message;
        public EntityNotFoundException(object id) : base()
        {
            Id = id;
            _message = $"Entity {id} not found";
        }
        public EntityNotFoundException(string message, object id) : base()
        {
            Id = id;
            _message = message;
        }

    }
}
