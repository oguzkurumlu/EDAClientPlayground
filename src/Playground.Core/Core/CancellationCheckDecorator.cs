using System;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.Core
{
    public class CancellationCheckDecorator : IDeliverer
    {
        private readonly IDeliverer decorated;

        public CancellationCheckDecorator(IDeliverer decorated)
        {
            this.decorated = decorated;
        }

        public void Dispose()
        {
            decorated.Dispose();
        }

        public Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            return decorated.SendAsync(message, cancellationToken);
        }
    }
}
