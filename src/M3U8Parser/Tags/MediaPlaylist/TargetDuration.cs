namespace M3U8Parser.Tags.MediaPlaylist
{
    public class TargetDuration : AbstractTagOneValue<int>
    {
        public TargetDuration()
        {
        }

        public TargetDuration(string str)
        {
            ReadValue(str);
        }

        protected override string TagName => Tag.EXTXTARGETDURATION;
    }
}