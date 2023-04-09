using SampleProject.CorePackage;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace SampleProject.Config
{
    [JsonObject("testSettings")]
    public class TestSettings
    {
        [JsonProperty("executionType")]
        public string ExecutionType { get; set; }

        [JsonProperty("executionEnv")]
        public string ExecutionEnv { get; set; }

        [JsonProperty("browser")]
        public BrowserType Browser { get; set; }


        [JsonProperty("EnvironmentData")]
        public List<EnvironmentData> EnvironmentData { get; set; }

    }

    public class EnvironmentData
    {
        [JsonProperty("environment")]
        public string Region { get; set; }

        [JsonProperty("aut")]
        public string Aut { get; set; }

        [JsonProperty("testType")]
        public string TestType { get; set; }

        [JsonProperty("isLog")]
        public string IsLog { get; set; }

        [JsonProperty("logPath")]
        public string LogPath { get; set; }
    }
}
