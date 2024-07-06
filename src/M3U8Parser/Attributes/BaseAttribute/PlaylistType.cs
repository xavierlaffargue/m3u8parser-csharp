namespace M3U8Parser.Attributes.BaseAttribute
{
    using System;
    using M3U8Parser.Interfaces;

    public class PlaylistType : ICustomAttribute, IEquatable<PlaylistType>
    {
        public PlaylistType()
        {
        }

        private PlaylistType(string value)
        {
            _value = value;
        }

        private string _value { get; }

        public static PlaylistType Event => new ("EVENT");

        public static PlaylistType Vod => new ("VOD");

        public object ParseFromString(string value)
        {
            switch (value)
            {
                case "EVENT":
                    return Event;

                case "VOD":
                    return Vod;

                default:
                    return null;
            }
        }

        public bool Equals(PlaylistType other)
        {
            if (other.ToString() == ToString())
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