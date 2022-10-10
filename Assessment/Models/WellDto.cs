using System.ComponentModel.DataAnnotations.Schema;

namespace Assessment.Models
{
    public class WellDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int PlatformId { get; set; }

        public string? UniqueName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual PlatformDto Platform { get; set; }
    }
}
