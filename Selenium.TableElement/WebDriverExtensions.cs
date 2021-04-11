using OpenQA.Selenium;
using Selenium.TableElement.Interfaces;

namespace Selenium.TableElement
{
    public static class WebDriverExtensions
    {
        public static ITableElement FindTableElement(this IWebDriver webDriver, By headersSelector, By rowsSelector)
        {
            return new TableElement(webDriver, headersSelector, rowsSelector);
        }
    }
}
