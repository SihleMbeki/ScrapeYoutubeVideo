using ExtractTopYoutubeComments.Pages;
using ExtractTopYoutubeComments.Properties;
using NLog;
using System;
using TechTalk.SpecFlow;

namespace ExtractTopYoutubeComments
{
    [Binding]
    public class ExtractVideoCommentsSteps
    {
        Youtube youtube;
        ILogger _logger;
        public ExtractVideoCommentsSteps()
        {
            _logger = LogManager.GetCurrentClassLogger();
            youtube = new Youtube();
        }
      [Given(@"the video url is valid")]
        public void GivenTheVideoUrlIsValid()
        {
        _logger.Info("URL Valid");
    }

        [When(@"I open the video ""(.*)""")]
        public void WhenIOpenTheVideo(string url)
        {
            youtube.Execute(url);
           _logger.Info(url);
        }

        [When(@"extract video comments")]
        public void WhenExtractVideoComments()
        {
            _logger.Info("Comments");
        }
        
        [Then(@"the results file should be created")]
        public void ThenTheResultsFileShouldBeCreated()
        {
            _logger.Info("Results");
        }
    }
}
