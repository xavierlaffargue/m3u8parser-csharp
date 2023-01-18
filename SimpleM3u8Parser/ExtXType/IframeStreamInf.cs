using System.Text;

namespace SimpleM3u8Parser;

public class IframeStreamInf : IExtXType
{
    public static string Prefix { get => "#EXT-X-I-FRAME-STREAM-INF"; }
	    
    private Attribute<string> _uri = new ("URI");
    private Attribute<long> _bandwidth = new ("BANDWIDTH");
    private Resolution _resolution = new ();
    private Attribute<string> _codecs = new ("CODECS");
	    
    public string Uri { get => _uri.Value; set => _uri.Value = value; }  
    public long Bandwidth { get => _bandwidth.Value; set => _bandwidth.Value = value; }  
    public string Resolution { get => _resolution.Value; set => _resolution.Value = value; }  
    public string Codecs { get => _codecs.Value; set => _codecs.Value = value; } 
    
    public IframeStreamInf() {}

    public IframeStreamInf(string str)
    {
        _bandwidth.Read(str);
        _resolution.Read(str);
        _codecs.Read(str);
        _uri.Read(str); 
    }

    public new string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append(Prefix);
        strBuilder.Append(":");
        strBuilder.AppendSeparatorIfTextNotNull(_bandwidth.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_resolution.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_codecs.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_uri.ToString(), ",");
            
        return strBuilder.ToString().RemoveLastCharacter();
    }
}