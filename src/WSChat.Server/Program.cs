using System;
using WSChat.Shared.Domain;

namespace WSChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Room room = new Room("ROOM");

            room.SetMessageHandler(Console.WriteLine);

            room.Connect();

            
            Console.Read();

            room.Close();
        }
    }
}
