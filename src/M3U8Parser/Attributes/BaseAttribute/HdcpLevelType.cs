using System;
using M3U8Parser.Interfaces;

namespace M3U8Parser.Attributes.BaseAttribute
{
	public class HdcpLevelType : ICustomAttribute, IEquatable<HdcpLevelType>
	{
		public HdcpLevelType()
		{
		}

		private HdcpLevelType(string value)
		{
			_value = value;
		}

		private string _value { get; }

		public static HdcpLevelType NONE => new ("NONE");

		public static HdcpLevelType TYPE_0 => new ("TYPE-0");

		public static HdcpLevelType TYPE_1 => new ("TYPE-1");

        public override string ToString()
		{
			return _value;
		}

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
	}
}