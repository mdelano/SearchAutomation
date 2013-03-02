using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Globalization;

namespace SearchAutomation.SearchEngine
{
    public class GoogleSearchEngine : ISearchEngine
    {
        public GoogleSearchEngine(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            Initialize();
        }

        private IWebDriver _webDriver { get; set; }

        public SearchResult Search(string searchTerms)
        {
            _webDriver.Navigate().Refresh();
            var wait = GetDriverWait();
            var query = wait.Until((d) => d.FindElement(By.Name("q")));
            query.Clear();
            query.SendKeys(searchTerms);
            query.SendKeys(Keys.Return);
            

            return new SearchResult(GetResultCount());
        }

        private long GetResultCount()
        {
            try
            {
                var wait = GetDriverWait();
                var resultStatsElement = GetResultsElement();
                var resultStats = resultStatsElement.Text;

                var about = "About ";
                if (resultStats.StartsWith(about))
                {
                    resultStats = resultStats.Replace(about, String.Empty);
                }

                var results = " results";

                if (resultStats.Contains(results))
                {
                    resultStats = resultStats.Remove(resultStats.IndexOf(results));
                }

                return Int64.Parse(resultStats, NumberStyles.AllowThousands);
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Append("The search engine return no result count.");
                return 0;
            }
            catch(ElementNotVisibleException){
                Logger.Append("Unable to find the count element on the page.");
                return 0;
            }
        }

        private IWebElement GetResultsElement()
        {
            var wait = GetDriverWait();
            IWebElement resultStatsElement;
            
            try
            {
                resultStatsElement = wait.Until((d) => { return d.FindElement(By.Id("resultStats")); });
                resultStatsElement.Click();
                return resultStatsElement;
            }
            catch (StaleElementReferenceException)
            {
                resultStatsElement = wait.Until((d) => { return d.FindElement(By.Id("resultStats")); });
                return resultStatsElement;
            }
        }

        private WebDriverWait GetDriverWait(){
            return new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
        }

        private void Initialize()
        {
            _webDriver.Navigate().GoToUrl("http://www.google.com/");
        }

        public void Dispose()
        {
            _webDriver.Quit();
            _webDriver = null;
        }
    }
}
