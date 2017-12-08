using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class OwnerBDO
    {
        public int co_id { get; set; }
        public string co_name { get; set; }
        public string co_fax { get; set; }
        public string co_phone { get; set; }
        public string co_address { get; set; }
        public int parent { get; set; }
    }
}