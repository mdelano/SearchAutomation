using System;

namespace SearchAutomation.SearchEngine
{
    public interface ISearchEngine : IDisposable
    {
        SearchResult Search(string searchTerms);
    }
}
