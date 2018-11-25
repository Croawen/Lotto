using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.GameHub.GameConnectionHandler
{
    public interface IGameConnectionHandler
    {
        bool CanHandle(int? userIdFromQs);
        void HandleConnected(int userIdFromQs, string connectionId);
        void HandleDisconnected(int userId, string connectionId);
        ConnectedUser GetConnectedUser(int userId);
    }
}
