using System.IO;

namespace GraphTask
{
    class Inspector
    {
        private readonly Processor processor = new Processor();
        private Data data = new Data();
        private readonly Parser parser = new Parser();

        public Inspector(StreamReader stream)
        {
            parser.Init(stream);
        }

        public bool CheckCollision()
        {
            return processor.ProcessTrains(data);
        }

        public void PrepareRailway()
        {
            data = parser.PrepareData();
        }
    }
}
