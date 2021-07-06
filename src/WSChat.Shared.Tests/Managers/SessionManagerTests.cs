using System;
using System.Linq;
using WSChat.Shared.Domain;
using WSChat.Shared.Managers;
using Xunit;

namespace WSChat.Shared.Tests.Managers
{
    public class SessionManagerTests
    {
        [Fact(DisplayName = "When create a SessionManager then SessionList should be empty")]
        public void WhenCreateASessionManagerThenSessionListShouldBeEmpty()
        {
            SessionManager manager = new();

            Assert.NotNull(manager);
            Assert.Empty(manager.SessionList);
        }

        [Fact(DisplayName = "When add a session to SessionManager then SessionList should have the added session")]
        public void WhenAddASessionToSessionManagerThenSessionListShouldHaveTheAddedSession()
        {
            SessionManager manager = new();
            Session session = new();

            manager.AddSession(session);

            Assert.NotNull(manager);
            Assert.NotEmpty(manager.SessionList);
            Assert.Single(manager.SessionList);
            Assert.Equal(session.Token, manager.SessionList.First().Token);
        }

        [Fact(DisplayName = "When remove a session from SessionManager then SessionList doesnt should have the deleted session")]
        public void WhenRemoveASessionFromSessionManagerThenSessionListDoesntShouldHaveTheDeletedSession()
        {
            SessionManager manager = new();
            Session session = new();

            manager.AddSession(session);

            manager.RemoveSession(session.Token);
            Assert.Empty(manager.SessionList);
        }
    }
}
