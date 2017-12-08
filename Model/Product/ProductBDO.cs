using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfService.Model
{
    public class ProductBDO
    {
        public int product_id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public int category_id { get; set; }
        public bool in_store { get; set; }
        public bool is_discount { get; set; }
        public double price { get; set; }
        //public double discount { get; set; }
        public string photo { get; set; }
        public string barcode { get; set; }
        public string product_code { get; set; }

        //public int status_id { get; set; }
        //public string status_name { get; set; }
        //public string last_update { get; set; }
        //public bool is_delete { get; set; }
        public bool scale { get; set; }
        public bool ask_price { get; set; }
    }
}