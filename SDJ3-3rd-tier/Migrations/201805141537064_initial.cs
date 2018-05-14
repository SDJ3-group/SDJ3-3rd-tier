namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
                        Name = c.String(nullable: false),
                        Weight = c.Double(nullable: false),
                        PalletId = c.Int(nullable: false),
                        PreviusPalletId = c.Int(),
                        PackageId = c.Int(nullable: false),
                        Car_VIN = c.String(nullable: false, maxLength: 128),
                        Pallet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Car", t => t.Car_VIN, cascadeDelete: true)
                .ForeignKey("dbo.Package", t => t.PackageId, cascadeDelete: true)
                .ForeignKey("dbo.Pallet", t => t.Pallet_Id)
                .ForeignKey("dbo.Pallet", t => t.PalletId, cascadeDelete: true)
                .ForeignKey("dbo.Pallet", t => t.PreviusPalletId)
                .Index(t => t.PalletId)
                .Index(t => t.PreviusPalletId)
                .Index(t => t.PackageId)
                .Index(t => t.Car_VIN)
                .Index(t => t.Pallet_Id);
            
            CreateTable(
                "dbo.Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Repacking = c.Boolean(nullable: false),
                        Content = c.String(nullable: false),
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
            DropForeignKey("dbo.Part", "PreviusPalletId", "dbo.Pallet");
            DropForeignKey("dbo.Part", "PalletId", "dbo.Pallet");
            DropForeignKey("dbo.Part", "Pallet_Id", "dbo.Pallet");
            DropForeignKey("dbo.Part", "PackageId", "dbo.Package");
            DropForeignKey("dbo.Part", "Car_VIN", "dbo.Car");
            DropIndex("dbo.Part", new[] { "Pallet_Id" });
            DropIndex("dbo.Part", new[] { "Car_VIN" });
            DropIndex("dbo.Part", new[] { "PackageId" });
            DropIndex("dbo.Part", new[] { "PreviusPalletId" });
            DropIndex("dbo.Part", new[] { "PalletId" });
            DropTable("dbo.Pallet");
            DropTable("dbo.Package");
            DropTable("dbo.Part");
            DropTable("dbo.Car");
        }
    }
}
