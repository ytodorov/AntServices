namespace AntDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLostField : DbMigration
    {
        public override void Up()
        {
            AddColumn(table: "dbo.PingResponseSummaries", name: "Lost", columnAction: c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn(table: "dbo.PingResponseSummaries", name: "Lost");
        }
    }
}
