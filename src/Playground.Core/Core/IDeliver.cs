using System.Threading.Tasks;

namespace Playground.Core.Core
{
    public interface IDeliver
    {
        Task SendAsync(IMessage message);
    }
}