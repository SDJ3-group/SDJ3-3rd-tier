namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SDJ3_3rd_tier.DAL.FactoryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SDJ3_3rd_tier.DAL.FactoryContext context)
        {
            Models.Car car = new Models.Car
            {
                VIN = "sadsada",
                Weight = 12,
                Model = "Srot"
            };

            context.Cars.Add(car);

            context.SaveChanges();
        }
    }
}
