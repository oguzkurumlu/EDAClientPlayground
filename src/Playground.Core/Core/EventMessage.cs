namespace Playground.Core.Core
{
    public class EventMessage : IMessage
    {
        public string Key { get; set; }

        public object Message { get; set; }
    }
}
