using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Models
{
    public class EmailFormModel
    {
        [Required, Display(Name = "Recipient name")]
        public string ToName { get; set; }
        [Required, Display(Name = "Recipient email"), EmailAddress]
        public string ToEmail { get; set; }
        [Required]
        public string Message { get; set; }
    }
}