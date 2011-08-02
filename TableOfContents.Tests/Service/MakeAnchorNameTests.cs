using NUnit.Framework;
using Telligent.Evolution.Extensions.TableOfContents.Tests.Mocks;

namespace Telligent.Evolution.Extensions.TableOfContents.Tests.Service
{
	[TestFixture]
	public class MakeAnchorNameTests
	{
		private TableOfContentsService _tableOfContentsService;
		[TestFixtureSetUp]
		public void SetUp()
		{
			_tableOfContentsService = new TableOfContentsService(new DummyHtmlStripper());
		}


		[Test]
		public void Initial_Non_Letter_Characters_Ignored()
		{
			Assert.AreEqual("R", _tableOfContentsService.MakeAnchorName("1._+=-R"));
		}

		[Test]
		public void Subsequent_Letters_Are_Untouched()
		{
			Assert.AreEqual("Begin", _tableOfContentsService.MakeAnchorName("Begin"));

		}

		[Test]
		public void Subsequent_Digits_Are_Untouched()
		{
			Assert.AreEqual("R1234", _tableOfContentsService.MakeAnchorName("R1234"));
		}

		[Test]
		public void Puncutation_Gets_Replaced_By_Underscore()
		{
			Assert.AreEqual("Initial_Heading", _tableOfContentsService.MakeAnchorName("Initial:Heading"));
		}

		[Test]
		public void Whitespace_Gets_Replaced_By_Underscore()
		{
			Assert.AreEqual("The_Mary_Rose", _tableOfContentsService.MakeAnchorName("The Mary Rose"));
		}

		[Test]
		public void Dash_Is_Left_Untouched()
		{
			Assert.AreEqual("R-r", _tableOfContentsService.MakeAnchorName("R-r"));
		}

		[Test]
		public void Symbols_Are_Ignored()
		{
			Assert.AreEqual("Copyright", _tableOfContentsService.MakeAnchorName("Copyright©"));
		}

		[Test]
		public void Trailing_Dashes_Removed()
		{
			Assert.AreEqual("R", _tableOfContentsService.MakeAnchorName("R-"));
		}

		[Test]
		public void Trailing_Underscores_Removed()
		{
			Assert.AreEqual("R", _tableOfContentsService.MakeAnchorName("R_"));
		}

		[Test]
		public void Multiple_Whitespace_and_Punctuation_Characters_In_A_Row_Get_Replaced_By_Single_Underscore()
		{
			Assert.AreEqual("HarryPotter_ChamberOfSecrets", _tableOfContentsService.MakeAnchorName("HarryPotter :- ChamberOfSecrets"));
		}

	}
}
