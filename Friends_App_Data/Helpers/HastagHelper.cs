using System.Text.RegularExpressions;

namespace Friends_Data.Helpers
{
    public static class HastagHelper 
    {
        public static List<string> GetHastags(string postText)
        {
            var hastagPattern = new Regex(@"#\w+");
            var matches = hastagPattern.Matches(postText)
                .Select(m => m.Value.TrimEnd('.', ',', '!', '?').ToLower())
                .Distinct().ToList();

            return matches;
        } 
    }
}
 