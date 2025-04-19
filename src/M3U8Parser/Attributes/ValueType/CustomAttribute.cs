namespace M3U8Parser.Attributes.ValueType
{
    using System;
    using System.Text.RegularExpressions;
    using M3U8Parser.Interfaces;

    public class CustomAttribute<T> : IAttribute
    {
        public CustomAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        public virtual T Value { get; set; }

        protected string AttributeName { get; }

        public override string ToString()
        {
            return Value != null ? $"{AttributeName}={Value}" : string.Empty;
        }

        public virtual void Read(string content)
        {
            var match = Regex.Match(content.Trim(), $"[,|:](?={AttributeName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);

            var type = typeof(T);

            if (match.Success)
            {
                var valueFounded = match.Groups[0].Value.Split('=')[1];

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (typeof(ICustomAttribute).IsAssignableFrom(type))
                {
                    var instanceAttribute = (ICustomAttribute)Activator.CreateInstance(type, false);
                    if (instanceAttribute != null)
                    {
                        Value = (T)instanceAttribute.ParseFromString(valueFounded);
                    }
                }
                else
                {
                    if (type != null)
                    {
                        Value = (T)Convert.ChangeType(valueFounded, type);
                    }
                }
            }
            else
            {
                Value = default;
            }
        }
    }
}