using Confluent.Kafka;
using Playground.Core;
using Playground.Core.Core;

class Program
{
    static async Task Main(string[] args)
    {
        var apiFacade  = new ApiFacade();
        Console.WriteLine("Type messages. Press Ctrl+C to exit.");
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                var message = new EventMessage() { Key = "oguz", Message=line };
                await apiFacade.SendAsync(message);
            }
            catch (ProduceException<string, string> ex)
            {
                Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
            }
        }
    }
}
