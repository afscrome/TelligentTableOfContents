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

		[Test]
		public void Test_Inserts_Anchor_Immediatly_Inside_Heading_Tag_When_Heading_Has_No_Anchor()
		{
			EnsureHeadersHaveAnchorTest("<h2>Heading</h2>"
												, "<h2><a name=\"Heading\"></a>Heading</h2>");
		}

		[Test]
		public void Test_Leaves_Html_Untouched_When_Heading_Has_Anchor_At_Begining_Of_Heading()
		{
			const string input = "<h3><a name=\"heading\"></a>Heading</h3>";
			EnsureHeadersHaveAnchorTest(input, input);
		}
		[Test]
		public void Test_Leaves_Html_Untouched_When_Heading_Has_Anchor_At_End_Of_Heading()
		{
			const string input = "<h4>Heading<a name=\"heading\"></a></h4>";
			EnsureHeadersHaveAnchorTest(input, input);
		}

		[Test]
		public void Test_Leaves_Html_Untouched_When_Heading_Has_Anchor_In_Middle_Of_Heading()
		{
			const string input = "<h5>Anchor<a name=\"heading\"></a>In Middle</h5>";
			EnsureHeadersHaveAnchorTest(input, input);
		}

		[Test]
		public void Test_Leaves_Html_Untouched_When_Html_Contains_No_Headings()
		{
			const string input = "<p>Here is some HTML without headings</p>";
			EnsureHeadersHaveAnchorTest(input, input);
		}

		[Test]
		public void Test_Leaves_Html_Before_Last_Heading_Untouched()
		{
			const string prefix = "<p>Content at begining of article</p>";
			var result = _tableOfContentsService.EnsureHeadersHaveAnchors(prefix + "<h2>Heading</h2>");

			StringAssert.StartsWith(prefix, result);
		}

		[Test]
		public void Test_Leaves_Html_Between_Headings_Untouched()
		{
			const string middleContent = "<p>Content in middle of article</p>";
			var result = _tableOfContentsService.EnsureHeadersHaveAnchors("<h2>First heading</h2>" + middleContent + "<h3>Second heading</h3>");

			StringAssert.Contains(middleContent, result);
		}

		[Test]
		public void Test_Leaves_Html_After_Last_Heading_Untouched()
		{
			const string suffix = "<p>Content at the end of article</p>";
			var result = _tableOfContentsService.EnsureHeadersHaveAnchors("<h2>Heading</h2>" + suffix);

			StringAssert.EndsWith(suffix, result);
		}

		private void EnsureHeadersHaveAnchorTest(string input, string expectedOutput)
		{
			Assert.AreEqual(expectedOutput, _tableOfContentsService.EnsureHeadersHaveAnchors(input));
		}

	}
}
