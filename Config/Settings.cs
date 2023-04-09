using SampleProject.CorePackage;


namespace SampleProject.Config
{
    public class Settings
    {
        public static string ExecutionType { get; set; }
        public static string ExecutionEnv { get; set; }

        public static string TestType { get; set; }

        public static string AUT { get; set; }


        public static BrowserType BrowserType { get; set; }



        public static string IsLog { get; set; }

        public static string LogPath { get; set; }
    }
}
