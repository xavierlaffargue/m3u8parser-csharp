using System.Text;

namespace SimpleM3u8Parser;

public class Media : IExtXType
{
    private readonly Attribute<bool> _autoSelect = new("AUTOSELECT");
    private readonly Attribute<string> _characteristics = new("CHARACTERISTICS");
    private readonly Attribute<bool> _default = new("DEFAULT");
    private readonly Attribute<string> _groupId = new("GROUP-ID");
    private readonly Attribute<string> _instreamId = new("INSTREAM-ID");
    private readonly Attribute<string> _language = new("LANGUAGE");
    private readonly Attribute<MediaType> _mediaType = new("TYPE");
    private readonly Attribute<string> _name = new("NAME");
    private readonly Attribute<string> _uri = new("URI");

    public Media()
    {
    }

    public Media(string str)
    {
        _language.Read(str);
        _name.Read(str);
        _mediaType.Read(str);
        _autoSelect.Read(str);
        _uri.Read(str);
        _default.Read(str);
        _groupId.Read(str);
        _instreamId.Read(str);
        _characteristics.Read(str);
    }

    public string Uri
    {
        get => _uri.Value;
        set => _uri.Value = value;
    }

    public string Language
    {
        get => _language.Value;
        set => _language.Value = value;
    }

    public string Name
    {
        get => _name.Value;
        set => _name.Value = value;
    }

    public bool AutoSelect
    {
        get => _autoSelect.Value;
        set => _autoSelect.Value = value;
    }

    public bool Default
    {
        get => _default.Value;
        set => _default.Value = value;
    }

    public string GroupId
    {
        get => _groupId.Value;
        set => _groupId.Value = value;
    }

    public string InstreamId
    {
        get => _instreamId.Value;
        set => _instreamId.Value = value;
    }

    public MediaType Type
    {
        get => _mediaType.Value;
        set => _mediaType.Value = value;
    }

    public string Characteristics
    {
        get => _characteristics.Value;
        set => _characteristics.Value = value;
    }

    public static string Prefix => "#EXT-X-MEDIA";

    public new string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append(Prefix);
        strBuilder.Append(":");
        strBuilder.AppendWithSeparator(_mediaType.ToString(), ",");
        strBuilder.AppendWithSeparator(_groupId.ToString(), ",");
        strBuilder.AppendWithSeparator(_language.ToString(), ",");
        strBuilder.AppendWithSeparator(_name.ToString(), ",");
        strBuilder.AppendWithSeparator(_autoSelect.ToString(), ",");
        strBuilder.AppendWithSeparator(_default.ToString(), ",");
        strBuilder.AppendWithSeparator(_uri.ToString(), ",");
        strBuilder.AppendWithSeparator(_instreamId.ToString(), ",");
        strBuilder.AppendWithSeparator(_characteristics.ToString(), ",");

        return strBuilder.ToString().RemoveLastCharacter();
    }
}