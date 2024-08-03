namespace M3U8Parser.Attributes.BaseAttribute
{
    using System;
    using M3U8Parser.Interfaces;

    public class MediaType : ICustomAttribute, IEquatable<MediaType>
    {
        private readonly string _value;

        public MediaType()
        {
        }

        private MediaType(string value)
        {
            _value = value;
        }

        public static MediaType Audio => new ("AUDIO");

        public static MediaType Video => new ("VIDEO");

        public static MediaType Subtitles => new ("SUBTITLES");

        public static MediaType CloseCaptions => new ("CLOSED-CAPTIONS");

        public object ParseFromString(string value)
        {
            return value switch
            {
                "AUDIO" => Audio,
                "VIDEO" => Video,
                "SUBTITLES" => Subtitles,
                "CLOSED-CAPTIONS" => CloseCaptions,
                _ => (object)null
            };
        }

        public bool Equals(MediaType other)
        {
            return other != null && other.ToString() == ToString();
        }

        public override string ToString()
        {
            return _value;
        }
    }
}