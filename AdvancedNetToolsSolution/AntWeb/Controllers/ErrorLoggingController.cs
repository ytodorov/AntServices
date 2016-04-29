using AntDal;
using AntDal.Entities;
using AutoMapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAgo;

namespace SmartAdminMvc.Controllers
{
    public class ErrorLoggingController : BaseController
    {
        public ActionResult Index(int? id)
        {

            //if (id.HasValue)
            //{
            //    using (AntDbContext context = new AntDbContext())
            //    {
            //        ErrorLogging el = context.ErrorLoggings.Include(e => e.ErrorLoggingViewModel).FirstOrDefault(d => d.Id == id);
            //        if (el != null)
            //        {
            //            ErrorLoggingViewModel ppvm = Mapper.Map<ErrorLoggingViewModel>(el);
            //            return View(model: ppvm);
            //        }
            //    }
            //}
            return View();
        }
        public ActionResult ReadErrorLoggings([DataSourceRequest] DataSourceRequest request, string address, bool? allErrors = true)
        {
            List<ErrorLogging> errorLoggins;
            if (!allErrors.GetValueOrDefault())
            {
                errorLoggins = GetErrorsForCurrentUser(address);
            }
            else
            {
                using (AntDbContext context = new AntDbContext())
                {
                    errorLoggins = context.ErrorLoggings.Where(p => p.ShowInHistory == true).OrderByDescending(k => k.Id).Take(count: 10).ToList();
                }
            }

            List<ErrorLoggingViewModel> errorLoggingsViewModels = Mapper.Map<List<ErrorLoggingViewModel>>(errorLoggins);
            foreach (var p in errorLoggingsViewModels)
            {
                p.DateCreatedTimeAgo = p.DateCreated.GetValueOrDefault().TimeAgo();
            }

            JsonResult dsResult = Json(errorLoggingsViewModels.ToDataSourceResult(request));
            return dsResult;

        }

        private List<ErrorLogging> GetErrorsForCurrentUser(string address)
        {
            using (AntDbContext context = new AntDbContext())
            {
                string userIpAddress = Request.UserHostAddress;
                List<ErrorLogging> errorLoggings;
                if (string.IsNullOrEmpty(address))
                {
                    errorLoggings = context.ErrorLoggings.Where(p => p.UserCreatedIpAddress == userIpAddress && p.ShowInHistory == true).ToList();
                }
                else
                {
                    errorLoggings = context.ErrorLoggings.Where(p => p.DestinationAddress == address && p.ShowInHistory == true).ToList();
                }
                return errorLoggings;
            }
        }
    }
}