using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetUser
{
    public class GetUserDetailsRequest : IRequest<GetUserDetailsDto>
    {
        [Required]
        public Guid UserId { get; init; }
        
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string? ServerId { get; init; }
    }
}