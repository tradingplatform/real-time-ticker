namespace Infusion.Trading.MarketData.Models
{
    public static class NullableExtensions
    {
        public static string ToStringOrDefault<T>(this T? source) where T: struct
        {
            return source?.ToString() ?? "N/A";
        }

        public static string ToStringOrDefault(this string source)
        {
            return source ?? "N/A";
        }
    }
}