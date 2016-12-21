using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Diagnostics;


namespace CheckBrowserLogTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;
        private WebDriverWait wait;


        [TestInitialize]
        public void Init()
        {
            var options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
        }


        [TestMethod]
        public void CheckBrowserLogTest()
        {
            driver.Url = "http://litecart/admin/";

            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Click();

            driver.FindElement(By.CssSelector("a[href='http://litecart/admin/?app=catalog&doc=catalog']")).Click();

            driver.FindElement(By.CssSelector("a[href='http://litecart/admin/?app=catalog&doc=catalog&category_id=1']")).Click();

            var products = driver.FindElements(By.CssSelector("#content table.dataTable a[href*='product_id']:not([title])"));

            for (int i = 0; i < products.Count; i++)
            {
                products[i].Click();

                var logs = driver.Manage().Logs.GetLog(LogType.Browser);
                if (logs.Count > 0)
                {
                    Debug.WriteLine("На странице " + driver.Url + " возникли сообщения в логе: ");
                }
                foreach (var log in logs)
                {
                    Debug.WriteLine(log.Message);
                }

                driver.Navigate().Back();
                products = driver.FindElements(By.CssSelector("#content table.dataTable a[href*='product_id']:not([title])"));
            }

        }


        [TestCleanup]
        public void Finish()
        {
            driver.Quit();
            //driver = null;
        }
    }
}
