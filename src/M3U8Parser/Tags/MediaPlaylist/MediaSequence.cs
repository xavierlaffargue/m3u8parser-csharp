namespace M3U8Parser.Tags.MediaPlaylist
{
    public class MediaSequence : AbstractTagOneValue<int>
    {
        public MediaSequence()
        {
        }

        public MediaSequence(string str)
        {
            ReadValue(str);
        }

        protected override string TagName => Tag.EXTXMEDIASEQUENCE;
    }
}