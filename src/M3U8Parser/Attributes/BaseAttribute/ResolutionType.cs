namespace M3U8Parser.Attributes.BaseAttribute
{
    using System;
    using M3U8Parser.Interfaces;

    public class ResolutionType : ICustomAttribute, IEquatable<ResolutionType>
    {
        public long Width { get; set; }

        public long Height { get; set; }

        public object ParseFromString(string value)
        {
            var widthAndHeight = value.Split('x');
            Width = long.Parse(widthAndHeight[0]);
            Height = long.Parse(widthAndHeight[1]);

            return this;
        }

        public bool Equals(ResolutionType other)
        {
            if (other!.Height == Height && other!.Width == Width) return true;

            return false;
        }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
    }
}