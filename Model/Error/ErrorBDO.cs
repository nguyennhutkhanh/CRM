namespace WcfService.Model
{
    public class ErrorDBO
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class ErrorDetail
    {
        public string error_info { get; set; }
        public string error_detail { get; set; }
    }
}
