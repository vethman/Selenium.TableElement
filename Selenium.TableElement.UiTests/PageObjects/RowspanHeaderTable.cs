using OpenQA.Selenium;
using System;
using System.IO;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class RowspanHeaderTable
    {
        private readonly IWebDriver _webDriver;

        public ITableElement RowspanHeaderTableElement => _webDriver.FindTableElement(By.XPath("//table/thead/tr/th"), By.XPath("//table/tbody/tr"));

        public RowspanHeaderTable(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void Open()
        {
            _webDriver.Navigate().GoToUrl(new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "DemoHtml/rowspanheadertable.html")).AbsoluteUri);
        }
    }
}
