using AntDal;
using AntDal.Entities;
using AutoMapper;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}