using FluentValidation;
using Sparkle.Application.Common.Validation;

namespace Sparkle.Application.Chats.GroupChats.Commands.AddMemberToGroupChat
{
    public class AddMemberToGroupChatCommandValidator : AbstractValidator<AddMemberToGroupChatCommand>
    {
        public AddMemberToGroupChatCommandValidator()
        {
            RuleFor(x => x.ChatId).NotNull().IsObjectId();
            RuleFor(x => x.NewMemberId).NotNull();
        }
    }
}
