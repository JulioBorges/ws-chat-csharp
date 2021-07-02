using System;
using WSChat.Shared.Domain;
using Xunit;

namespace WSChat.Shared.Tests.Domain
{
    public class RoomTests
    {
        [Fact(DisplayName = "When creating a room it should have a name")]
        public void When_Creating_Room_Should_Have_A_Name()
        {
            Room room = new("ROOM");

            Assert.Equal("ROOM", room.Name);
        }

        [Fact(DisplayName = "When creating a room it should have a valid Id")]
        public void When_Creating_A_Room_It_Should_Have_A_Valid_Id()
        {
            Room room = new("ROOM");
            Assert.NotEqual(Guid.Empty, room.Id);
        }

        [Fact(DisplayName = "When creating a room it should have a empty users list")]
        public void When_Creating_A_Room_It_Should_Have_A_Empty_Users_List()
        {
            Room room = new("ROOM");
            Assert.NotNull(room.Users);
            Assert.Empty(room.Users);
        }
    }
}
