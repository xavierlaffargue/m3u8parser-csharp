namespace M3U8Parser.CustomType
{
    public interface ICustomAttribute
    {
        public object ParseFromString(string value);
    }
}