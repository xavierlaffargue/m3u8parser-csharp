using System;

namespace M3U8Parser;

public static class Tools
{
	public static T ParseEnum<T>(string value)
	{
		return (T)Enum.Parse(typeof(T), value, true);
	}
}