using System.Reflection;

namespace Sparkle.Application.Common.Constants
{
    public static partial class Constants
    {
        public static class Policies
        {
#pragma warning disable IDE1006 // Naming Styles
            public const string ManageMessages = "ManageMessages";
            public const string ManageChannels = "ManageChannels";
            public const string ManageRoles = "ManageRoles";
            public const string ManageServer = "ManageServer";
            public const string ChangeName = "ChangeServerName";
            public const string ChangeSomeoneName = "ChangeSomeoneServerName";
            public const string RemoveMembers = "RemoveMembers";
            public const string SendMessages = "SendMessages";
            public const string DeleteServer = "DeleteServer";
#pragma warning restore IDE1006 // Naming Styles

            public static string[] GetPolicies()
            {
                Type type = typeof(Policies);

                List<string?> constants = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
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
        public static class Claims
        {
#pragma warning disable IDE1006 // Naming Styles
            public const string ManageMessages = "ManageMessages";
            public const string ManageChannels = "ManageChannels";
            public const string ManageRoles = "ManageRoles";
            public const string ManageServer = "ManageServer";
            public const string ChangeServerName = "ChangeServerName";
            public const string ChangeSomeoneServerName = "ChangeSomeoneServerName";
            public const string RemoveMembers = "RemoveMembers";
#pragma warning restore IDE1006 // Naming Styles

            public static string[] GetClaims()
            {
                Type type = typeof(Claims);

                List<string?> constants = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
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
