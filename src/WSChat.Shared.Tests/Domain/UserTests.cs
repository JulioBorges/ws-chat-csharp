using System;
using WSChat.Shared.Domain;
using Xunit;

namespace WSChat.Shared.Tests.Domain
{
    [CollectionDefinition("User tests", DisableParallelization = false)]
    public class UserTests
    {
        [Fact(DisplayName = "When creating a User, should have a nickname")]
        public void WhenCreatingUserShouldHaveANickname()
        {
            User user = new("NICK");

            Assert.Equal("NICK", user.NickName);
        }

        [Fact(DisplayName = "When creating a User, should have a valid Id")]
        public void WhenCreatingUserShouldhavAValidId()
        {
            User user = new();

            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact(DisplayName = "When setting a room should be a not null ActiveRoom")]
        public void WhenSettingARoomShouldBeANotNullActiveRoom()
        {
            User user = new("NICK");
            Room room = new("ROOM");
            user.SetActiveRoom(room);

            Assert.NotNull(user.ActiveRoom);
            Assert.Equal(room.Name, user.ActiveRoom.Name);
        }
    }
}
