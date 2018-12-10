using System;

namespace DayEight
{
    public static class SpanExtensions
    {
        public static ReadOnlySpan<char> ParseInt(this in ReadOnlySpan<char> span, out int @int)
        {
            var index = 0;

            while (char.IsDigit(span[index]) && index + 1 < span.Length)
            {
                index++;
            }

            if (index == 0)
            {
                @int = int.Parse(span);
            }
            else
            {
                @int = int.Parse(span.Slice(0, index));
            }
            return span.Slice(index + 1);
        }
    }
}
