using AutoFixture.NUnit3;

namespace Selenium.TableElement.UnitTests.AutoFixture
{
    public class AutoDomainInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoDomainInlineDataAttribute(params object[] arguments) : base(AutoDomainDataAttribute.GetConfiguredFixture(), arguments)
        {
        }
    }
}
