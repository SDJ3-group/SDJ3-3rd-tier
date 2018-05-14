namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datachange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pallet", "MaximumCapacity", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pallet", "MaximumCapacity");
        }
    }
}
