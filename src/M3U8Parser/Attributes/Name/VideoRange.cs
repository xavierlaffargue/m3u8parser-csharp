namespace M3U8Parser.Attributes.Name
{
    using M3U8Parser.Attributes.ValueType;

    public class VideoRange : CustomAttribute<VideoRangeType>
    {
        public VideoRange()
            : base("VIDEO-RANGE")
        {
        }
    }
}