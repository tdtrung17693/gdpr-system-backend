using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Infrastructure.Data.EntityFramework
{
    public class GdprContext : DbContext
    {
        public GdprContext(DbContextOptions<GdprContext> options) : base(options)
        {

        }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        public DbSet<Request> Requests { get; set; }
    }
}