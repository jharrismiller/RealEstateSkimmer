using Data.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Data
{
    public class RealEstateContext : DbContext, IRealEstateContext
    {

        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyTags>()
                .HasKey(k => new { k.PropertyId, k.PropertyTagId });

            modelBuilder.Entity<PropertyTags>()
                .Property(b => b.CreatedOn)
                    .HasDefaultValueSql("getdate()");

           
            modelBuilder.Entity<Property>().Property(r => r.Beds)
            .HasColumnType("decimal(3,1)");

        }
        public DbSet<Property> Property { get; set; }
        public DbSet<PropertyStatus> PropertyStatus { get; set; }
        public DbSet<PropertyTags> PropertyTags { get; set; }
        public DbSet<PropertyTag> PropertyTag { get; set; }
        public DbSet<PropertyType> PropertyType { get; set; }
        public string MLS { get; set; }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=RealEstateAnalysis;Integrated Security=True");
        //}


    }
}
