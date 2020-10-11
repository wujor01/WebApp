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
        public virtual DbSet<DailyEmployee> DailyEmployees { get; set; }
        public virtual DbSet<DailyList> DailyLists { get; set; }
        public virtual DbSet<DayOff> DayOffs { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<ReExType> ReExTypes { get; set; }
        public virtual DbSet<RevenueExpenditure> RevenueExpenditures { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<StatisticDepartment> StatisticDepartments { get; set; }
        public virtual DbSet<StatisticEmployee> StatisticEmployees { get; set; }
        public virtual DbSet<StatisticTicket> StatisticTickets { get; set; }
        public virtual DbSet<Taxi> Taxis { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<Violator> Violators { get; set; }
        public virtual DbSet<ViolatorKTV> ViolatorKTVs { get; set; }
        public virtual DbSet<ViolatorType> ViolatorTypes { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

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

            modelBuilder.Entity<DailyEmployee>()
                .Property(e => e.Tip)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyEmployee>()
                .Property(e => e.Tour)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyEmployee>()
                .Property(e => e.Clean)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DailyList>()
                .Property(e => e.PricewithVoucher)
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

            modelBuilder.Entity<DailyList>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.DailyList)
                .HasForeignKey(e => e.DailyList_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DayOff>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<DayOff>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Tour)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Department>()
                .Property(e => e.Clean)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.DailyLists)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Department_ID);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Department_ID);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.RevenueExpenditures)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Department_ID);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Rooms)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Department_ID);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.StatisticDepartments)
                .WithOptional(e => e.Department)
                .HasForeignKey(e => e.Deparment_ID);

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
                .HasMany(e => e.DailyEmployees)
                .WithRequired(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.DayOffs)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.StatisticEmployees)
                .WithRequired(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Violators)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.ViolatorKTVs)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Amount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OrderDetail>()
                .HasMany(e => e.DailyEmployees)
                .WithRequired(e => e.OrderDetail)
                .HasForeignKey(e => e.Order_ID)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<Role>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Room)
                .HasForeignKey(e => e.Room_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StatisticDepartment>()
                .Property(e => e.TotalListinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticDepartment>()
                .Property(e => e.TicketPriceinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticDepartment>()
                .Property(e => e.TipinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticDepartment>()
                .Property(e => e.RevenueinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticDepartment>()
                .Property(e => e.ExpenditureinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticDepartment>()
                .Property(e => e.RevenueinDatefromEmployee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticEmployee>()
                .Property(e => e.TipinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticEmployee>()
                .Property(e => e.CleaninDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticEmployee>()
                .Property(e => e.RevenueinDatefromEmployee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticEmployee>()
                .Property(e => e.TourinDate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<StatisticTicket>()
                .Property(e => e.TicketPriceinDate)
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
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Ticket)
                .HasForeignKey(e => e.Ticket_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>()
                .HasMany(e => e.StatisticTickets)
                .WithRequired(e => e.Ticket)
                .HasForeignKey(e => e.Ticket_ID)
                .WillCascadeOnDelete(false);

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
                .Property(e => e.TipinDate)
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

            modelBuilder.Entity<Voucher>()
                .Property(e => e.DiscountPercent)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .HasMany(e => e.DailyLists)
                .WithOptional(e => e.Voucher)
                .HasForeignKey(e => e.Voucher_ID);
        }
    }
}
