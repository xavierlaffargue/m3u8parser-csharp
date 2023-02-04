namespace M3U8Parser.Attributes
{
    using M3U8Parser.CustomType;
	using M3U8Parser.Interfaces;
	using System;
    using System.Text.RegularExpressions;

    public class CustomAttribute<T> : IAttribute
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

            var type = typeof(T);
            
			if (match.Success)
			{
				var valueFounded = match.Groups[0].Value.Split('=')[1];

				if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) 
				{
					type = Nullable.GetUnderlyingType(type);
				}
				
				if (typeof(ICustomAttribute).IsAssignableFrom(type))
				{
					var instanceAttribute = (ICustomAttribute)Activator.CreateInstance(type, false);
					Value = (T)instanceAttribute.ParseFromString(valueFounded);
				}
				else
				{
					Value = (T)Convert.ChangeType(valueFounded, type);
				}
			}
			else
			{
				Value = default(T);
			}
		}
	}
}