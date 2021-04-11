using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.TableElement.UnitTests.AutoFixture;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Selenium.TableElement.UnitTests
{
    public class TableElementTests
    {
        [Theory]
        [AutoDomainInlineData("a", "b", "c", "a", "b", "c")]
        [AutoDomainInlineData("a", "b", "b", "a", "b_1", "b_2")]
        [AutoDomainInlineData("a", "b", "", "a", "b", "")]
        [AutoDomainInlineData("a", "", "", "a", "_1", "_2")]
        public void TableHeaderValues_ReturnCollectionOfHeadersFound(
            string headerTextElement1,
            string headerTextElement2,
            string headerTextElement3,
            string headerTextExpected1,
            string headerTextExpected2,
            string headerTextExpected3,
            [Frozen] Mock<IWebDriver> webDriverMock,
            [Frozen] Mock<IWebElement> webElementMock,
            ReadOnlyCollection<IWebElement> headerWebElements,
            By headerSelector,
            By rowsSelector)
        {
            webElementMock.SetupSequence(x => x.Text).Returns(headerTextElement3);
            webElementMock.SetupSequence(x => x.Text).Returns(headerTextElement2);
            webElementMock.SetupSequence(x => x.Text).Returns(headerTextElement1);
            webDriverMock.Setup(x => x.FindElements(headerSelector)).Returns(headerWebElements);
            webDriverMock.Setup(x => x.FindElements(rowsSelector)).Returns(new List<IWebElement>().AsReadOnly());

            var sut = new TableElement(webDriverMock.Object, headerSelector, rowsSelector);

            var expected = new List<string> { headerTextExpected1, headerTextExpected2, headerTextExpected3 };
            var result = sut.TableHeaderValues;

            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory, AutoDomainData]
        public void TableHeaderValues_HeaderWithColspan_ReturnCollectionOfHeadersFound(
            [Frozen] Mock<IWebDriver> webDriverMock,
            [Frozen] Mock<IWebElement> webElementMock,
            IWebElement headerWebElement,
            By headerSelector,
            By rowsSelector)
        {
            webElementMock.Setup(x => x.GetAttribute("colspan")).Returns("3");
            webElementMock.Setup(x => x.Text).Returns("a");
            webDriverMock.Setup(x => x.FindElements(headerSelector)).Returns(new List<IWebElement>() { headerWebElement }.AsReadOnly());
            webDriverMock.Setup(x => x.FindElements(rowsSelector)).Returns(new List<IWebElement>().AsReadOnly());

            var sut = new TableElement(webDriverMock.Object, headerSelector, rowsSelector);

            var expected = new List<string> { "a_1", "a_2", "a_3" };
            var result = sut.TableHeaderValues;

            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }
    }
}