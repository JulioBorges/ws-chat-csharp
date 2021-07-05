using WSChat.Shared.Enums;

namespace WSChat.Shared.Domain
{
    public class MessageData
    {
        public MessageType Type { get; set; }

        public string Data { get; set; }

        internal MessageData() { }

        public MessageData(MessageType type) => Type = type;
    }
}
