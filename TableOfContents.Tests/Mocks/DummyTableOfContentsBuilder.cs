using System.Collections.Generic;

namespace Telligent.Evolution.Extensions.TableOfContents.Tests.Mocks
{
	public class DummyTableOfContentsBuilder : ITableOfContentsBuilder
	{
		public string Output { get; set; }

		public string BuildTableOfContents(ICollection<HierarchyItem<Heading>> headings)
		{
			return Output;
		}
	}
}
