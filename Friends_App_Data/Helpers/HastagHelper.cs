using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Friends_App_Data.Helpers
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
 