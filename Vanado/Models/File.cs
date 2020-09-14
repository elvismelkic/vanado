using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class File
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int FailureId { get; set; }
    }
}
