using Microsoft.AspNetCore.Mvc;
using migration.Models;
using migration.Services;
using System.Linq;

namespace migration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly CsvService _csvService;
        private readonly TemplateService _templateService;
        private static List<InventoryItem> _loadedData = new List<InventoryItem>();

        public MigrationController(CsvService csvService, TemplateService templateService)
        {
            _csvService = csvService;
            _templateService = templateService;
        }

        // Test endpoint
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Migration API is working!", itemCount = _loadedData.Count });
        }

        // Simple load with hardcoded path
        [HttpGet("load-sample")]
        public IActionResult LoadSampleCsv()
        {
            try
            {
                string filePath = "/Users/d.imtiaz/Desktop/sample_data.csv";
                _loadedData = _csvService.LoadFromFile(filePath);
                return Ok(new {
                    success = true,
                    message = $"Loaded {_loadedData.Count} items successfully",
                    itemCount = _loadedData.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }



        // Load CSV data
        [HttpPost("load")]
        public IActionResult LoadCsv([FromBody] LoadCsvRequest request)
        {
            try
            {
                _loadedData = _csvService.LoadFromFile(request.FilePath);
                return Ok(new {
                    success = true,
                    message = $"Loaded {_loadedData.Count} items successfully",
                    itemCount = _loadedData.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Get data preview
        [HttpGet("preview")]
        public IActionResult GetPreview()
        {
            if (_loadedData.Count == 0)
            {
                return BadRequest(new { success = false, message = "No data loaded" });
            }

            return Ok(new {
                success = true,
                data = _loadedData.Take(10).ToList(),
                totalCount = _loadedData.Count
            });
        }

        // Generate report
        [HttpGet("report")]
        public IActionResult GenerateReport()
        {
            if (_loadedData.Count == 0)
            {
                return BadRequest(new { success = false, message = "No data loaded" });
            }

            try
            {
                string htmlContent = _templateService.GenerateReport(_loadedData);
                return Ok(new {
                    success = true,
                    htmlContent = htmlContent,
                    message = "Report generated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    public class LoadCsvRequest
    {
        public string FilePath { get; set; } = "";
    }
}