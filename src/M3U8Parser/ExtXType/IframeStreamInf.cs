using M3U8Parser.Attributes.BaseAttribute;

namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;

    public class IframeStreamInf : BaseExtX
    {
		private readonly Bandwidth _bandwidth = new ();
		private readonly AverageBandwidth _averageBandwidth = new ();
		private readonly Codecs _codecs = new ();
		private readonly FrameRate _frameRate = new ();
		private readonly VideoRange _videoRange = new ();
		private readonly HdcpLevel _hdcpLevel = new ();
		private readonly Audio _audio = new ();
		private readonly Video _video = new ();
		private readonly Subtitles _subtitles = new ();
		private readonly ClosedCaptions _closedCaptions = new ();
		private readonly Resolution _resolutionAttribute = new ();
		private readonly Uri _uri = new ();

		public IframeStreamInf()
		{
		}

		public IframeStreamInf(string str) : base(str) { }

		public long Bandwidth {
			get => _bandwidth.Value;
			set => _bandwidth.Value = value;
		}
        
		public long? AverageBandwidth {
			get => _averageBandwidth.Value;
			set => _averageBandwidth.Value = value;
		}

		public string Codecs {
			get => _codecs.Value;
			set => _codecs.Value = value;
		}
        
		public decimal? FrameRate {
			get => _frameRate.Value;
			set => _frameRate.Value = value;
		}
        
		public VideoRangeType VideoRange {
			get => _videoRange.Value;
			set => _videoRange.Value = value;
		}
        
		public HdcpLevelType HdcpLevel {
			get => _hdcpLevel.Value;
			set => _hdcpLevel.Value = value;
		}
		public string Audio {
			get => _audio.Value;
			set => _audio.Value = value;
		}

		public string Video {
			get => _video.Value;
			set => _video.Value = value;
		}

        
		public string Subtitles {
			get => _subtitles.Value;
			set => _subtitles.Value = value;
		}
        
		public string ClosedCaptions {
			get => _closedCaptions.Value;
			set => _closedCaptions.Value = value;
		}

		public ResolutionType Resolution {
			get => _resolutionAttribute.Value;
			set => _resolutionAttribute.Value = value;
		}


        public static string Prefix = "#EXT-X-I-FRAME-STREAM-INF";

        protected override string ExtPrefix => Prefix;
	}
}