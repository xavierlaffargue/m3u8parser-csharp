namespace M3U8Parser.Tags.MediaPlaylist
{
    using M3U8Parser.Attributes.ValueType;

    public class PlaylistTypeExt : AbstractTagOneValue<PlaylistType>
    {
        public PlaylistTypeExt()
        {
        }

        public PlaylistTypeExt(string str)
        {
            ReadValue(str);
        }

        protected override string TagName => Tag.EXTXPLAYLISTTYPE;
    }
}