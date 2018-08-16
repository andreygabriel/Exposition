namespace GraphTask
{
    enum Direction
    {
        FromFirstToSecond,
        FromSecondToFirst
    }

    class Waybill
    {
        private int DeparturePoint { get; }
        private int ArrivalPoint { get; }

        public Waybill(int firstStation, int secondStation)
        {
            DeparturePoint = firstStation;
            ArrivalPoint = secondStation;
        }

        public Direction TravelDirection()
        {
            if (DeparturePoint < ArrivalPoint)
            {
                return Direction.FromFirstToSecond;
            }
            else
            {
                return Direction.FromSecondToFirst;
            }
        }

        public UnorderedPair ToUnorderedPair()
        {
            return new UnorderedPair(DeparturePoint, ArrivalPoint);
        }
    }
}
