using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace SampleProject.CorePackage
{
    public class DriverHelper
    {
        public IWebDriver Driver { get; set; }

        public PageInstance CurrentPage { get; set; }

        public MediaEntityModelProvider CaptureScreenshot(string name)
        {
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString, name).Build();
        }
    }
}
