using OpenQA.Selenium;
using System;
using System.IO;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class DivTable
    {
        private readonly IWebDriver _webDriver;

        public ITableElement DivTableElementBySelectors => _webDriver.FindTableElement(By.ClassName("divTableHead"), By.CssSelector(".divTableBody>.divTableRow"), By.ClassName("divTableCell"));

        public ITableElement DivTableElementByElements
        {
            get
            {
                var headers = _webDriver.FindElements(By.ClassName("divTableHead"));
                var rows = _webDriver.FindElements(By.CssSelector(".divTableBody>.divTableRow"));
                return _webDriver.FindTableElement(headers, rows, By.ClassName("divTableCell"));
            }
        }

        public DivTable(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void Open()
        {
            _webDriver.Navigate().GoToUrl(new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "DemoHtml/divtable.html")).AbsoluteUri);
        }
    }
}
