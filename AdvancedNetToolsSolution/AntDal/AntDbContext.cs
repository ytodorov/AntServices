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
            : base(nameOrConnectionString: "name=AntModel")
        {
            //Database.SetInitializer<AntDbContext>(new DropCreateDatabaseAlways<AntDbContext>());
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<PingPermalink> PingPermalinks { get; set; }

        public virtual DbSet<PingResponseSummary> PingResponseSummaries { get; set; }

        public virtual DbSet<PortPermalink> PortPermalinks { get; set; }

        public virtual DbSet<PortResponseSummary> PortResponseSummaries { get; set; }


        public virtual DbSet<SpeedPermalink> SpeedPermalinks { get; set; }

        public virtual DbSet<SpeedResponseSummary> SpeedResponseSummaries { get; set; }


        public virtual DbSet<IpLocation> IpLocations { get; set; }

        public virtual DbSet<ErrorLogging> ErrorLoggings { get; set; }

        public virtual DbSet<PingSuccess> PingSuccesses { get; set; }

        public virtual DbSet<TraceroutePermalink> TraceroutePermalinks { get; set; }

        public virtual DbSet<TracerouteResponseSummary> TracerouteResponseSummaries { get; set; }

        public virtual DbSet<TracerouteResponseDetail> TracerouteResponseDetails { get; set; }

        public virtual DbSet<MelissaIpLocation> MelissaIpLocations { get; set; }
        

    }
}