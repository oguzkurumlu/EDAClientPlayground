using Confluent.Kafka;
using Playground.Core.Core;
using Playground.Core.File;
using Playground.Core.Kafka;
using Playground.Core.RabbitMQ;
using System;
using System.Threading.Tasks;

namespace Playground.Core
{
    public class ApiFacade
    {
        private readonly IDeliver deliver;

        private ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = "asdasdlocalhost:9092",

            EnableIdempotence = false,
            MessageSendMaxRetries = 0,
            RetryBackoffMs = 1,
            MessageTimeoutMs = 3000,

            SocketConnectionSetupTimeoutMs = 1000,
            SocketTimeoutMs = 1000,

            Acks = Acks.All
        };

        public ApiFacade()
        {
            deliver = new ChainDecorator(new KafkaDeliver("eda_client", config), new ChainDecorator(new RabbitDeliver("localhost22", "kafka_events"), new FileDeliver("kafka.log")));
        }

        public async Task SendAsync(IMessage message)
        {
            await deliver.SendAsync(message);
        }
    }
}
