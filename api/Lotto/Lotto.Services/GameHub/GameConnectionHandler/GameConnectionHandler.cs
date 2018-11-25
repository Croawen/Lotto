using Lotto.Services.GameHub.GameConnectionStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.GameHub.GameConnectionHandler
{
    public class GameConnectionHandler : IGameConnectionHandler
    {
        private readonly IGameConnectionStore _store;

        public GameConnectionHandler(IGameConnectionStore store)
        {
            _store = store;
        }

        public bool CanHandle(int? userIdFromQs)
        {
            return userIdFromQs.HasValue;
        }

        public ConnectedUser GetConnectedUser(int userId)
        {
            return _store.Get(userId);
        }

        public void HandleConnected(int agentIdFromQs, string connectionId)
        {
            _store.Add(new ConnectedUser(connectionId, agentIdFromQs));
        }

        public void HandleDisconnected(int userId, string connectionId)
        {
            _store.Remove(userId, connectionId);
        }
    }
}
