using SDJ3_3rd_tier.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace SDJ3_3rd_tier.DAL
{
    public class FactoryContext : DbContext
    {
        public FactoryContext() : base("mssqllocaldb")
        {
        }


        public DbSet<Car> Cars { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Pallet> Pallets { get; set; }
        public DbSet<Package> Packages { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}