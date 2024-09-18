using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Utilities.FileHelper;

namespace GeoCodeUtilityTests
{
    [TestClass]
    public class GeoCodeIntegrationTests
    {
        private static DataTable _DataTable = new DataTable();
        private static string _UtilityPath;
        private static string _APIKey;
        public static List<string> output = new List<string>();
        private string ExpectedResult;
        private string TestData;
        public TestContext TestContext { get; set; }

        private string _TestData;

        [ClassInitialize]
        public static void InitializeTestSuite(TestContext testContext)

        {
            _DataTable = FileHelper.ReadAsDataTable("GeoCodeTestData.xlsx");
            // _APIKey = testContext.Properties["APIKey"].ToString();
            _UtilityPath = "geocodeUtility.exe";
        }

        [TestMethod]
        public async Task ValidLocationName_Single()
        {
            
             ReturnTestData("ValidLocationName_Single");
             output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
            
        }

        [TestMethod]
        public async Task ValidLocationNameAndZipCode()
        {
            
            ReturnTestData("ValidLocationNameAndZipCode");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));

        }

        [TestMethod]
        public async Task ValidZipCode_Single()
        {
            
            ReturnTestData("ValidZipCode_Single");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        [TestMethod]
        public async Task ValidLocationName_Multiple()
        {

            ReturnTestData("ValidLocationName_Multiple");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }

        [TestMethod]
        public async Task DisplayMessageWhenNoInputs()
        {

            ReturnTestData("DisplayMessageWhenNoInputs");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        [TestMethod]
        public async Task InvalidLocationName_Single()
        {

            ReturnTestData("InvalidLocationName_Single");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        [TestMethod]
        public async Task InvalidZipCode_Single()
        {

            ReturnTestData("InvalidZipCode_Single");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        [TestMethod]
        public async Task ValidLocatoinName_InvalidZipCode()
        {

            ReturnTestData("ValidLocatoinName_InvalidZipCode");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }



        [TestMethod]
        public async Task InvalidLocatoinName_ValidZipCode
()
        {

            ReturnTestData("InvalidLocatoinName_ValidZipCode");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        [TestMethod]
        public async Task LocationNameWithSpecialChar_Single()
        {

            ReturnTestData("LocationNameWithSpecialChar_Single");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }

        [TestMethod]
        public async Task ZipCodeWithSpecialChar_Single()
        {

            ReturnTestData("ZipCodeWithSpecialChar_Single");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }

        [TestMethod]
        public async Task InvalidLocationName_InvalidZipCode()
        {

            ReturnTestData("InvalidLocationName_InvalidZipCode");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        [TestMethod]
        public async Task ValidLocationNameWithUnicode_Single()
        {

            ReturnTestData("ValidLocationNameWithUnicode_Single");
            output = await RungeoCodeUtility(TestData);
            Assert.IsTrue(ValidateOutput(output));
        }


        private void ReturnTestData(string identifier)
        {
            DataRow dataRow = _DataTable.Select("Identifier='" + identifier + "'")[0];

            TestData =  dataRow["TestData"].ToString();
            ExpectedResult = dataRow["Expected"].ToString();

        }

        private async Task<List<string>> RungeoCodeUtility(string args)
        {
            var processStartInfo = new ProcessStartInfo();

            processStartInfo.FileName = _UtilityPath;
            
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = false;
            if (!(args.ToLower().Contains("no inputs"))) {
                processStartInfo.Arguments = args;
            }

            
            
            using (var process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.Start();
                string result = await process.StandardOutput.ReadToEndAsync();
                List<string> resultList = new List<string>();
                foreach (string line in result.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries))
                { 

                    resultList.Add(line);
                }

                process.WaitForExit();
                return resultList;
            }
        }


        private bool ValidateOutput(List<string> TestOutput) {
            string[] TestDataArray = ExpectedResult.Split(',').ToArray();

            TestOutput.Sort();
            Array.Sort(TestDataArray);
            

            for (int i = 0; i<= TestOutput.Count-1;i++)
            {
                if (!TestOutput[i].Contains(TestDataArray[i])) { 
                    return false;
                }
            }
            return true;


        }


       

        [TestInitialize]
        public  void TestSetup()
        { 
            output.Clear();
        }

        [TestCleanup]
        public  void TestFinalize()
        {

            FileHelper.UpdateCell("GeoCodeTestData.xlsx", TestContext.CurrentTestOutcome.ToString(), TestContext.TestName,  string.Join(",", output.ToArray()));
            foreach (var item in output)
            {
                Console.WriteLine(item.ToString());
            }

            
        }
    }
}