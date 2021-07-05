using System;
using System.Threading;
using WSChat.Shared.Helpers;

namespace WSChat.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            const string title = "CHAT CLIENT";

            (string ip, int port) = ConsoleHelper.LoadOrShowConfigurationMenu(args);

            ConsoleHelper.PrintHeaderChat(ip, port, title);

            ChatClient chatClient = new();

            ConfigureClient(ip, port, chatClient);

            Console.WriteLine("SERVER SUCCESSFULLY CONFIGURED !");
            Console.WriteLine();
            bool registered = false;
            string nickName = GetNickName(chatClient, ref registered);

            if (!JoinRoom(chatClient))
                return;

            Thread.Sleep(250);
            Console.Clear();

            ConsoleHelper.PrintHeaderChat(ip, port, title);
            
            ChatLoop(chatClient, nickName);

            Console.Read();

            chatClient.StopClient();
        }

        private static void ChatLoop(ChatClient chatClient, string nickName)
        {
            Console.WriteLine("*** Type your message (put /p to private message) QUIT to quit chat");

            string message;
            do
            {
                message = Console.ReadLine();

                if (message.StartsWith("/p"))
                {
                    var clearMsg = message[(message.IndexOf(" ") + 1)..];
                    var user = clearMsg.Substring(0, clearMsg.IndexOf(" "));
                    var msg = clearMsg[(message.IndexOf(" ") + 1)..];

                    chatClient.TypeMessagePrivate(user, msg);
                }
                else
                    chatClient.TypeMessage(message);

                if (message == "QUIT")
                    chatClient.QuitChat(nickName);

            } while (message != "QUIT");
        }

        private static bool JoinRoom(ChatClient chatClient)
        {
            Console.WriteLine("Please type a room name to join.");

            string room = Console.ReadLine();

            if (!chatClient.TryJoinRoom(room))
            {
                Console.WriteLine("Error when attempt to join or create room");
                Console.Read();
                return false;
            }

            return true;
        }

        private static string GetNickName(ChatClient chatClient, ref bool registered)
        {
            Console.WriteLine("Please type your nickname.");
            var nickName = Console.ReadLine();

            do
            {
                if (chatClient.TryRegisterUser(nickName))
                    registered = true;
                else
                {
                    Console.WriteLine($"*** Sorry, the nickName {nickName} is already taken. " +
                        $"Please choose a different one:");

                    nickName = Console.ReadLine();
                }
            } while (!registered);
            return nickName;
        }

        private static void ConfigureClient(string ip, int port, ChatClient chatClient)
        {
            chatClient.SetErrorHandler(error =>
            {
                Console.WriteLine($"[ERROR]: {error.Message}");
                Console.WriteLine();
            });

            chatClient.SetMessageHandler(msg =>
            {
                Console.WriteLine(msg);
                Console.Write(">");
            });

            chatClient.SetServer(ip, port);
            chatClient.ConnectToServer();

            chatClient.StartThreads();
        }
    }
}