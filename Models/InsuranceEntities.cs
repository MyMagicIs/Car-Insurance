
using Microsoft.EntityFrameworkCore;

namespace CarInsurance.Models
{
    public class InsuranceEntities : DbContext
    {
        public InsuranceEntities(DbContextOptions<InsuranceEntities> options)
            : base(options)
        {
        }

        public DbSet<Insuree> Insurees { get; set; }
    }
}

