using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class FileDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int FailureId { get; set; }
    }
}
