using WSChat.Shared.Enums;

namespace WSChat.Shared.Domain
{
    public class MessageData
    {
        public MessageType Type { get; set; }

        public string Data { get; set; }

        public MessageData()
        {
        }

        public MessageData(MessageType type, string data)
        {
            Type = type;
            Data = data;
        }

        public MessageData(MessageType type) => Type = type;
    }
}
