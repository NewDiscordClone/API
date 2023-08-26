using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using AutoMapper;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.Messages.AddReaction
{
    public class AddReactionRequestHandler : RequestHandlerBase, IRequestHandler<AddReactionRequest, Reaction>
    {
        public async Task<Reaction> Handle(AddReactionRequest request, CancellationToken cancellationToken)
        {
            Message message = await Context.FindByIdAsync<Message>(request.MessageId, cancellationToken);
            Chat chat = await Context.FindByIdAsync<Chat>(message.ChatId, cancellationToken);
            User user = await Context.FindSqlByIdAsync<User>(UserId, cancellationToken);

            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("You are not a member of the Chat");

            Reaction reaction = new Reaction
            {
                User = Mapper.Map<UserLookUp>(user),
                Emoji = request.Emoji,
            };

            await Context.Messages.UpdateOneAsync(
                Context.GetIdFilter<Message>(request.MessageId),
                Builders<Message>.Update.Push(m => m.Reactions, reaction),
                null, cancellationToken);
            
            return reaction;
        }

        public AddReactionRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper) :
            base(context, userProvider, mapper)
        {
        }
    }
}