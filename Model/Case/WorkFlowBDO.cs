namespace WcfService.Model
{
    public class WorkFlowBDO
    {
        public int id { get; set; }
        public int wf_define_id { get; set; }
        public string wf_desc { get; set; }  
        //public int case_id { get; set; }
        public int call_id { get; set; }
        public int next { get; set; }        
        public int department_id { get; set; }
        public byte status { get; set; }
    }

   
}