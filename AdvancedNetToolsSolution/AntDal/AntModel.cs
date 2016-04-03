namespace AntDal
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class AntDbContext : DbContext
    {
        // Your context has been configured to use a 'AntModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'AntDal.AntModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AntModel' 
        // connection string in the application configuration file.
        public AntDbContext()
            : base("name=AntModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<PingPermalink> PingPermalinks { get; set; }

        public virtual DbSet<PingResponseSummary> PingResponseSummaries { get; set; }
    }

    public class PingPermalink : EntityBase
    {   
        public PingPermalink()
        {
            PingResponseSummaries = new List<PingResponseSummary>();
        }

        public string DestinationAddress { get; set; }
        public virtual List<PingResponseSummary> PingResponseSummaries { get; set; }
    }

    public class PingResponseSummary : EntityBase
    {
        public string SourceAddress { get; set; }

        public double? MaxRtt { get; set; }

        public double? MinRtt { get; set; }

        public double? AvgRtt { get; set; }

        public int? PingPermalinkId { get; set; }
        public virtual PingPermalink PingPermalink { get; set; }
    }

    public class EntityBase
    {
        public int Id { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}