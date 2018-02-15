using Microsoft.EntityFrameworkCore;
using Data.Model;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Data
{
    public interface IRealEstateContext
    {
        DatabaseFacade Database { get; }
        DbSet<Property> Property { get; set; }
        DbSet<PropertyStatus> PropertyStatus { get; set; }
        DbSet<PropertyTags> PropertyTags { get; set; }
        DbSet<PropertyTag> PropertyTag { get; set; }
        DbSet<RentBits> RentBits { get; set; }
        DbSet<PropertyType> PropertyType { get; set; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
