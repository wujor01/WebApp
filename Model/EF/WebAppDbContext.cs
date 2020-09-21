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
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ReExType> ReExTypes { get; set; }
        public virtual DbSet<RevenueExpenditure> RevenueExpenditures { get; set; }
        public virtual DbSet<Taxi> Taxis { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<Violator> Violators { get; set; }
        public virtual DbSet<ViolatorKTV> ViolatorKTVs { get; set; }
        public virtual DbSet<ViolatorType> ViolatorTypes { get; set; }
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

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.DailyLists)
                .WithOptional(e => e.Customer)
                .HasForeignKey(e => e.Customer_ID);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Room)
                .IsFixedLength();

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Tip)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Code)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.Discount)
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

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Department_ID);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Tickets)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Department_ID);

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
                .HasMany(e => e.DailyLists)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.DayOffs)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Violators)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.ViolatorKTVs)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<ReExType>()
                .HasMany(e => e.RevenueExpenditures)
                .WithRequired(e => e.ReExType)
                .HasForeignKey(e => e.Type_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RevenueExpenditure>()
                .Property(e => e.Money)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RevenueExpenditure>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<RevenueExpenditure>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

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

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.DailyLists)
                .WithOptional(e => e.Ticket)
                .HasForeignKey(e => e.Ticket_ID);

            modelBuilder.Entity<UserGroup>()
                .Property(e => e.GroupID)
                .IsUnicode(false);

            modelBuilder.Entity<Violator>()
                .Property(e => e.Loan)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Violator>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Violator>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ViolatorKTV>()
                .Property(e => e.Tour)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ViolatorKTV>()
                .Property(e => e.Fruit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ViolatorKTV>()
                .Property(e => e.Elevator)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ViolatorKTV>()
                .Property(e => e.Substitution)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ViolatorKTV>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ViolatorKTV>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<ViolatorType>()
                .HasMany(e => e.Violators)
                .WithRequired(e => e.ViolatorType)
                .HasForeignKey(e => e.Type_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.ID)
                .IsUnicode(false);
        }
    }
}
