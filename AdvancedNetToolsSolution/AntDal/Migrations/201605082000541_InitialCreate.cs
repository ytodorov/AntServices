namespace AntDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLoggings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Data = c.String(),
                        ShowInHistory = c.Boolean(),
                        UserCreatedIpAddress = c.String(),
                        DestinationAddress = c.String(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IpLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusCode = c.String(),
                        StatusMessage = c.String(),
                        IpAddress = c.String(),
                        CountryCode = c.String(),
                        CountryName = c.String(),
                        RegionName = c.String(),
                        CityName = c.String(),
                        ZipCode = c.String(),
                        Latitude = c.Double(),
                        Longitude = c.Double(),
                        TimeZone = c.String(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PingPermalinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationAddress = c.String(),
                        UserCreatedIpAddress = c.String(),
                        ShowInHistory = c.Boolean(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PingResponseSummaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SourceIpAddress = c.String(),
                        SourceHostName = c.String(),
                        DestinationIpAddress = c.String(),
                        DestinationHostName = c.String(),
                        MaxRtt = c.Double(),
                        MinRtt = c.Double(),
                        AvgRtt = c.Double(),
                        PacketsSent = c.Int(),
                        PacketsReceived = c.Int(),
                        Latency = c.Double(),
                        PingPermalinkId = c.Int(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PingPermalinks", t => t.PingPermalinkId)
                .Index(t => t.PingPermalinkId);
            
            CreateTable(
                "dbo.PingSuccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeNeeded = c.Time(nullable: false, precision: 7),
                        StatusMessage = c.String(),
                        IpAddress = c.String(),
                        Successful = c.Boolean(nullable: false),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PortPermalinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationAddress = c.String(),
                        UserCreatedIpAddress = c.String(),
                        ShowInHistory = c.Boolean(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PortResponseSummaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PortNumber = c.Int(),
                        Protocol = c.String(),
                        State = c.String(),
                        Service = c.String(),
                        Version = c.String(),
                        PortPermalinkId = c.Int(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PortPermalinks", t => t.PortPermalinkId)
                .Index(t => t.PortPermalinkId);
            
            CreateTable(
                "dbo.SpeedPermalinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationAddress = c.String(),
                        UserCreatedIpAddress = c.String(),
                        ShowInHistory = c.Boolean(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpeedResponseSummaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationIpAddress = c.String(),
                        DestinationHostName = c.String(),
                        BitsPerSecond = c.Int(),
                        SpeedPermalinkId = c.Int(),
                        UserCreated = c.String(),
                        UserModified = c.String(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpeedPermalinks", t => t.SpeedPermalinkId)
                .Index(t => t.SpeedPermalinkId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SpeedResponseSummaries", "SpeedPermalinkId", "dbo.SpeedPermalinks");
            DropForeignKey("dbo.PortResponseSummaries", "PortPermalinkId", "dbo.PortPermalinks");
            DropForeignKey("dbo.PingResponseSummaries", "PingPermalinkId", "dbo.PingPermalinks");
            DropIndex("dbo.SpeedResponseSummaries", new[] { "SpeedPermalinkId" });
            DropIndex("dbo.PortResponseSummaries", new[] { "PortPermalinkId" });
            DropIndex("dbo.PingResponseSummaries", new[] { "PingPermalinkId" });
            DropTable("dbo.SpeedResponseSummaries");
            DropTable("dbo.SpeedPermalinks");
            DropTable("dbo.PortResponseSummaries");
            DropTable("dbo.PortPermalinks");
            DropTable("dbo.PingSuccesses");
            DropTable("dbo.PingResponseSummaries");
            DropTable("dbo.PingPermalinks");
            DropTable("dbo.IpLocations");
            DropTable("dbo.ErrorLoggings");
        }
    }
}
