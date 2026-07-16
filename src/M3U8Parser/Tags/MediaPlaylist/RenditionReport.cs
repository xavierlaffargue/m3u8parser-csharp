namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class RenditionReport : AbstractTag
    {
        private readonly StringAttribute _uri = new ("URI");
        private readonly IntegerAttribute _lastMsn = new ("LAST-MSN");
        private readonly IntegerAttribute _lastPart = new ("LAST-PART");

        public RenditionReport()
        {
        }

        public RenditionReport(string str)
            : base(str)
        {
        }

        public string Uri
        {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public int? LastMsn
        {
            get => _lastMsn.Value;
            set => _lastMsn.Value = value;
        }

        public int? LastPart
        {
            get => _lastPart.Value;
            set => _lastPart.Value = value;
        }

        protected override string TagName => Tag.EXTXRENDITIONREPORT;
    }
}
