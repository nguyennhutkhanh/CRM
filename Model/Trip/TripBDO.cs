using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService.Model
{
    public abstract class Trip
    {
        public int tr_id { get; set; }
        public string tr_name { get; set; }
        public string tr_description { get; set; }
        public int tr_cat_id { get; set; }
        public int co_id { get; set; }
        public double price { get; set; }
        public TimeSpan departure_hour { get; set; }
        public byte arrival_hour { get; set; }
        public short arrival_duration { get; set; }
        public string apply_from { get; set; }
        public string apply_to { get; set; }
        public short frequency { get; set; }
        public string days_of_week { get; set; }
        public int bus_id { get; set; }
        public short? days_sale_before { get; set; }
        public short status { get; set; }
        public int user_id { get; set; }
        public short? dis_method { get; set; }
        public int? dis_qty { get; set; }
        public string dis_seat_list { get; set; }
        public short? dis_before_abort { get; set; }
    }

    public class TripBDO : Trip
    {
            
        public List<seat_list> lock_seat_list { get; set; }
    }

    public class TripResponseBDO
    {
        public int tr_id { get; set; }
        public string tr_name { get; set; }
        public string tr_description { get; set; }
        public int? tr_cat_id { get; set; }
        public int co_id { get; set; }
        public double? price { get; set; }
        public string departure_hour { get; set; }
        public byte? arrival_hour { get; set; }
        public short? arrival_duration { get; set; }
        public string apply_from { get; set; }
        public string apply_to { get; set; }
        public short? frequency { get; set; }
        public string days_of_week { get; set; }
        public int bus_id { get; set; }
        public short? days_sale_before { get; set; }
        public short status { get; set; }
        public int? user_id { get; set; }
        public short? dis_method { get; set; }
        public int? dis_qty { get; set; }
        public string dis_seat_list { get; set; }
        public short? dis_before_abort { get; set; }
    }

    public class seat_list
    {
        public int seat_id { get; set; }
        public int co_id { get; set; }
    }

}
