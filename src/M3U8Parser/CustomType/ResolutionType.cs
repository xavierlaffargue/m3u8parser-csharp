// <copyright file="ResolutionType.cs" company="PlaceholderCompany">Copyright (c) PlaceholderCompany. All rights reserved.</copyright>

namespace M3U8Parser.CustomType
{
    using System;

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

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }
        
        public bool Equals(ResolutionType other)
        {
            if (other!.Height == Height && other!.Width == Width)
            {
                return true;
            }

            return false;
        }
    }
}