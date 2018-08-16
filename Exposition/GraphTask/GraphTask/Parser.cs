using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace GraphTask
{
    struct Data
    {
        public List<Railway> Railways;
        public IEnumerable<IEnumerable<Waybill>> TrainEnumerator;
    }

    class Parser
    {
        private StreamReader Input;

        private IEnumerable<Waybill> Waybills()
        {
            var stationsSet = ParseNextIntArray();

            for (int i = 0; i < stationsSet.Length - 1; ++i)
            {
                yield return new Waybill(stationsSet[i], stationsSet[i + 1]);
            }
        }

        private IEnumerable<IEnumerable<Waybill>> Trains()
        {
            while (!Input.EndOfStream)
            {
                yield return Waybills();
            }
        }

        private List<Railway> ParseRailway()
        {
            var railways = new List<Railway>();

            for (;;)
            {
                var railwayParams = ParseNextIntArray();
                if (railwayParams == null)
                {
                    break;
                }

                railways.Add(new Railway(railwayParams[0], railwayParams[1], railwayParams[2]));
            }
            return railways;
        }

        public void Init(StreamReader stream)
        {
            Input = stream;
        }

        public Data PrepareData()
        {
            Data data = new Data();
            data.TrainEnumerator = Trains();
            data.Railways = ParseRailway();

            return data;
        }

        private int[] ParseNextIntArray()
        {
            var line = Input.ReadLine();
            if (line == null || line == ".")
            {
                return null;
            }

            return line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
        }
    }
}
