using Confluent.Kafka;
using Playground.Core.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.Kafka
{
    public class KafkaDeliverer : IDeliverer
    {
        private readonly IProducer<string, object> producer;
        private readonly string topic;

        public KafkaDeliverer(string topic, ProducerConfig producerConfig)
        {
            producer = new ProducerBuilder<string, object>(producerConfig)
                .SetValueSerializer(new ObjectJsonSerializer())
                .Build();
            this.topic = topic;
        }

        public void Dispose()
        {
            producer.Dispose();
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            var kafkaMessage = new Message<string, object>();
            kafkaMessage.Value = message.Message;
            kafkaMessage.Key = message.Key;

            var deliveryResult = await producer.ProduceAsync(topic, kafkaMessage, cancellationToken);

            if(deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception("Message is not persisted.");
            }
        }
    }
}