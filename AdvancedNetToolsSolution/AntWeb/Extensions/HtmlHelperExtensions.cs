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
            builder.Name("gridName" + Utils.RandomString(10))
.Columns(columns =>
{
    columns.Bound(c => c.DestinationAddress).Title("Address");
    columns.Bound(c => c.PermalinkAddress).Title("Permalink").ClientTemplate("<a href='#: data.PermalinkAddress #'>#: data.PermalinkAddress #</a>");
    columns.Bound(c => c.DateCreatedTimeAgo).Title("Created on");//.Format("{0:u}");
})
.Scrollable(s => s.Enabled(false))
.ToolBar(t =>
{
    t.Excel().Text(string.Empty);
    t.Pdf().Text(string.Empty);
})
.Excel(e => e.FileName("pingExport(www.toolsfornet.com).xlsx").ProxyURL(urlHelper.Action("Excel", "Export")))
.Pdf(e => e.AllPages().FileName("pingExport(www.toolsfornet.com).pdf").ProxyURL(urlHelper.Action("Pdf", "Export")))
.Pageable()
.Sortable()
.Filterable(f => f.Extra(false))
.DataSource(dataSource =>
{
    var ajaxDatasource = dataSource
      .Ajax()
      .Sort(s => s.Add(ss => ss.DateCreated).Descending())
      .ServerOperation(false)
      
      .Read(r =>
      {
          var cub = r.Action("ReadPingPermalinks", "Ping");
          if (!string.IsNullOrEmpty(readDataJavascriptMethodName))
          {
              cub.Data(readDataJavascriptMethodName);
          }
      });
    if (HttpContext.Current.Request.Browser.IsMobileDevice)
    {
        ajaxDatasource.PageSize(5);
    }
    else
    {
        ajaxDatasource.PageSize(10);
    }
}
);

           
builder.Deferred();
            return builder;
        }
    }
}