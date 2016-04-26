namespace AntDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreFields : DbMigration
    {
        public override void Up()
        {
            AddColumn(table: "dbo.PingResponseSummaries", name: "Latency", columnAction: c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn(table: "dbo.PingResponseSummaries", name: "Latency");
        }
    }
}
