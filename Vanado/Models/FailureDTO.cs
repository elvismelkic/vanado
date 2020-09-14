using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class FailureDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFixed { get; set; } = false;
        public string Priority { get; set; } = "moderate";
    }

    public class FailureFullDTO : FailureDTO
    {
        public int MachineId { get; set; }
        public string MachineName { get; set; }
        public List<FileDTO> Files { get; set; }
    }

    public class FailureInputDTO : FailureDTO
    {
        [Required]
        public int MachineId { get; set; }
    }
}
