﻿using System;
using System.Collections.Generic;
using System.Linq;
using WSChat.Shared.Base;
using WSChat.Shared.Domain;

namespace WSChat.Shared.Managers
{
    public class RoomManager : BaseManager<Room>
    {
        public IEnumerable<Room> RoomsList => ListData;

        public void AddRoom(Room room) => AddItem(room);

        public void RemoveRoom(string name)
        {
            Room room = ListData.FirstOrDefault(r => r.Name == name);

            if (room == null)
                throw new Exception($"Room {name} not exists");

            RemoveItem(room);
        }

        protected override bool ItemExists(Room room) =>
            ListData.Any(r => r.Name == room.Name);

        public bool RoomExists(string name) => ListData.Any(r => r.Name == name);
    }
}
