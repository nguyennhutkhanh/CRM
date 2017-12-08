using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class LocDrpPckBDO
    {
        public int location_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public int co_id { get; set; }
        public int user_id { get; set; }

    }
}