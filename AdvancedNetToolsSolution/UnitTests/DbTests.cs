using AntDal;
using AntDal.Entities;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class DbTests
    {
        [Fact]
        public void SaveEntity()
        {
            //AntDbContext adc = new AntDbContext();
            //adc.PingPermalinks.Add(new PingPermalink() { DestinationAddress = "test" });
            //adc.SaveChanges();

            PingResponseSummary pr = new PingResponseSummary();
            pr.AvgRtt = 12;
            pr.MinRtt = 566;
            pr.MaxRtt = 1241313;

            PingResponseSummaryViewModel res = AutoMapper.Mapper.DynamicMap<PingResponseSummaryViewModel>(pr);
        }
    }
}
