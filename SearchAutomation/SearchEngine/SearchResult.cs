namespace SearchAutomation.SearchEngine
{
    public class SearchResult
    {
        public SearchResult(long totalResults)
        {
            TotalResults = totalResults;
        }

        public long TotalResults { get; private set; }
    }
}
