#region Using

using SmartAdminMvc.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class IpLocationController : Controller
    {
        public ActionResult Index(string ip)
        {
            bool isUserRequestedAddressIp = false;
            IPAddress dummy1;
            isUserRequestedAddressIp = IPAddress.TryParse(ip, out dummy1);
            if (!isUserRequestedAddressIp)
            {
                ip = Utils.GetIpAddressFromHostName(hostName: ip, locationOfDeployedService: Utils.GetDeployedServicesUrlAddresses[0]);
            }
            return View((object)ip);   
        }

    }
}