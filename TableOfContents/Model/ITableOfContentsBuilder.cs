using System.Collections.Generic;

namespace Telligent.Evolution.Extensions.TableOfContents
{
	public interface ITableOfContentsBuilder
	{
		string BuildTableOfContents(ICollection<HierarchyItem<Heading>> headings);
	}
}
