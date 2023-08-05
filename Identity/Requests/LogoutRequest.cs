using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Identity.Requests
{
    public record LogoutRequest : IRequest<string?>
    {
        [Required]
        public string LogoutId { get; init; }
    }
}
