namespace M3U8Parser.CustomType
{
    using System;

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

		public override string ToString()
		{
			return _value;
		}

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

        public override int GetHashCode()
		{
			return _value != null ? _value.GetHashCode() : 0;
		}
	}
}