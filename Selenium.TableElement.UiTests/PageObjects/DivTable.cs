using OpenQA.Selenium;
using System;
using System.IO;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class DivTable
    {
        private readonly IWebDriver _webDriver;

        public ITableElement DivTableElement => _webDriver.FindTableElement(By.ClassName("divTableHead"), By.CssSelector(".divTableBody>.divTableRow"), By.ClassName("divTableCell"));

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
