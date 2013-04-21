
namespace Telligent.Evolution.Extensions.TableOfContents
{
	public interface ITableOfContentsBuilder
	{
		string BuildTableOfContents(HierarchyCollection<Heading> headings);
	}
}
