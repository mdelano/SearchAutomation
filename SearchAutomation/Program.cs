using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using SearchAutomation.SearchEngine;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SearchAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Append(string.Format("beginning search automation task: {0}", DateTime.Now));

            var searchProgram = new Program();
            int counter = 0;
            string line;

            // Read the file and display it line by line.
            Console.WriteLine("Enter the path of the search string file:");
            var filePath = Console.ReadLine();
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);


            while ((line = file.ReadLine()) != null)
            {
                if (line.Length == 0)
                    continue;

                using (var searchEngine = new GoogleSearchEngine(new FirefoxDriver()))
                {
                    Console.WriteLine(String.Format("Searching terms: {0}", line));
                    var searchResult = searchEngine.Search(line.Trim());
                    Console.WriteLine(String.Format("Total results found for terms: {0}", searchResult.TotalResults));
                    Logger.Append(string.Format("{0}, {1}", line.Trim(), searchResult.TotalResults));
                    Thread.Sleep(GetWaitTime());
                    counter++;
                }
            }
            

            file.Close();

            // Suspend the screen.
            Console.WriteLine("Finished searching. Hit enter to quit.");
            Console.ReadLine();
        }

        private static int GetWaitTime()
        {
            var waitTimeValue = System.Configuration.ConfigurationManager.AppSettings["WaitTimeBetweenSearches"];

            return Int32.Parse(waitTimeValue);
        }
    }
}
