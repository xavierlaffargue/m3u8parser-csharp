using M3U8Parser.Attributes.BaseAttribute;

namespace M3U8Parser.Attributes
{
    public class VideoRange : CustomAttribute<VideoRangeType>
    {
        public VideoRange() : base("VIDEO-RANGE")
        {
        }
    }
}