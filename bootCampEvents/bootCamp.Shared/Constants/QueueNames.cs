namespace bootCamp.Shared.Constants
{
    public static class QueueNames
    {
        public const string LowCost = "LowCost";
        public const string Premiere = "Premiere";
        public const string Luxury = "Luxury";
        public const string LowCostDeadLetter = "LowCost/$DeadLetterQueue";
        public const string LuxuryDeadLetter = "Luxury/$DeadLetterQueue";
        public const string PremiereDeadLetter = "Premiere/$DeadLetterQueue";
    }
}
