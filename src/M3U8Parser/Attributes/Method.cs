using M3U8Parser.CustomType;

namespace M3U8Parser.Attributes
{
    public class Method : CustomAttribute<MethodType>
    {
        public Method() : base("METHOD")
        {
        }
    }
}