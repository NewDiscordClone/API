namespace Sparkle.Application.Common.Constants
{
    public static partial class Constants
    {
        public static class Roles
        {
#pragma warning disable IDE1006 // Naming Styles


            public const int RoleNameMaxLength = 100;
            public const string DefaultMemberRoleName = "Member";
            public const string DefaultOwnerRoleName = "Owner";

            public static readonly Guid GroupChatOwnerId = Guid.Parse("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5");
            public const string GroupChatOwnerName = "GROUP-CHAT-OWNER";

            public const string DefaultColor = "#FF0000";

            public static readonly Guid GroupChatMemberId = Guid.Parse("ff55d9b4-b26f-47be-bf76-bdba29896ca3");
            public const string GroupChatMemberName = "GROUP-CHAT-MEMBER";

            public static readonly Guid PrivateChatMemberId = Guid.Parse("b36944ab-83e0-4447-bce4-ac699b6d3dc2");
            public const string PrivateChatMemberName = "PRIVATE-CHAT-MEMBER";

            public static readonly Guid ServerOwnerId = Guid.Parse("e26c471b-5418-4de1-bc35-d35310ab4ca7");
            public const string ServerOwnerName = "SERVER-OWNER";

            public static readonly Guid ServerMemberId = Guid.Parse("8ce979a9-c05c-4d70-bb45-6476892ced6c");
            public const string ServerMemberName = "SERVER-MEMBER";

            public static readonly Guid[] DefaultRoleIds = new Guid[]
            {
                GroupChatOwnerId,
                GroupChatMemberId,
                PrivateChatMemberId,
                ServerOwnerId,
                ServerMemberId
            };

            public static readonly string[] DefaultRoleNames = new string[]
            {
                GroupChatOwnerName,
                GroupChatMemberName,
                PrivateChatMemberName,
                ServerOwnerName,
                ServerMemberName
            };

#pragma warning restore IDE1006 // Naming Styles

        }
    }
}
