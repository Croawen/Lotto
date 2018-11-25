using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Services.GameHub
{
    public class ConnectedUser
    {
        public bool IsNull { get; set; }
        public int UserId { get; set; }
        public string ConnectionId { get; set; }

        public ConnectedUser()
        {
            IsNull = true;
        }

        public ConnectedUser(string connectionId, int userId)
        {
            IsNull = false;
            UserId = userId;
            ConnectionId = connectionId;
        }
    }
}
