namespace M3U8Parser.Tags.Basic
{
    public class HlsVersion : AbstractTagOneValue<int>
    {
        public HlsVersion()
        {
        }

        public HlsVersion(string str)
        {
            ReadValue(str);
        }

        protected override string TagName => Tag.EXTXVERSION;
    }
}