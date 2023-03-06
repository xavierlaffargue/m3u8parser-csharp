using System.Text.RegularExpressions;

namespace M3U8Parser.Attributes.BaseAttribute
{
	public class BoolAttribute : CustomAttribute<bool>
	{
        private const string YesValue = "YES";
        private const string NoValue = "NO";

        public BoolAttribute(string attributeName) : base(attributeName)
		{
		}

		public override string ToString()
		{
			return $"{AttributeName}={BoolToString(Value)}";
		}

		public override void Read(string content)
        {
            var pattern = $"(?={AttributeName})(.*?)(?=,|$)";
			var match = Regex.Match(content.Trim(), pattern, RegexOptions.Multiline & RegexOptions.IgnoreCase);
			
            if (match.Success)
			{
				var valueFounded = match.Groups[0].Value.Split('=')[1];
				Value = StringToBool(valueFounded);
			}
		}

		private string BoolToString(bool value)
		{
			return value ? YesValue : NoValue;
		}

		private bool StringToBool(string value)
		{
			return value == YesValue;
		}
	}
}