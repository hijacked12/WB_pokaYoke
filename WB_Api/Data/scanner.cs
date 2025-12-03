namespace WB_Api.Data
{
    public class scanner
    {
        public string id { get; set; }
        public string scan_value { get; set; }
        public string pn_no { get; set; }
        public virtual IList<scanner> Scanners { get; set; }
    }
}
