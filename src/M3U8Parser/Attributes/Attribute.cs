using System;
using System.Text.RegularExpressions;

namespace M3U8Parser
{

	using System.Reflection;

	public class Attribute<T>
	{
		private readonly string separator = "=";

		public Attribute(string attributeName)
		{
			AttributeName = attributeName;
		}

		public string AttributeName { get; }

		public T Value { get; set; }

		public override string ToString()
		{
			if (Value != null)
			{
				var textValue = $"{Value}";
				if (Value is bool boolValue)
					textValue = BoolToString(boolValue);
				else if (Value is string stringValue) textValue = $"\"{textValue}\"";

				return $"{AttributeName}{separator}{textValue}";
			}

			return string.Empty;
		}

		private string BoolToString(bool value)
		{
			if (value) return "YES";

			return "NO";
		}

		private bool StringToBool(string value)
		{
			if (value == "YES") return true;

			return false;
		}


		public void Read(string content)
		{
			var regexStr = Regex.Match(content.Trim(), $"(?<={AttributeName}=\")(.*?)(?=\",|\"$)",
				RegexOptions.Multiline & RegexOptions.IgnoreCase);

			if (regexStr.Success)
			{
				var valueFounded = regexStr.Groups[0].Value;
				Value = (T)Convert.ChangeType(valueFounded, typeof(T));
				return;
			}

			var match = Regex.Match(content.Trim(), $"(?={AttributeName})(.*?)(?=,|$)",
				RegexOptions.Multiline & RegexOptions.IgnoreCase);

			if (match.Success)
			{
				var valueFounded = match.Groups[0].Value.Split('=')[1];

				if (typeof(T) == typeof(bool))
					Value = (T)Convert.ChangeType(StringToBool(valueFounded), typeof(T));
				else if (typeof(IChangeType).IsAssignableFrom(typeof(T)))
				{
					//var result = ((IChangeType)(typeof(T))).LoadFromString(valueFounded);

					IChangeType myInterface = (IChangeType)Activator.CreateInstance(typeof(T), false);
					Value = (T)myInterface.LoadFromString(valueFounded);
				}
				else
					Value = (T)Convert.ChangeType(valueFounded, typeof(T));
			}
		}
	}
}