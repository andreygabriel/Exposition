namespace GraphTask
{
    class UnorderedPair
    {
        public int FirstValue { get; }
        public int SecondValue { get; }

        public UnorderedPair( int firstValue, int secondValue )
        {
            FirstValue = firstValue;
            SecondValue = secondValue;
        }

        /// Making pair unordered
        public override bool Equals(object obj)
        {
            if ((obj == null) || (!(obj is UnorderedPair)))
            {
                return false;
            }

            var Other = (UnorderedPair)obj;

            if ((Other.FirstValue == FirstValue && Other.SecondValue == SecondValue) ||
                (Other.FirstValue == SecondValue && Other.SecondValue == FirstValue))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (FirstValue * SecondValue * (FirstValue + SecondValue)).GetHashCode();
        }
    }

    class Segment
    {
        public int LeftBorder { get; }

        public int RightBorder { get; }


        public Segment(int leftBorder, int rightBorder)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
        }

        public bool HasSameBorder(Segment other)
        {
            if (LeftBorder == other.LeftBorder || RightBorder == other.RightBorder)
            {
                return true;
            }
            return false;
        }
        public bool Intersects(Segment other)
        {

            if ((LeftBorder >= other.LeftBorder) && (LeftBorder <= other.RightBorder))
            {
                return true;
            }

            if ((other.LeftBorder >= LeftBorder) && (other.LeftBorder <= RightBorder))
            {
                return true;
            }

            return false;
        }
    }
}
