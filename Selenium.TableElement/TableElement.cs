using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.TableElement
{
    public class TableElement
    {
        public readonly IEnumerable<TableRowElement> TableRowElements;

        public TableElement(IWebDriver webDriver, By headersSelector, By rowsSelector) : this(webDriver, headersSelector, rowsSelector, By.XPath("./td"))
        {
        }

        public TableElement(IWebDriver webDriver, By headersSelector, By rowsSelector, By rowColumnSelector)
        {
            var headers = webDriver.FindElements(headersSelector);
            var rows = webDriver.FindElements(rowsSelector);
            TableRowElements = rows.Select(x => new TableRowElement(headers, x, rowColumnSelector));
        }
    }
}
