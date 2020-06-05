using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WolftechApi.ExeptionClasses;
using WolftechApi.Models;

namespace WolftechApi.Data
{
    public class DataLoading
    {
        private List<LoadingClass> inputList { get; set; }
        private List<int> parentIds { get; set; }
        private CsvReader csvReader { get; set; }
        private StreamReader reader { get; set; }
        public List<Department> outputList { get; set; }


        public DataLoading(string path)
        {
            LoadMyData(path); //Loading data 

            ParseMyData(reader); //Parsing loaded data to LoadingClass
            MakeListOfParents(); //Creating a list of OIDs that are registered as parents
            outputList = CreateDepartmentList(); //Creating the required return structure
        }

        public DataLoading(List<LoadingClass> testData) //constructor for testing methods
        {
            inputList = testData;
            MakeListOfParents(); //Creating a list of OIDs that are registered as parents       
            outputList = CreateDepartmentList(); //Creating the required return structure    
        }

        public void LoadMyData(string path)
        {
            try
            {
                reader = new StreamReader(path);
            }
            catch (Exception e)
            {
                throw (new LoadingError($"Loading data from disk using StreamReader failed with the following message:\n {e.Message}"));
            }

        }

        public void ParseMyData(StreamReader inputReader)
        {
            try
            {
                csvReader = new CsvReader(inputReader, CultureInfo.InvariantCulture);

                IEnumerable<LoadingClass> dpts = csvReader.GetRecords<LoadingClass>();
                inputList = dpts.ToList();
            }
            catch (Exception e)
            {
                throw (new ParsingError($"Parsing data to List of LoadingClass failed with the following message:\n {e.Message}"));
            }
        }

        public List<int> MakeListOfParents()
        {
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

            return parentIds;
        }
        public List<Department> CreateDepartmentList()
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


            List<Department> decendantsOfChild; //Initialiszing list of potential children


            if (parentIds.Contains(parent.OID))
            {
                int errParam = 0; //Parameter avoiding infinite loop
                foreach (LoadingClass dept in inputList)
                {
                    if (dept.DepartmentParent_OID == parent.OID) //if current department examined is a child of 'parent' object
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
