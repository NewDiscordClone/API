namespace Sparkle.Application.Common.Exceptions
{
    public class NoPermissionsException : Exception
    {
        public NoPermissionsException() : base()
        {
        }

        public NoPermissionsException(string? message) : base(message)
        {
        }
    }
}