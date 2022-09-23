using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models.Common;

namespace Warehouse.Data.Models
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext()
        {

        }

        public WarehouseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Anbar> Ambars { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Anbar>().HasMany(p => p.Sender)
                .WithOne(x => x.Sender)
                .HasForeignKey(y => y.sender_id)
                .IsRequired(false);

            modelBuilder.Entity<Anbar>().HasMany(p => p.Receiver)
                .WithOne(x => x.Receiver)
                .HasForeignKey(y => y.receiver_id);



        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedDate = utcNow;

                            entry.Property("CreatedOn").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.CreatedDate = utcNow;
                            trackable.UpdatedDate = utcNow;
                            break;
                    }
                }
            }
        }

    }
}
