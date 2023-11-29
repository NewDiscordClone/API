namespace Sparkle.WebApi.Common.Options
{
    public class TenorOptions
    {
        public const string SectionName = "TenorOptions";
        public string Key { get; init; }
        public string BaseUrl { get; init; }
        public int Limit { get; init; }
    }
}
