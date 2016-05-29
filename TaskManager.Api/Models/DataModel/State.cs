using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskManager.Api.Models.DataModel
{
    public class State
    {
        [Key]
        public byte Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}