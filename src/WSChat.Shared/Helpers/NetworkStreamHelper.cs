using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WSChat.Shared.Helpers
{
    public static class NetworkStreamHelper
    {
        public static void WriteData<T>(this NetworkStream stream, T message)
        {
            var buffer = JsonHelper.Serialize(message);

            stream.Write(buffer.ToArray());
        }

        public static T ReadData<T>(this TcpClient client)
        {
            string request = default;
            StringBuilder sb = new();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesCount;

            if (client.GetStream().CanRead)
            {
                do
                {
                    bytesCount = client.GetStream().ReadAsync(buffer, 0, buffer.Length).Result;
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesCount));
                }
                while (client.GetStream().DataAvailable);

                request = sb.ToString();
            }

            return JsonConvert.DeserializeObject<T>(request);
        }

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
