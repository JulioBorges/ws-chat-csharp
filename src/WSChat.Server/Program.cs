using System;
using WSChat.Shared;
using WSChat.Shared.Helpers;

namespace WSChat.Server
{
    class Program
    {
        public static void Main(string[] args)
        {
            const string title = "CHAT SERVER";
            (string ip, int port) = ConsoleHelper.LoadOrShowConfigurationMenu(args);
            ConsoleHelper.PrintHeaderChat(ip, port, title);

            ChatServer chatServer = new();

            chatServer.SetLogHandler(log => {
                Console.WriteLine($"[LOG]: {log}");
            });

            chatServer.SetErrorHandler(error => {
                Console.WriteLine($"[ERROR]: {error.Message}");
                Console.WriteLine();
            });

            chatServer.StartServer(ip, port);

            if (chatServer.Running)
            {
                chatServer.StartThreads();
                Console.Read();
                chatServer.StopServer();
            }
            else
                Console.WriteLine("Server failure");
        }
    }
}
