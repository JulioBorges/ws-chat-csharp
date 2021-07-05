using System.Net.Sockets;

namespace WSChat.Shared.Helpers
{
    public static class NetworkStreamHelper
    {
        public static T ReadData<T>(this Socket socket)
        {
            byte[] bytes = new byte[socket.ReceiveBufferSize];
            socket.Receive(bytes);

            return JsonHelper.Desserialize<T>(bytes);
        }

        public static void WriteData<T>(this Socket socket, T data)
        {
            var bytes = JsonHelper.Serialize<T>(data);
            socket.Send(bytes);
        }
    }
}
