namespace M3U8Parser.Attributes.BaseAttribute
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

        public static MethodType AES_128 => new ("AES-128");

        public static MethodType SAMPLE_AES => new ("SAMPLE-AES");

        public object ParseFromString(string value)
        {
            switch (value)
            {
                case "NONE":
                    return None;

                case "AES-128":
                    return AES_128;

                case "SAMPLE-AES":
                    return SAMPLE_AES;

                default:
                    return null;
            }
        }

        public bool Equals(MethodType other)
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