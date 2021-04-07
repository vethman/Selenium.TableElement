using OpenQA.Selenium;

namespace Selenium.TableElement
{
    public static class WebDriverExtensions
    {
        public static TableElement FindTableElement(this IWebDriver webDriver, By headersSelector, By rowsSelector)
        {
            return new TableElement(webDriver, headersSelector, rowsSelector);
        }
    }
}
