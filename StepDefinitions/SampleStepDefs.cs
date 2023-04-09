using SampleProject.CorePackage;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleProject.Pages;
using SampleProject.Config;
using BDD_Core.Utils;

namespace SampleProject.StepDefinitions
{
    [Binding]
    public class SampleStepDefs
    {
        private ScenarioContext _scenarioContext;
        private DriverHelper _driverHelper;
        public SampleStepDefs(DriverHelper driverHelper, ScenarioContext scenarioContext)
        {
            _driverHelper = driverHelper;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I launch the yatra app")]
        public void GivenILaunchTheYatraApp()
        {
            _driverHelper.Driver.Navigate().GoToUrl(Settings.AUT); ;
        }

        [Then(@"I click on round trip button")]
        public void ThenIClickOnRoundTripButton()
        {
            _driverHelper.CurrentPage.As<HomePage>().clickOnRoundTripButton();
        }

        [Then(@"I enter credentials for ""([^""]*)"" from the file ""([^""]*)"" and click login")]
        public void ThenIEnterCredentialsForFromTheFileAndClickLogin(string userType, string fileName)
        {
            var inputData = new JSONHelper().ReadInputData(userType, fileName);
            _scenarioContext["testData"] = inputData;
            Console.WriteLine("User Name : "+inputData["username"].ToString());
            Console.WriteLine("Password : " + inputData["password"].ToString());

        }

        [Then(@"I read data from excel sheet ""([^""]*)"" for scenario ""([^""]*)""")]
        public void ThenIReadDataFromExcelSheetForScenario(string SheetName, string ScenarioName)
        {
            _driverHelper.CurrentPage.As<HomePage>().ReadDataFromExcel(SheetName, ScenarioName);
        }


    }
}
