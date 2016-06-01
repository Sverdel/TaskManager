using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Api.Models.Dto
{
    public class TaskDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreateDateTime { get; set; }

        //[Required]
        //public DateTime ChangeDatetime { get; set; }

        public int Version { get; set; }

        public double PlanedTimeCost { get; set; }

        public double ActualTimeCost { get; set; }

        public double RemainingTimeCost { get; set; }

        public int UserId { get; set; }

        public byte StateId { get; set; }

        public byte PriorityId { get; set; }
    }
}