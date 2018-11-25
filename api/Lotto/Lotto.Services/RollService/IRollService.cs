using Lotto.Services.RollService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lotto.Services.RollService
{
    public interface IRollService
    {
        Task MakeRoll();
        Task<GetNextRollResponseModel> GetNextRollData(int? userId);
        Task BuyTicket(BuyTicketModel model, int userId);
        Task<bool?> CheckRollWinner(int? userId, int rollId);
    }
}
