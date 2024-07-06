namespace M3U8Parser.Attributes.BaseAttribute
{
    using System;
    using M3U8Parser.Interfaces;

    public class VideoRangeType : ICustomAttribute, IEquatable<VideoRangeType>
    {
        private readonly string _value;

        public VideoRangeType()
        {
        }

        private VideoRangeType(string value)
        {
            _value = value;
        }

        public static VideoRangeType PQ => new ("PQ");

        public static VideoRangeType HLG => new ("HLG");

        public static VideoRangeType SDR => new ("SDR");

        public object ParseFromString(string value)
        {
            return value switch
            {
                "PQ" => PQ,
                "HLG" => HLG,
                "SDR" => SDR,
                _ => null
            };
        }

        public bool Equals(VideoRangeType other)
        {
            if (other!.ToString() == ToString())
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