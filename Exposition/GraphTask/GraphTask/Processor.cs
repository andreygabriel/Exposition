using System.Collections.Generic;
using System;
using System.Linq;

namespace GraphTask
{
    class Processor
    {
        private Dictionary<UnorderedPair, Railway> PrepareRailways(List<Railway> railways)
        {
            return railways.ToDictionary(r => r.Stations);
        }

        private Dictionary<Guid, Schedule> PrepareSchedules(List<Railway> railways)
        {
            var scheduleByRailway = new Dictionary<Guid, Schedule>();

            foreach (var railway in railways)
            {
                scheduleByRailway.Add(railway.Uid, new Schedule());
            }

            return scheduleByRailway;
        }

        /// False if collision was found
        public bool ProcessTrains(Data data)
        {
            var railways = PrepareRailways(data.Railways);
            var schedules = PrepareSchedules(data.Railways);

            foreach (var train in data.TrainEnumerator)
            {
                int actualTrainTime = 0;
                foreach (var waybill in train)
                {
                    var Key = waybill.ToUnorderedPair();
                    var railway = railways[Key];

                    var schedule = schedules[railway.Uid];

                    if (!schedule.TryMarkBusy(actualTrainTime, railway.Length, waybill.TravelDirection()))
                        return false;

                    const int speed = 1;
                    actualTrainTime += railway.Length / speed;
                }
            }
            return true;
        }
    }
}
