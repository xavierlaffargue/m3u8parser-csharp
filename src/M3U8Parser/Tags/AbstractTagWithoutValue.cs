namespace M3U8Parser.Tags
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using M3U8Parser.Extensions;
    using M3U8Parser.Interfaces;

    public abstract class AbstractTagWithoutValue<T> : ITag
    {
        protected AbstractTagWithoutValue()
        {
        }

        protected AbstractTagWithoutValue(string value)
        {
            if (value.Contains(TagName))
            {
                IsPresent = true;
            }
            else
            {
                IsPresent = false;
            }
        }

        public bool IsPresent { get; }

        protected virtual string TagName => string.Empty;

        public override string ToString()
        {
            if (IsPresent)
            {
                return TagName;
            }

            return string.Empty;
        }
    }
}