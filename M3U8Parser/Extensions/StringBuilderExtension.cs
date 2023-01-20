using System.Text;

namespace M3U8Parser;

public static class StringBuilderExtension
{
	public static StringBuilder AppendWithSeparator(this StringBuilder strBuilder, string text,
		string separator)
	{
		if (!string.IsNullOrEmpty(text)) strBuilder.Append(text + separator);

		return strBuilder;
	}
}