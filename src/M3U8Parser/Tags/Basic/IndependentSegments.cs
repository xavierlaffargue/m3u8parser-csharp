namespace M3U8Parser.Tags.Basic
{
    public class IndependentSegments : AbstractTagWithoutValue<int>
    {
        public IndependentSegments()
        {
        }

        public IndependentSegments(string str)
            : base(str)
        {
        }

        protected override string TagName => Tag.EXTXINDEPENDENTSEGMENTS;
    }
}