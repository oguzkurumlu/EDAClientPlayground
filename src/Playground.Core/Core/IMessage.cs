namespace Playground.Core.Core
{
    public interface IMessage
    {
        public string Key { get; set; }

        public object Message { get; set; }
    }
}