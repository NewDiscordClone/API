using MediatR;
using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    public record GetRelationshipQuery()
        : IRequest<List<Relationship>>;
}