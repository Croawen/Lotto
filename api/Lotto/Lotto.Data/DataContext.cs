using Lotto.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lotto.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RollEntity> Rolls { get; set; }
        public DbSet<UserRollEntity> UserRolls { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRollEntity>()
                 .HasKey(a => new { a.UserId, a.RollId });
        }
    }
}
