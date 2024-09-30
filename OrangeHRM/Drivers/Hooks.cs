using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace OrangeHRM.Drivers
{
    [Binding]
    public sealed class Hooks: ExtentReport
    {
        private readonly IObjectContainer container;
        public static ISpecFlowOutputHelper _specFlowOutputHelper;
        public IWebDriver driver;
        public static string application;
        public static string title;


        public Hooks(IObjectContainer container, ISpecFlowOutputHelper specFlowOutputHelper)
        {
            this.container = container;
            _specFlowOutputHelper = specFlowOutputHelper;
            this.driver = driver;
        }
        [BeforeTestRun]
        public static void BeforeTestRun() {
            ExtentReportInit();
        }
        [AfterTestRun]
        public static void AfterTestRun()
        {
            ExtentReportTearDown();
        }
        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featurecontext) {
            _feature = extentReports.CreateTest<Feature>(featurecontext.FeatureInfo.Title);
            string[] tags = featurecontext.FeatureInfo.Tags;
            application = tags.FirstOrDefault();
            title = featurecontext.FeatureInfo.Title;
        }
        [AfterFeature]
        public static void AfterFeature(FeatureContext featurecontext)
        {
            _specFlowOutputHelper.WriteLine("Feature file running completed");
        }
        [BeforeScenario (Order =1)]
        public void BeforeScenarioOrder(ScenarioContext scenariocontext) {

            string browsername = "Chrome";
            browsername = browsername.ToLower().Trim();
            _specFlowOutputHelper.WriteLine("Browser name is : "+browsername);

            switch (browsername)
            {
                case "chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("no-sandbox");
                    driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(),options,TimeSpan.FromMinutes(3));
                    driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(300));
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();      
            }
            driver.Manage().Window.Maximize();
            container.RegisterInstanceAs<IWebDriver>(driver);
            _scenario = _feature.CreateNode<Scenario>(scenariocontext.ScenarioInfo.Title);
        }
        [AfterScenario]
        public void AfterScenario() { 
           var driver = container.Resolve<IWebDriver>();
        }
        [AfterStep]
        public void AfterStep(ScenarioContext scenariocontext) { 
            string stepType = scenariocontext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepname = scenariocontext.StepContext.StepInfo.Text;
            var driver = container.Resolve<IWebDriver>();
            if (scenariocontext.TestError == null) {
                if (stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepname);
                }
                else if(stepType == "When")
                {
                    _scenario.CreateNode<When>(stepname);
                }
                else if (stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepname);
                }
            }
            if (scenariocontext.TestError != null)
            {
                if (stepType == "Given")
                {
                    _scenario.CreateNode<Given>(stepname).Fail(scenariocontext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver , scenariocontext)).Build());
                }
                else if (stepType == "When")
                {
                    _scenario.CreateNode<When>(stepname).Fail(scenariocontext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenariocontext)).Build()); ;
                }
                else if (stepType == "Then")
                {
                    _scenario.CreateNode<Then>(stepname).Fail(scenariocontext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenariocontext)).Build()); ;
                }
                else if (stepType == "And")
                {
                    _scenario.CreateNode<And>(stepname).Fail(scenariocontext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(addScreenshot(driver, scenariocontext)).Build()); ;
                }
            }

        }
    }
}
