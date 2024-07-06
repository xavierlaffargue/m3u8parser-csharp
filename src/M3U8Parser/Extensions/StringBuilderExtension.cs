namespace M3U8Parser.Extensions
{
    using System.Text;

    public static class StringBuilderExtension
    {
        public static void AppendWithSeparator(this StringBuilder strBuilder, string text, string separator)
        {
            if (!string.IsNullOrEmpty(text))
            {
                strBuilder.Append(text + separator);
            }
        }
    }
}