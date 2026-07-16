namespace M3U8Parser.Attributes.ValueType
{
    public class UnquotedStringAttribute : CustomAttribute<string>
    {
        public UnquotedStringAttribute(string attributeName)
            : base(attributeName)
        {
        }
    }
}
