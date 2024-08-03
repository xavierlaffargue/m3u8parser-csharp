namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class Method : CustomAttribute<MethodType>
    {
        public Method()
            : base("METHOD")
        {
        }
    }
}