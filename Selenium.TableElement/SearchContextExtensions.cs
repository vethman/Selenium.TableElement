using OpenQA.Selenium;

namespace Selenium.TableElement
{
    public static class SearchContextExtensions
    {
        public static ITableElement FindTableElement(this ISearchContext searchContext, By headersSelector, By rowsSelector)
        {
            return new TableElement(searchContext, headersSelector, rowsSelector);
        }
    }
}
