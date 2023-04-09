using BDD_AMN.Utils;
using OpenQA.Selenium;
using SampleProject.CorePackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Pages
{
    public class HomePage : BasePage
    {
        private DriverHelper _driverContext;

        public HomePage(DriverHelper driverContext) : base(driverContext)
        {
            _driverContext = driverContext;
        }

        By RoundTripBtn => By.XPath("//a[@title='Round Trip']");

        public void clickOnRoundTripButton()
        {
            Click(RoundTripBtn);
        }

        public void ReadDataFromExcel(string SheetName, string ScenarioName)
        {
            Dictionary<string, string> ExcelData = ExcelHelper.ReadDataFromExcel(SheetName, ScenarioName);
            Console.WriteLine("Data From Excel = " + ExcelData["Username"]);
            Console.WriteLine("Data From Excel = " + ExcelData["Password"]);
        }
    }
}
