using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class AddressViewModel
    {
        public AddressViewModel()
        {
            ShowAddressTextBox = true;
            ShowSaveInHistory = true;
            PlaceholderText = "IP address, URL or host name";
        }



        public string PanelTitle { get; set; }

        public string ButtonId { get; set; }

        public string ButtonText { get; set; }

        public string DefaultDestinationAddress { get; set; }

        public bool ShowAddressTextBox { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public bool ShowSaveInHistory { get; set; }

        public bool? OnlyWellknownPorts { get; set; }

        public string PlaceholderText { get; set; }

    }
}