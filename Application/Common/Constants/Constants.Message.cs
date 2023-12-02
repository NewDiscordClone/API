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
                public const string Sticker1Id = "656b3377a05b37a590db4d6a";
                public const string Sticker2Id = "656b3390a05b37a590db4d6b";
                public const string Sticker3Id = "656b3395a05b37a590db4d6c";
                public const string Sticker4Id = "656b339ba05b37a590db4d6d";
                public const string Sticker5Id = "656b33a0a05b37a590db4d6e";
                public const string Sticker6Id = "656b33a5a05b37a590db4d6f";
                public const string Sticker7Id = "656b33aba05b37a590db4d70";
                public const string Sticker8Id = "656b33bba05b37a590db4d72";
                public const string Sticker9Id = "656b33c0a05b37a590db4d73";
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
