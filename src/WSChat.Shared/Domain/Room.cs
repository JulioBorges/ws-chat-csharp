using System;
using System.Collections.Generic;

namespace WSChat.Shared.Domain
{
    public class Room : IComparable<Room>
    {

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public Room(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }

        public int CompareTo(Room other) => (Name == other.Name) ? 0 : 1;
    }
}
