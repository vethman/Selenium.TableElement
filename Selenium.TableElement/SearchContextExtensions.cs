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
        /// Create ITableElement within the context by selectors. Default columnselector By.XPath("./td")
        /// </summary>
        /// <param name="searchContext">The searchContext instance to extend.</param>
        /// <param name="headersSelector">Selector to find IWebElements representing headercolumns</param>
        /// <param name="rowsSelector">Selector to find IWebElements representing tablerows</param>
        /// <returns>ITableElement</returns>
        public static ITableElement FindTableElement(this ISearchContext searchContext, By headersSelector, By rowsSelector)
        {
            return new TableElement(searchContext, headersSelector, rowsSelector);
        }

        /// <summary>
        /// Create ITableElement within the context by selectors.
        /// </summary>
        /// <param name="searchContext">The searchContext instance to extend.</param>
        /// <param name="headersSelector">Selector to find IWebElements representing headercolumns</param>
        /// <param name="rowsSelector">Selector to find IWebElements representing tablerows</param>
        /// <param name="columnSelector">Selector to find IWebElements inside a row representing the columns</param>
        /// <returns>ITableElement</returns>
        public static ITableElement FindTableElement(this ISearchContext searchContext, By headersSelector, By rowsSelector, By columnSelector)
        {
            return new TableElement(searchContext, headersSelector, rowsSelector, columnSelector);
        }

        /// <summary>
        /// Create ITableElement within the context by selectors. Default columnselector By.XPath("./td")
        /// </summary>
        /// <param name="searchContext">The searchContext instance to extend.</param>
        /// <param name="headerElements">IWebElements representing headercolumns</param>
        /// <param name="rowElements">IWebElements representing tablerows</param>
        /// <returns>ITableElement</returns>
        public static ITableElement FindTableElement(this ISearchContext searchContext, IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements)
        {
            return new TableElement(headerElements, rowElements);
        }

        /// <summary>
        /// Create ITableElement within the context by selectors.
        /// </summary>
        /// <param name="searchContext">The searchContext instance to extend.</param>
        /// <param name="headerElements">IWebElements representing headercolumns</param>
        /// <param name="rowElements">IWebElements representing tablerows</param>
        /// /// <param name="columnSelector">Selector to find IWebElements inside a row representing the columns</param>
        /// <returns>ITableElement</returns>
        public static ITableElement FindTableElement(this ISearchContext searchContext, IEnumerable<IWebElement> headerElements, IEnumerable<IWebElement> rowElements, By columnSelector)
        {
            return new TableElement(headerElements, rowElements, columnSelector);
        }
    }
}
