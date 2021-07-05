using System;
using System.Collections.Generic;
using System.Linq;
using WSChat.Shared.Base;
using WSChat.Shared.Domain;

namespace WSChat.Shared.Managers
{
    public class UserManager : BaseManager<User>
    {
        public IEnumerable<User> UsersList => ListData;

        public void AddUser(string nickName) => AddItem(new User(nickName));

        public void RemoveUser(string nickName)
        {
            User user = GetUserByNickName(nickName);

            if (user == null)
                throw new Exception($"User {nickName} not exists");

            RemoveItem(user);
        }

        public User GetUser(User other)
        {
            User user = GetUserByNickName(other.NickName);

            if (user == null)
                throw new Exception($"User {other.NickName} not exists");

            return user;
        }

        protected override bool ItemExists(User item) =>
            ListData.Any(s => s.NickName == item.NickName);

        private User GetUserByNickName(string nickName) =>
            ListData.FirstOrDefault(u => u.NickName == nickName);
    }
}
