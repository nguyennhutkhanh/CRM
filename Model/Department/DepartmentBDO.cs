using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class DepartmentBDO
    {
        public int id { get; set; }
        public string dep_name { get; set; }
        public string dep_fullname { get; set; }
        public string permission { get; set; }
    }
}