using Kendo.Mvc.UI.Fluent;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static string AntCreateStandardPanel(this HtmlHelper htmlHelper, string headerColorClass = null, string headerText = null, string body = null)
        {
            if (headerColorClass == null)
            {
                headerColorClass = string.Empty;
            }
            if (headerText == null)
            {
                headerText = string.Empty;
            }
            if (body == string.Empty)
            {
                body = string.Empty;
            }

            string template = $@"<div class='col-lg-12'>
    <div class='hpanel {headerColorClass}'>
        <div class='panel-heading'>
            <p class='text-center'>
                {headerText}
            </p>
        </div>
        <div class='panel-body'>
           {body}
        </div>
    </div>
</div>";
            return template;
        }

        public static GridBuilder<PingPermalinkViewModel> CreatePingPermalinkGrid
            (this GridBuilder<PingPermalinkViewModel> builder, UrlHelper urlHelper, string readDataJavascriptMethodName = null)
        {
            builder.Name("gridName" + Utils.RandomString(length: 10))
.Columns(columns =>
{
    columns.Bound(c => c.DestinationAddress).Title(text: "Address");
    columns.Bound(c => c.PermalinkAddress).Title(text: "Permalink").ClientTemplate(value: "<a href='#: data.PermalinkAddress #'>#: data.PermalinkAddress #</a>");
    columns.Bound(c => c.DateCreatedTimeAgo).Title(text: "Created on");//.Format("{0:u}");
})
.Scrollable(s => s.Enabled(value: false))
.ToolBar(t =>
{
    t.Excel().Text(string.Empty);
    t.Pdf().Text(string.Empty);
})
.Excel(e => e.AllPages(allPages: true).FileName(fileName: "pingExport(www.toolsfornet.com).xlsx").ProxyURL(urlHelper.Action(actionName: "Excel", controllerName: "Export")))
.Pdf(e => e.AllPages().FileName(fileName: "pingExport(www.toolsfornet.com).pdf").ProxyURL(urlHelper.Action(actionName: "Pdf", controllerName: "Export")))
.Pageable()
.Sortable()
.Filterable(f => f.Extra(value: false))
.DataSource(dataSource =>
{
    AjaxDataSourceBuilder<PingPermalinkViewModel> ajaxDatasource = dataSource
      .Ajax()
      .Sort(s => s.Add(ss => ss.DateCreated).Descending())
      .ServerOperation(enabled: false)
      
      .Read(r =>
      {
          CrudOperationBuilder cub = r.Action(actionName: "ReadPingPermalinks", controllerName: "Ping");
          if (!string.IsNullOrEmpty(readDataJavascriptMethodName))
          {
              cub.Data(readDataJavascriptMethodName);
          }
      });
    if (HttpContext.Current.Request.Browser.IsMobileDevice)
    {
        ajaxDatasource.PageSize(pageSize: 5);
    }
    else
    {
        ajaxDatasource.PageSize(pageSize: 10);
    }
}
);

           
builder.Deferred();
            return builder;
        }

        public static GridBuilder<ErrorLoggingViewModel> CreateErrorLoggingGrid
            (this GridBuilder<ErrorLoggingViewModel> builder, UrlHelper urlHelper, string readDataJavascriptMethodName = null)
        {
            builder.Name("gridName" + Utils.RandomString(length: 10))
.Columns(columns =>
{
    columns.Bound(c => c.DestinationAddress).Title(text: "Address");
    columns.Bound(c => c.ErrorLogAddress).Title(text: "Error").ClientTemplate(value: "<a href='#: data.ErrorLogAddress #'>#: data.ErrorLogAddress #</a>");
    columns.Bound(c => c.DateCreatedTimeAgo).Title(text: "Created on");//.Format("{0:u}");
})
.Scrollable(s => s.Enabled(value: false))
.ToolBar(t =>
{
    t.Excel().Text(string.Empty);
    t.Pdf().Text(string.Empty);
})
.Excel(e => e.AllPages(allPages: true).FileName(fileName: "errorLog(www.toolsfornet.com).xlsx").ProxyURL(urlHelper.Action(actionName: "Excel", controllerName: "Export")))
.Pdf(e => e.AllPages().FileName(fileName: "errorLog(www.toolsfornet.com).pdf").ProxyURL(urlHelper.Action(actionName: "Pdf", controllerName: "Export")))
.Pageable()
.Sortable()
.Filterable(f => f.Extra(value: false))
.DataSource(dataSource =>
{
    AjaxDataSourceBuilder<ErrorLoggingViewModel> ajaxDatasource = dataSource
      .Ajax()
      .Sort(s => s.Add(ss => ss.DateCreated).Descending())
      .ServerOperation(enabled: false)

      .Read(r =>
      {
          CrudOperationBuilder cub = r.Action(actionName: "ReadErrorLoggings", controllerName: "ErrorLogging");
          if (!string.IsNullOrEmpty(readDataJavascriptMethodName))
          {
              cub.Data(readDataJavascriptMethodName);
          }
      });
    if (HttpContext.Current.Request.Browser.IsMobileDevice)
    {
        ajaxDatasource.PageSize(pageSize: 5);
    }
    else
    {
        ajaxDatasource.PageSize(pageSize: 10);
    }
}
);
            builder.Deferred();
            return builder;
        }
    }
}