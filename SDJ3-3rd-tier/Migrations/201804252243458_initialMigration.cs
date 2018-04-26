namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Car",
                c => new
                    {
                        VIN = c.String(nullable: false, maxLength: 128),
                        Model = c.String(nullable: false),
                        Weight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.VIN);
            
            CreateTable(
                "dbo.Part",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Weight = c.Double(nullable: false),
                        Car_VIN = c.String(maxLength: 128),
                        Package_Id = c.Int(),
                        Pallet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Car", t => t.Car_VIN)
                .ForeignKey("dbo.Package", t => t.Package_Id)
                .ForeignKey("dbo.Pallet", t => t.Pallet_Id)
                .Index(t => t.Car_VIN)
                .Index(t => t.Package_Id)
                .Index(t => t.Pallet_Id);
            
            CreateTable(
                "dbo.Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Repacking = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pallet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Part", "Pallet_Id", "dbo.Pallet");
            DropForeignKey("dbo.Part", "Package_Id", "dbo.Package");
            DropForeignKey("dbo.Part", "Car_VIN", "dbo.Car");
            DropIndex("dbo.Part", new[] { "Pallet_Id" });
            DropIndex("dbo.Part", new[] { "Package_Id" });
            DropIndex("dbo.Part", new[] { "Car_VIN" });
            DropTable("dbo.Pallet");
            DropTable("dbo.Package");
            DropTable("dbo.Part");
            DropTable("dbo.Car");
        }
    }
}
