using System;
using WSChat.Shared.Domain;
using Xunit;

namespace WSChat.Shared.Tests.Domain
{
    public class RoomTests
    {
        [Fact(DisplayName = "When creating a room it should have a name")]
        public void WhenCreatingRoomShouldHaveAName()
        {
            Room room = new("ROOM");
            Assert.Equal("ROOM", room.Name);
        }

        [Fact(DisplayName = "When creating a room it should have a valid Id")]
        public void WhenCreatingARoomItShouldHaveAValidId()
        {
            Room room = new("ROOM");
            Assert.NotEqual(Guid.Empty, room.Id);
        }
    }
}
