using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.GameHub.GameConnectionStore
{
    public class GameConnectionStore : IGameConnectionStore
    {
        private static readonly ConcurrentDictionary<int, ConnectedUser> _store =
            new ConcurrentDictionary<int, ConnectedUser>();

        public void Add(ConnectedUser user)
        {
            if (InStore(user.UserId))
                UpdateStore(user);
            else
                AddNew(user);
        }

        public ConnectedUser Get(int userId)
        {
            if (!InStore(userId))
                return null;

            return _store[userId];
        }

        public void Remove(int userId, string connectionId)
        {
            if (!InStore(userId))
                return;

            var user = Get(userId);

            if (user.ConnectionId == connectionId)
                _store.TryRemove(userId, out var val);
        }

        private bool InStore(int userId) => _store.ContainsKey(userId);

        private static void AddNew(ConnectedUser user)
        {
            _store.AddOrUpdate(user.UserId, user, (key, old) => user);
        }

        private void UpdateStore(ConnectedUser user)
        {
            _store.AddOrUpdate(user.UserId, user, (key, old) => new ConnectedUser(user.ConnectionId, user.UserId));
        }
    }
}
