namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class Autoselect : BoolAttribute
    {
        public Autoselect()
            : base("AUTOSELECT")
        {
        }
    }
}