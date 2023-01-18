using System;
using System.Text.RegularExpressions;

namespace SimpleM3u8Parser;

public class EnumTag<T> where T : struct, IConvertible
{
    private readonly T defaultValue;
    private readonly string AttributName;
    private readonly string Separator = "=";

    public EnumTag(string attributName)
    {
        AttributName = attributName;
    }

    public T Value { get; set; }

    public virtual string Parse()
    {
        if (!Value.Equals(defaultValue)) return AttributName + Separator + Value;

        return string.Empty;
    }

    public void Read(string content)
    {
        var regex = new Regex($"(?={AttributName})(.*?)(?=,|$)", RegexOptions.Multiline & RegexOptions.IgnoreCase);
        if (regex.IsMatch(content))
        {
            Value = Tools.ParseEnum<T>(regex.Match(content).Groups[0].Value.Split(Separator)[1]);
        }
    }
}