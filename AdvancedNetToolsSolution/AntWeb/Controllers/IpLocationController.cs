#region Using

using SmartAdminMvc.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{

    public class IpLocationController : BaseController
    {
        //[OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Server, VaryByParam = "ip")]
        public ActionResult Index(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                //return View(nameof(IpLocationController.Find));
                return View();

            }

            bool isUserRequestedAddressIp = false;
            IPAddress dummy1;
            isUserRequestedAddressIp = IPAddress.TryParse(ip, out dummy1);
            if (!isUserRequestedAddressIp)
            {
                ip = Utils.GetIpAddressFromHostName(hostName: ip, locationOfDeployedService: Utils.GetDeployedServicesUrlAddresses[0]);
            }
            return View((object)ip);
        }

        public ActionResult Find()
        {
            return View();
        }

        public ContentResult GetIpCountryCode(string ip)
        {
            var res = Utils.GetLocationDataForIp(ip);
            return Content(res?.CountryAbbreviation);
        }


    }
}