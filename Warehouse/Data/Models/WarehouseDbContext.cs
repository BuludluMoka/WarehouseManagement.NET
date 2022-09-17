using Microsoft.EntityFrameworkCore;

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
        public DbSet<Ambar> Ambars { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ambar>().HasMany(p => p.Sender)
                .WithOne(x => x.Sender)
                .HasForeignKey(y => y.sender_id)
                .IsRequired(false);

            modelBuilder.Entity<Ambar>().HasMany(p => p.Receiver)
                .WithOne(x => x.Receiver)
                .HasForeignKey(y => y.receiver_id);

            //modelBuilder.Entity<Transacction>()
            //     .HasOne(x => x.AmbarSender)
            //     .WithMany(x => x.senderWarehouse)
            //     .HasForeignKey(x => x.sender_id)
            //     .OnDelete(DeleteBehavior.ClientSetNull);

            //modelBuilder.Entity<Transacction>()
            //   .HasOne(x => x.AmbarReceiver)
            //   .WithMany(x => x.receiverWarehouse)
            //   .HasForeignKey(x => x.receiver_id)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
        }

    }
}
