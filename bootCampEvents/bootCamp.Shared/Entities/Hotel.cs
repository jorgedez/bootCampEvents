using System.Collections.Generic;

namespace bootCamp.Shared.Entities
{
    public class Hotel
    {
        public string sessionId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public int ranking { get; set; }
        public double minPrice { get; set; }
        public Room[] rooms { get; set; }
        public object packages { get; set; }
    }
}
