// ReSharper disable InconsistentNaming
namespace M3U8Parser.Attributes.ValueType
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
            return other!.ToString() == ToString();
        }

        public override string ToString()
        {
            return _value;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as VideoRangeType);
        }

        public override int GetHashCode()
        {
            return _value != null ? _value.GetHashCode() : 0;
        }
    }
}