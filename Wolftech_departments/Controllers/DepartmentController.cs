using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using WolftechApi.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Newtonsoft.Json;
using WolftechApi.Data;

namespace WolftechApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger _logger;
        private DataLoading dataObject;
        public DepartmentController(ILogger<DepartmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            try 
            {
                _logger.LogInformation("Loading and parsing data from disk.");
                dataObject = new DataLoading("./Data/departments.csv");
            }
            catch (Exception e)
            {
                string msg = $"\nERROR! Data parsing failed with message: {e.Message}";
                _logger.LogError(msg);
                return BadRequest(msg);
            }
            
            return JsonConvert.SerializeObject(dataObject.outputList);
        }
    }
}