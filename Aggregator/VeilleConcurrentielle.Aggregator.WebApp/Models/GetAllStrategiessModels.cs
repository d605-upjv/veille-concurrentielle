namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetAllStrategiessModels
    {
        public class GetAllStrategiesResponse
        {
            public GetAllStrategiesResponse(List<Strategy> strategies)
            {
                Strategies = strategies;
            }
            public List<Strategy> Strategies { get; set; }

            public class Strategy
            {
                public Strategy(string id, string name)
                {
                    Id = id;
                    Name = name;
                }

                public string Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
