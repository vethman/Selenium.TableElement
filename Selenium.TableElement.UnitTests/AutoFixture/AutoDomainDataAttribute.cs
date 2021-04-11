using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using System;
using System.Linq;

namespace Selenium.TableElement.UnitTests.AutoFixture
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute() : base(GetConfiguredFixture())
        {
        }

        public static Func<Fixture> GetConfiguredFixture()
        {
            var fixture = new Fixture();
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Customize(new AutoMoqCustomization());

            return () => fixture;
        }
    }
}
