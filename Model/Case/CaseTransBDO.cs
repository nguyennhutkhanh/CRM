using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class CaseTransBDO
    {
        public int id { get; set; }
        public int wf_define_id { get; set; }
        public int case_id { get; set; }
        public string case_desc { get; set; }
        public int call_id { get; set; }
        public int assign_agent_id { get; set; }
        public string content { get; set; }
        public string note { get; set; }
        public string reason { get; set; }
        public byte status { get; set; }
    }

    public class CaseTransHistoryBDO
    {
        public DateTime create_date { get; set; }
        public string case_desc { get; set; }
        public string wf_define_desc { get; set; }
        public byte status { get; set; }
    }
}