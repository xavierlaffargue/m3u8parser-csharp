namespace M3U8Parser.Tags
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using M3U8Parser.Extensions;
    using M3U8Parser.Interfaces;

    public abstract class AbstractTagOneValue<T> : ITag
    {
        private static readonly ConcurrentDictionary<Type, FieldInfo[]> AttributeFieldsCache = new ();

        protected AbstractTagOneValue()
        {
        }

        protected AbstractTagOneValue(string value)
        {
            ReadValue(value);
        }

        public T Value { get; set; }

        protected virtual string TagName => string.Empty;

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            WriteAllAttributes(strBuilder);

            if (strBuilder.Length > 0)
            {
                return $"{TagName}:{strBuilder}";
            }

            return TagName;
        }

        protected void ReadValue(string str)
        {
            var match = Regex.Match(str.Trim(), $"(?<={TagName}:)(.*?)(?=$)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

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
            var fields = GetAttributeFields(GetType());
            bool first = true;
            foreach (var field in fields)
            {
                var attr = field.GetValue(this) as IAttribute;
                if (attr != null)
                {
                    var str = attr.ToString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        if (!first)
                        {
                            strBuilder.Append(',');
                        }

                        strBuilder.Append(str);
                        first = false;
                    }
                }
            }
        }

        private static FieldInfo[] GetAttributeFields(Type type)
        {
            return AttributeFieldsCache.GetOrAdd(type, t =>
            {
                var list = new List<FieldInfo>();
                foreach (var field in t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    if (typeof(IAttribute).IsAssignableFrom(field.FieldType))
                    {
                        list.Add(field);
                    }
                }

                return list.ToArray();
            });
        }
    }
}
