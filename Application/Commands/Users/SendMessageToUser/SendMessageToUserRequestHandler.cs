using Application.Commands.Messages.AddMessage;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Application.Commands.Users.SendMessageToUser
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
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);
            User other = await Context.FindSqlByIdAsync<User>(request.UserId, cancellationToken);

            RelationshipList? userRelationship =
                await Context.RelationshipLists.FindAsync(UserId);
            RelationshipList? otherRelationship =
                await Context.RelationshipLists.FindAsync(request.UserId);

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
                Users = new List<UserLookUp>
                {
                    Mapper.Map<UserLookUp>(user),
                    Mapper.Map<UserLookUp>(other),
                }
            });
            Message message = await new AddMessageRequestHandler(Context, UserProvider, Mapper).Handle(new AddMessageRequest
            {
                ChatId = chat.Id,
                Text = request.Text,
                Attachments = request.Attachments
            }, cancellationToken); //Не впевнений що це правельне рішення

            return new MessageChatDto { ChatId = chat.Id, MessageId = message.Id };
        }
    }
}