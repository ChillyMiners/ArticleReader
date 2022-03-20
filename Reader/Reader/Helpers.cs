using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reader
{
    public static class Helpers
    {
        public static string GetStringBetween(this string fullString, string start, string end, bool getFirst, bool includeEnds)
        {
            int startPos = fullString.IndexOf(start) + start.Length;
            int endPos = getFirst ? fullString.IndexOf(end) : fullString.LastIndexOf(end);
            string between = fullString[startPos..endPos];
            return includeEnds
                ? start + between + end
                : between;
        }

        public static string GetStringBefore(this string fullString, string end, bool getFirst)
        {
            int endPos = getFirst ? fullString.IndexOf(end) : fullString.LastIndexOf(end);
            return fullString[..endPos];
        }

        public static string GetStringAfter(this string fullString, string start)
        {
            int startPos = fullString.IndexOf(start) + start.Length;
            int endPos = fullString.Length - 1;
            return fullString[startPos..];
        }

        public static string StripHTML(this string fullString)
        {
            return Regex.Replace(fullString, "<.*?>", String.Empty);
        }

        public static string RemoveSubstring(this string fullString, string toRemove)
        {
            return fullString.Replace(toRemove, string.Empty);
        }

    }
}
