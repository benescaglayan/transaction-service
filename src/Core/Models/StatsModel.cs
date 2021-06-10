namespace Core.Models
{
    public class StatsModel
    {
        public string Sum { get; set; } = "0.0";
        
        public string Avg { get; set; } = "0.0";
        
        public string Max { get; set; } = "0.0";

        public string Min { get; set; } = "0.0";
        
        public long Count { get; set; } = 0;
    }
}