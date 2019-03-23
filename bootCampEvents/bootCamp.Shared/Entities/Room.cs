using System.Collections.Generic;

namespace bootCamp.Shared.Entities
{
    public class Room
    {
        public string id { get; set; }
        public string code { get; set; }
        public int distributionId { get; set; }
        public string dealCode { get; set; }
        public string dealName { get; set; }
        public string name { get; set; }
        public object offers { get; set; }
        public double price { get; set; }
        public bool isPVP { get; set; }
        public object rateCodes { get; set; }
        public object rateNames { get; set; }
        public bool directPayment { get; set; }
        public bool canaryResident { get; set; }
        public List<CancellationCost> cancellationCosts { get; set; }
        public string bookingLink { get; set; }
        public string providerName { get; set; }
        public int maxOccupancy { get; set; }
        public int numRoomsAvailable { get; set; }
        public List<ExtraCharge> extraCharges { get; set; }
    }
}
