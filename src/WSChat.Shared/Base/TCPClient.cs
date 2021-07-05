using System;
using System.Net.Sockets;
using WSChat.Shared.Domain;
using WSChat.Shared.Helpers;

namespace WSChat.Shared.Base
{
    public abstract class TCPClient : HandlersSetter
    {
        protected TcpClient tcpClient;
        
        public int Port { get; private set; }

        public string IpServer { get; private set; }

        public bool Quit { get; protected set; }

        public TCPClient()
        {
            tcpClient = null;
            IpServer = null;
        }

        public void SetServer(string ipAddress, int port)
        {
            Port = port;
            IpServer = ipAddress;
        }

        public void ConnectToServer()
        {
            try
            {
                tcpClient = new TcpClient(IpServer, Port);
            }
            catch (SocketException e)
            {
                Quit = true;
                HandleError(e);
                throw new Exception(e.Message);
            }
        }

        public MessageData GetMessage()
        {
            if (!Quit)
            {
                try
                {
                    MessageData message = tcpClient.Client.ReadData<MessageData>();
                    HandleLog($"[TcpClient] ::  Receiving a message: {message.Type}");
                    return message;
                }
                catch (Exception e)
                {
                    HandleLog($"[TcpClient] :: GetMessage exception: {e.Message}");
                    Quit = true;
                    HandleError(e);
                }
            }

            return null;
        }

        protected void SendMessage(MessageData message)
        {
            if (!Quit)
            {
                HandleLog($"[TcpClient] :: Sending a message: {message.Type}");

                try
                {
                    tcpClient.Client.WriteData(message);
                }
                catch (Exception e)
                {
                    HandleLog($"[TcpClient] :: TCPClient sendMessage exception: {e.Message}");
                    Quit = true;
                }
            }
        }
    }
}
