using System.Text;

namespace SimpleM3u8Parser;

public class IframeStreamInf : IExtXType
{
    private readonly Attribute<long> _bandwidth = new("BANDWIDTH");
    private readonly Attribute<string> _codecs = new("CODECS");
    private readonly Resolution _resolution = new();
    private readonly Attribute<string> _uri = new("URI");

    public IframeStreamInf()
    {
    }

    public IframeStreamInf(string str)
    {
        _bandwidth.Read(str);
        _resolution.Read(str);
        _codecs.Read(str);
        _uri.Read(str);
    }

    public string Uri
    {
        get => _uri.Value;
        set => _uri.Value = value;
    }

    public long Bandwidth
    {
        get => _bandwidth.Value;
        set => _bandwidth.Value = value;
    }

    public string Resolution
    {
        get => _resolution.Value;
        set => _resolution.Value = value;
    }

    public string Codecs
    {
        get => _codecs.Value;
        set => _codecs.Value = value;
    }

    public static string Prefix => "#EXT-X-I-FRAME-STREAM-INF";

    public new string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append(Prefix);
        strBuilder.Append(":");
        strBuilder.AppendWithSeparator(_bandwidth.ToString(), ",");
        strBuilder.AppendWithSeparator(_resolution.ToString(), ",");
        strBuilder.AppendWithSeparator(_codecs.ToString(), ",");
        strBuilder.AppendWithSeparator(_uri.ToString(), ",");

        return strBuilder.ToString().RemoveLastCharacter();
    }
}