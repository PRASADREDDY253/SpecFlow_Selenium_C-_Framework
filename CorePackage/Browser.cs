using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.CorePackage
{
    public class Browser
    {
        private readonly DriverHelper _driver;

        public Browser(DriverHelper driver)
        {
            _driver = driver;
        }

        public BrowserType Type { get; set; }

    }

    public enum BrowserType
    {
        Edge,
        FireFox,
        Chrome
    }

}
