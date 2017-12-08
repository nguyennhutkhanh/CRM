namespace WcfService.Model
{
    public class BusBDO
    {
        public int bus_id { get; set; }
        public string bus_no { get; set; }
        public int driver_id { get; set; }
        public int num_seat { get; set; }
        public byte bus_type { get; set; }
        public int co_id { get; set; }
        public string img { get; set; } // sp do ghe
        public int user_id { get; set; }
        public BusUtilBDO util { get; set; }
    }

    public class BusUtilBDO
    {
        public bool util_1 { get; set; } //wifi
        public bool util_2 { get; set; } //condition_air
        public bool util_3 { get; set; } //wc
    }
}
