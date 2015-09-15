
namespace AlexCrome.Telligent.TableOfContents
{
	public interface ITableOfContentsBuilder
	{
		string BuildTableOfContents(HierarchyCollection<Heading> headings);
	}
}
