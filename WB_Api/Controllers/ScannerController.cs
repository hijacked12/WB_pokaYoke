using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Configuration;
using System.Data.SqlClient;
using WB_Api.Data;
using WB_Api.IRepository;
using WB_Api.Models;
using WB.Controllers;
using WB.Data;

namespace WB_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScannerController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ScannerController> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _db;


        public ScannerController(IUnitOfWork unitOfWork, ILogger<ScannerController> logger, IMapper mapper, IConfiguration configuration, DatabaseContext db)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _db = db;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetValues([FromBody] CreateScannerDTO scannerDTO)
        {
            
            var MN = scannerDTO.pn_no;
            var scan_value = scannerDTO.scan_value;
            var con_str = ("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MCsys;Integrated Security=True;");
            SqlConnection con = new SqlConnection(con_str);
            string query = "UPDATE dbo.scanner SET pn_no='" + MN + "', scan_value='" + scan_value + "' where id=1";
            SqlCommand cmd = new SqlCommand(query, con);

            //Pass values to Parameters
            cmd.Parameters.AddWithValue("@pn_no", MN);
            cmd.Parameters.AddWithValue("@scan_value", scan_value);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            //var scann = _mapper.Map<scanner>(scannerDTO);
            //var catcon = new CategoryController(_db);
            //catcon.getValues();
            return Ok();
            
        }

        private IActionResult Created()
        {
            throw new NotImplementedException();
        }
    }
}
