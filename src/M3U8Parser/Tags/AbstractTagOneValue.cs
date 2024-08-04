namespace M3U8Parser.Tags
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using M3U8Parser.Extensions;
    using M3U8Parser.Interfaces;

    public abstract class AbstractTagOneValue<T> : ITag
    {
        protected AbstractTagOneValue()
        {
        }

        protected AbstractTagOneValue(string value)
        {
            ReadValue(value);
        }

        public T Value { get; set; }

        protected virtual string TagName => string.Empty;

        public new string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(TagName);
            strBuilder.Append(':');

            WriteAllAttributes(strBuilder);

            return strBuilder.ToString().RemoveLastCharacter();
        }

        protected void ReadValue(string str)
        {
            var match = Regex.Match(str.Trim(), $"(?<={TagName}:)(.*?)(?=$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);

            var type = typeof(T);

            if (match.Success)
            {
                var valueFounded = match.Groups[0].Value;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
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
                Value = default;
            }
        }

        protected void WriteAllAttributes(StringBuilder strBuilder)
        {
            foreach (var field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var isAnAttribute = typeof(IAttribute).IsAssignableFrom(field.FieldType);

                if (isAnAttribute)
                {
                    var m = field.FieldType.GetMethod("ToString");
                    if (m != null)
                    {
                        var str = m.Invoke(field.GetValue(this), System.Array.Empty<object>());
                        strBuilder.AppendWithSeparator(str.ToString(), ",");
                    }
                }
            }
        }
    }
}