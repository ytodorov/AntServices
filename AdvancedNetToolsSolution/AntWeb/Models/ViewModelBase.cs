using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class ViewModelBase
    {
        public int Id { get; set; }

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        public DateTime? DateCreated { get; set; }

        public string DateCreatedTimeAgo { get; set; }

        public DateTime? DateModified { get; set; }
    }
}