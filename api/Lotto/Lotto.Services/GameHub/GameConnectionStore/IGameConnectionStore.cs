using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.GameHub.GameConnectionStore
{
    public interface IGameConnectionStore
    {
        void Add(ConnectedUser user);
        void Remove(int userId, string connectionId);
        ConnectedUser Get(int userId);
    }
}
