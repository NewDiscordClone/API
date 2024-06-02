#pragma warning disable IDE1006 // Naming Styles
namespace Sparkle.Application.Common.Constants
{
    public static partial class Constants
    {
        public static class User
        {
            public const int UserNameMaxLength = 32;
            public const int UserTextStatusMaxLength = 200;
            public static readonly Guid DefaultUser3Id = Guid.Parse("ba1ce081-e200-41da-9fb2-3d317627c9d4");
            public static readonly Guid DefaultUser2Id = Guid.Parse("7aef2538-e1b3-42d7-a3db-a2809a81ac91");
            public static readonly Guid DefaultUser1Id = Guid.Parse("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34");

            public const string Avatar1Id = "665c87da266a546febcc0b1e";
            public const string Avatar2Id = "665c87ea266a546febcc0b1f";
            public const string Avatar3Id = "665c87f8266a546febcc0b20";
            public const string Avatar4Id = "665c880d266a546febcc0b21";
            public const string Avatar5Id = "665c881c266a546febcc0b22";
            public const string Avatar6Id = "665c8827266a546febcc0b23";

            public static readonly string[] DefaultAvatarIds = new string[]
            {
                Avatar1Id,
                Avatar2Id,
                Avatar3Id,
                Avatar4Id,
                Avatar5Id,
                Avatar6Id
            };
        }
    }
}