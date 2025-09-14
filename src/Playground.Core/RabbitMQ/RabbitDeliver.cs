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
    public class RabbitDeliver : IDeliver
    {
        private readonly string hostname;
        private readonly string queue;

        public RabbitDeliver(string hostname, string queue)
        {
            this.hostname = hostname;
            this.queue = queue;
        }

        public async Task SendAsync(IMessage message)
        {
            //TODO : Fix this. Find a better solution
            var factory = new ConnectionFactory { HostName = hostname };
            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            
            var props = new BasicProperties();
            props.Headers = new Dictionary<string, object?>()
            {
                ["kafka_key"] = message.Key
            };

            var serializedMessage = JsonSerializer.Serialize(message);
            var encodedMessage = Encoding.UTF8.GetBytes(serializedMessage);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, mandatory:true, basicProperties: props, body: encodedMessage);
        }
    }
}
