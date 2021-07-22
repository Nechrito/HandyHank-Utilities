using System.Text.RegularExpressions;
using System.Globalization;

public static class StringExtension
{
    public static string SurroundedWith(this string input, string surround)          => surround + input + surround;
    public static string SurroundedWith(this string input, string start, string end) => start + input + end;

    public static string Underlined(this string input) => $"<u>{input}</u>";
    public static string Bold(this string input)       => $"<b>{input}</b>";
    public static string Italics(this string input)    => $"<i>{input}</i>";

	/// <summary>
    /// "Camel case string" => "CamelCaseString" 
    /// </summary>
    public static string ToCamelCase(this string input)
    {
        input = input.Replace("-", " ").Replace("_", " ");
        input = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input);
        input = input.Replace(" ", "");
        return input;
    }

    /// <summary>
    /// "CamelCaseString" => "Camel Case String"
    /// </summary>
    public static string SplitCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        string camelCase = Regex.Replace(Regex.Replace(input, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        string firstLetter = camelCase.Substring(0, 1).ToUpper();

        if (input.Length > 1)
        {
            string rest = camelCase.Substring(1);
            return firstLetter + rest;
        }

        return firstLetter;
    }
}
