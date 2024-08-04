namespace M3U8Parser
{
    public static class Tag
    {
        public static readonly string EXTX = "#EXT-X";
        public static readonly string EXTINF = "#EXTINF";
        public static readonly string EXTM3U = "#EXTM3U";
        public static readonly string EXTXVERSION = $"{EXTX}-VERSION";
        public static readonly string EXTXBYTERANGE = $"{EXTX}-X-BYTERANGE";
        public static readonly string EXTXMEDIA = $"{EXTX}-MEDIA";
        public static readonly string EXTXSTREAMINF = $"{EXTX}-STREAM-INF";
        public static readonly string EXTXIFRAMESTREAMINF = $"{EXTX}-I-FRAME-STREAM-INF";
        public static readonly string EXTXINDEPENDENTSEGMENTS = $"{EXTX}-INDEPENDENT-SEGMENTS";
        public static readonly string EXTXTARGETDURATION = $"{EXTX}-TARGETDURATION";
        public static readonly string EXTXMEDIASEQUENCE = $"{EXTX}-MEDIA-SEQUENCE";
        public static readonly string EXTXDISCONTINUITYSEQUENCE = "EXT-X-DISCONTINUITY-SEQUENCE";
        public static readonly string EXTXPLAYLISTTYPE = $"{EXTX}-PLAYLIST-TYPE";
        public static readonly string EXTXIFRAMESONLY = $"{EXTX}-I-FRAMES-ONLY";
        public static readonly string EXTXMAP = $"{EXTX}-MAP";
        public static readonly string EXTXKEY = $"{EXTX}-KEY";
        public static readonly string EXTXPROGRAMDATETIME = $"{EXTX}-PROGRAM-DATE-TIME";
        public static readonly string EXTXDISCONTINUITY = $"{EXTX}-DISCONTINUITY";
        public static readonly string EXTXCUEOUT = $"{EXTX}-CUE-OUT";
        public static readonly string EXTXCUEOUTCONT = $"{EXTX}-CUE-OUT-CONT";
        public static readonly string EXTXCUEIN = $"{EXTX}-CUE-IN";
        public static readonly string EXTXSTART = $"{EXTX}-START";
        public static readonly string EXTXENDLIST = $"{EXTX}-ENDLIST";
    }
}