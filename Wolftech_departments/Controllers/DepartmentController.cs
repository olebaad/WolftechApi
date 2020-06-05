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
        public ActionResult<List<Department>> Get()
        {
            //var reader = new StreamReader("./departments.csv");
            //var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            //var dpts = csvReader.GetRecords<LoadingClass>();
            //var departments = dpts.ToList();

            //var result = CreateDepartmentList(departments);

            //return result;

            try 
            {
                _logger.LogDebug("Loading and parsing data from disk.");
                dataObject = new DataLoading("./Data/departments.csv", _logger);
            }
            catch (Exception e)
            {
                _logger.LogError($"\nERROR! Data parsing failed with message: {e.Message}");
            }
            //DataLoading dataObject = new DataLoading("./Data/departments.csv", _logger);
            
            return dataObject.outputList;
        }

    //    private List<Department> CreateDepartmentList(List<LoadingClass> inputList)
    //    {
    //        List<int> parentIds = new List<int>();
    //        int insertValue;
    //        // Creating a list of parent IDs to ease the computational load in future operations
    //        foreach (LoadingClass dept in inputList)
    //        {
    //            if (dept.DepartmentParent_OID != null)
    //            {
    //                insertValue = dept.DepartmentParent_OID ?? default(int); //converting from nullable int
    //                parentIds.Add(insertValue);
    //            }
    //        }
               
    //        List<Department> result = new List<Department>();
    //        int counter = 0;
    //        foreach (LoadingClass dept in inputList)
    //        {
    //            if (dept.DepartmentParent_OID == null)
    //            {
    //                List<Department> children = new List<Department>();
    //                children = CreateInternalDepartmentList(inputList, dept, children, counter, parentIds);
                    
    //                result.Add(new Department(dept, children));
    //            }
    //            counter++;
    //        }

    //        return result;
    //    }

    //    private List<Department> CreateInternalDepartmentList (List<LoadingClass> input, LoadingClass parent, List<Department> listOfParentsChildren, int errorParameter, List<int> parentIds)
    //    {
    //        if (errorParameter >= input.Count()) 
    //        {
    //            throw new Exception($"Error in CreateInternalDepartmentList(), index:  {errorParameter} is out of range.");
    //        }


    //        List<Department> decendantsOfChild;


    //        if (parentIds.Contains(parent.OID))
    //        {
    //            int errParam = 0; //Parameter avoiding infinite loop
    //            foreach(LoadingClass dept in input)
    //            {
    //                if (dept.DepartmentParent_OID == parent.OID)
    //                {
    //                    decendantsOfChild = new List<Department>();
    //                    decendantsOfChild = CreateInternalDepartmentList(input, dept, decendantsOfChild, errParam, parentIds);
    //                    Department currentDepartment = new Department(dept, decendantsOfChild);
    //                    listOfParentsChildren.Add(currentDepartment);
    //                }
    //                errParam++;
    //            }
                
    //        }

    //        return listOfParentsChildren;
            
    //    }
    }
}