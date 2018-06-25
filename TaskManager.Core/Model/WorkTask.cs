using System;

namespace TaskManager.Core.Model
{
    public class WorkTask
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ChangeDatetime { get; set; }

        public int Version { get; set; }

        public double PlanedTimeCost { get; set; }

        public double ActualTimeCost { get; set; }

        public double RemainingTimeCost { get; set; }

        public string UserId { get; set; }

        public int StateId { get; set; }

        public int PriorityId { get; set; }
   }
}