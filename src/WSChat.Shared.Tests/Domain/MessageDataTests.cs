using WSChat.Shared.Domain;
using Xunit;

namespace WSChat.Shared.Tests.Domain
{
    public class MessageDataTests
    {
        [Fact(DisplayName = "When creating a Message this message should have a valid Type")]
        public void WhenCreatingAMessageWithTypeThisMessageShouldHaveAValidType()
        {
            MessageData msg = new(Enums.MessageType.SendMessage);

            Assert.NotNull(msg);
            Assert.Equal(Enums.MessageType.SendMessage, msg.Type);
        }

        [Fact(DisplayName = "When setting the data of Message this message should have a valide Data")]
        public void WhenSettingTheDataOfMessageThisMessageShouldHaveAValideData()
        {
            MessageData msg = new(Enums.MessageType.SendMessage);
            msg.Data = "DATA";
            Assert.NotNull(msg);
            Assert.Equal("DATA", msg.Data);
        }
    }
}
