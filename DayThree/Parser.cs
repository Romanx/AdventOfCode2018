using System;
using System.Collections.Generic;

namespace DayThree
{
    public static class Parser
    {
        public static IEnumerable<ClaimedArea> ParseClaims(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                var lineSpan = line.AsSpan();

                lineSpan = ParseId(lineSpan, out var id);
                lineSpan = ParseOffsets(lineSpan, out var offsets);
                ParseRectangle(lineSpan, out var rectangle);

                yield return new ClaimedArea(id, offsets.Left, offsets.Top, rectangle.Width, rectangle.Height);
            }
        }

        private static ReadOnlySpan<char> ParseId(in ReadOnlySpan<char> line, out int id)
        {
            if (line[0] != '#')
                throw new Exception("This is not an Id");

            int nextWhitespace = line.IndexOf(' ');
            var idSpan = line.Slice(1, nextWhitespace - 1);

            id = int.Parse(idSpan);
            return line.Slice(nextWhitespace + 1);
        }

        private static ReadOnlySpan<char> ParseOffsets(in ReadOnlySpan<char> line, out (uint Left, uint Top) offsets)
        {
            if (line[0] != '@')
                throw new Exception("This is not an Offset");

            var commaIndex = line.IndexOf(',');
            var colonIndex = line.IndexOf(':');

            offsets = (
                uint.Parse(line.Slice(1, commaIndex - 1)),
                uint.Parse(line.Slice(commaIndex + 1, colonIndex - commaIndex - 1))
            );

            return line.Slice(colonIndex);
        }

        private static void ParseRectangle(in ReadOnlySpan<char> line, out (uint Width, uint Height) offsets)
        {
            if (line[0] != ':')
                throw new Exception("This is not an Rectangle");

            var multiplyIndex = line.IndexOf('x');

            offsets = (
                uint.Parse(line.Slice(1, multiplyIndex - 1).Trim()),
                uint.Parse(line.Slice(multiplyIndex + 1).Trim())
            );
        }
    }
}
