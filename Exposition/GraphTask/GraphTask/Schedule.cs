using System.Collections.Generic;

namespace GraphTask
{
    class Schedule
    {
        /// Don't use in Dictionary, no GetHashCode override with that Equal
        private class BusyMarker
        {
            private Segment Segment;

            private readonly Direction Direction;

            public BusyMarker(int firstValue, int secondValue, Direction direction)
            {
                Segment = new Segment(firstValue, secondValue);
                Direction = direction;
            }

            public override bool Equals(object obj)
            {
                if ((obj == null) || (!(obj is BusyMarker)))
                {
                    return false;
                }
                var other = (BusyMarker)obj;

                /// Collision on station
                if (Direction == other.Direction)
                {
                    return Segment.HasSameBorder(other.Segment);
                }
                else
                {
                    /// Collision on railway
                    return Segment.Intersects(other.Segment);
                }
            }

            /// Only to avoid warnings
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        private readonly List<BusyMarker> busyTime = new List<BusyMarker>();

        public bool TryMarkBusy(int timeStart, int length, Direction direction)
        {
            BusyMarker Key = new BusyMarker(timeStart + 1, timeStart + length, direction);
            if (busyTime.Contains(Key))
            {
                return false;
            }

            busyTime.Add(Key);
            return true;
        }
    }
}
