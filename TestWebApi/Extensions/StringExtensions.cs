namespace TestWebApi.Extensions;

public static class StringExtensions
{
    public static string ToNormalView(this string str) => str.First().ToString().ToUpper() + str.ToLower().Substring(1);
}
