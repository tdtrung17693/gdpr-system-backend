using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Infrastructure.Data.EntityFramework
{
    public class GdprContext: DbContext
    {
        public GdprContext(DbContextOptions<GdprContext> opt) : base(opt)
        {

        }
        //public DbSet<User> User { get; set; }
        public DbSet<Server> Server { get; set; }
    }
}
