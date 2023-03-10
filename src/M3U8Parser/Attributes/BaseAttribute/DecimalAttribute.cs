using System.Globalization;
using System.Text.RegularExpressions;

namespace M3U8Parser.Attributes.BaseAttribute
{
	public class DecimalAttribute : CustomAttribute<decimal?>
	{
		public DecimalAttribute(string attributeName) : base(attributeName)
		{
		}

		public override string ToString()
		{
			if (Value != null)
			{
				return $"{AttributeName}={Value.Value.ToString(CultureInfo.InvariantCulture)}";
			}

			return string.Empty;
		}
		
		public override void Read(string content)
        {
            var pattern = $"(?={AttributeName})(.*?)(?=,|$)";
			var match = Regex.Match(content.Trim(), pattern, RegexOptions.Multiline & RegexOptions.IgnoreCase);
			
            if (match.Success)
			{
				var valueFounded = match.Groups[0].Value.Split('=')[1];
				Value = decimal.Parse(valueFounded, CultureInfo.InvariantCulture);
			}
		}
	}
}