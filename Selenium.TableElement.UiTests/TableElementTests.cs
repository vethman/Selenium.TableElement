using FluentAssertions;
using Microsoft.Edge.SeleniumTools;
using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.TableElement.UiTests.PageObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Selenium.TableElement.UiTests
{
    public class TableElementTests
    {
        private EdgeDriver _webDriver;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            new DriverManager().SetUpDriver(new EdgeConfig());

            var optionsEdge = new EdgeOptions()
            {
                UseChromium = true
            };

            _webDriver = new EdgeDriver(optionsEdge);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _webDriver.Quit();
        }

        [Test]
        public void SimpleTable_HeaderValues()
        {
            var expectedHeaders = new List<string>() { "First name", "Last name", "Date of birth" };

            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;

            tableElement.TableHeaderValues.Should().BeEquivalentTo(expectedHeaders, options => options.WithStrictOrdering());
            tableElement.TableRowElements.First().TableHeaderValues.Should().BeEquivalentTo(expectedHeaders, options => options.WithStrictOrdering());
        }

        [Test]
        public void SimpleTable_HeaderValueNotFound_ThrowsNoSuchElementException()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;

            Action act = () => tableElement.TableRowElements.First().GetColumn("NotExistingHeaderError");
            act.Should().Throw<NoSuchElementException>().WithMessage("Header 'NotExistingHeaderError' does not exist, available headers:\nFirst name\nLast name\nDate of birth");
        }

        [Test]
        public void SimpleTable_MatchHeaderWithColumn()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;
            var tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn("First name").Text == "Beta");

            tableRowElement.GetColumn("Last name").Text.Should().Be("Bit");
            tableRowElement.GetColumn("Date of birth").Text.Should().Be("01-10-2002");
        }

        [Test]
        public void SimpleTable_HasRowsAndHeaders()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;

            tableElement.TableHeaderValues.Should().HaveCount(3);
            tableElement.TableRowElements.Should().HaveCount(2);
        }

        [Test]
        public void SimpleTable_ReturnsITableElementAndITableRowElements()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;
            
            tableElement.Should().BeAssignableTo<ITableElement>();
            tableElement.TableRowElements.Should().BeAssignableTo<ReadOnlyCollection<ITableRowElement>>();
        }

        [Test]
        public void SimpleTable_FindTableWithinElement_SameOutComeAsFromWebDriver()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElementFromWebDriver = simpleTable.ColspanTableElement;
            var tableElementFromElement = simpleTable.TableParent.FindTableElement(By.XPath("./thead/tr/th"), By.XPath("./tbody/tr"));

            tableElementFromElement.Should().BeEquivalentTo(tableElementFromWebDriver, options => options.Excluding(x => x.SelectedMemberPath.EndsWith("Id")));
        }

        [Test]
        public void SimpleTable_FindTableElement_UseCollectionOfHeaderElementsAndRowElements()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElementFromWebDriver = simpleTable.ColspanTableElement;

            var tableHeaderCollection = simpleTable.TableHeaders;
            var tableRowCollection = simpleTable.TableRows;
            var tableElementFromElement = _webDriver.FindTableElement(tableHeaderCollection, tableRowCollection);

            tableElementFromElement.Should().BeEquivalentTo(tableElementFromWebDriver, options => options.Excluding(x => x.SelectedMemberPath.EndsWith("Id")));
        }

        [Test]
        public void SimpleTable_TableRowElementWithColumnSelector()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableRowElement = simpleTable.SingleTableRowWithColumnSelector;

            tableRowElement.GetColumn(0).Text.Should().Be("Ronald");
            tableRowElement.GetColumn("Last name").Text.Should().Be("Veth");
            tableRowElement.GetColumn("Date of birth").Text.Should().Be("22-12-1987");
        }

        [Test]
        public void SimpleTable_TableRowElementWithoutColumnSelector()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableRowElement = simpleTable.SingleTableRowWithoutColumnSelector;

            tableRowElement.GetColumn(0).Text.Should().Be("Ronald");
            tableRowElement.GetColumn("Last name").Text.Should().Be("Veth");
            tableRowElement.GetColumn("Date of birth").Text.Should().Be("22-12-1987");
        }

        [Test]
        public void ColspanHeaderTable_HeaderValueClonedByColspan()
        {
            var expectedHeaders = new List<string>() { "Name", "Color Combination_1", "Color Combination_2" };
            
            var colspanHeaderTable = new ColspanHeaderTable(_webDriver);

            colspanHeaderTable.Open();

            var tableElement = colspanHeaderTable.ColspanHeaderTableElement;
            var tableHeaderValues = tableElement.TableHeaderValues;

            tableHeaderValues.Should().BeEquivalentTo(expectedHeaders, options => options.WithStrictOrdering());
        }

        [Test]
        public void ColspanHeaderTable_FindColumnBySecondColspanHeader()
        {
            var colspanHeaderTable = new ColspanHeaderTable(_webDriver);

            colspanHeaderTable.Open();

            var tableElement = colspanHeaderTable.ColspanHeaderTableElement;
            var tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn("Name").Text == "BetaBit");

            tableRowElement.GetColumn("Color Combination_1").Text.Should().Be("White");
            tableRowElement.GetColumn("Color Combination_2").Text.Should().Be("Red");
        }

        [Test]
        public void ColspanRowTable_HeaderValues()
        {
            var expectedHeaders = new List<string>() { "Item", "Category", "Price" };

            var colspanRowTable = new ColspanRowTable(_webDriver);

            colspanRowTable.Open();

            var tableElement = colspanRowTable.ColspanRowTableElement;
            var tableHeaderValues = tableElement.TableHeaderValues;

            tableHeaderValues.Should().BeEquivalentTo(expectedHeaders, options => options.WithStrictOrdering());
        }

        [Test]
        public void ColspanRowTable_RowWithColspan_OnlyFirstCellOfColspanHasElement()
        {
            var colspanRowTable = new ColspanRowTable(_webDriver);

            colspanRowTable.Open();

            var tableElement = colspanRowTable.ColspanRowTableElement;
            var tableRowElementWithoutColspan = tableElement.TableRowElements.Single(x => x.GetColumn("Item").Text == "BetaBand");

            tableRowElementWithoutColspan.GetColumn("Category").Text.Should().Be("Music");
            tableRowElementWithoutColspan.GetColumn("Price").Text.Should().Be("3000");

            var tableRowElementWithColspan = tableElement.TableRowElements.Single(x => x.GetColumn(0).Text == "Total");
            
            tableRowElementWithColspan.GetColumn("Category").Text.Should().Be("8000");
            tableRowElementWithColspan.GetColumn("Price").Should().BeNull();
        }

        [Test]
        public void RowspanHeaderTable_HeaderWithRowSpan_ReturnsNotSupportedException()
        {
            var rowspanHeaderTable = new RowspanHeaderTable(_webDriver);

            rowspanHeaderTable.Open();

            rowspanHeaderTable.Invoking(x => x.RowspanHeaderTableElement)
                .Should().Throw<NotSupportedException>()
                .WithMessage("TableHeader including rowspan not supported");
        }

        [Test]
        public void RowspanRowTable_RowWithRowSpan_ReturnsNotSupportedException()
        {
            var rowspanRowTable = new RowspanRowTable(_webDriver);

            rowspanRowTable.Open();

            rowspanRowTable.Invoking(x => x.RowspanRowTableElement)
                .Should().Throw<NotSupportedException>()
                .WithMessage("TableRow including rowspan not supported");
        }

        [Test]
        public void DivTable_HeaderValues()
        {
            var expectedHeaders = new List<string>() { "First name", "Last name", "Specialty" };

            var divTable = new DivTable(_webDriver);

            divTable.Open();

            var tableElement = divTable.DivTableElementBySelectors;
            var tableHeaderValues = tableElement.TableHeaderValues;

            tableHeaderValues.Should().BeEquivalentTo(expectedHeaders, options => options.WithStrictOrdering());
        }

        [Test]
        public void DivTable_MatchHeaderWithColumn_TableElementBySelectors()
        {
            var divTable = new DivTable(_webDriver);

            divTable.Open();

            var tableElement = divTable.DivTableElementBySelectors;
            var tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn("First name").Text == "Beta");

            tableRowElement.GetColumn("Last name").Text.Should().Be("Bit");
            tableRowElement.GetColumn("Specialty").Text.Should().Be("Make special together");
        }

        [Test]
        public void DivTable_MatchHeaderWithColumn_TableElementByElements()
        {
            var divTable = new DivTable(_webDriver);

            divTable.Open();

            var tableElement = divTable.DivTableElementByElements;
            var tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn("First name").Text == "Beta");

            tableRowElement.GetColumn("Last name").Text.Should().Be("Bit");
            tableRowElement.GetColumn("Specialty").Text.Should().Be("Make special together");
        }

        [Test]
        public void DivTable_HasRowsAndHeaders()
        {
            var divTable = new DivTable(_webDriver);

            divTable.Open();

            var tableElement = divTable.DivTableElementBySelectors;

            tableElement.TableHeaderValues.Should().HaveCount(3);
            tableElement.TableRowElements.Should().HaveCount(3);
        }

        [Test]
        public void ButtonTable_ClickButtonInRow()
        {
            var buttonTable = new ButtonTable(_webDriver);

            buttonTable.Open();

            var buttonTableRows = buttonTable.buttonTableRows;

            buttonTableRows.Should().HaveCount(3);

            var secondRow = buttonTableRows.Single(x => x.Rownumber == "Row 2");
            secondRow.DeleteButton.Click();

            buttonTableRows = buttonTable.buttonTableRows;
            buttonTableRows.Should().HaveCount(2);
            buttonTableRows.Should().ContainSingle(x => x.Rownumber == "Row 1");
            buttonTableRows.Should().ContainSingle(x => x.Rownumber == "Row 3");
        }
    }
}