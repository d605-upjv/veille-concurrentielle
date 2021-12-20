namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class CompetitorDto
    {
        public CompetitorDto(string id, string name, string logoUrl)
        {
            Id = id;
            Name = name;
            LogoUrl = logoUrl;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
    }
}
