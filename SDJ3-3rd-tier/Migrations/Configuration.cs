namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
            context.Cars.RemoveRange(context.Cars);
            context.Parts.RemoveRange(context.Parts);
            context.Pallets.RemoveRange(context.Pallets);
            context.Packages.RemoveRange(context.Packages);

            context.SaveChanges();

            for (int i = 1; i < 21; i++)
            {
                context.Parts.Add(new Models.Part
                {
                    Name = "Name" + i,
                    Weight = 32,
                    Car = new Models.Car
                    {
                        VIN = "VIN" + i,
                        Weight = 10 + 1,
                        Model = "Srot" + 2 % i,
                    }
                });


                context.SaveChanges();
            }

                //for (int i = 1; i < 21; i++)
                //{
                //    context.Cars.Add(new Models.Car
                //    {
                //        VIN = "VIN" + i,
                //        Weight = 10 + 1,
                //        Model = "Srot" + 2 % i,
                //        Parts = new Collection<Models.Part>
                //        {
                //            new Models.Part
                //            {
                //                Id = i,
                //                Name = "Name" + i,
                //                Weight = 32,
                //                Package = new Models.Package
                //                    {
                //                        Id = i,
                //                        Repacking = 2%i == 1,
                //                        Content = "Srot" + 2 % i,
                //                    },
                //                Pallet = new Models.Pallet
                //                {
                //                    Id = i,
                //                    MaximumCapacity = 30+i,
                //                }
                //            }
                //        }

                //    });
                //}

                //context.SaveChanges();
            }
    }
}
