namespace M3U8Parser.Tags.MultivariantPlaylist
{
    using System.IO;
    using System.Text;
    using M3U8Parser.Attributes.Name;
    using M3U8Parser.Attributes.ValueType;
    using M3U8Parser.Extensions;
    using M3U8Parser.Interfaces;

    public class StreamInf : AbstractTag
    {
        private readonly Audio _audio = new ();
        private readonly AverageBandwidth _averageBandwidth = new ();
        private readonly Bandwidth _bandwidth = new ();
        private readonly ClosedCaptions _closedCaptions = new ();
        private readonly Codecs _codecs = new ();
        private readonly FrameRate _frameRate = new ();
        private readonly HdcpLevel _hdcpLevel = new ();
        private readonly Resolution _resolutionAttribute = new ();
        private readonly Subtitles _subtitles = new ();
        private readonly Video _video = new ();
        private readonly VideoRange _videoRange = new ();

        // New attributes introduced in RFC 8216 Bis / draft 22
        private readonly DecimalAttribute _score = new ("SCORE");
        private readonly StringAttribute _supplementalCodecs = new ("SUPPLEMENTAL-CODECS");
        private readonly StringAttribute _allowedCpc = new ("ALLOWED-CPC");
        private readonly StringAttribute _stableVariantId = new ("STABLE-VARIANT-ID");
        private readonly StringAttribute _pathwayId = new ("PATHWAY-ID");
        private readonly StringAttribute _reqVideoLayout = new ("REQ-VIDEO-LAYOUT");

        public StreamInf()
        {
        }

        public StreamInf(string str)
        {
            using var reader = new StringReader(str);
            var lineWithAttribute = reader.ReadLine();
            var lineWithUri = reader.ReadLine();

            _bandwidth.Read(lineWithAttribute);
            _averageBandwidth.Read(lineWithAttribute);
            _codecs.Read(lineWithAttribute);
            _frameRate.Read(lineWithAttribute);
            _videoRange.Read(lineWithAttribute);
            _hdcpLevel.Read(lineWithAttribute);
            _audio.Read(lineWithAttribute);
            _video.Read(lineWithAttribute);
            _subtitles.Read(lineWithAttribute);
            _closedCaptions.Read(lineWithAttribute);
            _resolutionAttribute.Read(lineWithAttribute);

            _score.Read(lineWithAttribute);
            _supplementalCodecs.Read(lineWithAttribute);
            _allowedCpc.Read(lineWithAttribute);
            _stableVariantId.Read(lineWithAttribute);
            _pathwayId.Read(lineWithAttribute);
            _reqVideoLayout.Read(lineWithAttribute);

            Uri = lineWithUri;
        }

        public string Uri
        {
            get;
            set;
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

        public decimal? FrameRate
        {
            get => _frameRate.Value;
            set => _frameRate.Value = value;
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

        public string Audio
        {
            get => _audio.Value;
            set => _audio.Value = value;
        }

        public string Video
        {
            get => _video.Value;
            set => _video.Value = value;
        }

        public string Subtitles
        {
            get => _subtitles.Value;
            set => _subtitles.Value = value;
        }

        public string ClosedCaptions
        {
            get => _closedCaptions.Value;
            set => _closedCaptions.Value = value;
        }

        public ResolutionType Resolution
        {
            get => _resolutionAttribute.Value;
            set => _resolutionAttribute.Value = value;
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

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(Tag.EXTXSTREAMINF);
            strBuilder.Append(':');
            strBuilder.AppendWithSeparator(_bandwidth.ToString(), ",");
            strBuilder.AppendWithSeparator(_averageBandwidth.ToString(), ",");
            strBuilder.AppendWithSeparator(_videoRange.ToString(), ",");
            strBuilder.AppendWithSeparator(_codecs.ToString(), ",");
            strBuilder.AppendWithSeparator(_resolutionAttribute.ToString(), ",");
            strBuilder.AppendWithSeparator(_frameRate.ToString(), ",");
            strBuilder.AppendWithSeparator(_closedCaptions.ToString(), ",");
            strBuilder.AppendWithSeparator(_hdcpLevel.ToString(), ",");
            strBuilder.AppendWithSeparator(_video.ToString(), ",");
            strBuilder.AppendWithSeparator(_audio.ToString(), ",");
            strBuilder.AppendWithSeparator(_subtitles.ToString(), ",");

            strBuilder.AppendWithSeparator(_score.ToString(), ",");
            strBuilder.AppendWithSeparator(_supplementalCodecs.ToString(), ",");
            strBuilder.AppendWithSeparator(_allowedCpc.ToString(), ",");
            strBuilder.AppendWithSeparator(_stableVariantId.ToString(), ",");
            strBuilder.AppendWithSeparator(_pathwayId.ToString(), ",");
            strBuilder.AppendWithSeparator(_reqVideoLayout.ToString(), ",");

            return strBuilder.ToString().RemoveLastCharacter() + "\r\n" + Uri;
        }
    }
}
