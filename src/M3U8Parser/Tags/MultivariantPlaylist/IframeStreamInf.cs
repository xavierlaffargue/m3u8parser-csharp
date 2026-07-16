namespace M3U8Parser.Tags.MultivariantPlaylist
{
    using M3U8Parser.Attributes.Name;
    using M3U8Parser.Attributes.ValueType;

    public class IframeStreamInf : AbstractTag
    {
        private readonly AverageBandwidth _averageBandwidth = new ();
        private readonly Bandwidth _bandwidth = new ();
        private readonly Codecs _codecs = new ();
        private readonly HdcpLevel _hdcpLevel = new ();
        private readonly Resolution _resolutionAttribute = new ();
        private readonly Uri _uri = new ();
        private readonly Video _video = new ();
        private readonly VideoRange _videoRange = new ();

        // New attributes introduced in RFC 8216 Bis / draft 22
        private readonly DecimalAttribute _score = new ("SCORE");
        private readonly StringAttribute _supplementalCodecs = new ("SUPPLEMENTAL-CODECS");
        private readonly StringAttribute _allowedCpc = new ("ALLOWED-CPC");
        private readonly StringAttribute _stableVariantId = new ("STABLE-VARIANT-ID");
        private readonly StringAttribute _pathwayId = new ("PATHWAY-ID");
        private readonly StringAttribute _reqVideoLayout = new ("REQ-VIDEO-LAYOUT");

        public IframeStreamInf()
        {
        }

        public IframeStreamInf(string str)
            : base(str)
        {
        }

        public long Bandwidth
        {
            get => _bandwidth.Value;
            set => _bandwidth.Value = value;
        }

        public long? AverageBandwidth
        {
            get => _averageBandwidth.Value;
            set => _averageBandwidth.Value = value;
        }

        public string Codecs
        {
            get => _codecs.Value;
            set => _codecs.Value = value;
        }

        public VideoRangeType VideoRange
        {
            get => _videoRange.Value;
            set => _videoRange.Value = value;
        }

        public HdcpLevelType HdcpLevel
        {
            get => _hdcpLevel.Value;
            set => _hdcpLevel.Value = value;
        }

        public string Video
        {
            get => _video.Value;
            set => _video.Value = value;
        }

        public ResolutionType Resolution
        {
            get => _resolutionAttribute.Value;
            set => _resolutionAttribute.Value = value;
        }

        public string Uri
        {
            get => _uri.Value;
            set => _uri.Value = value;
        }

        public decimal? Score
        {
            get => _score.Value;
            set => _score.Value = value;
        }

        public string SupplementalCodecs
        {
            get => _supplementalCodecs.Value;
            set => _supplementalCodecs.Value = value;
        }

        public string AllowedCpc
        {
            get => _allowedCpc.Value;
            set => _allowedCpc.Value = value;
        }

        public string StableVariantId
        {
            get => _stableVariantId.Value;
            set => _stableVariantId.Value = value;
        }

        public string PathwayId
        {
            get => _pathwayId.Value;
            set => _pathwayId.Value = value;
        }

        public string ReqVideoLayout
        {
            get => _reqVideoLayout.Value;
            set => _reqVideoLayout.Value = value;
        }

        protected override string TagName => Tag.EXTXIFRAMESTREAMINF;
    }
}
