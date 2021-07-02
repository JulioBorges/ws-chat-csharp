using System;
using System.Collections.Generic;

namespace WSChat.Shared.Domain
{
    public class Room
    {

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public List<User> Users { get; private set; }

        private bool _listening;
        private Action<string> _messageHandler = null;
        private Action<Exception> _errorHandler = null;

        private void HandleMessage(string message) => _messageHandler?.Invoke(message);
        private void HandleError(Exception exception) => _errorHandler?.Invoke(exception);

        public void SetMessageHandler(Action<string> messageHandler) => _messageHandler = messageHandler;
        public void SetErrorHandler(Action<Exception> errorHandler) => _errorHandler = errorHandler;

        public Room(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Ip = "127.0.0.1";
            Port = 34805;// new Random().Next(30000, 40500);
            Users = new List<User>();
        }

        public void Connect()
        {
        }

        public void Close()
        {
            _listening = false;
        }
    }
}
