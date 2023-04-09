using SampleProject.Config;
using SampleProject.SupportFunctions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SampleProject.CorePackage
{
    public class TestInitializeHooks : Steps
    {

        private DriverHelper _driver;
        public TestInitializeHooks(DriverHelper driver)
        {
            _driver = driver;
        }

        public void InitializeSettings()
        {
            //Set all the settings for framework
         //   ConfigReader.SetFrameworkSettings();
        }

        public void InitializeBrowser()
        {
            //Set Log
            //   LogHelpers.CreateLogFile();

            //Open Browser
            OpenBrowser(Settings.BrowserType);

            //    LogHelpers.Write("Initialized framework");

        }

        private void OpenBrowser(BrowserType browserType = BrowserType.Chrome)
        {
            var DownloadLocation = FileHandler.GetFilesDownloadLoation();
      
            switch (browserType)
            {
                case BrowserType.Edge:
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--start-maximized");
                    edgeOptions.AddArguments("--browser.download.dir=" + DownloadLocation);
                    edgeOptions.AddUserProfilePreference("download.default_directory", DownloadLocation);
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    _driver.Driver = new EdgeDriver(edgeOptions);

                    break;
                case BrowserType.FireFox:

                    break;
                case BrowserType.Chrome:
                    ChromeOptions option = new ChromeOptions();
                    //var caps = new ChromeOptions();
                    option.AddArgument("start-maximized");
                    option.AddArgument("--disable-gpu");

                    option.AddArguments("--browser.download.dir=" + DownloadLocation);
                    option.AddUserProfilePreference("download.default_directory", DownloadLocation);
                    //   option.AddArgument("--headless");
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    //_driver.Driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), caps);
                    _driver.Driver = new ChromeDriver(option);

                    break;
            }
           

        }

    }
}
