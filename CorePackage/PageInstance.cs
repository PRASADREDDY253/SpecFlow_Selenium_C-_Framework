using System;
using System.Collections.Generic;
using System.Text;

namespace SampleProject.CorePackage
{
    public class PageInstance
    {
        private DriverHelper _context;

        public PageInstance(DriverHelper drivercontext)
        {
            this._context = drivercontext;
        }

        public TPage GetInstance<TPage>() where TPage : PageInstance, new()
        {
            return (TPage)Activator.CreateInstance(typeof(TPage));
        }

        public TPage As<TPage>() where TPage : PageInstance
        {
            //try {
            //if (!String.Equals(_context.ngDriver.Url.ToString(), _context.Driver.Url.ToString()))
            //{ _context.ngDriver.Url = _context.Driver.Url; }}
            //catch (Exception) { }
            _context.CurrentPage = (TPage)Activator.CreateInstance(typeof(TPage), _context);
            return (TPage)_context.CurrentPage;
        }

        public TPage Page<TPage>() where TPage : PageInstance
        {
            return (TPage)this;
        }

        public TPage As<TPage>(object T) where TPage : PageInstance
        {
            //Uncomment if Angular based page is used
            //try {
            //if (!String.Equals(_context.ngDriver.Url.ToString(), _context.Driver.Url.ToString()))
            //{ _context.ngDriver.Url = _context.Driver.Url; }}
            //catch (Exception) { }

            _context.CurrentPage = (TPage)Activator.CreateInstance(typeof(TPage), _context, T);
            return (TPage)_context.CurrentPage;
        }
    }
}
