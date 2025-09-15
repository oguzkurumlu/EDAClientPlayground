using System;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.Core
{
    public interface IDeliverer : IDisposable
    {
        Task SendAsync(IMessage message, CancellationToken cancellationToken);
    }
}