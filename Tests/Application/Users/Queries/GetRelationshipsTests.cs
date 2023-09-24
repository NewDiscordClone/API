using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Models;
using Sparkle.Application.Users.Queries.GetRelationships;
using Sparkle.Tests.Common;

namespace Sparkle.Tests.Application.Users.Queries
{
    public class GetRelationshipsTests : TestBase
    {
        [Fact]
        public async Task Success()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserAId;

            SetAuthorizedUserId(userId);

            GetRelationshipQuery request = new();
            GetRelationshipQueryHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<RelationshipDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            RelationshipList relationship = await Context.RelationshipLists.FindAsync(userId);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.All(result, resrel =>
            {
                Assert.Contains(relationship.Relationships,
                    rel =>
                        rel.UserId == resrel.User.Id &&
                        rel.RelationshipType == resrel.RelationshipType
                );
            });
        }

        [Fact]
        public async Task Success_Empty()
        {
            //Arrange
            CreateDatabase();
            Guid userId = Ids.UserCId;

            SetAuthorizedUserId(userId);

            GetRelationshipQuery request = new();
            GetRelationshipQueryHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<RelationshipDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await Context.RelationshipLists.FindAsync(userId));
            Assert.Empty(result);
        }
    }
}