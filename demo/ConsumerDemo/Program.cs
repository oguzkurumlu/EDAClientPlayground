using System.Text.Json;
using Confluent.Kafka;
using ConsumerDemo;

class Program
{
    const string brokers = "localhost:9092";
    const string topic = "moneytransfer";
    const string groupId = "moneytransfer-writer";

    static async Task Main()
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = brokers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Latest,
            EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        consumer.Subscribe(topic);
        Console.WriteLine($"listening topic='{topic}' on brokers='{brokers}'");

        var cancelationTokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) => { e.Cancel = true; cancelationTokenSource.Cancel(); };

        try
        {
            while (!cancelationTokenSource.IsCancellationRequested)
            {
                try
                {
                    var current = consumer.Consume(cancelationTokenSource.Token);
                    var message = current.Message?.Value;
                    MoneyTransfer? moneyTransfer = GetMoneyTransfer(message);

                    if (moneyTransfer is null || string.IsNullOrWhiteSpace(moneyTransfer.From) || string.IsNullOrWhiteSpace(moneyTransfer.To))
                    {
                        consumer.Commit(current);
                        continue;
                    }

                    var isSuccess = await DbHelper.Save(moneyTransfer, cancelationTokenSource.Token);
                    if (isSuccess)
                    {
                        consumer.Commit(current);
                        Console.WriteLine($"Message consumed : {moneyTransfer}");
                    }
                }
                catch (ConsumeException)
                {
                    Console.WriteLine("Topic is not created yet.");
                }
            }
        }
        catch (OperationCanceledException)
        {

        }
        finally
        {
            consumer.Close();
        }
    }

    private static MoneyTransfer? GetMoneyTransfer(string msg)
    {
        MoneyTransfer? moneyTransfer = null;
        try
        {
            moneyTransfer = JsonSerializer.Deserialize<MoneyTransfer>(msg!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"json-deserialize-failed: {ex.Message}");
        }

        return moneyTransfer;
    }
}
