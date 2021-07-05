using System;

namespace WSChat.Shared.Domain
{
    public class User : IComparable<User>
    {
        public Guid Id { get; private set; }
        public string NickName { get; private set; }
        public Room ActiveRoom { get; private set; }

        public User(string name)
        {
            Id = Guid.NewGuid();
            NickName = name;
            ActiveRoom = null;
        }

        public User()
        {
            Id = Guid.NewGuid();
            ActiveRoom = null;
        }

        public void SetActiveRoom(Room room) => ActiveRoom = room;

        public int CompareTo(User other) => NickName == other.NickName ? 0 : -1;
    }
}
