using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class WorkFlowDefineBDO
    {
        public int wf_define_id { get; set; }
        public string wf_define_desc { get; set; }
        public int case_id { get; set; }
        public int user_create_id { get; set; }
        public List<WorkFlowBDO> WorkFlowList { get; set; }
    }
}