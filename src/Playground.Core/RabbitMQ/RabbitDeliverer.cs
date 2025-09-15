using Playground.Core.Core;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Playground.Core.RabbitMQ
{
    public class RabbitDeliverer : IDeliverer
    {
        private readonly string hostname;
        private readonly string queue;

        public RabbitDeliverer(string hostname, string queue)
        {
            this.hostname = hostname;
            this.queue = queue;
        }

        public void Dispose()
        {
            //TODO : implement this
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            //TODO : Fix this. Find a better solution
            var factory = new ConnectionFactory { HostName = hostname };
            var connection = await factory.CreateConnectionAsync(cancellationToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
            
            var props = new BasicProperties();
            props.Headers = new Dictionary<string, object?>()
            {
                ["kafka_key"] = message.Key
            };

            var serializedMessage = JsonSerializer.Serialize(message.Message);
            var encodedMessage = Encoding.UTF8.GetBytes(serializedMessage);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, mandatory:true, basicProperties: props, body: encodedMessage, cancellationToken);
        }
    }
}
