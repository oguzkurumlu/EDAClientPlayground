using Playground.Core.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Playground.Core.File
{
    public class FileDeliver : IDeliver
    {
        private readonly string fileName;

        public FileDeliver(string fileName)
        {
            this.fileName = fileName;
        }

        public async Task SendAsync(IMessage message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            await System.IO.File.AppendAllTextAsync(fileName, $"{serializedMessage}{Environment.NewLine}");
        }
    }
}
