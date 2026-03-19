namespace H2_Trainning.Dtos
{
    public class CheckInDto
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public decimal? Weight { get; set; }
        public int SleepQuality { get; set; }
        public int EnergyLevel { get; set; }
        public int Mood { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCheckInDto
    {
        public decimal? Weight { get; set; }
        public int SleepQuality { get; set; }
        public int EnergyLevel { get; set; }
        public int Mood { get; set; }
        public string? Notes { get; set; }
    }
}
