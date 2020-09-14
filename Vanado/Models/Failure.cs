using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Failure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFixed { get; set; } = false;
        public string Priority { get; set; } = "moderate";
        public DateTime InsertedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MachineId { get; set; }
        public Machine Machine { get; set; }
    }
}
