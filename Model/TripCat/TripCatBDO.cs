namespace WcfService.Model
{
    public class TripCatBDO
    {
        public int tr_cat_id { get; set; }
        public string name { get; set; }
        public int departure { get; set; }
        public int arrival { get; set; }
        public int location_pickup { get; set; }
        public int location_dropoff { get; set; }
        public int user_id { get; set; }
        public int co_id { get; set; }
    }
}
