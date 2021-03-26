using System.Text.RegularExpressions;

namespace EmailApi.Utilities
{
    public static class EmailUtilities
    {
        
        public static string EmailSearch(string text, string expr)
        {
            Match m = Regex.Match(text, expr);
            Group g = m.Groups[1];
            return g.ToString();

        }
    }
}