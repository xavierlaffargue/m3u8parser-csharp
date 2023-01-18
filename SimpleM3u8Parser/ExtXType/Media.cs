using System.Text;

namespace SimpleM3u8Parser;

public class Media : IExtXType
{
    public static string Prefix { get => "#EXT-X-MEDIA"; }

    private Attribute<string> _language = new ("LANGUAGE");
    private Attribute<string> _name = new ("NAME");
    private Attribute<string> _uri = new ("URI");
    private Attribute<bool> _autoSelect = new ("AUTOSELECT");
    private Attribute<bool> _default = new("DEFAULT");
    private EnumTag<MediaType> _mediaType = new("TYPE");
    private Attribute<string> _groupId = new ("GROUP-ID");
    private Attribute<string> _instreamId = new ("INSTREAM-ID");
    private Attribute<string> _characteristics = new ("CHARACTERISTICS");
    
    public string Uri { get => _uri.Value; set => _uri.Value = value; }  
    public string Language { get => _language.Value; set => _language.Value = value; }  
    public string Name { get => _name.Value; set => _name.Value = value; }  
    public bool AutoSelect { get => _autoSelect.Value; set => _autoSelect.Value = value; }  
    public bool Default { get => _default.Value; set => _default.Value = value; }  
    public string GroupId { get => _groupId.Value; set => _groupId.Value = value; }  
    public string InstreamId { get => _instreamId.Value; set => _instreamId.Value = value; }
    public MediaType Type { get => _mediaType.Value; set => _mediaType.Value = value; }
    public string Characteristics { get => _characteristics.Value; set => _characteristics.Value = value; }

    public Media()
    {}
		
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

    public new string ToString()
    {
        var strBuilder = new StringBuilder();
        strBuilder.Append(Prefix);
        strBuilder.Append(":");
        strBuilder.AppendSeparatorIfTextNotNull(_mediaType.Parse(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_groupId.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_language.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_name.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_autoSelect.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_default.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_uri.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_instreamId.ToString(), ",");
        strBuilder.AppendSeparatorIfTextNotNull(_characteristics.ToString(), ",");

        return strBuilder.ToString().RemoveLastCharacter();
    }
}