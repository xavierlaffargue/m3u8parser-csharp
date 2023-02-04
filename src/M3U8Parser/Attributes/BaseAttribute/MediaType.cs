namespace M3U8Parser.CustomType
{
    using System;

    public class MediaType : ICustomAttribute, IEquatable<MediaType>
	{
		public MediaType()
		{
		}

		private MediaType(string value)
		{
			_value = value;
		}

		private string _value { get; }

		public static MediaType Audio => new ("AUDIO");

		public static MediaType Video => new ("VIDEO");

		public static MediaType Subtitles => new ("SUBTITLES");

		public static MediaType CloseCaptions => new ("CLOSED-CAPTIONS");

		public override string ToString()
		{
			return _value;
		}

		public object ParseFromString(string value)
		{
			switch (value)
			{
				case "AUDIO":
					return Audio;

				case "VIDEO":
					return Video;

				case "SUBTITLES":
					return Subtitles;

				case "CLOSED-CAPTIONS":
					return CloseCaptions;

				default:
					return null;
			}
		}

		public bool Equals(MediaType other)
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