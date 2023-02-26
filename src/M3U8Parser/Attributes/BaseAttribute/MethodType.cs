namespace M3U8Parser.CustomType
{
    using System;

    public class MethodType : ICustomAttribute, IEquatable<MethodType>
	{
		public MethodType()
		{
		}

		private MethodType(string value)
		{
			_value = value;
		}

		private string _value { get; }

		public static MethodType None => new ("NONE");

        public static MethodType AES_128 => new("AES-128");

        public static MethodType SAMPLE_AES => new("SAMPLE-AES");

        public override string ToString()
		{
			return _value;
		}

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
			if (other.ToString() == ToString())
			{
				return true;
			}

			return false;
		}

        public override int GetHashCode()
		{
			return _value != null ? _value.GetHashCode() : 0;
		}
	}
}