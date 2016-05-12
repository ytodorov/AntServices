using AntDal.Entities;
using AutoMapper;
using SmartAdminMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
namespace SmartAdminMvc.App_Start
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            Mapper.Initialize(config =>
                {
                    config.CreateMap<PingResponseSummaryViewModel, PingResponseSummary>();
                    config.CreateMap<PingResponseSummary, PingResponseSummaryViewModel>();

                    config.CreateMap<PingPermalink, PingPermalinkViewModel>();
                    config.CreateMap<PingPermalinkViewModel, PingPermalink>();

                    config.CreateMap<PortPermalinkViewModel, PortPermalink>();
                    config.CreateMap<PortPermalink, PortPermalinkViewModel>();

                    config.CreateMap<PortResponseSummaryViewModel, PortResponseSummary>();
                    config.CreateMap<PortResponseSummary, PortResponseSummaryViewModel>();

                    config.CreateMap<IpLocationViewModel, IpLocation>();
                    config.CreateMap<IpLocation, IpLocationViewModel>();

                    config.CreateMap<ErrorLoggingViewModel, ErrorLogging>();
                    config.CreateMap<ErrorLogging, ErrorLoggingViewModel>();

                    config.CreateMap<TracerouteResponseDetailViewModel, TracerouteResponseDetail>().ReverseMap();
                    //config.CreateMap<TracerouteResponseDetail, TracerouteResponseDetailViewModel>();
                });
        }
    }
}