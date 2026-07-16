namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class DateRange : AbstractTag
    {
        private readonly StringAttribute _id = new ("ID");
        private readonly StringAttribute _class = new ("CLASS");
        private readonly StringAttribute _startDate = new ("START-DATE");
        private readonly StringAttribute _cue = new ("CUE");
        private readonly StringAttribute _endDate = new ("END-DATE");
        private readonly DecimalAttribute _duration = new ("DURATION");
        private readonly DecimalAttribute _plannedDuration = new ("PLANNED-DURATION");
        private readonly UnquotedStringAttribute _scte35Cmd = new ("SCTE35-CMD");
        private readonly UnquotedStringAttribute _scte35Out = new ("SCTE35-OUT");
        private readonly UnquotedStringAttribute _scte35In = new ("SCTE35-IN");
        private readonly UnquotedStringAttribute _endOnNext = new ("END-ON-NEXT");

        // HLS Interstitials Attributes (Appendix D.2)
        private readonly StringAttribute _xAssetUri = new ("X-ASSET-URI");
        private readonly StringAttribute _xAssetList = new ("X-ASSET-LIST");
        private readonly DecimalAttribute _xResumeOffset = new ("X-RESUME-OFFSET");
        private readonly DecimalAttribute _xPlayoutLimit = new ("X-PLAYOUT-LIMIT");
        private readonly StringAttribute _xSnap = new ("X-SNAP");
        private readonly StringAttribute _xRestrict = new ("X-RESTRICT");
        private readonly StringAttribute _xContentMayVary = new ("X-CONTENT-MAY-VARY");
        private readonly StringAttribute _xTimelineOccupies = new ("X-TIMELINE-OCCUPIES");
        private readonly StringAttribute _xTimelineStyle = new ("X-TIMELINE-STYLE");

        // HLS Skip Button Control Attributes (Appendix D.3)
        private readonly IntegerAttribute _xSkipControlOffset = new ("X-SKIP-CONTROL-OFFSET");
        private readonly IntegerAttribute _xSkipControlDuration = new ("X-SKIP-CONTROL-DURATION");
        private readonly StringAttribute _xSkipControlLabelId = new ("X-SKIP-CONTROL-LABEL-ID");

        // HLS Date Range Preloading Attributes (Appendix F)
        private readonly StringAttribute _xUri = new ("X-URI");
        private readonly StringAttribute _xTargetId = new ("X-TARGET-ID");
        private readonly StringAttribute _xTargetClass = new ("X-TARGET-CLASS");

        public DateRange()
        {
        }

        public DateRange(string str)
            : base(str)
        {
        }

        public string Id
        {
            get => _id.Value;
            set => _id.Value = value;
        }

        public string Class
        {
            get => _class.Value;
            set => _class.Value = value;
        }

        public string StartDate
        {
            get => _startDate.Value;
            set => _startDate.Value = value;
        }

        public string Cue
        {
            get => _cue.Value;
            set => _cue.Value = value;
        }

        public string EndDate
        {
            get => _endDate.Value;
            set => _endDate.Value = value;
        }

        public decimal? Duration
        {
            get => _duration.Value;
            set => _duration.Value = value;
        }

        public decimal? PlannedDuration
        {
            get => _plannedDuration.Value;
            set => _plannedDuration.Value = value;
        }

        public string Scte35Cmd
        {
            get => _scte35Cmd.Value;
            set => _scte35Cmd.Value = value;
        }

        public string Scte35Out
        {
            get => _scte35Out.Value;
            set => _scte35Out.Value = value;
        }

        public string Scte35In
        {
            get => _scte35In.Value;
            set => _scte35In.Value = value;
        }

        public string EndOnNext
        {
            get => _endOnNext.Value;
            set => _endOnNext.Value = value;
        }

        public string XAssetUri
        {
            get => _xAssetUri.Value;
            set => _xAssetUri.Value = value;
        }

        public string XAssetList
        {
            get => _xAssetList.Value;
            set => _xAssetList.Value = value;
        }

        public decimal? XResumeOffset
        {
            get => _xResumeOffset.Value;
            set => _xResumeOffset.Value = value;
        }

        public decimal? XPlayoutLimit
        {
            get => _xPlayoutLimit.Value;
            set => _xPlayoutLimit.Value = value;
        }

        public string XSnap
        {
            get => _xSnap.Value;
            set => _xSnap.Value = value;
        }

        public string XRestrict
        {
            get => _xRestrict.Value;
            set => _xRestrict.Value = value;
        }

        public string XContentMayVary
        {
            get => _xContentMayVary.Value;
            set => _xContentMayVary.Value = value;
        }

        public string XTimelineOccupies
        {
            get => _xTimelineOccupies.Value;
            set => _xTimelineOccupies.Value = value;
        }

        public string XTimelineStyle
        {
            get => _xTimelineStyle.Value;
            set => _xTimelineStyle.Value = value;
        }

        public int? XSkipControlOffset
        {
            get => _xSkipControlOffset.Value;
            set => _xSkipControlOffset.Value = value;
        }

        public int? XSkipControlDuration
        {
            get => _xSkipControlDuration.Value;
            set => _xSkipControlDuration.Value = value;
        }

        public string XSkipControlLabelId
        {
            get => _xSkipControlLabelId.Value;
            set => _xSkipControlLabelId.Value = value;
        }

        public string XUri
        {
            get => _xUri.Value;
            set => _xUri.Value = value;
        }

        public string XTargetId
        {
            get => _xTargetId.Value;
            set => _xTargetId.Value = value;
        }

        public string XTargetClass
        {
            get => _xTargetClass.Value;
            set => _xTargetClass.Value = value;
        }

        protected override string TagName => Tag.EXTXDATERANGE;
    }
}
