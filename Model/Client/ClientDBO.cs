using System;
using System.Collections.Generic;

namespace WcfService.Model
{
    public class ClientDBO
    {
        public int client_id { get; set;  }
        public string full_name { get; set;  }
        public string mobile { get; set; }
        public string email { get; set;  }
        public string address { get; set;  }
        public short? gender { get; set;  }
        public DateTime? DOB { get; set; }
        public int? co_id { get; set; }
        public List<ClientTransDBO> client_trans { get; set; }
    }

    public class ClientTransDBO
    {
        public string datetime { set; get; }
        public bool status { get; set; }
        public string status_name { get; set; }
        public string ticket_no { get; set; }
        public float ticket_price { get; set; }
    }
}
