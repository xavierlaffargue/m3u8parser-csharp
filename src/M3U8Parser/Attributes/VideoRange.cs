namespace M3U8Parser.Attributes
{
    using M3U8Parser.Attributes.BaseAttribute;

    public class VideoRange : CustomAttribute<VideoRangeType>
    {
        public VideoRange()
            : base("VIDEO-RANGE")
        {
        }
    }
}