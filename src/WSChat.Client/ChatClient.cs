using System;
using System.Net.Sockets;
using System.Threading;
using WSChat.Shared;
using WSChat.Shared.Base;
using WSChat.Shared.Domain;
using WSChat.Shared.Enums;

namespace WSChat.Client
{
    internal class ChatClient : TCPClient
    {
        public User User { get; private set; }

        public ChatClient()
        {
            User = new ();
            Quit = false;
        }

        internal void StartThreads()
        {
            new Thread(new ThreadStart(VerifyData)).Start();
            new Thread(new ThreadStart(CheckIsRunning)).Start();
        }

        internal void StopClient() => Quit = true;

        private void VerifyData()
        {
            while (!Quit)
            {
                try
                {
                    if (tcpClient.GetStream().DataAvailable)
                    {
                        Thread.Sleep(25);
                        MessageData message = GetMessage();

                        if (message != null)
                            new Thread(() => ProcessData(message)).Start();
                    }
                }
                catch (InvalidOperationException e)
                {
                    HandleError(e);
                }

                Thread.Sleep(5);
            }
        }

        private void CheckIsRunning()
        {
            while (!Quit)
            {
                Socket socket = tcpClient.Client;

                if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                {
                    Quit = true;
                    HandleLog("Client stopped: Server disconnected");
                }

                Thread.Sleep(5);
            }
        }

        private void ProcessData(MessageData message)
        {
            string[] msgData = message.Data.Split("##");

            switch (message.Type)
            {
                case MessageType.Register:
                    RegisterByMessage(msgData);
                    break;
                case MessageType.Quit:
                    QuitUserByMessage();
                    break;
                case MessageType.JoinRoom:
                    JoinRoomByMessage(msgData);
                    break;
                case MessageType.QuitRoom:
                    QuitRoomByMessage(msgData);
                    break;
                case MessageType.ListChatRooms:
                case MessageType.ListUsers:
                    HandleMessage(msgData[1]);
                    break;
                case MessageType.SendMessage:
                    SendMessageData(message);
                    break;
                case MessageType.SendPrivateMessage:
                    SendMessageData(message);
                    break;
            }
        }

        private void RegisterByMessage(string[] msgData)
        {
            if (msgData[0] == Constants.SUCCESS_FLAG)
                HandleLog($"Registration success: {User.NickName}");
            else
                HandleLog($"Registration failed: {User.NickName}");
        }

        private void QuitUserByMessage()
        {
            Quit = true;
            HandleLog("Server disconnected");
        }

        private void QuitRoomByMessage(string[] msgData)
        {
            if (msgData[0] == Constants.SUCCESS_FLAG)
            {
                User.SetActiveRoom(null);
                HandleLog($"Room left: {msgData[1]}");
            }
            else
                HandleLog($"Could not leave room: {msgData[1]}");
        }

        private void JoinRoomByMessage(string[] msgData)
        {
            if (msgData[0] == Constants.SUCCESS_FLAG)
            {
                User.SetActiveRoom(new Room(msgData[1]));
                HandleLog($"User Joined room: {msgData[1]}");
            }
            else
                HandleLog("User could not join room: {msgData[1]}");
        }

        private void SendMessageData(MessageData message)
        {
            HandleLog($"Message received: {message.Data}");
            HandleMessage(message.Data);
        }

        internal bool TryRegisterUser(string nickName)
        {
            MessageData msgRegister = new (MessageType.Register);
            msgRegister.Data = nickName;
            SendMessage(msgRegister);

            MessageData registerResult = GetMessage();
            if (registerResult == null)
                HandleError(new Exception("Connection Error"));

            var msgData = registerResult.Data.Split("##");
            return (msgData[0] == Constants.SUCCESS_FLAG);
        }

        internal bool TryJoinRoom(string room)
        {
            MessageData msgJoin = new(MessageType.JoinRoom);
            msgJoin.Data = room;
            SendMessage(msgJoin);

            MessageData joinesult = GetMessage();
            if (joinesult == null)
                HandleError(new Exception("Connection Error"));

            var msgData = joinesult.Data.Split("##");
            return (msgData[0] == Constants.SUCCESS_FLAG);
        }

        internal void QuitChat(string nickName)
        {
            MessageData msgQuit = new(MessageType.Quit);
            msgQuit.Data = nickName;
            SendMessage(msgQuit);

            MessageData quitResult = GetMessage();
            if (quitResult == null)
                HandleError(new Exception("Connection Error"));

        }

        internal void TypeMessage(string message)
        {
            MessageData msg = new(MessageType.SendMessage);
            msg.Data = message;
            SendMessage(msg);

            MessageData typeResult = GetMessage();
            if (typeResult == null)
                HandleError(new Exception("Connection Error"));

            HandleMessage(typeResult.Data);
        }

        internal void TypeMessagePrivate(string user, string message)
        {
            MessageData msg = new(MessageType.SendPrivateMessage);
            msg.Data = $"{user}##{message}";
            SendMessage(msg);
        }
    }
}
