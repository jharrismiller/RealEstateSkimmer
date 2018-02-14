using Data.Model;
using Microsoft.EntityFrameworkCore;
namespace Data
{
    public class RealEstateContext : DbContext, IRealEstateContext
    {
     
        public RealEstateContext(DbContextOptions<RealEstateContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<PropertyTags>()
                .HasKey(k => new { k.PropertyId, k.PropertyTagId });

            modelBuilder.Entity<PropertyTags>()
                .Property(b => b.CreatedOn)
                    .HasDefaultValueSql("getdate()");

        }
        public DbSet<Property> Property { get; set; }
        public DbSet<PropertyStatus> PropertyStatus { get; set; }
        public DbSet<PropertyTags> PropertyTags { get; set; }
        public DbSet<PropertyTag> PropertyTag { get; set; }
        public DbSet<PropertyType> PropertyType { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
            
        //    optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        //}


    }
}
