using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwesomeMvcDemo.Models
{
    public class BaseGridModel
    {
        public string search { get; set; }
        public string orderby { get; set; }
        public int? pagenumber { get; set; }
        public int pagesize { get; set; }
    }
}