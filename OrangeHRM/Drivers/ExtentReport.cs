using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OrangeHRM.Drivers
{
    public class ExtentReport
    {
        public static ExtentReports extentReports;
        public static ExtentTest _feature;
        public static ExtentTest _scenario;

        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static String testresultpath = dir.Replace("bin\\Debug\\net6.0", "TestResults");

        public static void ExtentReportInit() {

            var htmlReporter = new ExtentHtmlReporter(testresultpath);
            htmlReporter.Config.ReportName = "Automation status Report";
            htmlReporter.Config.DocumentTitle = "Automation Statuc Report";
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Start();

            extentReports = new ExtentReports();
            extentReports.AttachReporter(htmlReporter);
            extentReports.AddSystemInfo("Application", "Youtube");
            extentReports.AddSystemInfo("Browser", "Chrome");
            extentReports.AddSystemInfo("OS", "Windows");
        }
        public static void ExtentReportTearDown() {
            extentReports.Flush();
        }
        public string addScreenshot(IWebDriver driver, ScenarioContext scenarioContext) { 
      
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string screenshotlocation = (Path.Combine(testresultpath, scenarioContext.ScenarioInfo.Title + ".png"));
            screenshot.SaveAsFile(screenshotlocation);
            return screenshotlocation;
         }
    }
}
