namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changed_carId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Part", "PackageId", "dbo.Package");
            DropForeignKey("dbo.Part", "PalletId", "dbo.Pallet");
            DropIndex("dbo.Part", new[] { "PalletId" });
            DropIndex("dbo.Part", new[] { "PackageId" });
            RenameColumn(table: "dbo.Part", name: "Car_VIN", newName: "CarId");
            RenameIndex(table: "dbo.Part", name: "IX_Car_VIN", newName: "IX_CarId");
            AlterColumn("dbo.Part", "PalletId", c => c.Int());
            AlterColumn("dbo.Part", "PackageId", c => c.Int());
            CreateIndex("dbo.Part", "PalletId");
            CreateIndex("dbo.Part", "PackageId");
            AddForeignKey("dbo.Part", "PackageId", "dbo.Package", "Id");
            AddForeignKey("dbo.Part", "PalletId", "dbo.Pallet", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Part", "PalletId", "dbo.Pallet");
            DropForeignKey("dbo.Part", "PackageId", "dbo.Package");
            DropIndex("dbo.Part", new[] { "PackageId" });
            DropIndex("dbo.Part", new[] { "PalletId" });
            AlterColumn("dbo.Part", "PackageId", c => c.Int(nullable: false));
            AlterColumn("dbo.Part", "PalletId", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.Part", name: "IX_CarId", newName: "IX_Car_VIN");
            RenameColumn(table: "dbo.Part", name: "CarId", newName: "Car_VIN");
            CreateIndex("dbo.Part", "PackageId");
            CreateIndex("dbo.Part", "PalletId");
            AddForeignKey("dbo.Part", "PalletId", "dbo.Pallet", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Part", "PackageId", "dbo.Package", "Id", cascadeDelete: true);
        }
    }
}
