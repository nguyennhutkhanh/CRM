using System.Collections.Generic;

namespace WcfService.Model
{
    public class UserBDO
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public string hash_pwd { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string api_key { get; set; }
        public int user_group_id { get; set; }
        public string group_name { get; set; }
        public int department_id { get; set; }

        public List<UserModuleBDO> module_list { get; set; } 

    }

    public class UserModuleBDO
    {
        //public int role_id { get; set; }
        public int mobile_id { get; set; }
        public string module_name { get; set; }
        public bool allow { get; set; }
    }
}
