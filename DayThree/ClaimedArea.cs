using System.Collections.Generic;
using System.Linq;

namespace DayThree
{
    public struct ClaimedArea
    {
        public ClaimedArea(int id, uint left, uint top, uint width, uint height)
        {
            Id = id;
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public int Id { get; }

        public uint Left { get; }

        public uint Top { get; }

        public uint Width { get; }

        public uint Height { get; }

        public IEnumerable<(uint X, uint Y)> Points() {
            for (var x = Left; x < Left + Width; x++)
            {
                for (var y = Top; y < Top + Height; y++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}
