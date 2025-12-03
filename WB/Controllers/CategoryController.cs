using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WB.Data;
using WB.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;
using System.Collections;
using NuGet.Packaging.Signing;
using WB_Api.Models;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Protocol;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Identity;
using WB.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Drawing.Printing;
using System.Net.NetworkInformation;
using OfficeOpenXml;

namespace WB.Controllers
{
    public class CategoryController : Controller
    {
        private PLCConnector plcConnector;
        public String check;
        string checkValid = "";
        public string codeValue = "";
        public int changeValue;
        public DateTime date;
        public int count;
        public string new_val = "";
        public string new_val1 = "";
        string pn_db = "";
        public Dictionary<string, int> dups_store = new Dictionary<string, int>();
        public List<string> duplicate_list = new List<string>() { };
        public List<double> testlist = new List<double>();
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;
        private readonly UserManager<WBUser> _userManager;
        IEnumerable<Category> pageList;
        public List<string> duplicates = new List<string>();
        SetVal model = new();
        List<string> newList = new List<string>();
        List<string> comp_nos = new List<string>();

        
        public CategoryController(ApplicationDbContext db, IMemoryCache memoryCache, IDistributedCache cache, UserManager<WBUser> userManager)
        {
            _db = db;
            _memoryCache = memoryCache;
            _cache = cache;
            this._userManager = userManager;
            plcConnector = new PLCConnector();
        }

