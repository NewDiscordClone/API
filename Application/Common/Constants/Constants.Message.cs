#pragma warning disable IDE1006 // Naming Styles
using System.Reflection;

namespace Sparkle.Application.Common.Constants
{
    public static partial class Constants
    {
        public static class Message
        {
            public const int MaxTextLength = 2000;

            public static class Stickers
            {
                public const string Sticker1Id = "665c86f6266a546febcc0b15";
                public const string Sticker2Id = "665c8708266a546febcc0b16";
                public const string Sticker3Id = "665c8712266a546febcc0b17";
                public const string Sticker4Id = "665c8720266a546febcc0b18";
                public const string Sticker5Id = "665c8731266a546febcc0b19";
                public const string Sticker6Id = "665c873e266a546febcc0b1a";
                public const string Sticker7Id = "665c874c266a546febcc0b1b";
                public const string Sticker8Id = "665c8757266a546febcc0b1c";
                public const string Sticker9Id = "665c8762266a546febcc0b1d";
            }

            public static string[] StickerIds
            {
                get
                {
                    Type type = typeof(Stickers);

                    List<string?> constants = type.GetFields(BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly)
                        .Select(x => x.GetRawConstantValue()?.ToString())
                        .ToList();

                    List<string> result = new();
                    foreach (string? constant in constants)
                    {
                        if (constant != null)
                        {
                            result.Add(constant);
                        }
                    }

                    return result.ToArray();
                }
            }
        }
    }
}
