namespace Sparkle.WebApi.Common.Options
{
    public class JwtOptions
    {
        public const string SectionName = "JwtOptions";

        public string Authority { get; init; }
        public string Audience { get; init; }
        public string MetadataAddress { get; init; }
    }
}
