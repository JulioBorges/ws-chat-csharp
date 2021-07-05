using WSChat.Shared.Domain;
using Xunit;

namespace WSChat.Shared.Tests.Domain
{
    [CollectionDefinition("User tests", DisableParallelization = false)]
    public class UserTests
    {
        [Fact(DisplayName = "When creating a User it must have a nickname")]
        public void When_Creating_User_Should_Nickname()
        {
            User user = new("NICK");

            Assert.Equal("NICK", user.NickName);
        }
    }
}
