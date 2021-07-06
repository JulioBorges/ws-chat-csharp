using System.Collections.Generic;
using WSChat.Shared.Domain;

namespace WSChat.Shared.Contracts
{
    public interface IRoomManager
    {
        public IEnumerable<Room> RoomsList { get; }
        public void AddRoom(Room room);
        public void RemoveRoom(string name);
        public bool RoomExists(string name);
    }
}
