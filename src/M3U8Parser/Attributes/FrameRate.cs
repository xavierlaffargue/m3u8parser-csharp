namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class FrameRate : DecimalAttribute
    {
        public FrameRate()
            : base("FRAME-RATE")
        {
        }
    }
}