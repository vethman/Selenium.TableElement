using FluentAssertions;
using Microsoft.Edge.SeleniumTools;
using NUnit.Framework;
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
            var expectedHeaders = new List<string>() { "Firstname", "Lastname", "Date of birth" };

            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;
            var tableHeaderValues = tableElement.TableHeaderValues;

            tableHeaderValues.Should().BeEquivalentTo(expectedHeaders, options => options.WithStrictOrdering());
        }

        [Test]
        public void SimpleTable_MatchHeaderWithColumn()
        {
            var simpleTable = new SimpleTable(_webDriver);

            simpleTable.Open();

            var tableElement = simpleTable.ColspanTableElement;
            var tableRowElement = tableElement.TableRowElements.Single(x => x.GetColumn("Firstname").Text == "Beta");

            tableRowElement.GetColumn("Lastname").Text.Should().Be("Bit");
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
            var tableElementFromElement = simpleTable.ColspanTableElementFromWebElement;

            tableElementFromElement.Should().BeEquivalentTo(tableElementFromWebDriver, options => options.Excluding(x => x.SelectedMemberPath.EndsWith("Id")));
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
    }
}