namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class Method : CustomAttribute<MethodType>
    {
        public Method()
            : base("METHOD")
        {
        }
    }
}