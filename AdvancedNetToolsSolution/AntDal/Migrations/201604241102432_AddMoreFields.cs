namespace AntDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PingResponseSummaries", "Latency", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PingResponseSummaries", "Latency");
        }
    }
}
