
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SampleProject.Config
{
    public class ConfigReader
    {
        public static void SetFrameworkSettings()
        {
            TestSettings testSettings = new TestSettings();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName).AddJsonFile("appSettings.json");

            IConfigurationRoot configurationRoot = builder.Build();

            configurationRoot.Bind(testSettings);
            //   Settings.AUT = configurationRoot.GetSection("testSettings").Get<TestSettings>().AUT;
            Settings.ExecutionType = testSettings.ExecutionType;
            if (Settings.ExecutionType.Equals("Local"))
            {
                Settings.ExecutionEnv = testSettings.ExecutionEnv;
            }
            else
            {
                Settings.ExecutionEnv = Environment.GetEnvironmentVariable("executionEnv", EnvironmentVariableTarget.Process);
            }
            Settings.BrowserType = testSettings.Browser;
            var envDataSection = configurationRoot.GetSection("EnvironmentData");
            foreach (IConfigurationSection section in envDataSection.GetChildren())
            {
                var key = section.GetValue<string>("environment");
                if (key == Settings.ExecutionEnv)
                {
                    Settings.AUT = section.GetValue<string>("aut");
                    Settings.TestType = section.GetValue<string>("testType");
                    Settings.IsLog = section.GetValue<string>("isLog");
                    Settings.LogPath = section.GetValue<string>("logPath");
                }

            }

            //     Settings.TestType = configurationRoot.GetSection("testSettings").Get<TestSettings>().TestType;
            //     Settings.IsLog = configurationRoot.GetSection("testSettings").Get<TestSettings>().IsLog;


            //     Settings.LogPath = configurationRoot.GetSection("testSettings").Get<TestSettings>().LogPath;

        }

    }
}
