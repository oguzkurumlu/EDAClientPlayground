using Confluent.Kafka;
using Playground.Core;
using Playground.Core.Core;
using ProducerDemo;

class Program
{
    static async Task Main(string[] args)
    {
        var apiFacade  = new ApiFacade();
        Console.WriteLine("Type a key. Press Ctrl+C to exit.");
        while (true)
        {
            var key = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(key))
                continue;

            try
            {
                var message = new EventMessage()
                {
                    Key = key,
                    Message = new MoneyTransfer()
                    {
                        From = key,
                        To = "oguz",
                        Amount = 999
                    }
                };
                await apiFacade.SendAsync(message);
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
            }
        }
    }
}
