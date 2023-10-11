using FluentValidation;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.GroupChats.Commands.CreateGroupChat
{
    public class CreateGroupChatCommandValidator : AbstractValidator<CreateGroupChatCommand>
    {
        public CreateGroupChatCommandValidator(IAuthorizedUserProvider userProvider)
        {
            RuleFor(x => x.Title)!.RequiredMaximumLength(Constants.Channel.TitleMaxLength)
                .When(x => x.Title is not null);
            RuleFor(x => x.Image)!.IsMedia();
            RuleFor(x => x.UserIds)
                .Must(u => u.Count >= 2)
                .WithMessage(users => $"You cant create group chat with less than 2 people." +
                    $" You added {users.UserIds.Count} user");

            int maxUsersCount = Constants.Chats.GroupChatMaxUsers - 1;
            RuleFor(x => x.UserIds)
                .Must(u => u.Count <= maxUsersCount)
                .WithMessage(users => $"You cant create group chat with more than {maxUsersCount} people." +
                    $" You added {users.UserIds.Count} users");

            RuleForEach(x => x.UserIds)
                .NotEmpty();
            RuleForEach(x => x.UserIds)
                .NotCurrentUser(userProvider)
                .WithMessage("You cant create group chat with yourself");



        }
    }
}
