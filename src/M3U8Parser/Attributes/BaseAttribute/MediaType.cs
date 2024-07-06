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
            switch (value)
            {
                case "AUDIO":
                    return Audio;

                case "VIDEO":
                    return Video;

                case "SUBTITLES":
                    return Subtitles;

                case "CLOSED-CAPTIONS":
                    return CloseCaptions;

                default:
                    return null;
            }
        }

        public bool Equals(MediaType other)
        {
            if (other != null && other.ToString() == ToString())
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}