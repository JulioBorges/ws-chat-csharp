using System;

namespace WSChat.Shared.Domain
{
    public class User
    {
        public Guid Id { get; private set; }
        public string NickName { get; private set; }

        public User(string name)
        {
            NickName = name;
        }
    }
}
