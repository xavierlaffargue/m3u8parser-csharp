using System.IO;
using System.Text;

namespace M3U8Parser;

public class StreamInf : IExtXType
{
	private readonly Attribute<string> _audio = new ("AUDIO");
	private readonly Attribute<long> _bandwidth = new ("BANDWIDTH");
	private readonly Attribute<string> _codecs = new ("CODECS");
	private readonly Attribute<string> _video = new ("VIDEO");
	private readonly Attribute<string> _closedCaptions = new ("CLOSED-CAPTIONS");
	private readonly Resolution _resolution = new ();

	public StreamInf()
	{
	}

	public StreamInf(string str)
	{
		var lineWithAttribute = string.Empty;
		var lineWithUri = string.Empty;

		using var reader = new StringReader(str);
		lineWithAttribute = reader.ReadLine();
		lineWithUri = reader.ReadLine();

		_bandwidth.Read(lineWithAttribute);
		_codecs.Read(lineWithAttribute);
		_video.Read(lineWithAttribute);
		_audio.Read(lineWithAttribute);
		_closedCaptions.Read(lineWithAttribute);
		_resolution.Read(lineWithAttribute);
		Uri = lineWithUri;
	}

	public string Uri { get; set; }

	public long Bandwidth {
		get => _bandwidth.Value;
		set => _bandwidth.Value = value;
	}

	public string Codecs {
		get => _codecs.Value;
		set => _codecs.Value = value;
	}

	public string Video {
		get => _video.Value;
		set => _video.Value = value;
	}

	public string Audio {
		get => _audio.Value;
		set => _audio.Value = value;
	}

	public string ClosedCaptions {
		get => _closedCaptions.Value;
		set => _closedCaptions.Value = value;
	}

	public string Resolution {
		get => _resolution.Value;
		set => _resolution.Value = value;
	}

	public static string Prefix => "#EXT-X-STREAM-INF";

	public new string ToString()
	{
		var strBuilder = new StringBuilder();
		strBuilder.Append(Prefix);
		strBuilder.Append(":");
		strBuilder.AppendWithSeparator(_bandwidth.ToString(), ",");
		strBuilder.AppendWithSeparator(_resolution.ToString(), ",");
		strBuilder.AppendWithSeparator(_codecs.ToString(), ",");
		strBuilder.AppendWithSeparator(_video.ToString(), ",");
		strBuilder.AppendWithSeparator(_audio.ToString(), ",");
		strBuilder.AppendWithSeparator(_closedCaptions.ToString(), ",");

		return strBuilder.ToString().RemoveLastCharacter() + "\r\n" + Uri;
	}
}