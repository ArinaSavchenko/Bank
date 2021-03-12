using Bank.Datalayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Datalayer.Context
{
    public class BankContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<TypeOfAccount> TypesOfAccounts { get; set; }
        public DbSet<Office> Offices { get; set; }
        
        public BankContext(DbContextOptions<BankContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne<Client>(a => a.Client)
                .WithMany(c => c.Accounts)
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<Account>()
                .HasOne<TypeOfAccount>(a => a.TypeOfAccount)
                .WithMany(ta => ta.Accounts)
                .HasForeignKey(a => a.AccountTypeId);

            modelBuilder.Entity<Worker>()
                .HasOne<Office>(w => w.Office)
                .WithMany(o => o.Workers)
                .HasForeignKey(w => w.OfficeId);
            
            modelBuilder.Entity<Contract>()
                .HasOne<Worker>(c => c.Worker)
                .WithMany(w => w.Contracts)
                .HasForeignKey(c => c.WorkerId);

            modelBuilder.Entity<User>()
                .HasOne<Client>(u => u.Client)
                .WithOne(c => c.User)
                .HasForeignKey<Client>(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasOne<Worker>(u => u.Worker)
                .WithOne(w => w.User)
                .HasForeignKey<Worker>(w => w.UserId);
            
            modelBuilder.Entity<Account>()
                .HasOne<Contract>(a => a.Contract)
                .WithOne(c => c.Account)
                .HasForeignKey<Contract>(c => c.AccountId);
            
            modelBuilder.Entity<Account>()
                .HasOne<Contract>(a => a.Contract)
                .WithOne(c => c.Account)
                .HasForeignKey<Contract>(c => c.AccountId);
        }
    }
}