using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Simeon_Georgiev_employees.Contracts;
using Simeon_Georgiev_employees.Models;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace Simeon_Georgiev_employees.Controllers
{
    public class UploadCSVController : Controller
    {
        private readonly ICSVService csvService;

        public UploadCSVController(ICSVService _csvService)
        {
            this.csvService = _csvService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(IFormFile postedFile)
        {
            if (postedFile != null)
            {
                string filePath = Path.GetFullPath(postedFile.FileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    Delimiter = ", "
                };
                var textReader = new StreamReader(filePath);
                using var csv = new CsvReader(textReader, configuration);
                IEnumerable<RowDto> records = csv.GetRecords<RowDto>();
                List<Row> rows = new List<Row>();
                foreach(RowDto record in records)
                {
                    
                    DateOnly dateTo;
                    try
                    {
                        dateTo = DateOnly.Parse(record.DateTo);
                    }
                    catch(Exception e) 
                    {
                        dateTo = DateOnly.FromDateTime(DateTime.Now);
                    }
                    Row row = new Row
                    {
                        EmpId = record.EmpId,
                        ProjectId = record.ProjectId,
                        DateTo= dateTo,
                        DateFrom = record.DateFrom
                    };
                    rows.Add(row);
                }
                OutputEmployees output = csvService.getResult(rows);
                return View(output);
            }
            return View();
        }
    }
}
