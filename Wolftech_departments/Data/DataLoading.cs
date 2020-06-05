using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WolftechApi.Models;

namespace WolftechApi.Data
{
    public class DataLoading
    {
        private List<LoadingClass> inputList { get; set; }
        private List<int> parentIds { get; set; }
        private readonly ILogger _logger;
        private CsvReader csvReader { get; set; }
        private StreamReader reader { get; set; }
        public List<Department> outputList { get; set; }


        public DataLoading(string pathToDataFile, ILogger logger)
        {
            _logger = logger;
            try
            {
                reader = new StreamReader(pathToDataFile);
            }
            catch(IOException e)
            {
                _logger.LogError($"\nERROR! Reading data from {pathToDataFile} failed with the following error message:\n {e.Message}");
                throw (new IOException("Loading data from disk using SteamReader failed"));
            }

            try
            {
                csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

                IEnumerable<LoadingClass> dpts = csvReader.GetRecords<LoadingClass>();
                inputList = dpts.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"\nERROR! Parsing data from disk failed with the following error message:\n {e.Message}");
                throw (new Exception("Parsing data to List of LoadingClass failed"));
            }


            parentIds = new List<int>();
            int insertValue;
            // Creating a list of parent IDs for processing methods
            foreach (LoadingClass dept in inputList)
            {
                if (dept.DepartmentParent_OID != null)
                {
                    insertValue = dept.DepartmentParent_OID ?? default(int); //converting from nullable int
                    parentIds.Add(insertValue);
                }
            }

            try
            {
                outputList = CreateDepartmentList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Paring data to correct structure failed with the following error message: {e.Message}");
                throw (e);
            }
        }
        private List<Department> CreateDepartmentList()
        {
            List<Department> result = new List<Department>();
            int counter = 0;
            foreach (LoadingClass dept in inputList)
            {
                if (dept.DepartmentParent_OID == null)
                {
                    List<Department> children = new List<Department>();
                    children = CreateInternalDepartmentList(dept, children, counter);

                    result.Add(new Department(dept, children));
                }
                counter++;
            }

            return result;
        }

        private List<Department> CreateInternalDepartmentList(LoadingClass parent, List<Department> listOfParentsChildren, int errorParameter)
        {
            if (errorParameter >= inputList.Count())
            {
                throw new Exception($"Error in CreateInternalDepartmentList(), index:  {errorParameter} is out of range.");
            }


            List<Department> decendantsOfChild;


            if (parentIds.Contains(parent.OID))
            {
                int errParam = 0; //Parameter avoiding infinite loop
                foreach (LoadingClass dept in inputList)
                {
                    if (dept.DepartmentParent_OID == parent.OID)
                    {
                        decendantsOfChild = new List<Department>();
                        decendantsOfChild = CreateInternalDepartmentList(dept, decendantsOfChild, errParam);
                        Department currentDepartment = new Department(dept, decendantsOfChild);
                        listOfParentsChildren.Add(currentDepartment);
                    }
                    errParam++;
                }

            }

            return listOfParentsChildren;

        }
    }
}
