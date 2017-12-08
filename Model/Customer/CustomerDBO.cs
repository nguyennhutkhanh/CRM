using System;
using System.Collections.Generic;

namespace WcfService.Model
{
    public class CustomerDBO
    {
        public int id { get; set; }
        public string cust_first_name { get; set; }
        public string cust_last_name { get; set; }
        public DateTime birth_date { get; set; }
        public string mobile_1 { get; set; }
        public string mobile_2 { get; set; }
        public string mobile_3 { get; set; }
        public string email { get; set; }
    }

    public class CustomerHistories : CustomerDBO
    {
        public List<CallBDO> cust_histories { get; set;}
    }
}