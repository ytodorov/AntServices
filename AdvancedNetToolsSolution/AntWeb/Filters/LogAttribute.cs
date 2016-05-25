using AntDal;
using SmartAdminMvc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public AntDbContext Context { get; set; }

        public string Description { get; set; }

        public LogAttribute(string description, AntDbContext context)
        {
            Description = description;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // var userID = filterContext.HttpContext.User.Identity.GetUserId();
            // Context.Logs.Add(new LogAction(Description));

            Context.SaveChanges();
        }

    }
}