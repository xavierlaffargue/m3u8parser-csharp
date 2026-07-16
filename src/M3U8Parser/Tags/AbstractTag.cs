namespace M3U8Parser.Tags
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using M3U8Parser.Extensions;
    using M3U8Parser.Interfaces;

    public abstract class AbstractTag : ITag
    {
        private static readonly ConcurrentDictionary<Type, FieldInfo[]> AttributeFieldsCache = new ();

        protected AbstractTag()
        {
        }

        protected AbstractTag(string str)
        {
            ReadAllAttributes(str);
        }

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

        protected void ReadAllAttributes(string str)
        {
            using (M3U8AttributeParser.EnterContext(str))
            {
                var fields = GetAttributeFields(GetType());
                foreach (var field in fields)
                {
                    var attr = field.GetValue(this) as IAttribute;
                    attr?.Read(str);
                }
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
