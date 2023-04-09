using SampleProject.SupportFunctions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SampleProject.CorePackage
{
    public class BasePage : PageInstance
    {
        private DriverHelper _driverContext;


        public BasePage(DriverHelper drivercontext) : base(drivercontext)
        {
            this._driverContext = drivercontext;

        }

        protected const int DefaultTimeout = 30;

        // Object for storing Current window handle
        protected string CurrentWindow { get; set; }


        /// <summary>
        /// Wait for page load 
        /// </summary>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected void WaitforPageLoad(int timeout = DefaultTimeout)
        {
            IWait<IWebDriver> wait = new WebDriverWait(_driverContext.Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(driver1 => ((IJavaScriptExecutor)_driverContext.Driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        /// <summary>
        /// Wait for an element 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected IWebElement WaitOn(By by, int timeout = DefaultTimeout)
        {
            WaitforPageLoad();
            var wait = new WebDriverWait(_driverContext.Driver, TimeSpan.FromSeconds(timeout));
            wait.IgnoreExceptionTypes((typeof(NoSuchElementException)));
            wait.IgnoreExceptionTypes((typeof(ElementNotVisibleException)));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            try
            {
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));

            }
            catch (Exception)
            {
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));

            }
        }

        /// <summary>
        /// This will input text into the element, applicable webelement textbox, textarea
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <param name="specifiedTimeout"></param>
        protected void SetText(By by, string value, int timeout = DefaultTimeout)
        {
            WaitOn(by, timeout).SendKeys(value);
            Console.WriteLine($"{DateTime.Now}:Entered text as '{value}' using By Reference:" + by);
        }

        /// <summary>
        /// Click with wait time
        /// </summary>
        /// <param name="element"></param>
        protected void Click(By by, int timeout = DefaultTimeout)
        {
            int attempts = 0;
            while (attempts < 2)
            {
                try
                {
                    WaitOn(by, timeout).Click();
                    Console.WriteLine($"{DateTime.Now}:Clicked the element using By Reference:" + by);
                    break;
                }
                catch (StaleElementReferenceException)
                {
                    _driverContext.Driver.Navigate().Refresh();
                }
                attempts++;
            }

        }

        /// <summary>
        /// Wait for an element and verify the element is displayed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected bool IsDisplayed(By by, int timeout = DefaultTimeout)
        {
            try
            {
                if (WaitOn(by, timeout).Displayed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e) { Console.WriteLine($"{DateTime.Now} Elemet not visible using reference {by}\nMessage : {e.Message}"); return false; }
        }

        /// <summary>
        ///Mouse hover using action class
        /// </summary>
        /// <param name="element"></param>
        protected void MouseHover(By by, int timeout = DefaultTimeout)
        {
            Actions act = new Actions(_driverContext.Driver);
            act.MoveToElement(WaitOn(by, timeout)).DoubleClick().Build().Perform();
        }

        /// <summary>
        ///Text comparison between WebelementList and string list 
        /// </summary>
        /// <param name="WebelementList"></param>
        /// <param name="stringList"></param>
        protected bool ListToWebElementComparison(IList<IWebElement> ElementList, List<string> ExpectedValueList)
        {
            bool x = true;
            for (int i = 0; i < ElementList.Count; i++)
            {

                if (ElementList[i].Text != ExpectedValueList[i])
                {
                    x = false;
                    break;
                }
            }
            return x;
        }

        /// <summary>
        /// Wait for an element to be Invisible
        /// </summary>
        /// <param name="by"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected void WaitUntilElementInvisible(By by, int timeout = DefaultTimeout)
        {
            var wait = new WebDriverWait(_driverContext.Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(by));
        }

        /// <summary>
        /// This will read the text value of a webelement identified using by
        /// </summary>
        /// <param name="by"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected string GetText(By by, int timeout = DefaultTimeout)
        {
            if (IsDisplayed(by))
            {
                return WaitOn(by, timeout).Text;
            }
            return string.Empty;
        }


        /// <summary>
        /// This will read only character text value of a webelement identified using by
        /// </summary>
        /// <param name="by"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected string GetOnlyChar(By by, int timeout = DefaultTimeout)
        {
            if (IsDisplayed(by))
            {
                return new String((WaitOn(by, timeout).Text).Where(Char.IsLetter).ToArray());
            }
            return string.Empty;
        }


        /// <summary>
        /// Wait for an element to be clickable 
        /// </summary>
        /// <param name="by"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected void IsClickable(By by, int timeout = DefaultTimeout)
        {
            var wait = new WebDriverWait(_driverContext.Driver, TimeSpan.FromSeconds(timeout));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));


        }

        /// <summary>
        /// This will input text into the element, applicable webelement Autosuggest dropdown
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <param name="specifiedTimeout"></param>
        /// /// </summary>
        protected void FillAutoSuggestDropDown(By by, string value, int timeout = DefaultTimeout)
        {
            SetText(by, value + Keys.Enter);
            Console.WriteLine($"{DateTime.Now}:Entered text as '{value}' using By Reference:" + by);
        }
        protected string GetSubText(By by, char Char,int timeout = DefaultTimeout)
        {
            if (IsDisplayed(by))
            {
                string text = WaitOn(by, timeout).Text;
                return text.Substring(0, text.IndexOf(Char)).Trim();
            }
            return string.Empty;
        }

        /// <summary>
        /// Check whether specified file exists or not 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected bool FileExists(string filename, int timeout)
        {
            string filePath = Path.Combine(FileHandler.DownloadLocation, filename);
            if (File.Exists(filePath)) File.Delete(filePath);
            bool fileExists = false;
            IWait<IWebDriver> wait = new WebDriverWait(_driverContext.Driver, TimeSpan.FromSeconds(timeout));
            wait.Until<bool>(x => fileExists = File.Exists(filePath));
            return File.Exists(filePath);

        }

        /// <summary>
        /// This will return the browser title
        /// </summary>
        protected string GetTitle()
        {
            return _driverContext.Driver.Title;
        }

        /// <summary>
        /// This function will switch to new tab
        /// </summary>
        protected void SwtichToNewTab(string Title)
        {
            CurrentWindow = _driverContext.Driver.CurrentWindowHandle;
            foreach (var window in _driverContext.Driver.WindowHandles)
            {
                if (window != CurrentWindow)
                {
                    _driverContext.Driver.SwitchTo().Window(window);
                    if (GetTitle().Contains(Title))
                    {
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// Close the current working window
        /// </summary>
        protected void CloseActiveDriver()
        {
            _driverContext.Driver.Close();
        }

        /// <summary>
        /// Switch to default content
        /// </summary>
        protected void SwitchToParentWindow()
        {
            _driverContext.Driver.SwitchTo().Window(_driverContext.Driver.WindowHandles[0]);
        }

        /// <summary>
        /// This is used to fetch data from tables based on row and column numbers
        /// </summary>
        /// <param name="TableIdentifier"></param>
        /// <param name="RowRange"></param>
        /// <param name="ColRange"></param>
        /// <returns></returns>
        protected List<string> TableParsingCustomMethodToRetrieveText(string TableIdentifier,int RowRange, int ColRange)
        {
            List<string> ValueList = new List<string>();
       
            for(int i = 1;i<=RowRange;i++)
            {
                for(int j=1; j <= ColRange; j++)
                {
                    ValueList.Add(Regex.Replace(_driverContext.Driver.FindElement(By.XPath(TableIdentifier + "[" + i + "]" + "/div/div" + "[" + j + "]")).Text, @"\s+", ""));
                    Console.WriteLine("Table Values :" + ValueList[j - 1]);
                }
            }

            return ValueList;
        }

        /// <summary>
        /// This is used to fetch data from tables based on row and column numbers, ignoring any speific index
        /// </summary>
        /// <param name="TableIdentifier"></param>
        /// <param name="RowRange"></param>
        /// <param name="ColRange"></param>
        /// <param name="IgnoreIndex"></param>
        /// <returns></returns>

        protected List<string> TableParsingCustomMethodToRetrieveTextIgnoreIndex(string TableIdentifier, int RowRange, int ColRange, int IgnoreIndex)
        {
            List<string> ValueList = new List<string>();

            for (int i = 1; i <= RowRange; i++)
            {
                for (int j = 1; j <= ColRange; j++)
                {
                    if (j != IgnoreIndex)
                    {
                        ValueList.Add(Regex.Replace(_driverContext.Driver.FindElement(By.XPath(TableIdentifier + "[" + i + "]" + "/div/div" + "[" + j + "]")).Text, @"\s+", ""));
                    }
                }
            }

            return ValueList;
        }

        /// <summary>
        /// This will return True/False comparing two lists
        /// </summary>
        /// <param name="ActualValues"></param>
        /// <param name="ExpectedValues"></param>
        /// <returns></returns>
        protected bool CompareTwoLists(List<string> ActualValues, List<string> ExpectedValues)
        {
            return ActualValues.SequenceEqual(ExpectedValues);
        }

        /// <summary>
        /// This will return True/False comparing two strings
        /// </summary>
        /// <param name="Actual"></param>
        /// <param name="Expected"></param>
        /// <returns></returns>
        protected bool CompareTwoStringValues(String Actual, String Expected)
        {
            return Actual.Equals(Expected);
        }
        /// <summary>
        /// This function will return Text from Element without spaces
        /// </summary>
        /// <param name="by"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        protected string GetTextWithSpaceDeleted(By by, int timeout = DefaultTimeout)
        {
            if (IsDisplayed(by))
            {
                Console.WriteLine("Text Value : " + Regex.Replace(WaitOn(by, timeout).Text, @"\s+", ""));
                return Regex.Replace(WaitOn(by, timeout).Text, @"\s+", "");      
            }
            return string.Empty;
            
        }


        /// <summary>
        /// Move to an e;ement and perform click operation
        /// </summary>
        /// <param name="by"></param>
        /// <param name="specifiedTimeout"></param>
        /// <returns></returns>
        protected void MoveToElementAndClick(By by,int timeout=DefaultTimeout)
        {
            Actions act = new Actions(_driverContext.Driver);
            act.MoveToElement(WaitOn(by, timeout)).Click().Build().Perform();
        }

        /// <summary>
        /// Convert By to Ilist of webelements
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public IList<IWebElement> ByToIWebElementList(By by)
        {
            WaitOn(by);
            return _driverContext.Driver.FindElements(by);
        }


        /// <summary>
        /// Webelement compare and perform specified action 
        /// </summary>
        /// <param name="by"></param>
        /// <param name="ExpectedValuevalue"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        protected bool WebElementComparisonAndAction(By by, string ExpectedValue,string Action)
        {
            IList<IWebElement> ElementList = ByToIWebElementList(by);
            bool flag = false;
            for (int i = 0; i < ElementList.Count; i++)
            {
                if (ElementList[i].Text.ToLower().Equals(ExpectedValue.ToLower()))
                {
                    flag = true;
                    if (Action.Equals("Click"))
                    {
                        ElementList[i].Click();
                    }
                    else
                    {
                        Console.WriteLine("No Action is performed");
                    }
                    break;
                }
            }
            if (flag == false)
            {
                throw new Exception("The Expected value " + ExpectedValue + " not present in current page");
            }
            return flag;
        }
 
        /// <summary>
        /// This will wait for a particular webelement
        /// </summary>
        /// <param name="stringElement"></param>
        /// <param name="specifiedTimeout"></param>
        protected IWebElement WaitOn(string element, int timeout = DefaultTimeout)
        {
            WaitforPageLoad();
            var wait = new WebDriverWait(_driverContext.Driver, TimeSpan.FromSeconds(timeout));
            wait.IgnoreExceptionTypes((typeof(NoSuchElementException)));
            wait.IgnoreExceptionTypes((typeof(ElementNotVisibleException)));
            wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
            try
            {
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(element)));
            }
            catch (Exception)
            {
                return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(element)));
            }
        }

        /// <summary>
        /// This is used to check list of webelements displayed or not 
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public Boolean CheckIWebElementListDisplayed(By by)
        {
            Boolean flag = true;
            IList<IWebElement> ElementList = ByToIWebElementList(by);
            if (ElementList.Count != 0)
            {
                foreach (IWebElement element in ElementList)
                {
                    Console.WriteLine("element in iteration : " + element.Text);

                    if (!element.Displayed)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
    }
 }
