using AutoMapper;
using MediatR;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Messages.Commands.AddMessage;
using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;

namespace Sparkle.Application.Users.Commands.SendMessageToUser
{
    public class SendMessageToUserRequestHandler : RequestHandlerBase, IRequestHandler<SendMessageToUserRequest, MessageChatDto>
    {
        public SendMessageToUserRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider,
            IMapper mapper) : base(context, userProvider, mapper)
        {
        }

        public async Task<MessageChatDto> Handle(SendMessageToUserRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            RelationshipList? userRelationship = await FindOrCreateRelationshipsAsync(UserId);
            RelationshipList? otherRelationship = await FindOrCreateRelationshipsAsync(request.UserId);

            //TODO: Додати реалізацію перевірки налаштуваннь користувача з дозволів відправляти повідомлення
            Relationship? otherToUser = otherRelationship.Relationships.Find(r => r.UserId == UserId);
            switch (otherToUser)
            {
                case { RelationshipType: RelationshipType.Blocked }:
                    throw new NoPermissionsException("You are blocked from this user");
                case null:
                    otherRelationship.Relationships.Add(new Relationship
                    {
                        UserId = UserId,
                        RelationshipType = RelationshipType.Acquaintance
                    });
                    await Context.RelationshipLists.UpdateAsync(otherRelationship);
                    userRelationship.Relationships.Add(new Relationship
                    {
                        UserId = request.UserId,
                        RelationshipType = RelationshipType.Acquaintance
                    });
                    await Context.RelationshipLists.UpdateAsync(userRelationship);
                    break;
                default:
                    throw new Exception("You already have chat with the user");
            }

            Chat chat = await Context.PersonalChats.AddAsync(new PersonalChat
            {
                Profiles = new List<UserProfile>
                {
                    new() { UserId = UserId },
                    new() { UserId = request.UserId }
                }
            });
            MessageDto message = await new AddMessageCommandHandler(Context, UserProvider, Mapper).Handle(new AddMessageCommand
            {
                ChatId = chat.Id,
                Text = request.Text,
                Attachments = request.Attachments
            }, cancellationToken); //Не впевнений що це правельне рішення

            return new MessageChatDto { ChatId = chat.Id, MessageId = message.Id };
        }

        private async Task<RelationshipList> FindOrCreateRelationshipsAsync(Guid id)
        {
            try
            {
                return await Context.RelationshipLists.FindAsync(id);
            }
            catch (EntityNotFoundException)
            {
                return await Context.RelationshipLists.AddAsync(new RelationshipList()
                {
                    Id = id,
                    Relationships = new List<Relationship>()
                });
            }
        }
    }
}