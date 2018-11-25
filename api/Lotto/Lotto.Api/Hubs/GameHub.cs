using Lotto.Services.GameHub.GameConnectionHandler;
using Lotto.Services.RollService;
using Lotto.Services.RollService.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotto.Api.Hubs
{
    public class GameHub : Hub
    {
        private readonly IGameConnectionHandler _gameConnectionHandler;
        private readonly IRollService _rollService;

        public GameHub(IGameConnectionHandler gameConnectionHandler, IRollService rollService)
        {
            _gameConnectionHandler = gameConnectionHandler;
            _rollService = rollService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();

            if (_gameConnectionHandler.CanHandle(userId))
                _gameConnectionHandler.HandleConnected(userId.Value, Context.ConnectionId);

            await base.OnConnectedAsync();

            //await InvokeNextRollData();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetUserId();

            if (_gameConnectionHandler.CanHandle(userId))
                _gameConnectionHandler.HandleDisconnected(userId.Value, Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        private int? GetUserId()
        {
            var query = Context.GetHttpContext().Request.Query;
            int? id = null;

            if (query.ContainsKey("userId"))
                id = int.Parse(query["userId"]);

            return id;
        }

        public async Task InvokePing(int userId)
        {
            var connectedUser = _gameConnectionHandler.GetConnectedUser(userId);

            if (!connectedUser.IsNull)
                await Clients.Client(connectedUser.ConnectionId).SendAsync("Ping");
        }

        public async Task OrderPing()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("Pong");
        }

        public async Task InvokeNextRollData()
        {
            var userId = GetUserId();
            
            var nextRoll = await _rollService.GetNextRollData(userId);

            if (nextRoll != null)
                await Clients.All.SendAsync("NextRollData", nextRoll);
        }

        public async Task InvokeHasWonRoll(int rollId)
        {
            var userId = GetUserId();

            var res = await _rollService.CheckRollWinner(userId, rollId);

            await Clients.Caller.SendAsync("HasWonRoll", res);
        }

        public async Task InvokeBuyTicket(BuyTicketModel model)
        {
            var userId = GetUserId();

            if (userId != null) {
                await _rollService.BuyTicket(model, userId.Value);
                await InvokeNextRollData();
            }

        }
    }
}
