using Application.Common.Exceptions;
using Application.Models;
using Application.Queries.GetRelationships;
using Tests.Common;

namespace Tests.Users.Queries
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

            GetRelationshipRequest request = new();
            GetRelationshipRequestHandler handler = new(Context, UserProvider, Mapper);

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

            GetRelationshipRequest request = new();
            GetRelationshipRequestHandler handler = new(Context, UserProvider, Mapper);

            //Act
            List<RelationshipDto> result = await handler.Handle(request, CancellationToken);

            //Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await Context.RelationshipLists.FindAsync(userId));
            Assert.Empty(result);
        }
    }
}