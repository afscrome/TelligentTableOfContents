using System.Collections.Generic;

namespace Telligent.Evolution.Extensions.TableOfContents.Tests.Mocks
{
	public class DummyTableOfContentsService : ITableOfContentsService
	{
		public string EnsureHeadersHaveAnchors(string html)
		{
			return html;
		}

		public IEnumerable<Heading> GetHeadings(string html)
		{
			return new List<Heading>();
		}

		public ICollection<HierarchyItem<Heading>> GetHeadingHierarchy(string html)
		{
			return new List<HierarchyItem<Heading>>();
		}
	}
}
