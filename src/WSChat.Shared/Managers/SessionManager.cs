using System;
using System.Collections.Generic;
using System.Linq;
using WSChat.Shared.Base;
using WSChat.Shared.Domain;

namespace WSChat.Shared.Managers
{
    public class SessionManager : BaseManager<Session>
    {
        public IEnumerable<Session> SessionList => ListData;
        
        public void AddSession(Session session) => AddItem(session);

        public void RemoveSession(Guid token)
        {
            Session session = ListData.FirstOrDefault(r => r.Token == token);
            if (session == null)
                throw new Exception($"Session {token} not exists");

            RemoveItem(session);
        }

        protected override bool ItemExists(Session session) =>
            ListData.Any(r => r.Token == session.Token); 
    }
}
