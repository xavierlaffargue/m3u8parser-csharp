namespace SimpleM3u8Parser;

public class MediaType
{
    private MediaType(string value)
    {
        _value = value;
    }

    private string _value { get; }

    public static MediaType Audio => new MediaType("AUDIO");
    public static MediaType Video => new MediaType("VIDEO");
    public static MediaType Subtitles => new MediaType("SUBTITLES");
    public static MediaType CloseCaptions => new MediaType("CLOSED-CAPTIONS");

    public override string ToString()
    {
        return _value;
    }
}