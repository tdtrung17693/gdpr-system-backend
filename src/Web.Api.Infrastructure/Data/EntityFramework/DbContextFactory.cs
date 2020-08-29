using Microsoft.EntityFrameworkCore;
 

namespace Web.Api.Infrastructure.Data.EntityFramework
{
    public class DbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
    {
        protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
        {
            return new ApplicationDbContext(options);
        }
    }

    //public class DbContextFactory : DesignTimeDbContextFactoryBase<GdprContext>
    //{
    //    protected override GpdrContext CreateNewInstance(DbContextOptions<GdprContext> options)
    //    {
    //        return new AppliContext(options);
    //    }
    //}
}
