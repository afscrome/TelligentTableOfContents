using System.Text;

namespace Telligent.Evolution.Extensions.TableOfContents
{
	public class TableOfContentsBuilder : TableOfContentsBuilderBase
	{
		private readonly ILocalisationService _localisationService;

		public TableOfContentsBuilder(ILocalisationService localisationService)
		{
			_localisationService = localisationService;
		}

		protected override void StartTableOfContents(StringBuilder builder)
		{
			builder.Append("<div class=\"table-of-contents\">");
			builder.Append("<h2 class=\"toc-title\">");
			builder.Append(_localisationService.GetString("TableOfContents_Title"));
			builder.Append("</h2>");
		}

		protected override void EndTableOfContents(StringBuilder builder)
		{
			builder.Append("</div>");
		}

		protected override void StartHierarchyList(StringBuilder builder)
		{
			builder.Append("<div class=\"hierarchy-list-header\"> </div>");
			builder.Append("<ul class=\"hierarchy-list\">");
		}

		protected override void EndHierarchyList(StringBuilder builder)
		{
			builder.Append("</ul>");
			builder.Append("<div class=\"hierarchy-list-footer\"> </div>");
		}

		protected override void StartHierarchyItem(StringBuilder builder)
		{
			builder.Append("<li class=\"hierarchy-item\">");
		}

		protected  override void EndHierarchyItem(StringBuilder builder)
		{
			builder.Append("</li>");
		}
	}
}
