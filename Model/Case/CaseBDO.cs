using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class CaseBDO
    {
        public int case_id { get; set; }
        public string case_desc { get; set; }
        public int case_fb_id { get; set; }
        public int user_create_id { get; set; }
    }
}