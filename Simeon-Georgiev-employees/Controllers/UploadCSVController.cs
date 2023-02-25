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
                IEnumerable<Row> records = csv.GetRecords<Row>();
                List<Row> rows = new List<Row>();
                foreach(Row record in records)
                {
                    rows.Add(record);
                }
                OutputEmployees output = csvService.getResult(rows);
                return View(output);
            }
            return View();
        }
    }
}
