namespace M3U8Parser.Attributes.BaseAttribute
{
    using System;
    using M3U8Parser.Interfaces;

    public class HdcpLevelType : ICustomAttribute, IEquatable<HdcpLevelType>
    {
        private readonly string _value;

        public HdcpLevelType()
        {
        }

        private HdcpLevelType(string value)
        {
            this._value = value;
        }

        // ReSharper disable once InconsistentNaming
        public static HdcpLevelType NONE => new ("NONE");

        // ReSharper disable once InconsistentNaming
        public static HdcpLevelType TYPE_0 => new ("TYPE-0");

        // ReSharper disable once InconsistentNaming
        public static HdcpLevelType TYPE_1 => new ("TYPE-1");

        public object ParseFromString(string value)
        {
            return value switch
            {
                "NONE" => NONE,
                "TYPE-0" => TYPE_0,
                "TYPE-1" => TYPE_1,
                _ => (object)null
            };
        }

        public bool Equals(HdcpLevelType other)
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