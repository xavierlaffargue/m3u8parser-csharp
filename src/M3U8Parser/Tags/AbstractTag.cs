namespace M3U8Parser.Tags
{
    using System.Reflection;
    using System.Text;
    using M3U8Parser.Extensions;
    using M3U8Parser.Interfaces;

    public abstract class AbstractTag : ITag
    {
        protected AbstractTag()
        {
        }

        protected AbstractTag(string str)
        {
            ReadAllAttributes(str);
        }

        protected virtual string TagName => string.Empty;

        public new string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(TagName);
            strBuilder.Append(':');

            WriteAllAttributes(strBuilder);

            return strBuilder.ToString().RemoveLastCharacter();
        }

        protected void ReadAllAttributes(string str)
        {
            foreach (var field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var isAnAttribute = typeof(IAttribute).IsAssignableFrom(field.FieldType);

                if (isAnAttribute)
                {
                    var m = field.FieldType.GetMethod("Read");
                    if (m != null)
                    {
                        m.Invoke(field.GetValue(this), new object[] { str });
                    }
                }
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