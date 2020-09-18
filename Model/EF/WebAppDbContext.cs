namespace Model.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WebAppDbContext : DbContext
    {
        public WebAppDbContext()
            : base("name=WebAppDbContext")
        {
        }

        public virtual DbSet<Credential> Credentials { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DailyList> DailyLists { get; set; }
        public virtual DbSet<DayOff> DayOffs { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Taxi> Taxis { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<Violator> Violators { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Credential>()
                .Property(e => e.UserGroupID)
                .IsUnicode(false);

            modelBuilder.Entity<Credential>()
                .Property(e => e.RoleID)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CardID)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Customer>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Employee_Code)
                .IsFixedLength();

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Room)
                .IsFixedLength();

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Ticket)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Tip)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Code)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Voucher)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Total)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DayOff>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DayOff>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Employee>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.GroupID)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.DayOffs)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Taxi>()
                .Property(e => e.Commission)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Taxi>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Taxi>()
                .Property(e => e.Phone)
                .IsFixedLength();

            modelBuilder.Entity<Taxi>()
                .HasMany(e => e.DailyLists)
                .WithOptional(e => e.Taxi)
                .HasForeignKey(e => e.Taxi_ID);

            modelBuilder.Entity<UserGroup>()
                .Property(e => e.GroupID)
                .IsUnicode(false);

            modelBuilder.Entity<Violator>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<Violator>()
                .Property(e => e.Loan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Violator>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Violator>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.ID)
                .IsUnicode(false);
        }
    }
}
