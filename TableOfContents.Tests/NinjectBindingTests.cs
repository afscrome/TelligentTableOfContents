using Ninject;
using NUnit.Framework;

// Using a different namespace to avoid using the TextFixtureSetUp class
namespace Telligent.Evolution.Extensions.TableOfContents.Tests
{
	[TestFixture]
	public class NinjectBindingTests
	{
		[TestFixtureSetUp]
		public void SetUp()
		{
			var kernel = new StandardKernel();
			kernel.Load("..\\..\\..\\TableOfContents\\Includes\\Ninject.config");

			Telligent.Common.Services.Initialize(kernel);
		}


		[Test]
		public void Test_ITableOfContentsService()
		{
			ServicesTest<ITableOfContentsService, TableOfContentsService>();
		}

		[Test]
		public void Test_ITableOfContentsBuilder()
		{
			ServicesTest<ITableOfContentsBuilder, TableOfContentsBuilder>();
		}

		[Test]
		public void Test_IHtmlStripper()
		{
			ServicesTest<IHtmlStripper, HtmlStripper>();
		}

		[Test]
		public void Test_ILocalisationService()
		{
			ServicesTest<ILocalisationService, LocalisationService>();
		}

		[Test]
		public void Test_Can_Create_TableOfContentsModule()
		{
			var module = new TableOfContentsModule();

			Assert.NotNull(module);
		}

		protected void ServicesTest<TService, TExpected>()
		{
			var obtainedService = Telligent.Common.Services.Get<TService>();
			Assert.NotNull(obtainedService);
			Assert.IsInstanceOf<TExpected>(obtainedService);
		}


		[TestFixtureTearDown]
		public void TearDown()
		{
			Telligent.Common.Services.Shutdown();
		}
	}
}
