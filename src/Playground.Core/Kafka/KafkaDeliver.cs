using Confluent.Kafka;
using Playground.Core.Core;
using System;
using System.Threading.Tasks;

namespace Playground.Core.Kafka
{
    public class KafkaDeliver : IDeliver
    {
        private readonly IProducer<string, object> producer;
        private readonly string topic;

        public KafkaDeliver(string topic, ProducerConfig producerConfig)
        {
            producer = new ProducerBuilder<string, object>(producerConfig)
                .SetValueSerializer(new ObjectJsonSerializer())
                .Build();
            this.topic = topic;
        }

        public async Task SendAsync(IMessage message)
        {
            var kafkaMessage = new Message<string, object>();
            kafkaMessage.Value = message.Message;
            kafkaMessage.Key = message.Key;

            var deliveryResult = await producer.ProduceAsync(topic, kafkaMessage);

            if(deliveryResult.Status == PersistenceStatus.NotPersisted)
            {
                throw new Exception("Message is not persisted.");
            }
        }
    }
}