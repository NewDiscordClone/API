namespace Sparkle.Application.Common.Exceptions
{
    public class NoSuchUserException : Exception
    {
        public NoSuchUserException() : base()
        {
        }

        public NoSuchUserException(string? message) : base(message)
        {
        }
    }
}