using Hangfire.Common;
using Lotto.Api.Hubs;
using Lotto.Services.RollService;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotto.Api.Jobs
{
    public class RollJob
    {
        private readonly IRollService _rollService;
        private readonly IHubContext<GameHub> _gameHubContext;

        public RollJob(IRollService rollService, IHubContext<GameHub> gameHubContext)
        {
            _rollService = rollService;
            _gameHubContext = gameHubContext;
        }

        public async Task Execute()
        {
            await _rollService.MakeRoll();
            await _gameHubContext.Clients.All.SendAsync("NewRollAvailable");
        }
    }
}
