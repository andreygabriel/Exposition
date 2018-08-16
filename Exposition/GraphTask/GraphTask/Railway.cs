using System;

namespace GraphTask
{
    class Railway
    {
        public UnorderedPair Stations { get; }
        public int Length { get; }

        public Guid Uid { get; } = Guid.NewGuid();

        public Railway(int firstStation, int secondStation, int length)
        {
            Length = length;
            Stations = new UnorderedPair(firstStation, secondStation);
        }
    };
}
