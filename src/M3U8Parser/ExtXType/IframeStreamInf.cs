namespace M3U8Parser.ExtXType
{
    using M3U8Parser.Attributes;
    using M3U8Parser.CustomType;
    using M3U8Parser.Extensions;
    using System.Text;

    public class IframeStreamInf : IExtXType
	{
		private readonly CustomAttribute<long> _bandwidth = new ("BANDWIDTH");
		private readonly StringAttribute _codecs = new ("CODECS");
		private readonly CustomAttribute<ResolutionType> _resolutionAttribute = new ("RESOLUTION");
		private readonly StringAttribute _uri = new ("URI");

		public IframeStreamInf()
		{
		}

		public IframeStreamInf(string str)
		{
			_bandwidth.Read(str);
			_resolutionAttribute.Read(str);
			_codecs.Read(str);
			_uri.Read(str);
		}

		public string Uri {
			get => _uri.Value;
			set => _uri.Value = value;
		}

		public long Bandwidth {
			get => _bandwidth.Value;
			set => _bandwidth.Value = value;
		}

		public ResolutionType Resolution {
			get => _resolutionAttribute.Value;
			set => _resolutionAttribute.Value = value;
		}

		public string Codecs {
			get => _codecs.Value;
			set => _codecs.Value = value;
		}

		public static string Prefix => "#EXT-X-I-FRAME-STREAM-INF";

		public override string ToString()
		{
			var strBuilder = new StringBuilder();
			strBuilder.Append(Prefix);
			strBuilder.Append(":");
			strBuilder.AppendWithSeparator(_bandwidth.ToString(), ",");
			strBuilder.AppendWithSeparator(_resolutionAttribute.ToString(), ",");
			strBuilder.AppendWithSeparator(_codecs.ToString(), ",");
			strBuilder.AppendWithSeparator(_uri.ToString(), ",");

			return strBuilder.ToString().RemoveLastCharacter();
		}
	}
}