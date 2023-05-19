using HistrixAPI.Enums;
using HistrixAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HistrixAPI.DB
{
    public class DataContext : DbContext
    {
        public DbSet<Timeframe> Timeframes { get; set; }
        public DbSet<CryptoPair> CryptoPairs { get; set; }
        public DbSet<CandleEntity> Candles { get; set; }
        public DbSet<Bot> Bots { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GridStrategy>();
            modelBuilder.Entity<SMAStrategy>();

            modelBuilder.Entity<CandleEntity>()
                .HasOne(c => c.Timeframe)
                .WithMany(t => t.Candles)
                .HasForeignKey(c => c.TimeframeId);

            modelBuilder.Entity<CandleEntity>()
                .HasOne(c => c.CryptoPair)
                .WithMany(p => p.Candles)
                .HasForeignKey(c => c.CryptoPairId);

            modelBuilder.Entity<Bot>()
                .HasOne(b => b.CryptoPair)
                .WithMany()
                .HasForeignKey(b => b.CryptoPairId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bot>()
                .HasOne(b => b.Timeframe)
                .WithMany()
                .HasForeignKey(b => b.TimeframeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bot>()
                .HasOne(b => b.Strategy)
                .WithMany()
                .HasForeignKey(b => b.StrategyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bot>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Bot)
                .WithMany()
                .HasForeignKey(p => p.BotId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
