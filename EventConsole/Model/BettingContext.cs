
namespace EventConsole.Model
{
        using EventConsole.Model.Entity;
        using Microsoft.EntityFrameworkCore;

        public partial class BettingContext : DbContext
        {
                private string _connectionString;

                public DbSet<Pool> PoolSet { get; set; }
                public DbSet<Wage> WageSet { get; set; }
                public DbSet<Event> EventSet { get; set; }
                public DbSet<Wager> WagerSet { get; set; }
                public DbSet<Runner> RunnerSet { get; set; }

                public BettingContext(string connectionString)
                {
                        _connectionString = connectionString;
                }

                protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                {
                        optionsBuilder.UseSqlite(_connectionString);

                        base.OnConfiguring(optionsBuilder);
                }

                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                        #region Key

                        modelBuilder.Entity<Pool>()
                                .HasKey(u => u.Id);

                        modelBuilder.Entity<Wager>()
                                .HasKey(u => u.Id);

                        modelBuilder.Entity<Runner>()
                                .HasKey(u => u.Id);

                        modelBuilder.Entity<Event>()
                                .HasKey(u => new { u.PoolId, u.RunnerId });

                        modelBuilder.Entity<Wage>()
                                .HasKey(u => new { u.PoolId, u.RunnerId, u.WagerId });

                        #endregion

                        #region Index

                        modelBuilder.Entity<Wager>()
                                .HasIndex(u => u.Name);

                        modelBuilder.Entity<Runner>()
                                .HasIndex(u => u.Name)
                                .IsUnique();

                        #endregion

                        #region Foreign key

                        modelBuilder.Entity<Pool>()
                                .HasMany(u => u.Events)
                                .WithOne(u => u.Pool)
                                .HasForeignKey(u => u.PoolId)
                                .IsRequired();
                                
                        modelBuilder.Entity<Runner>()
                                .HasMany(u => u.Events)
                                .WithOne(u => u.Runner)
                                .HasForeignKey(u => u.RunnerId)
                                .IsRequired();

                        modelBuilder.Entity<Event>()
                                .HasMany(u => u.Wages)
                                .WithOne(u => u.Event)
                                .HasForeignKey(u => new { u.PoolId, u.RunnerId })
                                .IsRequired();

                        modelBuilder.Entity<Wager>()
                                .HasMany(u => u.Wages)
                                .WithOne(u => u.Wager)
                                .HasForeignKey(u => u.WagerId)
                                .IsRequired();

                        #endregion

                        base.OnModelCreating(modelBuilder);
                }
        }
}
