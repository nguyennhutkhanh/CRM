using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class TicketBDO
    {
        public int ticket_id { get;  }
        public string ticket_no { get;  }
        public int bus_id { get;  }
        public int s_b_id { get;  }
        public int client_id { get;  }
        public float price { get;  }
        public int trip_id { get;  }
        public int trip_category_id { get;  }
        public int user_id { get;  }
    }

    public class TicketResponseBDO
    {
        public int ticket_id { get; set; }
        public string ticlet_no { get; set; }
        public int s_b_id { get; set; }
        public string s_b_no { get; set; }
        public int client_iid { get; set; }
        public string client_name { get; set; }
        public double price { get; set; }
        public byte status_id { get; set; }
        public string note { get; set; }

    }
}