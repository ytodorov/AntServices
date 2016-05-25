﻿using System;
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
        }



        public string PanelTitle { get; set; }

        public string ButtonId { get; set; }

        public string ButtonText { get; set; }

        public string DefaultDestinationAddress { get; set; }

        public bool ShowAddressTextBox { get; set; }

    }
}