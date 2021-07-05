using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using WSChat.Shared;
using WSChat.Shared.Base;
using WSChat.Shared.Domain;
using WSChat.Shared.Enums;
using WSChat.Shared.Managers;

namespace WSChat.Server
{
    internal class ChatServer : TCPServer
    {

        private readonly UserManager _userManager;
        private readonly SessionManager _sessionManager;
        private readonly RoomManager _roomManager;

        public volatile object threadLock;

        public ChatServer()
        {
            _userManager = new ();
            _sessionManager = new ();
            _roomManager = new ();
            threadLock = new ();
        }

        internal void StartThreads()
        {
            new Thread(new ThreadStart(VerifyData)).Start();
            new Thread(new ThreadStart(CheckIsRunning)).Start();
            new Thread(new ThreadStart(Listen)).Start();
        }

        private void Listen()
        {
            while (this.Running)
            {
                try
                {
                    HandleLog("Waiting for a connection...");
                    TcpClient client = _tcpListener.AcceptTcpClient();
                    Session session = new ();
                    session.SetClient(client);
                    _sessionManager.AddSession(session);

                    HandleLog("New client: " + session.Token);
                }
                catch (SocketException)
                {
                    HandleLog("Listener thread closed");
                }
            }
        }

        private void VerifyData()
        {
            while (this.Running)
            {
                try
                {
                    lock (threadLock)
                    {
                        if (_sessionManager.SessionList.Any())
                        {
                            foreach (Session session in _sessionManager.SessionList)
                            {
                                if (session != null && session.Client.GetStream().DataAvailable)
                                {
                                    Thread.Sleep(25);
                                    MessageData message = GetMessage(session.Client.Client);

                                    if (message != null)
                                    {
                                        Thread processDataThread = 
                                            new(() => ProcessData(session, message));
                                        processDataThread.Start();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (InvalidOperationException e)
                {
                    HandleError(e);
                }

                Thread.Sleep(5);
            }
        }

        private void QuitRoom(Session session, MessageData message)
        {
            try
            {
                if (session.User.ActiveRoom != null)
                {
                    MessageData messageSuccess = new (MessageType.QuitRoom);
                    messageSuccess.Data = $"success##{session.User.ActiveRoom.Name}";
                    SendMessage(messageSuccess, session.Client.Client);

                    BroadcastToRoom(session, $"left the chatroom \"{session.User.ActiveRoom.Name}\"");

                    HandleLog($"{session.User.NickName} left the chatroom: {session.User.ActiveRoom.Name}");

                    session.User.SetActiveRoom(null);
                }
            }
            catch
            {
                MessageData messageError = new (MessageType.QuitRoom);
                messageError.Data = $"error##{message.Data}";
                SendMessage(messageError, session.Client.Client);
            }
        }

        private void ProcessData(Session session, MessageData message)
        {
            if (session.User != null)
            {
                string[] msgData = message.Data.Split("##");

                switch (message.Type)
                {
                    case MessageType.Quit:
                        {
                            MessageData messageSuccess = new(MessageType.Quit);
                            messageSuccess.Data = Constants.SUCCESS_FLAG;
                            SendMessage(messageSuccess, session.Client.Client);

                            if (session.User.ActiveRoom != null)
                                BroadcastToRoom(session, "left the room \"" + session.User.ActiveRoom.Name + "\"");

                            session.Client.Close();
                            _sessionManager.RemoveSession(session.Token);

                            HandleLog("User logout: " + session.Token);
                        }
                        break;

                    case MessageType.JoinRoom:
                        if (session.User.ActiveRoom != null)
                            throw new Exception("Its not possible to join in another room");

                        try
                        {
                            if (!_roomManager.RoomExists(msgData[0]))
                            {
                                _roomManager.AddRoom(new Room(msgData[0]));
                                HandleLog("" + session.User.NickName + " : chatroom has been created: " + msgData[0]);
                            }

                            if (_roomManager.RoomsList.Any(x => x.Name == msgData[0]))
                            {
                                session.User.SetActiveRoom(new(msgData[0]));

                                HandleLog("" + session.User.NickName + " joined the room: " + msgData[0]);

                                MessageData messageSuccess = new(MessageType.JoinRoom);
                                messageSuccess.Data = $"success##{msgData[0]}";

                                SendMessage(messageSuccess, session.Client.Client);

                                MessageData messagePostBroadcast = new(MessageType.SendMessage);
                                BroadcastToRoom(session, $"joined the chatroom \"{msgData[0]}\"");
                            }
                        }
                        catch (Exception e)
                        {
                            MessageData messageSuccess = new(MessageType.JoinRoom);
                            messageSuccess.Data = $"error##{msgData[0]}\r\nChatroom Exception{e.Message}";
                            SendMessage(messageSuccess, session.Client.Client);
                        }
                        break;

                    case MessageType.QuitRoom:
                        QuitRoom(session, message);
                        break;
                    case MessageType.SendMessage:
                        HandleLog($"{session.User.NickName} : message received : {message.Data}");
                        BroadcastToRoom(session, message.Data);
                        break;

                    case MessageType.SendPrivateMessage:
                        HandleLog($"{session.User.NickName} : private message received : {message.Data}");
                        SendToPrivate(session, msgData[0], msgData[1]);
                        break;

                    case MessageType.ListUsers:
                        string room = msgData[0];

                        MessageData messageListUsers = new(MessageType.ListUsers);

                        var users = _sessionManager.SessionList.Where(s => s.User != null &&
                                s.User.ActiveRoom != null &&
                                s.User.ActiveRoom.Name == room)
                            .Select(s => s.User.NickName);

                        string listUsers = string.Join("\r\n", users);
                        messageListUsers.Data = listUsers;
                        SendMessage(messageListUsers, session.Client.Client);

                        break;
                }
            }
            else
            {
                if (message.Type == MessageType.Register)
                {
                    try
                    {
                        string nickName = message.Data;
                        _userManager.AddUser(nickName);

                        session.SetUser(new User(nickName));

                        MessageData messageSuccess = new(MessageType.Register);
                        messageSuccess.Data = Constants.SUCCESS_FLAG;
                        SendMessage(messageSuccess, session.Client.Client);

                        HandleLog($"Registration success : {nickName}");
                    }
                    catch (Exception e)
                    {
                        MessageData messageSuccess = new(MessageType.Register);
                        messageSuccess.Data = $"error##{e.Message}";
                        SendMessage(messageSuccess, session.Client.Client);

                        HandleLog($"Registration failed : {e.Message}");
                    }
                }
            }
        }

        private void CheckIsRunning()
        {
            while (this.Running)
            {
                try
                {
                    foreach (var session in _sessionManager.SessionList)
                    {
                        Socket socket = session.Client.Client;
                        if (socket.Poll(10, SelectMode.SelectRead) && socket.Available == 0)
                        {
                            HandleLog("User logged out : " + session.Token);

                            lock (threadLock)
                            {
                                if (session.User != null &&
                                    session.User.ActiveRoom != null)
                                {
                                    BroadcastToRoom(session,
                                        $"left the chatroom \"{session.User.ActiveRoom.Name}\"");
                                }

                                session.Client.Close();
                                _sessionManager.RemoveSession(session.Token);
                            }
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    HandleLog("Error ocurring during verification. One session was closed abruptally.");
                }

                Thread.Sleep(5);
            }
        }

        private void BroadcastToRoom(Session session, string message)
        {
            Room room = session.User.ActiveRoom;

            if (room != null && message != "")
            {
                MessageData messageToSend = new (MessageType.SendMessage);
                string formmatedMessage = $"{session.User.NickName}: {message}";
                messageToSend.Data = formmatedMessage;

                var sessionsFromRoom = _sessionManager.SessionList
                    .Where(s => s.User.ActiveRoom != null &&
                        s.User.ActiveRoom.Name == room.Name);

                foreach (Session sessionUser in sessionsFromRoom)
                    SendMessage(messageToSend, sessionUser.Client.Client);

                HandleLog("" + session.User.NickName + "'s message broadcast");
            }
            else
            {
                HandleLog($"User is not connected to any chatroom: {session.User.NickName}");
            }
        }

        private void SendToPrivate(Session session, string user, string message)
        {
            Room room = session.User.ActiveRoom;

            if (room != null && message != "")
            {
                MessageData messageToSend = new(MessageType.SendMessage);
                string formmatedMessage = $"{session.User.NickName} *[sends private]: {message}";
                messageToSend.Data = formmatedMessage;

                var sessionsFromRoom = _sessionManager.SessionList
                    .Where(s => s.User.ActiveRoom != null &&
                        s.User.ActiveRoom.Name == room.Name && s.User.NickName == user);

                foreach (Session sessionUser in sessionsFromRoom)
                    SendMessage(messageToSend, sessionUser.Client.Client);

                HandleLog($"{session.User.NickName}'s message private to {user}");
            }
            else
            {
                HandleLog($"User is not connected to any chatroom: {session.User.NickName}");
            }
        }
    }
}
