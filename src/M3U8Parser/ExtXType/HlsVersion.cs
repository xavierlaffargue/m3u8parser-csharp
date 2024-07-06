namespace M3U8Parser.ExtXType
{
    using System;
    using System.Text.RegularExpressions;
    using M3U8Parser.Interfaces;

    public class HlsVersion : BaseExtX
    {
        public const string Prefix = "#EXT-X-VERSION";

        public HlsVersion()
        {
        }

        public HlsVersion(string str)
        {
            Read(str);
        }

        public int Value { get; set; }

        protected override string ExtPrefix => Prefix;

        public void Read(string content)
        {
            var match = Regex.Match(content.Trim(), $"(?<={Prefix}:)(.*?)(?=$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);

            var type = typeof(int);

            if (match.Success)
            {
                var valueFounded = match.Groups[0].Value;

                if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (typeof(ICustomAttribute).IsAssignableFrom(type))
                {
                    var instanceAttribute = (ICustomAttribute)Activator.CreateInstance(type, false);
                    Value = (int)instanceAttribute.ParseFromString(valueFounded);
                }
                else
                {
                    Value = (int)Convert.ChangeType(valueFounded, type);
                }
            }
            else
            {
                Value = default;
            }
        }
    }
}