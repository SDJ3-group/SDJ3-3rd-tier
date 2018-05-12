namespace SDJ3_3rd_tier.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Part", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Part", "Name", c => c.String());
        }
    }
}
