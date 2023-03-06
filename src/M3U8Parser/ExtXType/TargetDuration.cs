using System;
using System.Text.RegularExpressions;
using M3U8Parser.Interfaces;

namespace M3U8Parser.ExtXType
{
    public class TargetDuration : BaseExtX
    {
        public static string Prefix = "#EXT-X-TARGETDURATION";

        protected override string ExtPrefix => TargetDuration.Prefix;

        public int Value { get; set; }

        public TargetDuration()
        {
        }

        public TargetDuration(string str)
        {
            Read(str);
        }

        public void Read(string content)
        {
            var match = Regex.Match(content.Trim(), $"(?<={Prefix}:)(.*?)(?=$)",
                RegexOptions.Multiline & RegexOptions.IgnoreCase);

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
                Value = default(int);
            }
        }
    }
}