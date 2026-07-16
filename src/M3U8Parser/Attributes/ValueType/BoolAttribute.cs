namespace M3U8Parser.Attributes.ValueType
{
    using System.Text.RegularExpressions;

    public class BoolAttribute : CustomAttribute<bool>
    {
        private const string YesValue = "YES";
        private const string NoValue = "NO";

        public BoolAttribute(string attributeName)
            : base(attributeName)
        {
        }

        public override string ToString()
        {
            return $"{AttributeName}={BoolToString(Value)}";
        }

        public override void Read(string content)
        {
            string valueFounded;
            if (!M3U8AttributeParser.TryGetValue(AttributeName, out valueFounded))
            {
                var pattern = $"(?={AttributeName})(.*?)(?=,|$)";
                var match = Regex.Match(content.Trim(), pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return;
                }

                var parts = match.Groups[0].Value.Split(new[] { '=' }, 2);
                valueFounded = parts.Length > 1 ? parts[1] : string.Empty;
            }

            Value = StringToBool(valueFounded);
        }

        private static string BoolToString(bool value)
        {
            return value ? YesValue : NoValue;
        }

        private static bool StringToBool(string value)
        {
            return value == YesValue;
        }
    }
}