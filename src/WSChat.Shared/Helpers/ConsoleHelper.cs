using System;

namespace WSChat.Shared.Helpers
{
    public static class ConsoleHelper
    {
        public static (string ip, int port) LoadOrShowConfigurationMenu(string[] args)
        {
            if (args.Length >= 2)
                return (args[0], Convert.ToInt32(args[1]));

            Console.WriteLine("Configure the server:");

            Console.WriteLine("Type the IP for chat " +
                "Server or tap ENTER to set default IP (127.0.0.1).");
            string ip = Console.ReadLine();

            if (string.IsNullOrEmpty(ip))
                ip = Constants.DEFAULT_IP;

            Console.WriteLine("Type the PORT for chat Server or tap ENTER to set default PORT (55400).");
            string strPort = Console.ReadLine();

            int port = (string.IsNullOrEmpty(strPort)) ? Constants.DEFAULT_PORT
                : Convert.ToInt32(strPort);

            return (ip, port);
        }

        public static void PrintHeaderChat(string ip, int port, string title)
        {
            Console.WriteLine($"######### {title} #########");
            Console.WriteLine($"SERVER ENDPOINT: {ip}:{port}");
        }
    }
}
