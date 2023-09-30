using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Queries.GetRelationships
{
    public record GetRelationshipQuery()
        : IRequest<List<Relationship>>;
}