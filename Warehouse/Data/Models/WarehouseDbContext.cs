using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Warehouse.Data.Models.Common;
using Warehouse.Data.Models.Common.Authentication;

namespace Warehouse.Data.Models
{
    public class WarehouseDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public WarehouseDbContext()
        {

        }

        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Anbar> Anbars { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Anbar>().HasKey(a => a.Id);
            builder.Entity<Anbar>()
                .HasMany(p => p.Sender)
                .WithOne(x => x.Sender)
                .HasForeignKey(y => y.sender_id)
                .IsRequired(false);

            builder.Entity<Anbar>()
                .HasMany(p => p.Receiver)
                .WithOne(x => x.Receiver)
                .HasForeignKey(y => y.receiver_id);

            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();
            builder.Ignore<IdentityUserClaim<string>>();

            builder.Entity<AppUser>()
                //.Ignore(n => n.NormalizedEmail)
                //.Ignore(n => n.NormalizedUserName)
                .Ignore(e => e.EmailConfirmed)
                .Ignore(c => c.ConcurrencyStamp)
                .Ignore(p => p.PhoneNumberConfirmed)
                .Ignore(t => t.TwoFactorEnabled)
                .Ignore(l => l.LockoutEnabled)
                .Ignore(l => l.LockoutEnd)
                .Ignore(a => a.AccessFailedCount);

            builder.Entity<AppUser>()
                .HasQueryFilter(x => x.Status);

            //builder.Entity<AppUser>()
            //.HasQueryFilter(x => x.Id != "a18be9c0-aa65-4af8-bd17-00bd9344e575");

            //builder.Entity<AppUser>()
            //    .HasOne(x => x.Anbar)
            //    .WithMany(x => x.Users)
            //    .HasForeignKey<AppUser>(x => x);


            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string ROLE_ID = ADMIN_ID;

            builder.Entity<AppRole>().HasData(new AppRole { Id = ROLE_ID, Name = "Admin", NormalizedName = "Admin".ToUpper() });

            var hasher = new PasswordHasher<AppUser>();
            builder.Entity<AppUser>().HasData(new AppUser
            {
                Id = ADMIN_ID,
                AnbarId = 1,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Status = true,
                Email = "buludlumoka@gmail.com",
                NormalizedEmail = "BULUDLUMOKA@GMAIL.COM",
                PasswordHash = hasher.HashPassword(null, "admin123"),
                Address = "WarehouseHome",
                SecurityStamp = string.Empty,
                PhoneNumber = "055557623415"
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            //////////////////////////////Seed Data

            builder.Entity<Anbar>().HasData(new Anbar[]
            {
                new Anbar
                {
                    Id = 1,
                    Name = "Yasamal",
                    Phone = "55623415",
                    Place = "Baki,Yasamal,Dalan4",
                    CreatedDate = DateTime.Now

                },
                new Anbar
                {
                    Id = 2,
                    Name = "Seki",
                    Phone = "55623415",
                    Place = "Seki,Xan Sarayi,Dalan4",
                    CreatedDate = DateTime.Now

                },
                new Anbar
                {
                    Id = 3,
                    Name = "Qebele",
                    Phone = "55623415",
                    Place = "Qebele,Dalan4",
                    CreatedDate = DateTime.Now

                },
                new Anbar
                {
                    Id = 4,
                    Name = "Nerimanov",
                    Phone = "55623415",
                    Place = "Baki,Nerimanov,Dalan4",
                    CreatedDate = DateTime.Now

                }
            });

            builder.Entity<Category>().HasData(new Category[]
            {
              new Category
              {
                  Id = 1,
                  Name = "Electronics",
                  CreatedDate = DateTime.Now
              },
              new Category
              {
                  Id = 2,
                  Name = "Medicine",
                  CreatedDate = DateTime.Now

              },
              new Category //3
              {
                  Id = 3,
                  Name = "Laptops",
                  ParentId = 1,
                  CreatedDate = DateTime.Now
              },
              new Category//4
              {
                  Id = 4,
                  Name = "Mouse & Keyboards",
                  ParentId = 1,
                  CreatedDate = DateTime.Now
              },
              new Category//5
              {
                  Id = 5,
                  Name = "Computer Components",
                  ParentId = 1,
                  CreatedDate = DateTime.Now
              },
              new Category//6
              {
                  Id = 6,
                  Name = "Accessories",
                  ParentId = 1,
                  CreatedDate = DateTime.Now
              },
              new Category//7
              {
                  Id = 7,
                  Name = "Electronic Medical Equipment",
                  ParentId = 2,
                  CreatedDate = DateTime.Now
              },
              new Category//8
              {
                  Id = 8,
                  Name = "Diagnostic Medical Equipment",
                  ParentId = 2,
                  CreatedDate = DateTime.Now
              },
              new Category//9
              {
                  Id = 9,
                  Name = "Durable Medical Equipment",
                  ParentId = 2,
                  CreatedDate = DateTime.Now
              }
            });

            builder.Entity<Product>().HasData(new Product[]
            {
                new Product
                {
                    Id = 1,
                    Name = "Xiaomi RedmiBook Pro 15 Laptop 15.6 Inch 3.2K 90Hz Super Retina Screen AMD R5 5600H 16GB 512GB AMD Radeon Graphics Card Notebook",
                    buyPrice = 1554.64F,
                    sellPrice = 1660.55F,
                    CategoryId = 3,
                    CreatedDate = DateTime.Now



                },
                new Product
                {
                    Id = 2,
                    Name = "Dere V9 MAX Laptop 15.6',Intel Core i7-1165G7, 16GB RAM + 1TB SSD, 2.5K IPS Screen, Computer Office Windows 11 Notebook",
                    buyPrice = 1111.34F,
                    sellPrice = 1300.56F,
                    CategoryId = 3,
                    CreatedDate = DateTime.Now

                },
                new Product
                {
                    Id = 3,
                    Name = "AMD RX 580 8G Computer Graphics Card,RX580 8G For GDDR5 GPU mining Video Card",
                    buyPrice = 185.50F,
                    sellPrice = 200,
                    CategoryId = 5,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 4,
                    Name = "AMD Ryzen 9 5900X R9 5900X 3.7 GHz Twelve-Core 24-Thread CPU Processor",
                    buyPrice = 777.60F,
                    sellPrice = 956.78F,
                    CategoryId = 5,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 5,
                    Name = "Domiso Mutil-use Laptop Sleeve With Handle For 14' 15.6' 17' Inch Notebook Computer Bag",
                    buyPrice = 61,
                    sellPrice = 74.60F,
                    CategoryId = 6,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 6,
                    Name = "Fan For Computer PC Laptop Notebook",
                    buyPrice = 3,
                    sellPrice = 3.60F,
                    CategoryId = 6,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 7,
                    Name = "Heart Rate Monitors",
                    buyPrice = 800.60F,
                    sellPrice = 996.78F,
                    CategoryId = 7,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 8,
                    Name = "Blood Pressure Monitors",
                    buyPrice = 14000.60F,
                    sellPrice = 15560.78F,
                    CategoryId = 7,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 9,
                    Name = "Ultrasound",
                    buyPrice = 23000.60F,
                    sellPrice = 35000.78F,
                    CategoryId = 7,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 10,
                    Name = "MRI Scans",
                    buyPrice = 12000.60F,
                    sellPrice = 18000.78F,
                    CategoryId = 8,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 11,
                    Name = "X-Rays",
                    buyPrice = 4600.60F,
                    sellPrice = 5000.78F,
                    CategoryId = 8,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 12,
                    Name = "Hospital beds",
                    buyPrice = 700.60F,
                    sellPrice = 956.78F,
                    CategoryId = 9,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 13,
                    Name = "Ventilators",
                    buyPrice = 80.60F,
                    sellPrice = 95.78F,
                    CategoryId = 9,
                    CreatedDate = DateTime.Now
                }
            });





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

                            entry.Property("CreatedDate").IsModified = false;
                            break;

                        case EntityState.Added:
                            trackable.CreatedDate = utcNow;
                            trackable.UpdatedDate = utcNow;
                            break;
                    }
                }
            }
        }

    }
}
