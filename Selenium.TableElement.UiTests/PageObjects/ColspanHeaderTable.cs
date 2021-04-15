using OpenQA.Selenium;
using Selenium.TableElement.Interfaces;
using System;
using System.IO;

namespace Selenium.TableElement.UiTests.PageObjects
{
    public class ColspanHeaderTable
    {
        private readonly IWebDriver _webDriver;

        public ITableElement ColspanHeaderTableElement => _webDriver.FindTableElement(By.XPath("//table/thead/tr/th"), By.XPath("//table/tbody/tr"));

        public ColspanHeaderTable(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void Open()
        {
            _webDriver.Navigate().GoToUrl(new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "DemoHtml/colspanheadertable.html")).AbsoluteUri);
        }
    }
}
