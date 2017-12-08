using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class ModuleBDO
    {
        public int id { get; set; }
        public string module_name { get; set; }
        public int role_id  { get; set; }
        public bool allow { get; set; }
        public int user_id { get; set; }
        public bool is_admin { get; set; }
    }
}