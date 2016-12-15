using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartAdminMvc.Models
{
    public class ViewModelBase
    {
        [Editable(false)]
        //[HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Editable(false)]
        public string UserCreated { get; set; }

        [Editable(false)]
        public string UserModified { get; set; }

        [Editable(false)]
        public DateTime? DateCreated { get; set; }

        [Editable(false)]
        public string DateCreatedTimeAgo { get; set; }

        [Editable(false)]
        public DateTime? DateModified { get; set; }
    }
}