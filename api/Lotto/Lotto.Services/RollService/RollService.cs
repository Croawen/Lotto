using Lotto.Common.Exceptions;
using Lotto.Common.Models;
using Lotto.DAL.TransactionRepository;
using Lotto.Data.Entities;
using Lotto.Services.RollService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto.Services.RollService
{
    public class RollService : IRollService
    {
        private readonly ITransactionRepository _transactionRepository;

        public RollService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        private GeolocationModel GenerateRandomCircle()
        {
            var rnd = new Random();

            return new GeolocationModel {
                Lat = 49 + (rnd.NextDouble() * (54 - 49)),
                Lng = 14 + (rnd.NextDouble() * (22 - 14))
            };
        }

        private double DistanceBetween(GeolocationModel locationOne, GeolocationModel locationTwo)
        {
            var R = 6371e3; //earth’s radius (mean radius = 6,371km)
            var rlat1 = Math.PI * locationOne.Lat / 180;
            var rlat2 = Math.PI * locationTwo.Lat / 180;
            var rtheta = Math.PI * (locationTwo.Lng - locationOne.Lng) / 180;

            var a = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);

            var c = Math.Acos(a);

            return R * c;
        }

        public async Task PickWinner(int rollId, GeolocationModel winningPoint)
        {
            var winningRoll = await _transactionRepository
                .GetAll<UserRollEntity>(r => r.RollId == rollId)
                .OrderBy(u => DistanceBetween(new GeolocationModel { Lng = u.Lng, Lat = u.Lat }, winningPoint))
                .FirstOrDefaultAsync();

            if (winningRoll != null)
            {
                winningRoll.HasWon = true;

                if (_transactionRepository.Update(winningRoll) == 0)
                    throw new TransactionRepositoryException("Failed to update winning user roll");
            }
        }

        public async Task MakeRoll()
        {
            var winningPoint = GenerateRandomCircle();

            _transactionRepository.Begin();

            var oldRoll = await FinishRoll(winningPoint);
            if (oldRoll != null)
                await PickWinner(oldRoll.Id, winningPoint);

            _transactionRepository.End();

            _transactionRepository.Begin();

            await CreateNewRoll(oldRoll);

            _transactionRepository.End();
        }

        public async Task<RollEntity> FinishRoll(GeolocationModel winningPoint)
        {
            var roll = await _transactionRepository.GetAll<RollEntity>(r => !r.IsFinished).SingleOrDefaultAsync();

            if (roll != null)
            {
                roll.IsFinished = true;
                roll.WinningLat = winningPoint.Lat;
                roll.WinningLng = winningPoint.Lng;

                if (_transactionRepository.Update(roll) == 0)
                    throw new TransactionRepositoryException("Failed to update old roll");

                return roll;
            }

            return null;
        }

        public async Task CreateNewRoll(RollEntity oldRoll)
        {
            var nextRollDate = DateTimeOffset.UtcNow.AddMinutes(1);

            //if (oldRoll != null)
            //{
            //    nextRollDate = oldRoll.RollDate.AddMinutes(1);
            //}

            var newRoll = new RollEntity
            {
                IsFinished = false,
                RollDate = nextRollDate,
                WinningLat = null,
                WinningLng = null
            };

            if (oldRoll != null)
                newRoll.PreviousRollId = oldRoll.Id;

            if (_transactionRepository.Add(newRoll) == 0)
                throw new TransactionRepositoryException("Failed to add new roll");
        }

        public async Task<bool?> CheckRollWinner(int? userId, int rollId)
        {
            if (userId == null)
                return null;

            var userRoll = await _transactionRepository.GetAll<UserRollEntity>(r => r.UserId == userId && r.RollId == rollId).SingleOrDefaultAsync();

            if (userRoll == null)
                return null;

            return userRoll.HasWon;
        }

        public async Task<GetNextRollResponseModel> GetNextRollData(int? userId)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var rollEntity = await _transactionRepository.GetAll<RollEntity>(r => !r.IsFinished).Include(r => r.Participants).SingleOrDefaultAsync();
            var rollData = new GetNextRollResponseModel
            {
                RollId = rollEntity.Id,
                RollDate = rollEntity.RollDate.ToUnixTimeSeconds() - now <= 0 ? 0 : rollEntity.RollDate.ToUnixTimeSeconds() - now,
                ParticipantsCount = rollEntity.Participants.Count,
            };

            return rollData;
        }

        public async Task BuyTicket(BuyTicketModel model, int userId)
        {
            var roll = await _transactionRepository.GetAll<RollEntity>(r => r.Id == model.RollId).SingleOrDefaultAsync();

            if (roll == null || (roll != null && roll.IsFinished == true))
                return;

            var userRoll = await _transactionRepository.GetAll<UserRollEntity>(r => r.RollId == model.RollId && r.UserId == userId).SingleOrDefaultAsync();

            if (userRoll != null)
                return;

            userRoll = new UserRollEntity
            {
                CreatedAt = DateTimeOffset.UtcNow,
                Lat = model.Latitude,
                Lng = model.Longitude,
                RollId = model.RollId,
                UserId = userId
            };

            if (_transactionRepository.Add(userRoll) == 0)
                throw new TransactionRepositoryException("Failed to add new user roll");
        }
    }
}
