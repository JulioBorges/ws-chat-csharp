using System;
using System.Net;
using System.Net.Sockets;
using WSChat.Shared.Domain;
using WSChat.Shared.Helpers;

namespace WSChat.Shared.Base
{
    public abstract class TCPServer : HandlersSetter
    {
        protected TcpClient _tcpClient;
        protected TcpListener _tcpListener;

        public bool Running { get; private set; }
        public int Port { get; private set; }
        public string IPServer { get; private set; }

        public void StartServer(string ipServer = Constants.DEFAULT_IP, int port = Constants.DEFAULT_PORT)
        {
            Port = port;
            IPServer = ipServer;
            Running = false;

            StartListener();
        }

        private void StartListener()
        {
            IPAddress myIp = IPAddress.Parse(IPServer);

            try
            {
                _tcpListener = new TcpListener(myIp, Port);
                _tcpListener.Start();
                
                Running = true;
                HandleLog("[TcpServer] :: Listener started ...");
            }
            catch (SocketException e)
            {
                HandleLog($"[TcpServer] :: The connection has failed: {e.Message}");
                HandleError(e);
            }
        }

        public void StopServer()
        {
            HandleLog("[TcpServer] :: Stopping server ...");
            Running = false;
            _tcpListener.Stop();
        }

        public MessageData GetMessage(Socket socket)
        {
            HandleLog("[TcpServer] :: Receiving a message");

            try
            {
                MessageData message = socket.ReadData<MessageData>();

                HandleLog($"[TcpServer] :: MessageType: {message.Type}");

                return message;
            }
            catch (Exception e)
            {
                HandleError(e);
            }

            return null;
        }

        public void SendMessage(MessageData message, Socket socket)
        {
            HandleLog($"[TcpServer] :: Sending a message: {message.Type}");

            try
            {
                socket.WriteData(message);
            }
            catch (Exception e)
            {
                HandleLog($"[TcpServer] :: Exception: {e.Message}");
                HandleError(e);
            }
        }
    }
}
