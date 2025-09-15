namespace ConsumerDemo
{
    public sealed class MoneyTransfer
    {
        public string From { get; set; } = default!;
        public string To { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{From} -> {To} : {Amount}";
        }
    }
}
