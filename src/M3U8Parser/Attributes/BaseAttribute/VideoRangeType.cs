using System;
using M3U8Parser.Interfaces;

namespace M3U8Parser.Attributes.BaseAttribute
{
	public class VideoRangeType : ICustomAttribute, IEquatable<VideoRangeType>
	{
		public VideoRangeType()
		{
		}

		private VideoRangeType(string value)
		{
			_value = value;
		}

		private string _value { get; }

		public static VideoRangeType PQ => new ("PQ");

		public static VideoRangeType HLG => new ("HLG");

		public static VideoRangeType SDR => new ("SDR");

        public override string ToString()
		{
			return _value;
		}

		public object ParseFromString(string value)
		{
			switch (value)
			{
				case "PQ":
					return PQ;

				case "HLG":
					return HLG;

				case "SDR":
					return SDR;
                
				default:
					return null;
			}
		}

		public bool Equals(VideoRangeType other)
		{
			if (other!.ToString() == ToString())
			{
				return true;
			}

			return false;
		}
	}
}