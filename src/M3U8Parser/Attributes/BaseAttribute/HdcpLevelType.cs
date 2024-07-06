namespace M3U8Parser.Attributes.BaseAttribute
{
    using System;
    using M3U8Parser.Interfaces;

    public class HdcpLevelType : ICustomAttribute, IEquatable<HdcpLevelType>
    {
        public HdcpLevelType()
        {
        }

        private HdcpLevelType(string value)
        {
            this.value = value;
        }

        public static HdcpLevelType NONE => new ("NONE");

        public static HdcpLevelType TYPE_0 => new ("TYPE-0");

        public static HdcpLevelType TYPE_1 => new ("TYPE-1");

        private string value { get; }

        public object ParseFromString(string value)
        {
            switch (value)
            {
                case "NONE":
                    return NONE;

                case "TYPE-0":
                    return TYPE_0;

                case "TYPE-1":
                    return TYPE_1;

                default:
                    return null;
            }
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
            return value;
        }
    }
}