        [Authorize]
        public IActionResult UpdateDb()
        {

            TempData["success"] = "Database Updated Successfully";
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Index(SetVal data1)
        {
            //var deet = await this._userManager.GetUserAsync(User);
            //var e = deet.FirstName;
            if (data1.X6 != null)
            {
                TempData["id"] = data1.X6.ToUpper();
            }
            var id = data1.X6;
            var scan__value = "";
            await _db.scanner.Where(u => u.id == id)
                              .ForEachAsync(b => b.scan_value = scan__value);

            new_val = "";
            if (id != null)
            {
                _cache.Remove(id);
            }
            TempData.Keep("id");
            IEnumerable<Category> objCategoryList = _db.Categories1;
            return View(objCategoryList);
        }

        [Authorize]
        public IActionResult List_Table(int pageNumber = 1, int pageSize = 20)
        {
            var filteredLogs = _db.Categories1;

            // Pagination
            var pagedResults = filteredLogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = filteredLogs.Count();

            var viewModel = new SearchLogViewModel
            {
                List = pagedResults,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectedPN(String nameobj)
        {
            //var cachekey = "id";
            //var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(5));
            //_memoryCache.Set(cachekey, "", cacheEntryOptions);
            var line = TempData.Peek("id").ToString();
            
            ViewBag.check10 = nameobj;
            ViewBag.check11 = line.ToUpper();
            _db.scanner.Where(u => u.id == line).ExecuteUpdate(
                b => b.SetProperty(u => u.pn_no, nameobj));
            ViewBag.check9 = "";
            var num = 0;
            pageList = _db.Categories1.Where(j => j.MaterialNumber == nameobj).ToList();
            if (pageList is not null)
            {
                var myDb = pageList.Where(j => j.MaterialNumber == nameobj).ToList();
                foreach (var obj in myDb)
                {
                    newList.Add(obj.ComponentNumber);
                    comp_nos.Add(obj.ComponentNumber);
                }
                num = newList.Count;
                duplicates = newList.GroupBy(x => x)
                                .SelectMany(g => g.Skip(1))
                                .Distinct()
                                .ToList();
                for (int i = 0; i < duplicates.Count; i++)
                {
                    if (!dups_store.ContainsKey(duplicates[i].ToString()))
                    {
                        dups_store.Add(duplicates[i].ToString(), 0);
                    }
                }
                foreach (var obj in myDb)
                {
                    if (dups_store.ContainsKey(obj.ComponentNumber))
                    {
                        duplicate_list.Add(obj.ComponentNumber);
                    }
                }
                if (_cache.GetObject<List<string>>(line) == null)
                {
                    _cache.SetObject(line, duplicate_list);
                }
                
                ViewBag.check1 = check;
                ViewBag.check2 = "background-color: #00FF00;";
                ViewBag.check3 = 0;
                ViewBag.check4 = num;
                //ViewBag.check5 = '';
                ViewBag.update = num;
                changeValue = num;
                ViewBag.check6 = testlist;
                ViewBag.check7 = duplicates;

                return View("SelectedPNUpdate", pageList);
            }

            else
            {
                pageList = _db.Categories1.Where(j => j.MaterialNumber == nameobj).ToList();

                foreach (var obj in pageList)
                {
                    newList.Add(obj.ComponentNumber);
                }
                num = newList.Count;
                duplicates = newList.GroupBy(x => x)
                                .SelectMany(g => g.Skip(1))
                                .Distinct()
                                .ToList();
                for (int i = 0; i < duplicates.Count; i++)
                {
                    if (!dups_store.ContainsKey(duplicates[i].ToString()))
                    {
                        dups_store.Add(duplicates[i].ToString(), 0);
                    }
                }

                check = "background-color: #ff0000;";
                ViewBag.check1 = check;
                ViewBag.check2 = "background-color: #00FF00;";
                ViewBag.check3 = 0;
                ViewBag.check4 = num;
                ViewBag.update = num;
                changeValue = num;
                ViewBag.check6 = testlist;
                ViewBag.check7 = duplicates;
            }
            return View("SelectedPN", pageList);
        }

        //GET
        [Authorize]
        public IActionResult Create()
        {

            return View();
        }

        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.MaterialNumber == obj.ComponentNumber.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot be the same as the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories1.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "PN added Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }

        //GET
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories1.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories1.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "PN updated Successfully";
                return RedirectToAction("Index");

            }
            return View();

        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories1.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Categories1.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Categories1.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "PN deleted Successfully";
            return RedirectToAction("Index");


            //return View();

        }

        public IActionResult DeleteLog(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var scanLogFromDb = _db.ScanLog.Find(id);
            if (scanLogFromDb == null)
            {
                return NotFound();
            }

            return View(scanLogFromDb);
        }

        [HttpPost, ActionName("DeleteLog")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteLogPOST(int? id)
        {
            var obj = _db.ScanLog.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.ScanLog.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "数据删除成功！";
            return RedirectToAction("LogView");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchResults(String SearchPhrase)
        {
            var obj = _db.Categories1.Find(SearchPhrase);
            if (obj == null)
            {
                return NotFound();
            }
            return View("Index", _db.Categories1.Where(j => j.MaterialNumber.Contains(SearchPhrase)));

        }

        public void getValues(string line)
        {
            var val1 = _db.scanner
                .Where(j => j.id == line)
                .Select(c =>
                    new
                    {
                        c.scan_value,
                        c.pn_no,
                    }
                ).ToList();
            foreach (var obj in val1)
            {
                new_val = obj.scan_value;
                pn_db = obj.pn_no;

                TempData["id"] = line;
            }


        }

        [HttpGet]
        public ActionResult GetCurrent()
        {
            var id = "";
            if (TempData.Peek("id") != null) { 
                id = TempData.Peek("id").ToString();
            }
            if (id != null)
            {
                var val1 = _db.scanner.Where(j => j.id == id);
                if (val1 != null)
                {
                    foreach (var obj in val1)
                    {

                        new_val = obj.scan_value;
                    }
                }
            }
            return Json(new_val, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpGet]
        public ActionResult SendResult()
        {
            var id = TempData.Peek("id").ToString();
            var val1 = _db.change_state
                .Where(j => j.id == id)
                .Select(c =>
                    new
                    {
                        c.state
                    }
                ).ToList();
            foreach (var obj in val1)
            {
                new_val1 = obj.state;
            }
                return Json(new_val1, new System.Text.Json.JsonSerializerOptions());
            
        }


        private IQueryable<ScanLog> FilterLogs(string materialNumber, string status, string supplier, string line, DateTime? startDate, DateTime? endDate)
        {
            DateOnly? startDateOnly = startDate != null ? new DateOnly(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day) : (DateOnly?)null;
            DateOnly? endDateOnly = endDate != null ? new DateOnly(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day) : (DateOnly?)null;

            IQueryable<ScanLog> query = _db.ScanLog;

            // Check for stored values
            string storedMaterialNumber = TempData.Peek("MaterialNumber")?.ToString();
            string storedStatus = TempData.Peek("Status")?.ToString();
            string storedSupplier = TempData.Peek("Supplier")?.ToString();
            string storedLine = TempData.Peek("Line")?.ToString();
            DateOnly? storedStartDate = TempData.Peek("StartDate") as DateOnly?;
            DateOnly? storedEndDate = TempData.Peek("EndDate") as DateOnly?;

            if (!string.IsNullOrEmpty(storedMaterialNumber) || !string.IsNullOrEmpty(storedStatus) || !string.IsNullOrEmpty(storedSupplier) || !string.IsNullOrEmpty(storedLine) ||
                storedStartDate.HasValue || storedEndDate.HasValue)
            {
                // Use stored search parameters to reconstruct the query
                if (!string.IsNullOrEmpty(storedMaterialNumber))
                {
                    query = query.Where(x => x.MaterialNumber == storedMaterialNumber);
                }

                if (!string.IsNullOrEmpty(storedStatus))
                {
                    query = query.Where(x => x.Status == storedStatus);
                }

                if (!string.IsNullOrEmpty(storedSupplier))
                {
                    query = query.Where(x => x.Supplier == storedSupplier);
                }

                if (!string.IsNullOrEmpty(storedLine))
                {
                    query = query.Where(x => x.Line == storedLine);
                }

                if (storedStartDate.HasValue)
                {
                    query = query.Where(x => x.ScanDate >= storedStartDate);
                }

                if (storedEndDate.HasValue)
                {
                    query = query.Where(x => x.ScanDate <= storedEndDate);
                }

                // Update query with new search parameters
                if (!string.IsNullOrEmpty(materialNumber))
                {
                    query = query.Where(x => x.MaterialNumber == materialNumber);
                    TempData["MaterialNumber"] = materialNumber;
                }

                if (!string.IsNullOrEmpty(status))
                {
                    query = query.Where(x => x.Status == status);
                    TempData["Status"] = status;
                }

                if (!string.IsNullOrEmpty(supplier))
                {
                    query = query.Where(x => x.Supplier == supplier);
                    TempData["Supplier"] = supplier;
                }

                if (!string.IsNullOrEmpty(line))
                {
                    if (line != "None")
                    {
                        query = query.Where(x => x.Line == line);
                        TempData["Line"] = line;
                    }
                }

                if (startDate.HasValue && endDate.HasValue)
                {
                    query = query.Where(x => x.ScanDate >= startDateOnly && x.ScanDate <= endDateOnly);
                    TempData["StartDate"] = startDate;
                    TempData["EndDate"] = endDate;
                }
            }
            else
            {
                // Initialize a new query with current search parameters
                query = _db.ScanLog.Where(x =>
                    (string.IsNullOrEmpty(materialNumber) || x.MaterialNumber == materialNumber) &&
                    (string.IsNullOrEmpty(status) || x.Status == status) &&
                    (string.IsNullOrEmpty(supplier) || x.Supplier == supplier) &&
                    ((line == "None" || string.IsNullOrEmpty(line)) || (x.Line == line)) &&
                    (startDate == null || x.ScanDate >= startDateOnly) &&
                    (endDate == null || x.ScanDate <= endDateOnly)
                );

                // Store search parameters for subsequent requests
                TempData["MaterialNumber"] = materialNumber;
                TempData["Status"] = status;
                TempData["Supplier"] = supplier;
                if (line != "None")
                {
                    TempData["Line"] = line;
                }
                TempData["StartDate"] = startDate;
                TempData["EndDate"] = endDate;
            }

            return query;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult LogView(string materialNumber, string status, string supplier, string line, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 20)
        {
            var filteredLogs = FilterLogs(materialNumber, status, supplier, line, startDate, endDate);
            var distinctValues = _db.ScanLog.Select(x => x.Line).Distinct().ToList();

            // Pagination
            var pagedResults = filteredLogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = filteredLogs.Count();

            var viewModel = new SearchLogViewModel
            {
                Logs = pagedResults,
                DistinctLines = distinctValues,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                MaterialNumber = materialNumber,
                Status = status,
                Supplier = supplier,
                Line = line,
                StartDate = startDate,
                EndDate = endDate
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CancelFilter(int pageNumber = 1, int pageSize = 20)
        {
            TempData["MaterialNumber"] = "";
            TempData["Status"] = "";
            TempData["Line"] = "";
            TempData["StartDate"] = "";
            TempData["EndDate"] = "";

            var filteredLogs = _db.ScanLog;
            var distinctValues = _db.ScanLog.Select(x => x.Line).Distinct().ToList();

            // Pagination
            var pagedResults = filteredLogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = filteredLogs.Count();

            var viewModel = new SearchLogViewModel
            {
                Logs = pagedResults,
                DistinctLines = distinctValues,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View("LogView", viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SearchLogView(string materialNumber, string status, string supplier, string line, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 20)
        {
            var filteredLogs = FilterLogs(materialNumber, status, supplier, line, startDate, endDate);
            var distinctValues = _db.ScanLog.Select(x => x.Line).Distinct().ToList();

            // Pagination
            var pagedResults = filteredLogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var totalItems = filteredLogs.Count();

            var viewModel = new SearchLogViewModel
            {
                Logs = pagedResults,
                DistinctLines = distinctValues,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                MaterialNumber = materialNumber,
                Status = status,
                Supplier = supplier,
                Line = line,
                StartDate = startDate,
                EndDate = endDate
            };
            
            return View("LogView", viewModel);
        }


        //[HttpPost]
        [ValidateAntiForgeryToken]
        public Action Update(string AddManual, string pn_No, string id, string? supplier, string? prodDate, string? batchNo)
        {
            if (!string.IsNullOrWhiteSpace(AddManual))
            {
                var currentTime = DateTime.Now;
                var todayDate = DateOnly.FromDateTime(DateTime.Today);
                var todayTime = TimeOnly.FromDateTime(currentTime);
                var thirtyMinutesAgo = currentTime.AddMinutes(-30);

                AddManual = AddManual.Trim();
                if (id == null)
                {
                    id = TempData.Peek("id").ToString();
                }
                List<string> Db = new();
                TempData["id"] = id;

                var val1 = _db.Categories1.Where(j => j.MaterialNumber == pn_No);
                SelectedPN(pn_No);
                foreach (var obj in val1)
                {
                    Db.Add(obj.ComponentNumber);
                }

                if (AddManual != null)
                {

                    count = Db.Where(x => x.ToString().Equals(AddManual)).Count();
                    if (dups_store.ContainsKey(AddManual))
                    {
                        var duplist = _cache.GetObject<List<string>>(id);
                        bool allEqual = duplist.All(x => x == "none");

                        if (allEqual)
                        {
                            duplist = duplicate_list;
                        }
                        var AddManualIndex = duplist.IndexOf(AddManual);
                        if (AddManualIndex != -1)
                        {
                            var count_duplicates = _cache.GetObject<int>("count_duplicates");
                            codeValue = AddManual + "_" + AddManualIndex.ToString();
                            duplist[AddManualIndex] = "none";
                            var increment_count = count_duplicates + 1;
                            _cache.SetObject("count_duplicates", increment_count);
                            _cache.SetObject(id, duplist);
                            checkValid = _db.change_state.FirstOrDefault(n => n.id == id)?.state;

                            var newScanLogEntry = new ScanLog
                            {
                                Line = id,
                                MaterialNumber = pn_No,
                                ComponentNumber = AddManual,
                                Status = checkValid,
                                Supplier = supplier,
                                ProductionDate = prodDate,
                                BatchNumber = batchNo,
                                ScanDate = todayDate,
                                ScanTime = todayTime.ToString("HH:mm:ss"),
                            };

                            _db.ScanLog.Add(newScanLogEntry);
                            _db.SaveChanges();
                        }
                    }
                    var nn = _cache.GetObject<List<string>>(id);
                    checkValid = "absent";
                    if (Db.Contains(AddManual))
                    {
                        checkValid = "present";
                    }
                    else
                    {
                        Console.WriteLine("here");
                    }

                    _db.change_state.Where(u => u.id == id).ExecuteUpdate(b => b.SetProperty(u => u.state, checkValid));

                }
                duplicates.Count();

                if (codeValue == "")
                {
                    _db.scanner.Where(u => u.id == id).ExecuteUpdate(b => b.SetProperty(u => u.scan_value, AddManual));
                }
                else
                {
                    _db.scanner.Where(u => u.id == id).ExecuteUpdate(b => b.SetProperty(u => u.scan_value, codeValue));
                }

                if (Request.Method == "POST")
                {
                    var existingEntry = _db.ScanLog.Where(entry =>
                        entry.Line == id &&
                        entry.MaterialNumber == pn_No &&
                        entry.ComponentNumber == AddManual &&
                        entry.ScanDate == todayDate &&
                        entry.Status == checkValid)
                        .OrderBy(entry => entry.ScanTime)
                        .LastOrDefault();


                    if (existingEntry != null)
                    {
                        var todayDateTime = DateTime.Today;
                        var ScanTimeChanged = DateTime.ParseExact(existingEntry.ScanTime, "HH:mm:ss", CultureInfo.InvariantCulture);
                        if (thirtyMinutesAgo > ScanTimeChanged)
                        {
                            var newScanLogEntry = new ScanLog
                            {
                                Line = id,
                                MaterialNumber = pn_No,
                                ComponentNumber = AddManual,
                                Status = checkValid,
                                Supplier = supplier,
                                ProductionDate = prodDate,
                                BatchNumber = batchNo,
                                ScanDate = todayDate,
                                ScanTime = todayTime.ToString("HH:mm:ss"),
                            };

                            _db.ScanLog.Add(newScanLogEntry);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        var newScanLogEntry = new ScanLog
                        {
                            Line = id,
                            MaterialNumber = pn_No,
                            ComponentNumber = AddManual,
                            Status = checkValid,
                            Supplier = supplier,
                            ProductionDate = prodDate,
                            BatchNumber = batchNo,
                            ScanDate = todayDate,
                            ScanTime = todayTime.ToString("HH:mm:ss"),
                        };

                        _db.ScanLog.Add(newScanLogEntry);
                        _db.SaveChanges();
                    }
                }
            }
            

            return null;

        }

        public Action Line_No(SetVal data1)
        {
            if (data1.X6 != null)
            {
                TempData["id"] = data1.X6.ToUpper();
            }
            
            return null;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult serial_port_data([FromBody] CreateScannerDTO scannerDTO)
        {


            var id = scannerDTO.id;
            var supplier = scannerDTO.supplier;
            var prodDate = scannerDTO.prodDate;
            var batchNo = scannerDTO.batchNo;
            var MN = "";
            //string cookieValue = Request.Headers["Cookie"];

            var val1 = _db.scanner.Where(j => j.id == id);
            foreach (var obj in val1)
            {
                MN = obj.pn_no;
            }

            pageList = _db.Categories1.Where(j => j.MaterialNumber == MN).ToList();
            var scan__value = scannerDTO.scan_value;
            var plc_addr = scannerDTO.plc_address;

            
            _db.scanner.Where(u => u.id == id).ExecuteUpdate(b => b.SetProperty(u => u.scan_value, scan__value));
            _db.scanner.Where(u => u.id == id).ExecuteUpdate(b => b.SetProperty(u => u.pn_no, MN));
            _db.scanner.Where(u => u.id == id).ExecuteUpdate(b => b.SetProperty(u => u.plc_address, plc_addr));

            getValues(id);
            pageList.Count();
            Update(scan__value, MN, id, supplier, prodDate, batchNo);
            GetCurrent();

            return Ok();

        }

        [HttpGet]
        public ActionResult ReceivePlcData()
        {
            var id = "";
            if (TempData.Peek("id") != null)
            {
                id = TempData.Peek("id").ToString();
            }
            if (id != null)
            {
                var val1 = _db.scanner.Where(j => j.id == id);
                if (val1 != null)
                {
                    foreach (var obj in val1)
                    {

                        new_val = obj.plc_address;
                    }
                }
            }
            return Json(new_val, new System.Text.Json.JsonSerializerOptions());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult heartBeat([FromBody] CreateScannerDTO scannerDTO)
        {
            var id = scannerDTO.id;
            var MN = "";

            GetCurrent();

            return Ok();

        }


        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please upload a valid Excel file.";
                return RedirectToAction("Index");
            }

            try
            {
                // Define expected headers  
                var expectedHeaders = new[] { "MaterialNumber", "ComponentNumber", "ComponentDescription", "Quantity", "ChangeNumber" };
                List<Category> excelData = new List<Category>();

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    byte[] fileBytes = stream.ToArray();
                    // Log the first few bytes to check the file content
                    Console.WriteLine(BitConverter.ToString(fileBytes.Take(10).ToArray()));
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        // Validate headers
                        var actualHeaders = worksheet.Cells[1, 1, 1, worksheet.Dimension.Columns]
                                                     .Select(cell => cell.Text.Trim())
                                                     .ToArray();

                        if (!expectedHeaders.SequenceEqual(actualHeaders))
                        {
                            TempData["Error"] = "The uploaded file does not match the required format. Please download the template and try again.";
                            return RedirectToAction("Index");
                        }

                        // Read data starting from row 2
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var category = new Category
                            {
                                MaterialNumber = worksheet.Cells[row, 1].Text,
                                ComponentNumber = worksheet.Cells[row, 2].Text,
                                ComponentDescription = worksheet.Cells[row, 3].Text,
                                Quantity = Convert.ToDouble(worksheet.Cells[row, 4].Text),
                                ChangeNumber = worksheet.Cells[row, 5].Text,
                                CreateDateTime = DateTime.Now
                            };
                            excelData.Add(category);
                        }
                    }
                }

                // Delete existing MaterialNumbers in the database
                var materialNumbersToDelete = excelData.Select(x => x.MaterialNumber).Distinct();
                var recordsToDelete = _db.Categories1
                                         .Where(x => materialNumbersToDelete.Contains(x.MaterialNumber))
                                         .ToList();

                _db.Categories1.RemoveRange(recordsToDelete);
                await _db.SaveChangesAsync();

                // Batch insert new data
                int batchSize = 100;
                for (int i = 0; i < excelData.Count; i += batchSize)
                {
                    var batch = excelData.Skip(i).Take(batchSize).ToList();
                    _db.Categories1.AddRange(batch);
                    await _db.SaveChangesAsync();
                }

                TempData["Success"] = "Data imported successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DownloadTemplate()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "UploadFormat-template.xlsx");
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UploadFormat-template.xlsx");
        }

    }
}