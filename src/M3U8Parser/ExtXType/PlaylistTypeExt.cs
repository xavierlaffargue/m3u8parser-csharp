﻿namespace M3U8Parser.ExtXType
{
    using System;
    using System.Text.RegularExpressions;
    using M3U8Parser.Attributes.BaseAttribute;
    using M3U8Parser.Interfaces;

    public class PlaylistTypeExt : BaseExtX
    {
        public const string Prefix = "#EXT-X-PLAYLIST-TYPE";

        public PlaylistTypeExt()
        {
        }

        public PlaylistTypeExt(string str)
        {
            Read(str);
        }

        public PlaylistType Value { get; set; }

        protected override string ExtPrefix => Prefix;

        public void Read(string content)
        {
            var match = Regex.Match(content.Trim(), $"(?<={Prefix}:)(.*?)(?=$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);

            var type = typeof(PlaylistType);

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
                    Value = (PlaylistType)instanceAttribute.ParseFromString(valueFounded);
                }
                else
                {
                    Value = (PlaylistType)Convert.ChangeType(valueFounded, type);
                }
            }
            else
            {
                Value = default;
            }
        }
    }
}