using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class CallBDO
    {
        public int call_id { get; set; }
        public string call_no { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public string content { get; set; }
        public string note { get; set; }
        public string url_rec { get; set; }
        public byte status { get; set; }
        public int user_id { get; set; }
        public int case_trans_id { get; set; }
    }

    public class CallTotalStatusBBO
    {
        public int total_waiting { get; set; }
        public int total_processing { get; set; }
        public int total_rejected { get; set; }
        public int total_confirm_waiting { get; set; }
        public int total_finish { get; set; }
    }
}
