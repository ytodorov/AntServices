namespace AntDal
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class AntDbContext : DbContext
    {        
        public AntDbContext()
            : base("name=AntModel")
        {
            //Database.SetInitializer<AntDbContext>(new DropCreateDatabaseIfModelChanges<AntDbContext>());
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<PingPermalink> PingPermalinks { get; set; }

        public virtual DbSet<PingResponseSummary> PingResponseSummaries { get; set; }

        public virtual DbSet<PortPermalink> PortPermalinks { get; set; }

        public virtual DbSet<PortResponseSummary> PortResponseSummaries { get; set; }

        public virtual DbSet<IpLocation> IpLocations { get; set; }

        public virtual DbSet<ErrorLogging> ErrorLoggings { get; set; }

        public virtual DbSet<PingSuccess> PingSuccesses { get; set; }
    }
}