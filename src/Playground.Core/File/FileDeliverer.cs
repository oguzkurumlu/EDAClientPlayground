using Playground.Core.Core;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Playground.Core.File
{
    public class FileDeliverer : IDeliverer
    {
        private readonly string fileName;

        public FileDeliverer(string fileName)
        {
            this.fileName = fileName;
        }

        public void Dispose()
        {
            ///TODO : Implement this.
        }

        public async Task SendAsync(IMessage message, CancellationToken cancellationToken)
        {
            var serializedMessage = JsonSerializer.Serialize(message.Message);
            await System.IO.File.AppendAllTextAsync(fileName, $"{serializedMessage}{Environment.NewLine}", cancellationToken);
        }
    }
}
