
using System.Text;

namespace Telligent.Evolution.TableOfContents
{
	public class TableOfContentsBuilder : TableOfContentsBuilderBase
	{
		private readonly ILocalisationService _localisationService;

		public TableOfContentsBuilder(ILocalisationService localisationService)
		{
			_localisationService = localisationService;
		}

		public override void StartTableOfContents(StringBuilder builder)
		{
			builder.Append("<div class=\"table-of-contents\">");
			builder.Append("<h2 class=\"title\">");
			builder.Append(_localisationService.GetString("TableOfContents_Title"));
			builder.Append("</h2>");
		}

		public override void EndTableOfContents(StringBuilder builder)
		{
			builder.Append("</div>");
		}

		public override void StartHierarchyList(StringBuilder builder)
		{
			builder.Append("<div class=\"hierarchy-list-header\"> </div>");
			builder.Append("<ul class=\"hierarchy-list\">");
		}

		public override void EndHierarchyList(StringBuilder builder)
		{
			builder.Append("</ul>");
			builder.Append("<div class=\"hierarchy-list-footer\"> </div>");
		}

		public override void StartHierarchyItem(StringBuilder builder)
		{
			builder.Append("<li class=\"hierarchy-item\">");
		}

		public override void EndHierarchyItem(StringBuilder builder)
		{
			builder.Append("</li>");
		}
	}
}
