using System;
using System.Net.Sockets;

namespace WSChat.Shared.Domain
{
    public class Session
    {
        public Guid Token { get; private set; }

        public User User { get; private set; }

        public TcpClient Client { get; private set; }

        public Session()
        {
            Token = Guid.NewGuid();
            Client = null;
            User = null;
        }

        public void SetClient(TcpClient client) => Client = client;

        public void SetUser(User user) => User = user;
    }
}
