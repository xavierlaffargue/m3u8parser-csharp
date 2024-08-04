namespace M3U8Parser.Attributes.ValueType
{
    using System;
    using M3U8Parser.Interfaces;

    public class MethodType : ICustomAttribute, IEquatable<MethodType>
    {
        private readonly string _value;

        public MethodType()
        {
        }

        private MethodType(string value)
        {
            _value = value;
        }

        public static MethodType None => new ("NONE");

        // ReSharper disable once InconsistentNaming
        public static MethodType AES_128 => new ("AES-128");

        // ReSharper disable once InconsistentNaming
        public static MethodType SAMPLE_AES => new ("SAMPLE-AES");

        public object ParseFromString(string value)
        {
            return value switch
            {
                "NONE" => None,
                "AES-128" => AES_128,
                "SAMPLE-AES" => SAMPLE_AES,
                _ => (object)null
            };
        }

        public bool Equals(MethodType other)
        {
            return other != null && other.ToString() == ToString();
        }

        public override string ToString()
        {
            return _value;
        }
    }
}