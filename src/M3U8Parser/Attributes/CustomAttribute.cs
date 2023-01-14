namespace M3U8Parser.Attributes
{
    using M3U8Parser.CustomType;
    using System;
    using System.Text.RegularExpressions;

    public class CustomAttribute<T>
	{
		public CustomAttribute(string attributeName)
		{
			AttributeName = attributeName;
		}
        
		public T Value { get; set; }
        
        protected string AttributeName { get; }

		public override string ToString()
		{
			if (Value != null)
			{
				return $"{AttributeName}={Value}";
			}

			return string.Empty;
		}

		public virtual void Read(string content)
		{
            var match = Regex.Match(content.Trim(), $"[,|:](?={AttributeName})(.*?)(?=,|$)",
				RegexOptions.Multiline & RegexOptions.IgnoreCase);

			if (match.Success)
			{
				var valueFounded = match.Groups[0].Value.Split('=')[1];

				if (typeof(ICustomAttribute).IsAssignableFrom(typeof(T)))
				{
					var instanceAttribute = (ICustomAttribute)Activator.CreateInstance(typeof(T), false);
					Value = (T)instanceAttribute.ParseFromString(valueFounded);
				}
				else
				{
					Value = (T)Convert.ChangeType(valueFounded, typeof(T));
				}
			}
		}
	}
}