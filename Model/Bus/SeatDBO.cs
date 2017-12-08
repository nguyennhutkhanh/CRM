using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class SeatDBO
    {
        public int s_b_id { get; set; }
        public string s_no { get; set; }
        public byte status { get; set; }
        public int bus_id { get; set; }
    }
}