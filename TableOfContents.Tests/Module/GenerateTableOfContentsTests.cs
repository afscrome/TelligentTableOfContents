using NUnit.Framework;
using Telligent.Evolution.Extensions.TableOfContents.Tests.Mocks;

namespace Telligent.Evolution.Extensions.TableOfContents.Tests.Module
{
	[TestFixture]
	public class GenerateTableOfContentsTests
	{
		private TableOfContentsModule _tableOfContentsModule;
		private DummyTableOfContentsBuilder _tableOfContentsBuilder;
		[TestFixtureSetUp]
		public void SetUp()
		{
			_tableOfContentsBuilder = new DummyTableOfContentsBuilder();
			_tableOfContentsModule = new TableOfContentsModule(new DummyTableOfContentsService(), _tableOfContentsBuilder);	
		}

		[Test]
		public void Test_Leaves_Html_Alone_If_Does_Not_Contain_TOC_Tag()
		{
			const string input = "<h2><a name=\"Content\"></a>Content</h2><p>Here Is Come Content</p>";

			Assert.AreEqual(input, _tableOfContentsModule.InsertTableOfContents(input));
		}

		[Test]
		public void Test_Removes_TOC_Tag_When_No_TableOfContents_To_Render()
		{
			_tableOfContentsBuilder.Output = string.Empty;
			const string input = "[toc]<h2><a name=\"Content\"></a>Content</h2><p>Here Is Come Content</p>";
			var result = _tableOfContentsModule.InsertTableOfContents(input);
			Assert.AreEqual("<h2><a name=\"Content\"></a>Content</h2><p>Here Is Come Content</p>", result);
		}

		[Test]
		public void Test_Replaces_TOC_Tag_With_TableOfContents()
		{
			_tableOfContentsBuilder.Output = "[[TABLE-OF-CONTENTS]]";
			const string input = "[toc]<h2><a name=\"Content\"></a>Content</h2><p>Here Is Come Content</p>";
			var result = _tableOfContentsModule.InsertTableOfContents(input);
			Assert.AreEqual("[[TABLE-OF-CONTENTS]]<h2><a name=\"Content\"></a>Content</h2><p>Here Is Come Content</p>", result);
		}

	}
}
