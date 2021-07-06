using System.Linq;
using WSChat.Shared.Managers;
using Xunit;

namespace WSChat.Shared.Tests.Managers
{
    public class UserManagerTests
    {
        [Fact(DisplayName = "When create a UserManager then UserList should be empty")]
        public void WhenCreateAUserManagerThenUserListShouldBeEmpty()
        {
            UserManager manager = new();

            Assert.NotNull(manager);
            Assert.Empty(manager.UsersList);
        }

        [Fact(DisplayName = "When add a user to UserManager then UserList should have the added user")]
        public void WhenAddAUserToUserManagerThenUserListShouldHaveTheAddedUser()
        {
            UserManager manager = new();

            manager.AddUser(new("A"));

            Assert.NotNull(manager);
            Assert.NotEmpty(manager.UsersList);
            Assert.Single(manager.UsersList);
            Assert.Equal("A", manager.UsersList.First().NickName);
        }

        [Fact(DisplayName = "When remove a user from UserManager then UserList doesnt should have the deleted user")]
        public void WhenRemoveAUserFromUserManagerThenUserListDoesntShouldHaveTheDeletedUser()
        {
            UserManager manager = new();
            manager.AddUser(new("A"));

            manager.RemoveUser("A");
            Assert.Empty(manager.UsersList);
        }
    }
}
