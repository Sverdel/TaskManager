using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManager.Api.Models.DataModel
{
    public class WorkTask
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }

        //[Required]
        //public DateTime ChangeDatetime { get; set; }

        public int Version { get; set; }

        public double PlanedTimeCost { get; set; }

        public double ActualTimeCost { get; set; }

        [Index]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public byte StateId { get; set; }

        [ForeignKey("StateId")]
        public State State { get; set; }

        public byte PriorityId { get; set; }

        [ForeignKey("PriorityId")]
        public Priority Priority { get; set; }
    }
}