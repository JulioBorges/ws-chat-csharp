using System;

namespace WSChat.Shared.Base
{
    public abstract class HandlersSetter
    {
        private Action<string> MessageHandler { get; set; }
        private Action<string> LogHandler { get; set; }
        private Action<Exception> ErrorHandler { get; set; }

        public void SetMessageHandler(Action<string> handler) => MessageHandler = handler;

        public void SetLogHandler(Action<string> handler) => LogHandler = handler;

        public void SetErrorHandler(Action<Exception> handler) => ErrorHandler = handler;

        protected void HandleMessage(string message) => MessageHandler?.Invoke(message);

        protected void HandleError(Exception exception) => ErrorHandler?.Invoke(exception);

        protected void HandleLog(string log) => LogHandler?.Invoke(log);
    }
}
