using System.Linq;
using WSChat.Shared.Managers;
using Xunit;

namespace WSChat.Shared.Tests.Managers
{
    public class RoomManagerTests
    {   
        [Fact(DisplayName = "When create a RoomManager then RoomList should be empty")]
        public void WhenCreateARoomManagerThenRoomListShouldBeEmpty()
        {
            RoomManager manager = new();

            Assert.NotNull(manager);
            Assert.Empty(manager.RoomsList);
        }

        [Fact(DisplayName = "When add a room to RoomManager then RoomList should have the added room")]
        public void WhenAddARoomToRoomManagerThenRoomListShouldHaveTheAddedRoom()
        {
            RoomManager manager = new();

            manager.AddRoom(new("A"));

            Assert.NotNull(manager);
            Assert.NotEmpty(manager.RoomsList);
            Assert.Single(manager.RoomsList);
            Assert.Equal("A", manager.RoomsList.First().Name);
        }

        [Fact(DisplayName = "When remove a room from RoomManager then RoomList doesnt should have the deleted room")]
        public void WhenRemoveARoomFromRoomManagerThenRoomListDoesntShouldHaveTheDeletedRoom()
        {
            RoomManager manager = new ();
            manager.AddRoom(new("A"));

            manager.RemoveRoom("A");
            Assert.Empty(manager.RoomsList);
        }

        [Fact(DisplayName = "When search specific Room should be find romm at RoomManager")]
        public void WhenSearchSpecificRoomShouldBeFindRommAtRoomManager()
        {
            RoomManager manager = new();
            manager.AddRoom(new("A"));

            Assert.True(manager.RoomExists("A"));
            Assert.False(manager.RoomExists("B"));
        }
    }
}
