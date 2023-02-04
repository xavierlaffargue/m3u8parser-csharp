namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;
    using M3U8Parser.CustomType;
    using M3U8Parser.Extensions;
    using System.Text;

    public class IframeStreamInf : IExtXType
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

		public IframeStreamInf(string str)
		{
			_bandwidth.Read(str);
			_averageBandwidth.Read(str);
			_codecs.Read(str);
			_frameRate.Read(str);
			_videoRange.Read(str);
			_hdcpLevel.Read(str);
			_audio.Read(str);
			_video.Read(str);
			_subtitles.Read(str);
			_closedCaptions.Read(str);
			_resolutionAttribute.Read(str);
			_uri.Read(str);
		}

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

		public static string Prefix => "#EXT-X-I-FRAME-STREAM-INF";

		public override string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(Prefix);
			strBuilder.Append(":");
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
			strBuilder.AppendWithSeparator(_uri.ToString(), ",");

			return strBuilder.ToString().RemoveLastCharacter();
		}
	}
}