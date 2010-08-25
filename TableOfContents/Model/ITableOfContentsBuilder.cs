using System.Collections.Generic;

namespace Telligent.Evolution.TableOfContents
{
	public interface ITableOfContentsBuilder
	{
		string BuildTableOfContents(ICollection<HierarchyItem<Heading>> headings);
	}
}
