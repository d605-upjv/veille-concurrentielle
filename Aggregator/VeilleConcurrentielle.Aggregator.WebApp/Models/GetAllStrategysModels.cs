namespace VeilleConcurrentielle.Aggregator.WebApp.Models
{
    public class GetAllStrategysModels
    {
        public class GetAllStrategysResponse
        {
            public GetAllStrategysResponse(List<Strategy> strategys)
            {
                Strategys = strategys;
            }
            public List<Strategy> Strategys { get; set; }

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
