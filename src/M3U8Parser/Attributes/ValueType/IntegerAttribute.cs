namespace M3U8Parser.Attributes.ValueType
{
    public class IntegerAttribute : CustomAttribute<int?>
    {
        public IntegerAttribute(string attributeName)
            : base(attributeName)
        {
        }
    }
}