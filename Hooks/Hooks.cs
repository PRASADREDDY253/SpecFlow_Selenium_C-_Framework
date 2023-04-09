using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using SampleProject.Helpers;
using SampleProject.CorePackage;
using TechTalk.SpecFlow;

namespace SampleProject.Hooks
{
    [Binding]
    public sealed class Hooks : TestInitializeHooks
    {

        private ScenarioContext _scenarioContext;
        private DriverHelper _driverHelper;
        private ExtentTest _currentScenarioName;
        private static ExtentTest _featureName;
        private static ExtentReports _extent;
        private readonly FeatureContext _featureContext;

        public Hooks(DriverHelper driver, ScenarioContext scenarioContext, FeatureContext featureContext) : base(driver)
        {
            _driverHelper = driver;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _driverHelper.CurrentPage = new PageInstance(_driverHelper);
        }

        [BeforeTestRun]
        public static void TestInitalize()
        {
            //Initialize Extent report before test starts
            _extent = Reports.InitializeReports();
        }

        [BeforeScenario]
        public void SetUpBeforeScenario()
        {
            InitializeBrowser();

            //Get feature Name
            _featureName = _extent.CreateTest<Feature>(_featureContext.FeatureInfo.Title);
            //Create dynamic scenario name
            _currentScenarioName = _featureName.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);

        }
             
        [AfterStep]
        public void AfterEachStep()
        {

            var stepType = _scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();

            if (_scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text);
                else if (stepType == "And")
                    _currentScenarioName.CreateNode<And>(_scenarioContext.StepContext.StepInfo.Text);
            }
            else if (_scenarioContext.TestError != null)
            {

                var MediaEntity = _driverHelper.CaptureScreenshot(_scenarioContext.ScenarioInfo.Title.Trim());
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.InnerException, MediaEntity);
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.InnerException, MediaEntity);
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(_scenarioContext.StepContext.StepInfo.Text).Fail(_scenarioContext.TestError.Message, MediaEntity);
            }
            else if (_scenarioContext.ScenarioExecutionStatus.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    _currentScenarioName.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    _currentScenarioName.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    _currentScenarioName.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driverHelper.Driver.Quit();
        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            _extent.Flush();

        }
    }
}
