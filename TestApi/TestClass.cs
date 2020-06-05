using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using WolftechApi.Controllers;
using WolftechApi.Data;
using WolftechApi.ExeptionClasses;
using WolftechApi.Models;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestApi
{
    public class TestClass
    {
        private List<LoadingClass> testData;
        private List<Department> testOutput;
        private List<int> parentCorrectList;
        private string path;
        private Logger<DepartmentController> _logger;

        public TestClass()
        {
            _logger = new Logger<DepartmentController>(new LoggerFactory());
            testData = new List<LoadingClass>
            {
                new LoadingClass
                {
                    OID = 1,
                    Title = "US News",
                    Color = "#F52612",
                    DepartmentParent_OID = null
                },
                new LoadingClass
                {
                    OID = 2,
                    Title = "Crime + Justice",
                    Color = "#F52612",
                    DepartmentParent_OID = 3
                },
                new LoadingClass
                {
                    OID = 3,
                    Title = "Energy",
                    Color = "#F52612",
                    DepartmentParent_OID = 1
                },
                new LoadingClass
                {
                    OID = 4,
                    Title = "Weather",
                    Color = "#F52612",
                    DepartmentParent_OID = 1
                },
                new LoadingClass
                {
                    OID = 5,
                    Title = "Science",
                    Color = "#F52612",
                    DepartmentParent_OID = 1
                },

            };
            
            parentCorrectList = new List<int> { 3, 1, 1, 1 };

            testOutput = new List<Department>
            {
                new Department(testData[0], new List<Department>
                {
                    new Department(2, "Crime + Justice", "#F52612", 0),
                    new Department(3, "Energy", "#F52612", 0),
                    new Department(4, "Weather", "#F52612", 0),
                    new Department(5, "Science", "#F52612", 0),
                }),
            };

            // Find testdata location
            string fullPath = Directory.GetCurrentDirectory();
            int indx = fullPath.IndexOf("TestApi");
            path = fullPath.Substring(0, indx + 7);
        }

        /**
         * Testing the MakeListOfParents() function
         */
        [Fact]
        public void TestMakeListOfParents()
        {
            DataLoading testObject = new DataLoading(testData);
            CollectionAssert.AreEqual(parentCorrectList, testObject.MakeListOfParents());
        }

        /**
         * Testing that the size of the final list is as expected
         */
        [Fact]
        public void TestCreateDepartmentListSize1()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual(1, testObject.outputList.Count);
        }

        /**
         * Testing that the size of the list of direct children to first entry in output is as expected 
         */
        [Fact]
        public void TestCreateDepartmentListSize2()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual(3, testObject.outputList[0].Departments.Count);
        }

        /**
         * Testing that the size of the list of children of the first child of the first entry in the output 
         * is as expected 
         */
        [Fact]
        public void TestCreateDepartmentListSize3()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual(1, testObject.outputList[0].Departments[0].Departments.Count);
        }

        /**
         * The following 5 tests are designed to check that the API supports a hierarchy structure 
         * of any depth, as required in the specifications.
         */
        [Fact]
        public void TestCreateDepartmentListFunction1()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual("US News", testObject.outputList[0].Title);
        }

        [Fact]
        public void TestCreateDepartmentListFunction2()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual("Energy", testObject.outputList[0].Departments[0].Title);
        }

        [Fact]
        public void TestCreateDepartmentListFunction3()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual("Crime + Justice", testObject.outputList[0].Departments[0].Departments[0].Title);
        }

        [Fact]
        public void TestCreateDepartmentListFunction4()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual(0, testObject.outputList[0].Departments[0].Departments[0].Departments.Count);
        }

        [Fact]
        public void TestCreateDepartmentListFunction5()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual(0, testObject.outputList[0].Departments[1].Departments.Count);
        }

        /**
         * Test to check if the number of decendants is calculated correctly
         */
        [Fact]
        public void TestNumDecendants()
        {
            DataLoading testObject = new DataLoading(testData);
            Assert.AreEqual(4, testObject.outputList[0].NumDecendants);
        }

        /**
         * Test checking if data loads without errors
         */
        [Fact]
        public void TestLoadMyData()
        {
            new DataLoading(path + "/testDepartments.csv");
        }

        /**
         * Test checking if correct error is thrown if SteamReader fails
         */
        [Fact]
        public void TestLoadMyDataError()
        {
            DataLoading testObj = new DataLoading(path + "/testDepartments.csv");
            Assert.ThrowsException<LoadingError>(
                () => testObj.LoadMyData("./thisShouldNotExist")
            );
        }

        /**
         * Test checking if correct error is thrown if ParseMyData fails
         */
        [Fact]
        public void TestParseMyDataError()
        {
            DataLoading testObj = new DataLoading(path + "/testDepartments.csv");

            Assert.ThrowsException<ParsingError>(
                () => testObj.ParseMyData(new StreamReader(path+"/TestClass.cs"))
            );
        }

        /**
         * Testing if GET request returns a value
         */
        [Fact]
        public void TestController ()
        {
            var controller = new DepartmentController(_logger);
            var actionResult = controller.Get();
            Assert.IsNotNull(actionResult);
        }
    }
}
