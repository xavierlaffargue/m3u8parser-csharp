namespace M3U8Parser;

using System;

public class MediaType : IChangeType, IEquatable<MediaType>
{
	public MediaType()
	{
	}

	private MediaType(string value)
	{
		_value = value;
	}

	private string _value { get; }

	public static MediaType Audio => new MediaType("AUDIO");

	public static MediaType Video => new MediaType("VIDEO");

	public static MediaType Subtitles => new MediaType("SUBTITLES");

	public static MediaType CloseCaptions => new MediaType("CLOSED-CAPTIONS");

	public override string ToString()
	{
		return _value;
	}

	public object LoadFromString(string value)
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
		if (other.ToString() == this.ToString())
			return true;

		return false;
	}

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj))
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != this.GetType())
		{
			return false;
		}

		return Equals((MediaType)obj);
	}

	public override int GetHashCode()
	{
		return (_value != null ? _value.GetHashCode() : 0);
	}
}

public interface IChangeType
{
	public object LoadFromString(string value);
}