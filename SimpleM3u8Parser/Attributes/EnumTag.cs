using System;
using System.Text.RegularExpressions;

namespace SimpleM3u8Parser;

public class EnumTag<T> where T : struct, IConvertible
{
    private string Separator = "=";
    private string AttributName;
    private readonly T defaultValue;
    public T Value { get; set; }

    public EnumTag(string attributName)
    {
        AttributName = attributName;
    }

    public virtual string Parse()
    {
        if (!Value.Equals(defaultValue))
        {
            return AttributName + Separator + Value;
        }

        return string.Empty;
    }

    public void Read(string content)
    {
        var regex = new Regex($"(?={AttributName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
        if (regex.IsMatch(content))
        {
            Value = Tools.ParseEnum<T>(regex.Match(content).Groups[0].Value.Split(Separator)[1]);
            return;
        }
    }
}