namespace M3U8Parser.Attributes.ValueType
{
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class DecimalAttribute : CustomAttribute<decimal?>
    {
        public DecimalAttribute(string attributeName)
            : base(attributeName)
        {
        }

        public override string ToString()
        {
            return Value != null
                ? $"{AttributeName}={Value.Value.ToString(CultureInfo.InvariantCulture)}"
                : string.Empty;
        }

        public override void Read(string content)
        {
            string valueFounded;
            if (!M3U8AttributeParser.TryGetValue(AttributeName, out valueFounded))
            {
                var pattern = $"(?={AttributeName})(.*?)(?=,|$)";
                var match = Regex.Match(content.Trim(), pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var parts = match.Groups[0].Value.Split(new[] { '=' }, 2);
                    valueFounded = parts.Length > 1 ? parts[1] : string.Empty;
                }
                else
                {
                    Value = null;
                    return;
                }
            }

            if (!string.IsNullOrEmpty(valueFounded))
            {
                Value = decimal.Parse(valueFounded, CultureInfo.InvariantCulture);
            }
            else
            {
                Value = null;
            }
        }
    }
}