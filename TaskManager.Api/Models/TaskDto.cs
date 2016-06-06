using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Api.Models
{
    public class TaskDto
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}