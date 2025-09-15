using System;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.Core
{
    public class ChainDecorator : IDeliverer
    {
        private readonly IDeliverer first;
        private readonly IDeliverer second;

        public ChainDecorator(IDeliverer first, IDeliverer second)
        {
            this.first = first;
            this.second = second;
        }

        public void Dispose()
        {
            first.Dispose(); 
            second.Dispose();
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            try
            {
                await first.SendAsync(message, cancellationToken);
            }
            catch (Exception)
            {
                await second.SendAsync(message, cancellationToken);
            }
        }
    }
}
