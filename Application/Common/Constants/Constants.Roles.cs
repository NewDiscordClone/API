namespace Sparkle.Application.Common.Constants
{
    public static partial class Constants
    {
        public static class Roles
        {
            public static readonly Guid GroupChatOwnerId = Guid.Parse("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5");
            public const string GroupChatOwnerName = "GROUP-CHAT-OWNER";
            public const string DefaultColor = "#FF0000";
            public static readonly Guid GroupChatMemberId = Guid.Parse("ff55d9b4-b26f-47be-bf76-bdba29896ca3");
            public const string GroupChatMemberName = "GROUP-CHAT-MEMBER";
            public static readonly Guid PrivateChatMemberId = Guid.Parse("b36944ab-83e0-4447-bce4-ac699b6d3dc2");
            public const string PrivateChatMemberName = "PRIVATE-CHAT-MEMBER";
        }
    }
}
