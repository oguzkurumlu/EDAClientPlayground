using System;
using System.Collections.Generic;
using System.Text;

namespace Playground.Core.Core
{
    public class EventMessage : IMessage
    {
        public string Key { get; set; }

        public string Message { get; set; }
    }
}
