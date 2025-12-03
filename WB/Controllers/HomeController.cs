using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WB.Data;
using WB.Controllers;
using WB.Models;
using System.IO.Ports;
using System.Data;
using System.Data.SqlClient;

namespace WB.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchResults(String SearchPhrase)
        {
            if (SearchPhrase == null)
            {
                return NotFound();
            }
            return View("ShowSearch", _db.Categories1.Where(j => j.MaterialNumber.Contains(SearchPhrase)));


            //return View();

        }

        public IActionResult UpdateDb()
        {

            TempData["success"] = "Database Updated Successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectedPN(String nameobj)
        {
            var categoryFromDb = _db.Categories1.Where(j => j.MaterialNumber == nameobj);

            return View("SelectedPN", categoryFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult checker(SetVal data1)
        {

            return null;

        }
        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            var barcode = indata.Split(";");
            var codeValue = barcode[0].Substring(barcode[0].IndexOf(":") + 1);
        }

        
    }
}