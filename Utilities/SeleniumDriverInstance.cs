using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NLog;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace ExtractTopYoutubeComments.Properties
{
    class SeleniumDriverInstance
    {
        IWebDriver driver;
        ILogger _logger;
        public SeleniumDriverInstance()
        {
            _logger = LogManager.GetCurrentClassLogger();
            new WebDriverManager.DriverManager().SetUpDriver(new WebDriverManager.DriverConfigs.Impl.ChromeConfig());
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
        }
   
        public IWebElement findElement(By xpath)=> driver.FindElement(xpath);
        public IList<IWebElement> findElements(By xpath) => driver.FindElements(xpath);
        public IWebDriver getDriverInstance { get { return this.driver; } }

        public bool waitForElementClickable(By element) {
          try { 
              var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
              wait.Until(ExpectedConditions.ElementIsVisible(element));
              wait.Until(ExpectedConditions.ElementToBeClickable(element));
                return true;
           }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public bool waitForElementVisible(By element)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
                wait.Until(ExpectedConditions.ElementIsVisible(element));
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void openURL(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public bool WaitUntilElementIsDisplayed(By element, int timeoutInSeconds)
        {
            for (int i = 0; i < timeoutInSeconds; i++)
            {
                if (waitForElementVisible(element))
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }

        public void moveToElement(IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        public void pageDown()
        {
            Actions action = new Actions(driver);
            action.SendKeys(Keys.PageDown).Perform();
        }

        public void close()
        {
            driver.Close();
        }

    }
}
