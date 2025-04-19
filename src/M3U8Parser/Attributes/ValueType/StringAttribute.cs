namespace M3U8Parser.Attributes.ValueType
{
    using System;
    using System.Text.RegularExpressions;

    public class StringAttribute : CustomAttribute<string>
    {
        public StringAttribute(string attributeName)
            : base(attributeName)
        {
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return string.Empty;
            }

            Value = Regex.Replace(Value, "(?<!\\\\)\"", "\\\"");
            return $"{AttributeName}=\"{Value}\"";
        }

        public override void Read(string content)
        {
            var regexStr = Regex.Match(content.Trim(), $"(?<={AttributeName}=\")(.*?)(?=\",|\"$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
            if (!regexStr.Success)
            {
                return;
            }

            var valueFounded = regexStr.Groups[0].Value;
            Value = (string)Convert.ChangeType(valueFounded, typeof(string));
        }
    }
}