namespace M3U8Parser.Extensions
{
    public static class StringExtension
    {
        public static string RemoveLastCharacter(this string str)
        {
            return str.Remove(str.Length - 1, 1);
        }
    }
}