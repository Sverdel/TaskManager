using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Api.Models.DataModel
{
    public class WorkTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        [Required]
        public DateTime CreateDateTime { get; set; }

        [Required]
        public DateTime ChangeDatetime { get; set; }

        public int Version { get; set; }

        public double PlanedTimeCost { get; set; }

        public double ActualTimeCost { get; set; }

        public double RemainingTimeCost { get; set; }

        [Index]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        public int StateId { get; set; }

        [ForeignKey("StateId")]
        [JsonIgnore]
        public State State { get; set; }

        public int PriorityId { get; set; }

        [ForeignKey("PriorityId")]
        [JsonIgnore]
        public Priority Priority { get; set; }
    }
}