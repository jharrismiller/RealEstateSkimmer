using Common;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class RealEstateContext : DbContext, IRealEstateContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb);Database=RealEstateAnalysis;Trusted_Connection=True;");
        }


    }
}
