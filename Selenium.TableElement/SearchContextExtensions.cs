using OpenQA.Selenium;
using System.Collections.Generic;

namespace Selenium.TableElement
{
    /// <summary>
    /// Provides extension methods for convenience in using ISearchContext.
    /// </summary>
    public static class SearchContextExtensions
    {
        /// <summary>
        /// Create ITableElement within the context by selectors.
        /// </summary>
        /// <param name="searchContext">The searchContext instance to extend.</param>
        /// <param name="headersSelector">Selector to find IWebElements representing headercolumns</param>
        /// <param name="rowsSelector">Selector to find IWebElements representing tablerows</param>
        /// <returns></returns>
        public static ITableElement FindTableElement(this ISearchContext searchContext, By headersSelector, By rowsSelector)
        {
            return new TableElement(searchContext, headersSelector, rowsSelector);
        }

        public static ITableElement FindTableElement(this ISearchContext searchContext, By headersSelector, By rowsSelector, By columnSelector)
        {
            return new TableElement(searchContext, headersSelector, rowsSelector, columnSelector);
        }

        public static ITableElement FindTableElement(this ISearchContext searchContext, IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements)
        {
            return new TableElement(headerElements, rowElements);
        }

        public static ITableElement FindTableElement(this ISearchContext searchContext, IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements, By columnSelector)
        {
            return new TableElement(headerElements, rowElements, columnSelector);
        }
    }
}
