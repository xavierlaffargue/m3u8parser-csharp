namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class ClosedCaptions : StringAttribute
    {
        private const string ClosedCaptionsAttribute = "CLOSED-CAPTIONS";

        public ClosedCaptions()
            : base(ClosedCaptionsAttribute)
        {
        }

        public override void Read(string content)
        {
            base.Read(content);
            if (Value is null)
            {
                Value = "NONE";
            }
        }

        public override string ToString()
        {
            if (Value == "NONE")
            {
                return $"{ClosedCaptionsAttribute}=NONE";
            }

            return base.ToString();
        }
    }
}