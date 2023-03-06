using M3U8Parser.Attributes.BaseAttribute;
using M3U8Parser.Interfaces;

namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;
    using M3U8Parser.Extensions;
    using System.IO;
    using System.Text;

    public class StreamInf : IExtXType
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

		public StreamInf()
		{
		}

		public StreamInf(string str)
		{
			string lineWithAttribute;
			string lineWithUri;

			using var reader = new StringReader(str);
			lineWithAttribute = reader.ReadLine();
			lineWithUri = reader.ReadLine();

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
			Uri = lineWithUri;
		}

		public string Uri { get; set; }

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

		public static string Prefix => "#EXT-X-STREAM-INF";

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

			return strBuilder.ToString().RemoveLastCharacter() + "\r\n" + Uri;
		}
	}
}