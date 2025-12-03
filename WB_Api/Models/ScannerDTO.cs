using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WB_Api.Models
{
    public class CreateScannerDTO
    {
        public string id { get; set; }
        public string scan_value { get; set; }
        public string pn_no { get; set; }
        
    }

    public class ScannerDTO : CreateScannerDTO
    {
        public IList<ScannerDTO> Scanners { get; set; }
    }
}
