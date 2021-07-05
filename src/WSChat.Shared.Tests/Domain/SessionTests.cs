using System;
using System.Net.Sockets;
using WSChat.Shared.Domain;
using Xunit;

namespace WSChat.Shared.Tests.Domain
{
    public class SessionTests
    {
        [Fact(DisplayName = "When creating a session it should have a valid token")]
        public void WhenCreatingASessionItShouldHaveAValidToken()
        {
            Session session = new();
            Assert.NotEqual(Guid.Empty, session.Token);
        }

        [Fact(DisplayName = "When setting a user it should not null")]
        public void WhenSettingAUserItShouldNotNull()
        {
            Session session = new();
            var user = new User("USER");
            session.SetUser(user);

            Assert.NotNull(session.User);
            Assert.Equal(user.NickName, session.User.NickName);
        }

        [Fact(DisplayName = "When setting a Client it should not null")]
        public void WhenSettingAClientItShouldNotNull()
        {
            Session session = new();
            var client = new TcpClient();
            session.SetClient(client);

            Assert.NotNull(session.Client);
        }
    }
}
