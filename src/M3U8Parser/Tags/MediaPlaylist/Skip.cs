namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class Skip : AbstractTag
    {
        private readonly IntegerAttribute _skippedSegments = new ("SKIPPED-SEGMENTS");
        private readonly StringAttribute _recentlyRemovedDateRanges = new ("RECENTLY-REMOVED-DATERANGES");

        public Skip()
        {
        }

        public Skip(string str)
            : base(str)
        {
        }

        public int? SkippedSegments
        {
            get => _skippedSegments.Value;
            set => _skippedSegments.Value = value;
        }

        public string RecentlyRemovedDateRanges
        {
            get => _recentlyRemovedDateRanges.Value;
            set => _recentlyRemovedDateRanges.Value = value;
        }

        protected override string TagName => Tag.EXTXSKIP;
    }
}
