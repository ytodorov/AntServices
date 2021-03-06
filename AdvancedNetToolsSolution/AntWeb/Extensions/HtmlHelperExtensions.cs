﻿using Kendo.Mvc.UI.Fluent;
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
        public static string AntCreateStandardPanel(this HtmlHelper htmlHelper,
            string headerColorClass = null, string headerText = null, string body = null)
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

            string template = $@"<div>
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


        public static GridBuilder<PortPermalinkViewModel> CreatePortscanPermalinkGrid
            (this GridBuilder<PortPermalinkViewModel> builder, UrlHelper urlHelper,
            string readDataJavascriptMethodName = null,
            bool serverOperations = false)
        {
            builder.Name("gridName" + Utils.RandomString(length: 10))
.Columns(columns =>
{
    columns.Bound(c => c.DestinationAddress).Title(text: "Url");
    columns.Bound(c => c.DestinationIpAddress).Title(text: "IP");
    columns.Bound(c => c.OpenPortsCount).Title(text: "Open ports");
    columns.Bound(c => c.PermalinkAddress).Title(text: "Permalink").ClientTemplate(value: "<a href='#: data.PermalinkAddress #'>#: data.PermalinkAddress #</a>");
    columns.Bound(c => c.DateCreatedTimeAgo).Title(text: "Created on");//.Format("{0:u}");
})
.Scrollable(s => s.Enabled(value: false))
.ToolBar(t =>
{
    t.Excel().Text(string.Empty);
    t.Pdf().Text(string.Empty);
})
.Excel(e => e.AllPages(allPages: true).FileName(fileName: "portScanExport(www.toolsfornet.com).xlsx").ProxyURL(urlHelper.Action(actionName: "Excel", controllerName: "Export")))
.Pdf(e => e.AllPages().FileName(fileName: "portScanExport(www.toolsfornet.com).pdf").ProxyURL(urlHelper.Action(actionName: "Pdf", controllerName: "Export")))
.Pageable()
.Sortable()
.Filterable(f => f.Extra(value: false))
.DataSource(dataSource =>
{
    AjaxDataSourceBuilder<PortPermalinkViewModel> ajaxDatasource = dataSource
      .Ajax()
      .Sort(s => s.Add(ss => ss.DateCreated).Descending())
      .ServerOperation(enabled: serverOperations)

      .Read(r =>
      {
          object routeValues = null;
          //if (serverOperations)
          //{
          //    routeValues = new { maxResults = 100000 };
          //}
          CrudOperationBuilder cub = r.Action("ReadPortPermalinks", "Portscan", routeValues);
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



        public static GridBuilder<TraceroutePermalinkViewModel> CreateTreaceroutePermalinkGrid
            (this GridBuilder<TraceroutePermalinkViewModel> builder, UrlHelper urlHelper, string readDataJavascriptMethodName = null)
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
.Excel(e => e.AllPages(allPages: true).FileName(fileName: "tracerouteExport(www.toolsfornet.com).xlsx").ProxyURL(urlHelper.Action(actionName: "Excel", controllerName: "Export")))
.Pdf(e => e.AllPages().FileName(fileName: "tracerouteExport(www.toolsfornet.com).pdf").ProxyURL(urlHelper.Action(actionName: "Pdf", controllerName: "Export")))
.Pageable()
.Sortable()
.Filterable(f => f.Extra(value: false))
.DataSource(dataSource =>
{
    AjaxDataSourceBuilder<TraceroutePermalinkViewModel> ajaxDatasource = dataSource
      .Ajax()
      .Sort(s => s.Add(ss => ss.DateCreated).Descending())
      .ServerOperation(enabled: false)

      .Read(r =>
      {
          CrudOperationBuilder cub = r.Action(actionName: "ReadTraceroutePermalinks", controllerName: "Traceroute");
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

        public static GridBuilder<WellKnownPortViewModel> CreateWellKnownPortGrid
         (this GridBuilder<WellKnownPortViewModel> builder, UrlHelper urlHelper)
        {
            builder.Name("gridName" + Utils.RandomString(length: 10))
.Columns(columns =>
{
    columns.Bound(c => c.PortNumber).Title(text: "Port");
    columns.Bound(c => c.ServiceName);
    columns.Bound(c => c.Description);//.Format("{0:u}");
    columns.Bound(c => c.Frequency);

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
    AjaxDataSourceBuilder<WellKnownPortViewModel> ajaxDatasource = dataSource
      .Ajax()
      .Sort(s => s.Add(ss => ss.PortNumber))
      .ServerOperation(enabled: false)

      .Read(r =>
      {
          CrudOperationBuilder cub = r.Action(actionName: "ReadWellKnownPorts", controllerName: "Portscan");
         
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
    columns.Bound(c => c.Message).Title(text: "Message");
    columns.Bound(c => c.StackTrace).Title(text: "Stacktrace");
    columns.Bound(c => c.Data).Title(text: "Data");//.Format("{0:u}");
    columns.Bound(c => c.DateCreatedTimeAgo).Title(text: "Date");//.Format("{0:u}");
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
        ajaxDatasource.PageSize(pageSize: 100);
    }
}
);
            builder.Deferred();
            return builder;
        }
    }
}