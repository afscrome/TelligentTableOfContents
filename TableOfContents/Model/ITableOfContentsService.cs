using System.Collections.Generic;

namespace Telligent.Evolution.Extensions.TableOfContents
{
	public interface ITableOfContentsService
	{
		string EnsureHeadersHaveAnchors(string html);
		IEnumerable<Heading> GetHeadings(string html);
		ICollection<HierarchyItem<Heading>> GetHeadingHierarchy(string html);
	}
}
