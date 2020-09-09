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

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<DayOff> DayOff { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Taxi> Taxi { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<List>()
                .Property(e => e.Room)
                .IsFixedLength();

            modelBuilder.Entity<List>()
                .Property(e => e.Fare)
                .HasPrecision(18, 0);

            modelBuilder.Entity<List>()
                .Property(e => e.Tip)
                .HasPrecision(18, 0);

            modelBuilder.Entity<List>()
                .Property(e => e.Code)
                .IsFixedLength();

            modelBuilder.Entity<List>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<List>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Profile>()
                .Property(e => e.ModifiedBy)
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

            modelBuilder.Entity<User>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);
        }
    }
}
