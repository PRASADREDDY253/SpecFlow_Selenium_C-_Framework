using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace SampleProject.SupportFunctions
{
    public class FileHandler
    {
        public static string DownloadLocation;
        public static string GetFilesDownloadLoation()
        {

            try
            {
                DownloadLocation = GetProjectDirectory() + Path.DirectorySeparatorChar + "Downloads" + Path.DirectorySeparatorChar;
                if (!Directory.Exists(DownloadLocation))
                {
                    Directory.CreateDirectory(DownloadLocation);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }


            return DownloadLocation;
        }
        public static string GetProjectDirectory()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        }
    }
}
