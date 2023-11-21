using System.Text.RegularExpressions;

namespace Utils
{
    public static class TextUtils
    {
        private static readonly Regex _regex = new Regex("[^a-zA-Z0-9 _-]");
        
        public static string RemoveSpecialCharacters(this string text)
        {
            return _regex.Replace(text, "");
        }

        public static string RemoveWhitespaces(this string text)
        {
            int len = text.Length;
            var src = text.ToCharArray();
            int dstIdx = 0;

            for (int i = 0; i < len; i++) {
                var ch = src[i];

                switch (ch) {

                    case '\u0020': case '\u00A0': case '\u1680': case '\u2000': case '\u2001':

                    case '\u2002': case '\u2003': case '\u2004': case '\u2005': case '\u2006':

                    case '\u2007': case '\u2008': case '\u2009': case '\u200A': case '\u202F':

                    case '\u205F': case '\u3000': case '\u2028': case '\u2029': case '\u0009':

                    case '\u000A': case '\u000B': case '\u000C': case '\u000D': case '\u0085':
                        continue;

                    default:
                        src[dstIdx++] = ch;
                        break;
                }
            }
            return new string(src, 0, dstIdx);
        }
    }
}