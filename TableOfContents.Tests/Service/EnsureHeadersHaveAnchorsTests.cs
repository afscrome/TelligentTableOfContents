using NUnit.Framework;
using Telligent.Evolution.TableOfContents.Tests.Mocks;

namespace Telligent.Evolution.TableOfContents.Tests.Service
{
	[TestFixture]
	public class EnsureHeadersHaveAnchorsTests
	{
		private TableOfContentsService _tableOfContentsService;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_tableOfContentsService = new TableOfContentsService(new DummyHtmlStripper());
		}

		//TODO: Currently only test H2.  Test H3,4,5&6
		[Test]
		public void Test_Inserts_Anchor_Immediatly_Inside_Heading_Tag_When_Heading_Has_No_Anchor()
		{
			EnsureHeadersHaveAnchorTest("<h2>Heading</h2>"
												, "<h2><a name=\"Heading\"></a>Heading</h2>");
		}

		[Test]
		public void Test_Leaves_Html_Untouched_When_Heading_Has_Anchor_At_Begining_Of_Heading()
		{
			EnsureHeadersHaveAnchorTest("<h3><a name=\"heading\"></a>Heading</h3>"
												, "<h3><a name=\"heading\"></a>Heading</h3>");
		}
		[Test]
		public void Test_Leaves_Html_Untouched_When_Heading_Has_Anchor_At_End_Of_Heading()
		{
			EnsureHeadersHaveAnchorTest("<h4>Heading<a name=\"heading\"></a></h4>"
												, "<h4>Heading<a name=\"heading\"></a></h4>");
		}

		[Test]
		public void Test_Leaves_Html_Untouched_When_Heading_Has_Anchor_In_Middle_Of_Heading()
		{
			EnsureHeadersHaveAnchorTest("<h5>Heading<a name=\"heading\"></a>Test</h5>"
												, "<h5>Heading<a name=\"heading\"></a>Test</h5>");

		}

		private void EnsureHeadersHaveAnchorTest(string input, string expectedOutput)
		{
			Assert.AreEqual(expectedOutput, _tableOfContentsService.EnsureHeadersHaveAnchors(input));
		}

	}
}
