using System.ComponentModel.DataAnnotations.Schema;

namespace Assessment.Models
{
    public class PlatformDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string? UniqueName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<WellDto> Well { get; } = new List<WellDto>();
    }
}
