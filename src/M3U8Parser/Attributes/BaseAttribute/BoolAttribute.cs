namespace M3U8Parser.Attributes.BaseAttribute
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
            var pattern = $"(?={AttributeName})(.*?)(?=,|$)";
            var match = Regex.Match(content.Trim(), pattern, RegexOptions.Multiline & RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return;
            }

            var valueFounded = match.Groups[0].Value.Split('=')[1];
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