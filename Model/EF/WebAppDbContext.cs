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

        public virtual DbSet<Credential> Credential { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DailyList> DailyList { get; set; }
        public virtual DbSet<DayOff> DayOff { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Taxi> Taxi { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<Violator> Violator { get; set; }
        public virtual DbSet<Role> Role { get; set; }

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

            modelBuilder.Entity<Taxi>()
                .Property(e => e.Commission)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Taxi>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Taxi>()
                .Property(e => e.Phone)
                .IsFixedLength();

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
