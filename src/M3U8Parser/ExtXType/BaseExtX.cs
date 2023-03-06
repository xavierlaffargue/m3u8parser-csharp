using M3U8Parser.Extensions;
using M3U8Parser.Interfaces;
using System.Reflection;
using System.Text;

namespace M3U8Parser.ExtXType
{
    public abstract class BaseExtX : IExtXType
    {
        protected virtual string ExtPrefix => "";

        public BaseExtX() { }

        public BaseExtX(string str)
        {
            ReadAllAttributes(str);
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(ExtPrefix);
            strBuilder.Append(":");

            WriteAllAttributes(strBuilder);

            return strBuilder.ToString().RemoveLastCharacter();
        }

        protected void ReadAllAttributes(string str)
        {
            foreach (var field in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var isAnAttribute = typeof(IAttribute).IsAssignableFrom(field.FieldType);

                if (isAnAttribute)
                {
                    MethodInfo m = field.FieldType.GetMethod("Read");
                    m.Invoke(field.GetValue(this), new object[] { str });
                }
            }
        }

        protected void WriteAllAttributes(StringBuilder strBuilder)
        {
            foreach (var field in this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var isAnAttribute = typeof(IAttribute).IsAssignableFrom(field.FieldType);

                if (isAnAttribute)
                {
                    MethodInfo m = field.FieldType.GetMethod("ToString");
                    var str = m.Invoke(field.GetValue(this), new object[] { });
                    strBuilder.AppendWithSeparator(str.ToString(), ",");
                }
            }
        }
    }
}
