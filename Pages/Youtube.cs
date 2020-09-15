using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractTopYoutubeComments.Properties;
using NLog;
using NLog.Fluent;
using OpenQA.Selenium;

namespace ExtractTopYoutubeComments.Pages
{
    class Youtube
    {
        string content;
        SeleniumDriverInstance driver;
        ILogger _logger;
        public Youtube()
        {
            driver = new SeleniumDriverInstance();
            _logger = LogManager.GetCurrentClassLogger();
        }

        public By header => By.XPath("//h1[@class='title style-scope ytd-video-primary-info-renderer']");
        public IWebElement views => driver.findElement(By.XPath("//span[@class='view-count style-scope yt-view-count-renderer']"));
        public IWebElement deslikes => driver.findElement(By.XPath("(//div[@id='columns']//div[@id='info']//div[@id='menu-container']//yt-formatted-string)[2]"));
        public IWebElement likes => driver.findElement(By.XPath("(//div[@id='columns']//div[@id='info']//div[@id='menu-container']//yt-formatted-string)[1]"));
        public IWebElement subscribers => driver.findElement(By.XPath("//yt-formatted-string[@id='owner-sub-count']"));
        public By showMore => By.XPath("//yt-formatted-string[text()='Show more']");
        public IWebElement descriptionLinks => driver.findElement(By.XPath("//div[@class='style-scope ytd-expander']/div[@id='description']//a"));
        public IWebElement commentsCount => driver.findElement(By.XPath("//h2[@id='count']/yt-formatted-string"));
        public IList<IWebElement> comments => driver.findElements(By.XPath("//div[@id='main']//div[@id='content']//yt-formatted-string[@id='content-text']"));

        public void Execute(string url)
        {
            driver.openURL(url);
            if (driver.waitForElementVisible(header))
            {
                content += $"Header {driver.getDriverInstance.FindElement(header).Text}" + "\n";
                _logger.Info("Header");
                content += $"Views {views.Text}" + "\n";
                content += $"deslikes {deslikes.Text}" + "\n";
                _logger.Info("deslikes");
                content += $"Likes {likes.Text}" + "\n";
                _logger.Info("Likes");
                content += $"subscribers {subscribers.Text}" + "\n";
                _logger.Info("subscribers");

                if (driver.waitForElementVisible(showMore))
                {
                    if (driver.waitForElementClickable(showMore))
                    {
                        driver.getDriverInstance.FindElement(showMore).Click();
                        _logger.Info("Clicked show mroe");
                        try
                        {
                            var description = driver.findElements(By.XPath("//div[@class='style-scope ytd-expander']/div[@id='description']//span[@dir='auto']"));
                            foreach (IWebElement element in description)
                            {
                                content += $"Description {element.Text}" + "\n";
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e.Message);
                        }


                        _logger.Info("Description");
                    }
                }
                driver.pageDown();
                try
                {
                    if (!driver.waitForElementVisible(By.XPath("//h2[@id='count']/yt-formatted-string")))
                    {
                        _logger.Info("Comments header missing");
                    }
                    driver.moveToElement(commentsCount);
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }



                try
                {
                    if (!driver.WaitUntilElementIsDisplayed(By.XPath("//div[@class='style-scope ytd-expander']/div[@id='description']//a"), 10))
                    {
                        _logger.Error("Social site Links not displpayed");
                    }
                    var descriptionlinks = driver.findElements(By.XPath("//div[@class='style-scope ytd-expander']/div[@id='description']//a"));

                    if (!driver.WaitUntilElementIsDisplayed(By.XPath("//div[@id='main']//div[@id='content']//yt-formatted-string[@id='content-text']"), 10))
                    {
                        _logger.Error("comments not displpayed");
                    }

                    foreach (IWebElement link in descriptionlinks)
                    {
                        content += $"Link {link.Text}" + "\n";
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }

                try
                {

                    foreach (IWebElement comment in comments)
                    {
                        content += $"Comment {comment.Text}" + "\n";
                    }


                     this.writecontent();


                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }

            }
        }

        public void writecontent()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory + @"\..\..\Datasets\";
            string filename = DateTime.Now.ToString("HHmmss");
            var path = directory + filename + ".txt";
            File.WriteAllText(path, content);
            _logger.Info("text written");

            driver.close();
        }
    }
}
