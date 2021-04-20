using OpenQA.Selenium;
using System.Collections.Generic;

namespace Selenium.TableElement
{
    public static class SearchContextExtensions
    {
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
