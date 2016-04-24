namespace AntDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLostField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PingResponseSummaries", "Lost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PingResponseSummaries", "Lost");
        }
    }
}
