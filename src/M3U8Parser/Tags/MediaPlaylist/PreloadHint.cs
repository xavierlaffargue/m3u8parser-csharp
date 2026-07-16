namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class PreloadHint : AbstractTag
    {
        private readonly UnquotedStringAttribute _type = new ("TYPE");
        private readonly StringAttribute _uri = new ("URI");
        private readonly IntegerAttribute _byterangeStart = new ("BYTERANGE-START");
        private readonly IntegerAttribute _byterangeLength = new ("BYTERANGE-LENGTH");

        public PreloadHint()
        {
        }

        public PreloadHint(string str)
            : base(str)
        {
        }

        public string Type
        {
            get => _type.Value;
            set => _type.Value = value;
        }

        public string Uri
        {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public int? ByteRangeStart
        {
            get => _byterangeStart.Value;
            set => _byterangeStart.Value = value;
        }

        public int? ByteRangeLength
        {
            get => _byterangeLength.Value;
            set => _byterangeLength.Value = value;
        }

        protected override string TagName => Tag.EXTXPRELOADHINT;
    }
}
