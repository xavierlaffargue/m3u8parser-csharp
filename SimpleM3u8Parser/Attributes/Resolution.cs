namespace SimpleM3u8Parser;

public class Resolution : Attribute<string>
{
    private readonly string separator = "=";

    public Resolution() : base("RESOLUTION")
    {
    }

    public override string ToString()
    {
        if (Value != null) return $"{AttributeName}{separator}{Value}";

        return string.Empty;
    }
}