using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Playground.Core.Core
{
    public class ChainDecorator : IDeliver
    {
        private readonly IDeliver first;
        private readonly IDeliver second;

        public ChainDecorator(IDeliver first, IDeliver second)
        {
            this.first = first;
            this.second = second;
        }

        public async Task SendAsync(IMessage message)
        {
            try
            {
                await first.SendAsync(message);
            }
            catch (Exception)
            {
                await second.SendAsync(message);
            }
        }
    }
}
