using System.IO;
using System.Text;

namespace SimpleM3u8Parser;

public class StreamInf : IExtXType
{
    private readonly Attribute<string> _audio = new("AUDIO");
    private readonly Attribute<long> _bandwidth = new("BANDWIDTH");
    private readonly Attribute<string> _codecs = new("CODECS");

    private readonly Attribute<int> _programId = new("PROGRAM-ID");
    private readonly Attribute<string> _video = new("VIDEO");

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

        _programId.Read(lineWithAttribute);
        _bandwidth.Read(lineWithAttribute);
        _codecs.Read(lineWithAttribute);
        _video.Read(lineWithAttribute);
        _audio.Read(lineWithAttribute);
        Uri = lineWithUri;
    }

    public string Uri { get; set; }

    public int ProgramId
    {
        get => _programId.Value;
        set => _programId.Value = value;
    }

    public long Bandwidth
    {
        get => _bandwidth.Value;
        set => _bandwidth.Value = value;
    }

    public string Codecs
    {
        get => _codecs.Value;
        set => _codecs.Value = value;
    }

    public string Video
    {
        get => _video.Value;
        set => _video.Value = value;
    }

    public string Audio
    {
        get => _audio.Value;
        set => _audio.Value = value;
    }

    public static string Prefix => "#EXT-X-STREAM-INF";

    public new string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append(Prefix);
        strBuilder.Append(":");
        strBuilder.AppendSeparatorIfTextNotNull(_programId.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_bandwidth.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_codecs.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_video.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_audio.ToString(), ",");

        return strBuilder.ToString().RemoveLastCharacter() + "\r\n" + Uri;
    }
}