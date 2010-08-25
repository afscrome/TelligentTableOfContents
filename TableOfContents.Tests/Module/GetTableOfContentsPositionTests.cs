using NUnit.Framework;

namespace Telligent.Evolution.TableOfContents.Tests.Module
{
	[TestFixture]
	public class GetTableOfContentsPositionTests
	{
		private TableOfContentsModule _tableOfContentsModule;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_tableOfContentsModule = new TableOfContentsModule(null, null);
		}

		[Test]
		public void Test_Returns_Negative1_If_Input_Does_Not_Contain_TOC_Tag()
		{
			var input = "<p>Content</p>";
			Assert.AreEqual(-1, _tableOfContentsModule.GetTableOfContentsPosition(ref input));
		}

		[Test]
		public void Test_Returns_Index_Of_TOC_Tag()
		{
			var input = "<p>Content[toc]</p>";
			Assert.AreEqual(10, _tableOfContentsModule.GetTableOfContentsPosition(ref input));
		}

		[Test]
		public void Test_Removes_TOC_Tag_From_Input()
		{
			var input = "[toc]";
			_tableOfContentsModule.GetTableOfContentsPosition(ref input);
			StringAssert.DoesNotContain("[toc]", input);
		}

		[Test]
		public void Test_Removes_TOC_Tag_And_Surrounding_Paragraph_From_Input_When_Paragraph_Only_Contains_TOC_Tag()
		{
			var input = "<p>[toc]</p>";
			_tableOfContentsModule.GetTableOfContentsPosition(ref input);
			StringAssert.DoesNotContain("[toc]", input);
			StringAssert.DoesNotContain("<p></p>", input);
		}


	}
}
