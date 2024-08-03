namespace M3U8Parser.ExtXType
{
    using System;
    using System.Text.RegularExpressions;
    using M3U8Parser.Interfaces;

    public class MediaSequence : BaseExtX
    {
        public const string Prefix = "#EXT-X-MEDIA-SEQUENCE";

        public MediaSequence()
        {
        }

        public MediaSequence(string str)
        {
            Read(str);
        }

        public int Value { get; set; }

        protected override string ExtPrefix => HlsVersion.Prefix;

        public void Read(string content)
        {
            var match = Regex.Match(content.Trim(), $"(?<={Prefix}:)(.*?)(?=$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);

            var type = typeof(int);

            if (match.Success)
            {
                var valueFounded = match.Groups[0].Value;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
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