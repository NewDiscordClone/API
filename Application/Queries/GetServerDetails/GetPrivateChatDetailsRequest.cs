using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Queries.GetServerDetails
{
    public record GetServerDetailsRequest : IRequest<ServerDetailsDto>
    {
        [Required]
        public int ServerId { get; init; }
    }
}
