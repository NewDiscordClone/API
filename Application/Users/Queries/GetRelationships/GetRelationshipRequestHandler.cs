﻿using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Users.Queries.GetRelationships
{
    public class GetRelationshipRequestHandler : RequestHandlerBase,
        IRequestHandler<GetRelationshipRequest, List<RelationshipDto>>
    {
        public GetRelationshipRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }

        public async Task<List<RelationshipDto>> Handle(GetRelationshipRequest request,
            CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            RelationshipList? relationships =
                await Context.RelationshipLists.FindOrDefaultAsync(UserId) ??
                new RelationshipList()
                {
                    Id = UserId,
                    Relationships = new List<Relationship>()
                };
            List<RelationshipDto> relationshipDtos = new();
            foreach (Relationship relationship in relationships.Relationships)
            {
                relationshipDtos.Add(new RelationshipDto()
                {
                    User = Mapper.Map<UserLookUp>(
                        await Context.SqlUsers.FindAsync(relationship.UserId)),
                    RelationshipType = relationship.RelationshipType
                });
            }

            return relationshipDtos;
        }
    }
}