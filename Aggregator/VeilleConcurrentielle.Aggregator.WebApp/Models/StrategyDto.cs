namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class StrategyDto
    {
        public StrategyDto(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
