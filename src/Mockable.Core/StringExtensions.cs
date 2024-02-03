using System.Text;

namespace Mockable.Core;

internal static class StringExtensions
{
    public static string ToPascalCase(this string str)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < str.Length; i++)
        {
            if (char.IsLetter(str[i]))
            {
                sb.Append(char.ToUpper(str[i]));
                sb.Append(str.Substring(i + 1));
                return sb.ToString();
            }

            sb.Append(str[i]);
        }

        return sb.ToString();
    }
}
