using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class ReportBDO
    {
        public int id { get; set; }
        public string report_name { get; set; }
        public List<Report_detail> report_list { get; set; }
    }

    public class Report_detail
    {
        public int id { get; set; }
        //public int report_id { get; set; }
        public string name { get; set; }
    }
}