namespace Assessment.Models
{
    public class PlatformDummyDto
    {
        public int Id { get; set; }

        public string? UniqueName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime LastUpdate { get; set; }

        public ICollection<WellDummyDto> Wells { get; } = new List<WellDummyDto>();
    }
}
