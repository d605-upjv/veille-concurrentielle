namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetAllCompetitorsModels
    {
        public class GetAllCompetitorResponse
        {
            public GetAllCompetitorResponse(List<Competitor> competitors)
            {
                Competitors = competitors;
            }
            public List<Competitor> Competitors { get; set; }

            public class Competitor
            {
                public Competitor(string id, string name, string logoUrl)
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
    }
}
