using AntDal;
using Quartz;
using SmartAdminMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SmartAdminMvc.Infrastructure.Jobs
{
    public class PortScanAddressesJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            using (AntDbContext dbContext = new AntDbContext())
            {
                var topsites = Utils.TopSitesGlobal.Take(20);
                //foreach (var siteUrl in topsites)
                //{
                //    var ip = Utils.GetIpAddressFromHostName(siteUrl, Utils.GetDeployedServicesUrlAddresses[0]);
                //    PortscanController portC = new PortscanController();
                //    var res3 = portC.GenerateId(ip, true, true, false, false);
                //}

                Parallel.ForEach(topsites, siteUrl =>
                {
                    var random = Utils.RandomNumberGenerator.Next(0, Utils.GetDeployedServicesUrlAddresses.Count - 1);
                    var ip = Utils.GetIpAddressFromHostName(siteUrl, Utils.GetDeployedServicesUrlAddresses[random]);
                    PortscanController portC = new PortscanController();
                    var res3 = portC.GenerateId(ip, true, true, false, false);
                });
              
            }
        }
    }
}